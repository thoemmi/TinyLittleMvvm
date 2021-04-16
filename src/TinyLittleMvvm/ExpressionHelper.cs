using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TinyLittleMvvm {
    /// <summary>
    /// Extension methods for <see cref="Expression"/>s.
    /// </summary>
    public static class ExpressionHelper {
        /// <summary>
        /// Gets the name of a class' property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>The name of the property.</returns>
        public static string GetMemberName<TProperty>(Expression<Func<TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        /// <summary>
        /// Gets the name of a class' property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <typeparam name="TEntity">The type of the class owning the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>The name of the property.</returns>
        public static string GetMemberName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        private static MemberInfo GetMemberInfo(Expression expression) {
            var lambdaExpression = (LambdaExpression) expression;
            return
                (lambdaExpression.Body is UnaryExpression unaryExpression
                    ? (MemberExpression) unaryExpression.Operand
                    : (MemberExpression) lambdaExpression.Body
                )
                .Member;
        }
    }
}