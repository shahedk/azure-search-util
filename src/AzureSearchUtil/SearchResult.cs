using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSearchUtil
{
    public class SearchResult<T>
    {
        // ReSharper disable once InconsistentNaming
        public List<T> value = new List<T>();
    }
}
