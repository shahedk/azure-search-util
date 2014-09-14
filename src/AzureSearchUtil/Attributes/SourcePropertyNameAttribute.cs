using System;

namespace AzureSearchUtil.Attributes
{
    public class SourcePropertyNameAttribute : Attribute
    {
        private readonly string _fieldName;

        public SourcePropertyNameAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }
    }
}
