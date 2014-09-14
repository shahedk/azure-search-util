using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using AzureSearchUtil.Attributes;
using AzureSearchUtil.Exceptions;
using Newtonsoft.Json;

namespace AzureSearchUtil
{

    public static class AzureSearchExtensions
    {
        /// <summary>
        /// Returns a Azure suported data type (eg. Edm.String) for .NET data types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetDataTypeOfAzureSearch(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }
            else if (type == typeof(string))
            {
                return "Edm.String";
            }
            else if (type == typeof(List<String>) || type == typeof(String[]))
            {
                return "Collection(Edm.String)";
            }
            else if (type == typeof(bool))
            {
                return "Edm.Boolean";
            }
            else if (type == typeof(int))
            {
                return "Edm.Int32";
            }
            else if (type == typeof(double) || type == typeof(float))
            {
                return "Edm.Double";
            }
            else if (type == typeof(DateTimeOffset) || type == typeof(DateTime))
            {
                return "Edm.DateTimeOffset";
            }
            else
            {
                // Unknown: default is string
                return "Edm.String";
            }
        }

        public static string ToIndexDefinition(this Type type, string indexName)
        {
            bool isKeyFieldSpecified = false;

            var index = new IndexDefinition() { name = indexName };

            var namingConvention = NamingConventions.None;
            var customAttributes = type.GetCustomAttributes(typeof(NamingConvensionAttribute), true);
            if (customAttributes.Length > 0)
            {
                namingConvention = ((NamingConvensionAttribute)customAttributes[0]).Convention;
            }

            // 1. Get list of all public properties
            // 2. Convert field names and get data types
            // 3. Create index definition
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var fieldDefObj = new ExpandoObject();
                var fieldDef = fieldDefObj as IDictionary<string, object>;

                fieldDef["type"] = GetDataTypeOfAzureSearch(propertyInfo.PropertyType);

                foreach (var attribute in propertyInfo.CustomAttributes)
                {
                    //if (attribute.AttributeType.Name == typeof(WeightAttribute).Name)
                    //{
                    //    var weight = float.Parse(attribute.ConstructorArguments[0].Value.ToString());
                    //    fieldDef["name"] = weight;
                    //}

                    if (attribute.AttributeType.Name == typeof(PropertyNameAttribute).Name)
                    {
                        var destinationFieldName = (string)attribute.ConstructorArguments[0].Value;
                        fieldDef["name"] = destinationFieldName;
                    }
                    else if (attribute.AttributeType.Name == typeof(FieldPropertiesAttribute).Name)
                    {
                        var indexAttrs = (FieldOptions)attribute.ConstructorArguments[0].Value;

                        if ((indexAttrs & FieldOptions.Searchable) == FieldOptions.Searchable)
                        {
                            fieldDef["searchable"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Filterable) == FieldOptions.Filterable)
                        {
                            fieldDef["filterable"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Sortable) == FieldOptions.Sortable)
                        {
                            fieldDef["sortable"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Facetable) == FieldOptions.Facetable)
                        {
                            fieldDef["facetable"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Suggestions) == FieldOptions.Suggestions)
                        {
                            fieldDef["suggestions"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Key) == FieldOptions.Key)
                        {
                            isKeyFieldSpecified = true;
                            fieldDef["key"] = true;
                        }
                        if ((indexAttrs & FieldOptions.Retrievable) == FieldOptions.Retrievable)
                        {
                            fieldDef["retrievable"] = true;
                        }
                    }
                }

                if (!fieldDef.ContainsKey("name"))
                {
                    if (namingConvention == NamingConventions.CamelCase)
                    {
                        fieldDef["name"] = propertyInfo.Name.ToCamelCase();
                    }
                    else if (namingConvention == NamingConventions.PascalCase)
                    {
                        fieldDef["name"] = propertyInfo.Name.ToPascalCase();
                    }
                    else
                    {
                        fieldDef["name"] = propertyInfo.Name;
                    }
                }

                index.fields.Add(fieldDefObj);
            }

            if (!isKeyFieldSpecified)
            {
                throw new InvalidSchemaException("There must be one Key field. Please use [FieldProperties(FieldOptions.Key)] attribute to specify the key field.");
            }

            // 4. Serialize into JSON
            return JsonConvert.SerializeObject(index);

        }

        public static ExpandoObject ToFieldDefinition(this IndexFieldDefinition def)
        {
            var item = new ExpandoObject();

            return item;
        }

        public static bool IsKeyField(this PropertyInfo self)
        {
            var attr = self.GetCustomAttribute(typeof(FieldPropertiesAttribute)) as FieldPropertiesAttribute;

            return (attr != null && ((attr.Attributes & FieldOptions.Key) == FieldOptions.Key));
        }

        public static ExpandoObject ToSearchIndexItem(this object obj)
        {
            var item = new ExpandoObject();
            var itemDic = item as IDictionary<string, Object>;

            var properties = obj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var name = propertyInfo.Name.ToCamelCase();
                var value = propertyInfo.GetValue(obj);

                if (value == null)
                {
                    if (propertyInfo.IsKeyField())
                    {
                        throw new Exception("Key field cannot be null!");
                    }
                }
                else
                {
                    if (value is DateTime)
                    {
                        value = new DateTimeOffset((DateTime)value);
                    }

                    itemDic.Add(name, value);
                }
            }

            return item;
        }
    }
}
