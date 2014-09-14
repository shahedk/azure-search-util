using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace AzureSearchUtil
{
    public static class MongoDbPropertyExtensions
    {

        public static void FillObject(this BsonDocument doc, object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var sourceFieldName = propertyInfo.GetSourceFieldName();

                if (doc.Contains(sourceFieldName))
                {
                    var val = doc[sourceFieldName].RawValue;

                    if (val != null)
                    {
                        obj.SetValue(propertyInfo, val);
                    }
                }
                else if (sourceFieldName.Contains("."))
                {
                    var propNames = sourceFieldName.Split(".".ToCharArray());

                    var propValue = GetValue(doc, propNames);
                    if (propValue != null)
                    {
                        obj.SetValue(propertyInfo, propValue);
                    }
                }
            }
        }

        private static object GetValue(BsonDocument doc, string[] propNames)
        {
            if (propNames.Length > 0)
            {
                var propName = propNames[0];

                var newPropNames = new string[propNames.Length - 1];
                for (var i = 1; i < propNames.Length; i++)
                {
                    newPropNames[i-1] = propNames[i];
                }

                if (!doc.Contains(propName))
                    return null;

                if (doc[propName].IsBsonArray)
                {
                    // NOTE: We know AzureSearch only supports string collections
                    var list = new List<string>();

                    var arr = doc[propName].AsBsonArray;
                    foreach (var val in arr)
                    {
                        if (val.IsBsonDocument)
                        {
                            var item = GetValue(val.AsBsonDocument, newPropNames).ToString();
                            list.Add(item);
                        }
                        else
                        {
                            list.Add(val.AsString);
                        }
                    }

                    return list.ToArray();
                }
                else
                {
                    if (doc[propName].IsBsonDocument)
                    {
                        var bsonDoc = doc[propName].AsBsonDocument;
                        return GetValue(bsonDoc, newPropNames);
                    }
                    else
                    {
                        return doc[propName].RawValue;
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
