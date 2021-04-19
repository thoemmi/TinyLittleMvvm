using System;
using System.Collections.Generic;

namespace TinyLittleMvvm {
    internal class ViewModelRegistry
    {
        public Dictionary<Type, Type> ViewModelTypeTo2ViewType { get; } = new();
    }
}