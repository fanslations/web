using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public class PropertyMapper<T,T1>
    {
        T FromModel { get; set; }
        T1 ToModel { get; set; }
        public PropertyMapper(T fromModel, T1 toModel)
        {
            FromModel = fromModel;
            ToModel = toModel;
        }
        public void Map(string inlineEditProperty = null)
        {
            var fromType = FromModel.GetType();
            var fromProperties = fromType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            var toType = ToModel.GetType();
            var toProperties = toType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // if include list is not null then only update property in the list
            if (!string.IsNullOrWhiteSpace(inlineEditProperty))
            {
                toProperties = toProperties.Where(w => w.Name == inlineEditProperty).ToArray();
            }

            foreach (var toPropertyInfo in toProperties)
            {
                var fromPropertyInfo = fromProperties.SingleOrDefault(w => w.Name == toPropertyInfo.Name);
                if (fromPropertyInfo == null) continue;

                HandleBool(toPropertyInfo, fromPropertyInfo.GetValue(FromModel, null));
                HandleInt(toPropertyInfo, fromPropertyInfo.GetValue(FromModel, null));
                HandleString(toPropertyInfo, fromPropertyInfo.GetValue(FromModel, null));
                HandleDateTime(toPropertyInfo, fromPropertyInfo.GetValue(FromModel, null));
            }

        }

        void HandleBool(PropertyInfo propertyInfo, object newValue)
        {
            if (propertyInfo.PropertyType != typeof(bool)) return;

            // pre-process value

            propertyInfo.SetValue(ToModel, (bool)newValue, null);
        }

        void HandleInt(PropertyInfo propertyInfo, object newValue)
        {
            if (propertyInfo.PropertyType != typeof (int)) return;

            // pre-process value
            var value = (int) newValue;
            if (value == 0) return; // don't set value fonr int if its 0

            propertyInfo.SetValue(ToModel, (int)newValue, null);
        }

        void HandleString(PropertyInfo propertyInfo, object newValue)
        {
            if (propertyInfo.PropertyType != typeof(string)) return;

            // pre-process value
            var value = (string) newValue;
            if (string.IsNullOrEmpty(value))
                value = DefaultValue.String;
            value = value.Trim();

            propertyInfo.SetValue(ToModel, value, null);
        }

        void HandleDateTime(PropertyInfo propertyInfo, object newValue)
        {
            if (propertyInfo.PropertyType != typeof(DateTime)) return;

            // pre-process value
            var value = (DateTime)newValue;
            if (value == DateTime.MinValue)
                value = DefaultValue.DateTime;

            propertyInfo.SetValue(ToModel, value, null);
        }

    }
}
