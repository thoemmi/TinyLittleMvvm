using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace TinyLittleMvvm {
    /// <summary>
    /// A base class for <see cref="DialogViewModel"/> providing validation.
    /// </summary>
    public abstract class ValidationPropertyChangedBase : PropertyChangedBase, INotifyDataErrorInfo {
        private readonly Dictionary<string, List<string>> _errors = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<ValidationRule>> _validationRules = new();

        private class ValidationRule {
            public string ErrorMessage { get; set; } = null!;
            public Func<bool> IsValid { get; set; } = null!;
        }

        /// <summary>
        /// Adds a rule for validation.
        /// </summary>
        /// <param name="property">The property to validate.</param>
        /// <param name="validate">The function to validate the value of the property.</param>
        /// <param name="errorMsg">The error message if the validation fails.</param>
        /// <typeparam name="TProperty">The type of the property to validate.</typeparam>
        protected void AddValidationRule<TProperty>(Expression<Func<TProperty>> property, Func<TProperty, bool> validate, string errorMsg) {
            var propertyName = ExpressionHelper.GetMemberName(property);

            var rule = new ValidationRule {
                ErrorMessage = errorMsg,
                IsValid = () => {
                    var val = property.Compile()();
                    return validate(val);
                }
            };

            if (!_validationRules.TryGetValue(propertyName, out List<ValidationRule>? ruleSet)) {
                ruleSet = new List<ValidationRule>();
                _validationRules.Add(propertyName, ruleSet);
            }
            ruleSet.Add(rule);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <returns>
        /// The validation errors for the property or entity.
        /// </returns>
        /// <param name="propertyName">The name of the property to retrieve validation errors for;
        /// or null or <see cref="F:System.String.Empty"/>, to retrieve entity-level errors.</param>
        public IEnumerable GetErrors(string? propertyName) => 
            _errors.TryGetValue(propertyName ?? string.Empty, out var propertyErrors)
                ? propertyErrors
                : Enumerable.Empty<string>();

        /// <summary>
        /// Raises the <see cref="PropertyChangedBase.PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected override void NotifyOfPropertyChange(string? propertyName = null) {
            base.NotifyOfPropertyChange(propertyName);

            if (propertyName == null || string.IsNullOrEmpty(propertyName)) {
                ValidateAllRules();
            } else {
                if (_validationRules.TryGetValue(propertyName, out List<ValidationRule>? ruleSet)) {
                    foreach (var rule in ruleSet) {
                        if (rule.IsValid()) {
                            RemoveError(propertyName, rule.ErrorMessage);
                        } else {
                            AddError(propertyName, rule.ErrorMessage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Runs all validation rules.
        /// </summary>
        /// <returns><see langword="true"/> if all rules are valid; otherwise, <see langword="false"/>.</returns>
        protected bool ValidateAllRules() {
            foreach (var ruleSet in _validationRules) {
                foreach (var rule in ruleSet.Value) {
                    if (rule.IsValid()) {
                        RemoveError(ruleSet.Key, rule.ErrorMessage);
                    } else {
                        AddError(ruleSet.Key, rule.ErrorMessage);
                    }
                }
            }
            return !HasErrors;
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <returns>
        /// true if the entity currently has validation errors; otherwise, false.
        /// </returns>
        public bool HasErrors => _errors.Any();

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// Called when the validation state of a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the validated property.</param>
        protected virtual void OnErrorsChanged(string propertyName) {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property's error state.
        /// </summary>
        /// <param name="isValid"><see langword="true"/> if the property is valid; otherwise, <see langword="false"/>.</param>
        /// <param name="property">The validated property.</param>
        /// <param name="error">The error message.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        protected void Validate<TProperty>(bool isValid, Expression<Func<TProperty>> property, string error) {
            Validate(isValid, ExpressionHelper.GetMemberName(property), error);
        }

        /// <summary>
        /// Sets a property's error state.
        /// </summary>
        /// <param name="isValid"><see langword="true"/> if the property is valid; otherwise, <see langword="false"/>.</param>
        /// <param name="propertyName">The name of the validated property.</param>
        /// <param name="error">The error message.</param>
        protected void Validate(bool isValid, string propertyName, string error) {
            if (isValid) {
                RemoveError(propertyName, error);
            } else {
                AddError(propertyName, error);
            }
        }

        /// <summary>
        /// Adds a validation error message for a property.
        /// </summary>
        /// <param name="property">The validated property.</param>
        /// <param name="error">The error message.</param>
        /// <typeparam name="TProperty">The type of the validated property.</typeparam>
        protected void AddError<TProperty>(Expression<Func<TProperty>> property, string error) {
            AddError(ExpressionHelper.GetMemberName(property), error);
        }

        /// <summary>
        /// Adds a validation error message for a property.
        /// </summary>
        /// <param name="propertyName">The name of the validated property.</param>
        /// <param name="error">The error message.</param>
        protected void AddError(string? propertyName, string error) {
            var safePropertyName = propertyName ?? string.Empty;
            if (!_errors.TryGetValue(safePropertyName, out List<string>? propertyErrors)) {
                propertyErrors = new List<string>();
                _errors.Add(safePropertyName, propertyErrors);
            }
            if (!propertyErrors.Contains(error)) {
                propertyErrors.Add(error);
                OnErrorsChanged(safePropertyName);
            }
        }

        /// <summary>
        /// Removes a validation error message for a property.
        /// </summary>
        /// <param name="property">The validated property.</param>
        /// <param name="error">The error message.</param>
        /// <typeparam name="TProperty">The type of the validated property.</typeparam>
        protected void RemoveError<TProperty>(Expression<Func<TProperty>> property, string error) {
            RemoveError(ExpressionHelper.GetMemberName(property), error);
        }

        /// <summary>
        /// Removes a validation error message for a property.
        /// </summary>
        /// <param name="propertyName">The name of the validated property.</param>
        /// <param name="error">The error message.</param>
        protected void RemoveError(string? propertyName, string error) {
            var safePropertyName = propertyName ?? string.Empty;
            if (_errors.TryGetValue(safePropertyName, out var propertyErrors)) {
                if (propertyErrors.Contains(error)) {
                    propertyErrors.Remove(error);
                    if (!propertyErrors.Any()) {
                        _errors.Remove(safePropertyName);
                    }
                    OnErrorsChanged(safePropertyName);
                }
            }
        }
    }
}