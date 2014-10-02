using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace AzureSearchUtil
{
    public static class JsonObjectExtensions
    {
        public static void FillObject(this JObject sourceObj, object objToFill)
        {
            var props = objToFill.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var sourceFieldName = propertyInfo.GetSourceFieldName();

                if (sourceObj[sourceFieldName] != null)
                {
                    var val = sourceObj[sourceFieldName];

                    if (val != null)
                    {
                        objToFill.SetValue(propertyInfo, val);
                    }
                }
                else if (sourceFieldName.Contains("."))
                {
                    var propNames = sourceFieldName.Split(".".ToCharArray());

                    var propValue = GetValue(sourceObj, propNames);
                    if (propValue != null)
                    {
                        objToFill.SetValue(propertyInfo, propValue);
                    }
                }
            }
        }

        private static object GetValue(JObject doc, string[] propNames)
        {
            if (propNames.Length > 0)
            {
                var propName = propNames[0];

                var newPropNames = new string[propNames.Length - 1];
                for (var i = 1; i < propNames.Length; i++)
                {
                    newPropNames[i - 1] = propNames[i];
                }

                if (doc[propName] == null)
                    return null;

                if (doc[propName].Type == JTokenType.Array)
                {
                    // Since AzureSearch only supports string collections
                    var list = new List<string>();

                    var arr = doc[propName] as JArray;
                    foreach (var val in arr)
                    {
                        if (val.Type == JTokenType.Object)
                        {
                            var item = GetValue(val as JObject, newPropNames).ToString();
                            list.Add(item);
                        }
                        else if (val.Type == JTokenType.Array)
                        {
                            throw new Exception("Unsupported data structure");
                        }
                        else
                        {
                            list.Add(val.ToString());
                        }
                    }

                    return list.ToArray();
                }
                else
                {
                    if (doc[propName].Type == JTokenType.Object)
                    {
                        var bsonDoc = doc[propName] as JObject;
                        return GetValue(bsonDoc, newPropNames);
                    }
                    else
                    {
                        return doc[propName];
                    }
                }

            }
            else
            {
                return string.Empty;
            }
        }



    }
}
