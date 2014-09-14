using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureSearchUtil.Attributes;
using Newtonsoft.Json;

namespace AzureSearchUtil.Test
{
    [NamingConvension(NamingConventions.CamelCase)]
    public class TestDocument
    {
        [FieldProperties(FieldOptions.Key)]
        [SourcePropertyName("_id")]
        public string Id { get; set; }

        // Text descriptions
        [FieldProperties(FieldOptions.Searchable | FieldOptions.Retrievable | FieldOptions.Sortable | FieldOptions.Suggestions)]
        [FieldName("title")]
        public string Title { get; set; }


        public string Description { get; set; }

        public string[] Labels { get; set; }
        public string Thumbnail { get; set; }

        [ReplaceValue("free=0;none=0")]
        public int Cost { get; set; }

        public DateTime PubDate { get; set; }
        public DateTimeOffset StartDate { get; set; }

        public string[] DownloadUrls { get; set; }
    }

    public class SearchResultItem : TestDocument
    {
        [JsonProperty(PropertyName = "@search.score")]
        public string SearchScore { get; set; }

        [JsonProperty(PropertyName = "@search.highlights")]
        public Dictionary<string, List<string>> SearchHighlights { get; set; }
    }

}
