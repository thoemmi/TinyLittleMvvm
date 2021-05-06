using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyLittleMvvm.Analyzers {
    [Generator]
    public class NotifyPropertyChangedGenerator : ISourceGenerator {
#pragma warning disable RS2008 // Enable analyzer release tracking
        private static readonly DiagnosticDescriptor DoesNotDeriveFromPropertyChangedBaseError = new(id: "TLM001",
            title: "Must implement base class",
            messageFormat: "AutoNotify is disabled for class '{0}' because it does not derive from '{1}'",
            category: "TinyLittleMvvm",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor NestedClassesNotAllowedError = new(id: "TLM002",
            title: "Nested classes are not supported",
            messageFormat: "AutoNotify is applied to a field of the nested class '{0}', which is not supported",
            category: "TinyLittleMvvm",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
#pragma warning restore RS2008 // Enable analyzer release tracking

        private const string attributeText = @"
using System;
namespace TinyLittleMvvm
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional(""AutoNotifyGenerator_DEBUG"")]
    sealed class AutoNotifyAttribute : Attribute
    {
        public AutoNotifyAttribute()
        {
        }
        public string? PropertyName { get; set; }
        public string[]? AffectedProperties { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register the attribute source
            context.RegisterForPostInitialization(initializationContext => initializationContext.AddSource("AutoNotifyAttribute", attributeText));

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
                return;

            // get the added attribute, and INotifyPropertyChanged
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("TinyLittleMvvm.AutoNotifyAttribute")!;
            var notifySymbol = context.Compilation.GetTypeByMetadataName("TinyLittleMvvm.PropertyChangedBase")!;

            // group the fields by class, and generate the source
            foreach (var group in receiver.Fields.GroupBy(f => f.ContainingType, SymbolEqualityComparer.Default))
            {
                if (group.Key is INamedTypeSymbol namedTypeSymbol) {
                    var classSource = ProcessClass(namedTypeSymbol, group, attributeSymbol, notifySymbol, context);
                    if (classSource != null)
                        context.AddSource($"{group.Key.Name}_autoNotify.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        private string? ProcessClass(INamedTypeSymbol classSymbol, IEnumerable<IFieldSymbol> fields, ISymbol attributeSymbol, INamedTypeSymbol propertyChangedBaseSymbol, GeneratorExecutionContext context)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    NestedClassesNotAllowedError,
                    attributeSymbol.Locations.FirstOrDefault(),
                    classSymbol.ToDisplayString()));

                return null;
            }

            var baseType = classSymbol.BaseType;
            var ok = false;
            while (baseType != null)
            {
                ok |= SymbolEqualityComparer.Default.Equals(baseType, propertyChangedBaseSymbol);
                baseType = baseType.BaseType;
            }
            if (!ok)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DoesNotDeriveFromPropertyChangedBaseError,
                    attributeSymbol.Locations.FirstOrDefault(),
                    classSymbol.ToDisplayString(), propertyChangedBaseSymbol.ToDisplayString()));

                return null;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // begin building the generated source
            var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public partial class {classSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}
    {{
");

            // create properties for each field 
            foreach (var fieldSymbol in fields)
            {
                ProcessField(source, fieldSymbol, attributeSymbol);
            }

            source.AppendLine(@"    }
}");
            return source.ToString();
        }

        private void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol)
        {
            // get the name and type of the field
            var fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;

            // get the AutoNotify attribute from the field, and any associated data
            var attributeData = fieldSymbol.GetAttributes().Single(ad => ad.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) == true);
            var optionalNameOverride = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

            string propertyName = GetPropertyName(fieldName, optionalNameOverride);
            if (propertyName.Length == 0 || propertyName == fieldName)
            {
                //TODO: issue a diagnostic that we can't process this field
                return;
            }

            source.AppendLine($@"
        public {fieldType} {propertyName}
        {{
            get 
            {{
                return this.{fieldName};
            }}
            set
            {{
                if (!System.Collections.Generic.EqualityComparer<{fieldType}>.Default.Equals(this.{fieldName}, value))
                {{
                    var oldValue = this.{fieldName};
                    this.{fieldName} = value;
                    this.NotifyOfPropertyChange(nameof({propertyName}));");

            var affectedPropertiesProperty = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "AffectedProperties").Value;
            if (!affectedPropertiesProperty.IsNull) {
                foreach (var affectedProperty in affectedPropertiesProperty.Values) {
                    var affectedPropertyName = affectedProperty.Value!.ToString();
                    source.AppendLine($@"                    this.NotifyOfPropertyChange(nameof({affectedPropertyName}));");
                }
            }

            source.Append($@"                    On{propertyName}Changed(oldValue, value);
                }}
            }}
        }}
        
#pragma warning disable IDE0060 // Remove unused parameter
        partial void On{propertyName}Changed({fieldType} old{propertyName}, {fieldType} new{propertyName});
#pragma warning restore IDE0060 // Remove unused parameter
");

        }

        private static string GetPropertyName(string fieldName, TypedConstant optionalNameOverride)
        {
            if (!optionalNameOverride.IsNull)
            {
                return optionalNameOverride.Value!.ToString()!;
            }

            fieldName = fieldName.TrimStart('_');
            if (fieldName.Length == 0)
                return string.Empty;

            if (fieldName.Length == 1)
                return fieldName.ToUpper();

            return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        private class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<IFieldSymbol> Fields { get; } = new();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                // any field with at least one attribute is a candidate for property generation
                if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
                    && fieldDeclarationSyntax.AttributeLists.Count > 0)
                {
                    foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                    {
                        // Get the symbol being declared by the field, and keep it if its annotated
                        if (context.SemanticModel.GetDeclaredSymbol(variable) is IFieldSymbol fieldSymbol &&
                            fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == "TinyLittleMvvm.AutoNotifyAttribute"))
                        {
                            Fields.Add(fieldSymbol);
                        }
                    }
                }
            }
        }
    }
}