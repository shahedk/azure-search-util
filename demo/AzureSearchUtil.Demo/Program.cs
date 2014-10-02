using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureSearchUtil.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string testIndexName = "contents";

            // 1. Create an instance of the service to communicate with AzureSearch service
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey, TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);
            searchService.DeleteIndex(testIndexName);
            // 2. Create the index and check status
            var result = searchService.CreateIndex(typeof(Content), testIndexName);
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Index: {0} was created successfully.", testIndexName);
            }
            else
            {
                var task = result.Content.ReadAsStringAsync();
                task.Wait();

                var errorMessage = task.Result;
                Console.WriteLine();
            }

            //3. Prepare the list of documents/items to upload and upload in batch
            var jsonDocuments = GetData();
            var itemsToUpload = new List<object>();

            int count = 1;
            foreach (var doc in jsonDocuments)
            {
                count++;
                var item = new Content();

                (doc as JObject).FillObject(item);
                itemsToUpload.Add(item);

                if (count > TestSettings.AzureSearchBatchUpdateLimit)
                {
                    searchService.AddContent(testIndexName, itemsToUpload);

                    itemsToUpload.Clear();
                    count = 0;
                }
            }

            // Upload any remaining items
            if (itemsToUpload.Count > 0)
            {
                searchService.AddContent(testIndexName, itemsToUpload);
            }

            // 5. Check total document count
            //    Depending on service tier and server load, azure service may take some time 
            //    to process the uploaded data. Let's wait for 1 sec before checking the count
            Thread.Sleep(1000);

            var totalDocuments = searchService.GetCount(testIndexName);
            Console.WriteLine("Total documents in index: " + totalDocuments);
            Console.WriteLine("");

            // 6. Run a search query
            var searchResult = searchService.Search<SearchResultItem>(testIndexName, "Windows 8");
            Console.WriteLine("Search result for term: Windows 8");
            foreach (SearchResultItem item in searchResult.value)
            {
                Console.WriteLine("Title: {0} \nScore: {1}\n", item.Title, item.SearchScore);
            }

            // Uncomment this line if you like to delete the index
            // searchService.DeleteIndex(testIndexName);


            Console.Read();
        }

        /// <summary>
        /// Instead of getting the data from MongoDB database, it returns data from file in a similar format as MongoDB clients.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<JToken> GetData()
        {
            var jsonData = File.ReadAllText(@"..\..\data.json");
            return JArray.Parse(jsonData);
        }
    }
}
