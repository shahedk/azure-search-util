using System;

namespace AzureSearchUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FieldPropertiesAttribute : Attribute
    {
        private readonly FieldOptions _attributes;

        public FieldOptions Attributes
        {
            get { return _attributes; }
        }

        public FieldPropertiesAttribute(FieldOptions attributes)
        {
            _attributes = attributes;
        }
    }


}
