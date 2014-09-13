using System;

namespace AzureSearchUtil.Attributes
{
    public class FieldNameAttribute : Attribute
    {
        private readonly string _fieldName;

        public FieldNameAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }
    }
}
