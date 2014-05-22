using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using Splat;

namespace TestApp.BindingTypeConverters
{
    class ImageSourceBindingConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            return fromType == typeof(IBitmap) ? 100 : 0;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if(from == null)
            {
                result = null;
                return true;
            }
            result = ((IBitmap)from).ToNative();
            return true;
        }
    }
}
