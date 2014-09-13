using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSearchUtil.Attributes
{
    public class NamingConvensionAttribute: Attribute
    {
        private readonly NamingConventions _convention;

        public NamingConvensionAttribute(NamingConventions convention)
        {
            _convention = convention;
        }

        public NamingConventions Convention
        {
            get { return _convention; }
        }
    }
}
