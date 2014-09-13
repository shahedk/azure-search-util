using System;

namespace AzureSearchUtil
{
    [Flags]
    public enum FieldOptions
    {
        /// <summary>
        /// Default: true [only Edm.String and Collection(Edm.String) fields can be searchabl]
        /// </summary>
        Searchable = 1,

        /// <summary>
        /// Default: true [Collection(Edm.String) fields cannot be sortable]
        /// </summary>
        Sortable = 2,

        /// <summary>
        /// Default: true [Edm.GeographyPoint fields cannot be facetable]
        /// </summary>
        Facetable = 4,

        /// <summary>
        /// Default: false [only Edm.String and Collection(Edm.String) fields can be used for suggestions]
        /// </summary>
        Suggestions = 8,

        /// <summary>
        /// Default: false [only Edm.String fields can be keys]
        /// </summary>
        Key = 16,

        /// <summary>
        /// Default: true
        /// </summary>
        Retrievable = 32,

        /// <summary>
        /// Default: true
        /// </summary>
        Filterable = 64
    }
}