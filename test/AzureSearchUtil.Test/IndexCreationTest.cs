using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AzureSearchUtil.Test
{
    [TestClass]
    public class IndexCreationTest
    {

        [TestMethod]
        public void CreateIndex()
        {
            const string testIndexName = "con92";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);

            // Test: verify index creation process
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            // Cleanup: delete the newly created index
            searchService.DeleteIndex(testIndexName);
        }

        [TestMethod]
        public void GetIndex()
        {
            const string testIndexName = "con94";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);

            // Prepare test env: create a new index
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            // Test: try to get the newly created index and verify structure
            var indexDefinitionInJson = searchService.GetIndex(testIndexName);
            var jsonObject = JsonConvert.DeserializeObject(indexDefinitionInJson, typeof(ExpandoObject));
            var data = jsonObject as IDictionary<string, object>;

            Assert.IsNotNull(data, "Failed to retrieve index information");

            // A valid response will contain the following six properties:
            // @odata.context, name, fields, scoringProfile, defaultScoringProfile, corsOptions
            Assert.IsTrue(data.Keys.Count == 6, "Failed to retrieve index information");

            // Cleanup: delete the newly created index
            searchService.DeleteIndex(testIndexName);
        }


        [TestMethod]
        public void DeleteIndex()
        {
            const string testIndexName = "con94";
            var searchService = new AzureSearchService(TestSettings.AzureSearchApiKey,
                    TestSettings.AzureSearchUrlPrefix, TestSettings.AzureSearchApiVersion);

            // Prepare test env

            // 1. create a new index
            var result = searchService.CreateIndex(typeof(TestDocument), testIndexName);
            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to create index. " + result.StatusCode);

            // Test: try to delete the newly created index
            var deleteResult = searchService.DeleteIndex(testIndexName);

            Assert.IsTrue(result.IsSuccessStatusCode, "Failed to delete index. " + result.StatusCode);
        }
    }
}
