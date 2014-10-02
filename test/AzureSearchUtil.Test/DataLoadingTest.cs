using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AzureSearchUtil.Test
{
    [TestClass]
    public class DataLoadingTest : TestBase
    {
        [TestMethod]
        public void InsertDocumentInBulk()
        {
            const string testIndexName = "con0";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);
            
            // Prepare test env: create a new index
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            try
            {
                var itemsToAdd = GetData(2);
                searchService.AddContent(testIndexName, itemsToAdd);

                // Wait a bit, give AzureSearch some time to process
                Thread.Sleep(1000);

                // Test: try to get document count
                var count = searchService.GetCount(testIndexName);
                Assert.IsTrue(count >= 1, "Unable to get item count. ");
            }
            finally
            {
                // Cleanup: delete the newly created index
                searchService.DeleteIndex(testIndexName);
            }
        }

        [TestMethod]
        public void DeleteDocument()
        {
            const string testIndexName = "con12";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);
            
            // Prepare test env: create a new index
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            try
            {
                // Prepare test env (2): insert a document

                var itemsToAdd = GetData();
                searchService.AddContent(testIndexName, itemsToAdd);

                // Wait a bit, give AzureSearch some time to process
                Thread.Sleep(1000);

                // Test: Delete 
                var count = searchService.GetCount(testIndexName);
                Assert.IsTrue(count >= 1, "Unable to get item count. ");
            }
            finally
            {
                // Cleanup: delete the newly created index
                searchService.DeleteIndex(testIndexName);
            }
        }

        [TestMethod]
        public void Search()
        {
            const string testIndexName = "con12";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);
            
            // Prepare test env: create a new index
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            try
            {
                // Prepare test env (2): insert a document

                var itemsToAdd = GetData();
                searchService.AddContent(testIndexName, itemsToAdd);

                // Wait a bit, give AzureSearch some time to process
                Thread.Sleep(1000);

                // Test: Search  
                var searchResult = searchService.Search<SearchResultItem>(testIndexName, "Azure Portal");
                
                Assert.IsTrue(searchResult.value.Count > 0, "Failed to get search results!");
            }
            finally
            {
                // Cleanup: delete the newly created index
                searchService.DeleteIndex(testIndexName);
            }
        }
    }

}
