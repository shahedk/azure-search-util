using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace AzureSearchUtil.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string testIndexName = "contents";

            // 
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);

            /* 
             * Create the index in AzureSearch service
             */
            //var result = searchService.CreateIndex(typeof(Content), testIndexName);


            /*
             * Loading data into Azure in batch (batch size of 4 documents max)
             */

            var mongoDbDocuments = GetData();

            var itemsToUpload = new List<object>();
            foreach (var doc in mongoDbDocuments)
            {
                var item = new Content();
                doc.FillObject(item);

                itemsToUpload.Add(item);
            }
            searchService.AddContent(testIndexName, itemsToUpload);

            Thread.Sleep(1000);

            var count = searchService.GetCount(testIndexName);

            searchService.DeleteIndex(testIndexName);
        }

        private static IEnumerable<BsonDocument> GetData()
        {
            var jsonData = File.ReadAllText(@"..\..\data.json");
            var records = (List<ExpandoObject>)JsonConvert.DeserializeObject(jsonData, typeof(List<ExpandoObject>));

            return records.Select(record => new BsonDocument(record)).ToList();
        }
    }
}
