using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace TinyLittleMvvm {
    public abstract class PropertyChangedBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Refresh() {
            CommandManager.InvalidateRequerySuggested();
            NotifyOfPropertyChange(String.Empty);
        }

        protected virtual void NotifyOfPropertyChange(string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property) {
            NotifyOfPropertyChange(GetMemberName(property));
        }

        public static string GetMemberName<TProperty>(Expression<Func<TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        public static string GetMemberName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property) {
            return GetMemberInfo(property).Name;
        }

        protected static MemberInfo GetMemberInfo(Expression expression) {
            var lambdaExpression = (LambdaExpression)expression;
            return
                (!(lambdaExpression.Body is UnaryExpression)
                    ? (MemberExpression)lambdaExpression.Body : (MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand)
                    .Member;
        }
    }
}