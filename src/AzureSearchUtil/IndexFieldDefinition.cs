
namespace AzureSearchUtil
{
    public class IndexFieldDefinition
    {
        // ReSharper disable InconsistentNaming
        // NOTE: These naming convensions (e.g. name instead of Name) are required by the AzureSearch Rest API

        public string name;
        public string type;

        public bool? searchable = null;
        public bool? filterable = null;
        public bool? sortable = null;
        public bool? facetable  = null;
        public bool? suggestions  = null;
        public bool? key = null;
        public bool? retrievable = null;

        // ReSharper restore InconsistentNaming
    }
}
