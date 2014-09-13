using System;

namespace AzureSearchUtil.Attributes
{
    public class SourceAttribute : Attribute
    {
        private readonly string _fieldName;

        public SourceAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }
    }
}
