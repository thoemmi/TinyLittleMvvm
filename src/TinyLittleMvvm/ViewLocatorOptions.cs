using System;

namespace TinyLittleMvvm {
    /// <summary>
    /// Provides options specifying how TinyLittleMvvm can resolve the type of a view for a given view model.
    /// </summary>
    public class ViewLocatorOptions {
        /// <summary>
        /// The function to get the type of a view for a given view model type.
        /// </summary>
        /// <remarks>
        /// By default it takes the full name of the view model type, calls <see cref="GetViewTypeNameFromViewModelTypeName"/>
        /// and gets a type with the resulting name from the IoC container.
        /// E.g. if <see cref="GetViewTypeNameFromViewModelTypeName"/> is not changed, for type <em>MyApp.ViewModels.MyViewModel</em>
        /// it will return the type <em>MyApp.Views.MyView</em>
        /// </remarks>
        public Func<Type, Type> GetViewTypeFromViewModelType;

        /// <summary>
        /// This function returns for the full name of a view model type the corresponding name of the view type.
        /// </summary>
        /// <remarks>
        /// By default, this function simply replaces "ViewModel" with "View", e.g. for "MyApp.ViewModels.MyViewModel" it returns "MyApp.Views.MyView"
        /// </remarks>
        public Func<string, string> GetViewTypeNameFromViewModelTypeName;

        /// <summary>
        /// Creates a new <see cref="ViewLocatorOptions"/> instance.
        /// </summary>
        public ViewLocatorOptions()
        {
            GetViewTypeNameFromViewModelTypeName = viewModelTypeName => viewModelTypeName.Replace("ViewModel", "View");
            GetViewTypeFromViewModelType = type => {
                var viewModelTypeName = type.FullName;
                var viewTypeName = GetViewTypeNameFromViewModelTypeName(viewModelTypeName);
                var viewType = type.Assembly.GetType(viewTypeName);
                return viewType;
            };
        }
    }
}