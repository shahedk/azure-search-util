using System;

namespace AzureSearchUtil.Attributes
{
    public class ReplaceValueAttribute : Attribute
    {
        private readonly string[] _values;

        public ReplaceValueAttribute(params string[] values)
        {
            _values = values;
        }

        public string[] Values
        {
            get { return _values; }
        }
    }
}
