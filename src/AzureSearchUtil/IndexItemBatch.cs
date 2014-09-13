using System.Collections.Generic;

namespace AzureSearchUtil
{
    public class IndexItemBatch
    {
        // ReSharper disable InconsistentNaming
        // NOTE: These naming convensions (e.g. name instead of Name) are required by the AzureSearch Rest API

        public List<object>  value = new List<object>();

        // ReSharper restore InconsistentNaming 
    }
}

