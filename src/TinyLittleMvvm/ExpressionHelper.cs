using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TinyLittleMvvm {
    public static class ExpressionHelper {
        public static string GetMemberName<TProperty>(Expression<Func<TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        public static string GetMemberName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        private static MemberInfo GetMemberInfo(Expression expression) {
            var lambdaExpression = (LambdaExpression) expression;
            return
                (!(lambdaExpression.Body is UnaryExpression)
                    ? (MemberExpression) lambdaExpression.Body : (MemberExpression) ((UnaryExpression) lambdaExpression.Body).Operand)
                    .Member;
        }
    }
}