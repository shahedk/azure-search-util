using System.Collections.Generic;
using System.Dynamic;

namespace AzureSearchUtil 
{
// ReSharper disable InconsistentNaming
// NOTE: These naming convensions (e.g. name instead of Name) are required by the AzureSearch Rest API

    public class IndexDefinition
    {
        public string name;
        public List<ExpandoObject> fields = new List<ExpandoObject>();
    }

    // ReSharper restore InconsistentNaming
}
