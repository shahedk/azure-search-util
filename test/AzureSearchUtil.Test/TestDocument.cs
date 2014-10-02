using System.Collections.Generic;
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

        [SourcePropertyName("source.presenters.title")]
        public string[] Presenters { get; set; }

        [FieldProperties(FieldOptions.Searchable | FieldOptions.Suggestions | FieldOptions.Facetable)]
        public string Title { get; set; }

        public string Url { get; set; }

        [SourcePropertyName("desc.img.short")]
        public string Thumbnail { get; set; }
    }

    public class SearchResultItem : TestDocument
    {
        [JsonProperty(PropertyName = "@search.score")]
        public string SearchScore { get; set; }

        [JsonProperty(PropertyName = "@search.highlights")]
        public Dictionary<string, List<string>> SearchHighlights { get; set; }
    }

}
