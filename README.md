#A .NET Client Library for AzureSearch

`PM> Install-Package azure-search-util`

The library provides strongly typed interfaces to manage indexes, map and upload contents from other sources (e.g. MongoDB) and execute search operations.


##Creating an index
First, we need to create an instance of the `AzureSearchService` class.

`var searchService = new AzureSearchService("api key", "search service url", "api-version");`

We can specify AzureSearch specific field properties using attributes on any .net POCO class.
 
<pre><code>
public class Content
{
    [FieldProperties(FieldOptions.Key)]
	[SourcePropertyName("_id")]
    public string Id { get; set; }

    [FieldProperties(FieldOptions.Searchable | FieldOptions.Suggestions | FieldOptions.Facetable)]
    public string Title { get; set; }

	...

	...
}
</code></pre>

Then, we can use the CreateIndex or CreateIndexAsync method to create the index.

`var result = searchService.CreateIndex(typeof(Content), "index-name");`

###Set naming convention
While we generally use pascal casing in .NET/C# classes, JavaScript/json objects often use the camel casing. We can use the `NamingConvension` attribute at the class level to set camel casing for all property names in the index.

<pre><code>
[NamingConvension(NamingConventions.CamelCase)]
public class Content
{
...
</code>
</pre>

Or, we could also use the `PropertyName` attribute to specify different name for a specific property.

<pre><code>
[PropertyName("thumb")]
public string Thumbnail { get; set; }
</code></pre>

###Mapping fields
Some no-sql databases (eg. MongoDB) supports nested objects. But AzureSearch supports only few primitive types like string, interger, etc. Also, it only supports array of string types. The full list of supported data types can be found [here](msdn.microsoft.com/en-us/library/azure/dn798938.aspx).

We can map these properties using the `SourcePropertyName` attribute.

<pre>
<code>
[SourcePropertyName("desc.img.short")]
public string Thumbnail { get; set; }
The data structure in MongoDB looks like this:
</code>
</pre>

The same attribute can be used to map array type properties that maps to an array inside a nested object.

<pre>
<code>
[SourcePropertyName("source.presenters.title")]
public string[] Presenters { get; set; }
</code>
</pre>

##Loading data
From the performance point of view, inserting document in batch is better than inserting one document at a time. The `AddContent(...)` method takes a list of document and send all of them in a single REST call.

AzureSearch service allows up to 1000 documents in a single batch update. However, we should decide the batch size based on the size of our documents.

For example, the following code reads data from MongoDB database and adds 10 items at a time.
<pre>
<code>
var mongoDbDocuments = GetData();
var itemsToUpload = new List &lt;object>();

int count = 1;
foreach (var doc in mongoDbDocuments)
{
    count++;
    var item = new Content();
    doc.FillObject(item);
    itemsToUpload.Add(item);

    if (count > 10)
    {
        searchService.AddContent("index-name", itemsToUpload);

        itemsToUpload.Clear();
        count = 0;
    }
}

// Upload any remaining items
if (itemsToUpload.Count > 0)
{
    searchService.AddContent(testIndexName, itemsToUpload);
}
</code>
</pre>

## Executing Search
Although its not required, but its recommended that we create a `SearchResultItem` class to get the results.
<pre><code>
public class SearchResultItem : Content
{
    [JsonProperty(PropertyName = "@search.score")]
    public string SearchScore { get; set; }

    [JsonProperty(PropertyName = "@search.highlights")]
    public Dictionary &lt;string, List&lt;string>> SearchHighlights { get; set; }
}
</code></pre>

We can run search queries using the `Search(...)` method.

`var searchResult = searchService.Search<SearchResultItem>(testIndexName, "Windows 8");`

## Delete an index
Similar to creating index method, we could also delete an index:

`searchService.DeleteIndex(testIndexName);`

####Author's Note:
The library is in the early stage of development. I am still adding new functionalities. Please let me know if you are looking for any specific feature or find any bugs.

**nugget:**
PM> Install-Package azure-search-util

**blog:**
http://shahed.net

**code samples:**
master/demo/AzureSearchUtil.Demo
