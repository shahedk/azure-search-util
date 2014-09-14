using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureSearchUtil;
using AzureSearchUtil.Attributes;

namespace AzureSearchUtil.Demo
{
    [NamingConvension(NamingConventions.CamelCase)]
    public class Content
    {
        [FieldProperties(FieldOptions.Key)]
        [SourcePropertyName("_id")]
        public string Id { get; set; }

        [SourcePropertyName("source.presenters.title")]
        public string[] Presenters { get; set; }

        [FieldProperties(FieldOptions.Searchable | FieldOptions.Suggestions | FieldOptions.Facetable)]
        public string Title { get; set; }

        public string Url { get; set; }

[PropertyName("thumb")]
[SourcePropertyName("desc.img.short")]
public string Thumbnail { get; set; }
    }
}
