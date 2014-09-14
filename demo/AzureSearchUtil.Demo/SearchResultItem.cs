using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureSearchUtil.Demo
{
public class SearchResultItem : Content
{
    [JsonProperty(PropertyName = "@search.score")]
    public string SearchScore { get; set; }

    [JsonProperty(PropertyName = "@search.highlights")]
    public Dictionary<string, List<string>> SearchHighlights { get; set; }
}
}
