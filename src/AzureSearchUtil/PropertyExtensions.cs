using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using AzureSearchUtil.Attributes;
using Newtonsoft.Json;

namespace AzureSearchUtil
{
    public static class PropertyExtensions
    {
       
        public static string GetSourceFieldName(this MemberInfo info)
        {
            var propName = typeof(SourcePropertyNameAttribute).Name;
            var sourceFieldName = string.Empty;

            foreach (var attr in info.CustomAttributes.Where(attr => attr.AttributeType.Name == propName))
            {
                sourceFieldName = (string)attr.ConstructorArguments[0].Value;
                break;
            }

            if (string.IsNullOrWhiteSpace(sourceFieldName))
            {
                sourceFieldName = info.Name.ToCamelCase();
            }

            return sourceFieldName;
        }

        public static void SetValue(this object self, PropertyInfo info, object value)
        {
            if (info != null)
            {
                info.SetValue(self, ChangeType(info, value));
            }
        }

        private static object ChangeType(PropertyInfo info, object value)
        {
            try
            {
                return Convert.ChangeType(value, info.PropertyType);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
