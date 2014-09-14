using System;

namespace AzureSearchUtil.Attributes
{
    public class PropertyNameAttribute : Attribute
    {
        private readonly string _fieldName;

        public PropertyNameAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }
    }
}
