using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace TinyLittleMvvm {
    public abstract class ValidationPropertyChangedBase : PropertyChangedBase, INotifyDataErrorInfo {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<ValidationRule>> _validationRules = new Dictionary<string, List<ValidationRule>>();

        private class ValidationRule {
            public string ErrorMessage { get; set; }
            public Func<bool> IsValid { get; set; }
        }

        protected void AddValidationRule<TProperty>(Expression<Func<TProperty>> property, Func<TProperty, bool> validate, string errorMsg) {
            var propertyName = GetMemberName(property);

            var rule = new ValidationRule {
                ErrorMessage = errorMsg,
                IsValid = () => {
                    var val = property.Compile()();
                    return validate(val);
                }
            };

            List<ValidationRule> ruleSet;
            if (!_validationRules.TryGetValue(propertyName, out ruleSet)) {
                ruleSet = new List<ValidationRule>();
                _validationRules.Add(propertyName, ruleSet);
            }
            ruleSet.Add(rule);
        }

        public IEnumerable GetErrors(string propertyName) {
            if (!String.IsNullOrEmpty(propertyName)) {
                List<string> propertyErrors;
                return _errors.TryGetValue(propertyName, out propertyErrors) ? propertyErrors : null;
            } else {
                return _errors.SelectMany(err => err.Value.ToList());
            }
        }

        protected override void NotifyOfPropertyChange(string propertyName = null) {
            base.NotifyOfPropertyChange(propertyName);

            if (String.IsNullOrEmpty(propertyName)) {
                ValidateAllRules();
            } else {
                List<ValidationRule> ruleSet;
                if (_validationRules.TryGetValue(propertyName, out ruleSet)) {
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

        public bool HasErrors {
            get { return _errors.Any(); }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnErrorsChanged(string propertyName) {
            var handler = ErrorsChanged;
            if (handler != null) {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        protected void Validate<TProperty>(bool isValid, Expression<Func<TProperty>> property, string error) {
            Validate(isValid, GetMemberName(property), error);
        }

        protected void Validate(bool isValid, string propertyName, string error) {
            if (isValid) {
                RemoveError(propertyName, error);
            } else {
                AddError(propertyName, error);
            }
        }

        protected void AddError<TProperty>(Expression<Func<TProperty>> property, string error) {
            AddError(GetMemberName(property), error);
        }

        protected void AddError(string propertyName, string error) {
            List<string> propertyErrors;
            if (!_errors.TryGetValue(propertyName, out propertyErrors)) {
                propertyErrors = new List<string>();
                _errors.Add(propertyName, propertyErrors);
            }
            if (!propertyErrors.Contains(error)) {
                propertyErrors.Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void RemoveError<TProperty>(Expression<Func<TProperty>> property, string error) {
            RemoveError(GetMemberName(property), error);
        }

        protected void RemoveError(string propertyName, string error) {
            List<string> propertyErrors;
            if (_errors.TryGetValue(propertyName, out propertyErrors)) {
                if (propertyErrors.Contains(error)) {
                    propertyErrors.Remove(error);
                    if (!propertyErrors.Any()) {
                        _errors.Remove(propertyName);
                    }
                    OnErrorsChanged(propertyName);
                }
            }
        }
    }
}