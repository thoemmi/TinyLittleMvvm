using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TinyLittleMvvm {
    /// <summary>
    /// Base class implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Set fully implements a Setter for a read-write property,
        /// using CallerMemberName to raise the notification
        /// and the ref to the backing field to set the property.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="backingField">A Reference to the backing field for this property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property, usually automatically provided through the CallerMemberName attribute.</param>
        /// <returns>True if the PropertyChanged event was raised, false otherwise.</returns> 
        public bool Set<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue)) {
                return false;
            }

            backingField = newValue;
            NotifyOfPropertyChange(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies clients that all properties may have changed.
        /// </summary>
        /// <remarks>
        /// This method raises the <see cref="PropertyChanged"/> event with <see cref="String.Empty"/> as the property name.
        /// </remarks>
        protected void Refresh() {
            CommandManager.InvalidateRequerySuggested();
            NotifyOfPropertyChange(String.Empty);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property">The changed property.</param>
        /// <typeparam name="TProperty">The type of the changed property.</typeparam>
        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property) {
            NotifyOfPropertyChange(ExpressionHelper.GetMemberName(property));
        }
    }
}