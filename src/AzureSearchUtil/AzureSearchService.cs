using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureSearchUtil
{
    public class AzureSearchService
    {
        private HttpClient _client;
        private HttpClient Client
        {
            get
            {
                if (_client != null) return _client;

                _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("api-key", _apiKey);
                _client.DefaultRequestHeaders.Add("Accept", "application/json");

                return _client;
            }
        }

        private readonly string _apiKey;
        private readonly string _serviceUrl;
        private readonly string _apiVersion;

        public AzureSearchService(string apiKey, string serviceUrl, string apiVersion)
        {
            _apiKey = apiKey;
            _serviceUrl = serviceUrl;
            _apiVersion = apiVersion;
        }

        public async Task<HttpResponseMessage> CreateIndexAsync(Type type, string indexName)
        {
            const string urlFormat = "{0}/indexes/?api-version={1}";
            var indexDef = type.ToIndexDefinition(indexName);

            var url = string.Format(urlFormat, _serviceUrl, _apiVersion);

            var response = await Client.PostAsync(url, new StringContent(indexDef, Encoding.UTF8, "application/json"));

            return response;
        }

        public HttpResponseMessage CreateIndex(Type type, string indexName)
        {
            var task = CreateIndexAsync(type, indexName);
            task.Wait();

            return task.Result;
        }

        public async Task<string> GetCountAsync(string indexName)
        {
            const string urlFormat = "{0}/indexes/{1}/docs/$count?api-version={2}";
            var url = string.Format(urlFormat, _serviceUrl, indexName, _apiVersion);

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var task = await response.Content.ReadAsStringAsync();

            return task;
        }

        public int GetCount(string indexName)
        {
            var task = GetCountAsync(indexName);
            task.Wait();

            return int.Parse(task.Result);
        }


        public async Task<string> SearchAsync(string indexName, string searchText, uint pageSize = 10, uint pageNo = 1)
        {
            uint skip = 0;
            if (pageNo > 0)
            {
                skip = (pageNo - 1) * pageSize;
            }
            const string urlFormat = "{0}/indexes/{1}/docs?api-version={2}&search={3}&$top={4}&$skip={5}";
            var url = string.Format(urlFormat, _serviceUrl, indexName, _apiVersion, searchText, pageSize, skip);

            var response = await Client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public string Search(string indexName, string searchText, uint pageSize = 10, uint pageNo = 1)
        {
            var task = SearchAsync(indexName, searchText, pageSize, pageNo);
            task.Wait();

            return task.Result;

        }

        public SearchResult<T> SearchAsync<T>(string indexName, string searchText, uint pageSize = 10, uint pageNo = 1)
        {
            var json = SearchAsync(indexName, searchText, pageSize, pageNo).Result;
            var result = (SearchResult<T>)JsonConvert.DeserializeObject(json, typeof(SearchResult<T>));

            return result;
        }

        public SearchResult<T> Search<T>(string indexName, string searchText, uint pageSize = 10, uint pageNo = 1)
        {
            var json = Search(indexName, searchText, pageSize, pageNo);
            var result = (SearchResult<T>)JsonConvert.DeserializeObject(json, typeof(SearchResult<T>));

            return result;
        }



        public async Task<string> GetIndexAsync(string indexName)
        {
            const string urlFormat = "{0}/indexes/{1}?api-version={2}";
            var url = string.Format(urlFormat, _serviceUrl, indexName,
                _apiVersion);

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var task = await response.Content.ReadAsStringAsync();

            return task;
        }

        public string GetIndex(string indexName)
        {
            var task = GetIndexAsync(indexName);
            return task.Result;
        }

        public Task<HttpResponseMessage> DeleteIndexAsync(string indexName)
        {
            const string urlFormat = "{0}/indexes/{1}?api-version={2}";
            var url = string.Format(urlFormat, _serviceUrl, indexName,
                _apiVersion);

            return Client.DeleteAsync(url);
        }

        public HttpResponseMessage DeleteIndex(string indexName)
        {
            var task = DeleteIndexAsync(indexName);
            task.Wait();

            return task.Result;
        }

        /// <summary>
        /// Adds the specified list of items in the index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="items"></param>
        public void AddContent(string indexName, List<object> items)
        {
            SendContent(indexName, items, "upload");
        }

        /// <summary>
        /// Updates the specified list of items in the index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="items"></param>
        public void UpdateContent(string indexName, List<object> items)
        {
            SendContent(indexName, items, "merge");
        }

        /// <summary>
        /// Deletes the specified list of items from index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="items"></param>
        public void DeleteContent(string indexName, List<object> items)
        {
            SendContent(indexName, items, "delete");
        }


        private void SendContent(string indexName, IEnumerable<object> items, string searchAction)
        {
            const string urlFormat = "{0}/indexes/{1}/docs/index?api-version={2}";
            var url = string.Format(urlFormat, _serviceUrl, indexName, _apiVersion);

            var batch = new IndexItemBatch();
            foreach (var item in items.Select(entity => entity.ToSearchIndexItem()))
            {
                //(item as IDictionary<string, object>).Add("@search.action", searchAction);

                batch.value.Add(item);
            }

            var jsonPayload = JsonConvert.SerializeObject(batch);

            var response =
                Client.PostAsync(url,
                    new StringContent(jsonPayload, Encoding.UTF8, "application/json")).Result;

            response.EnsureSuccessStatusCode();
        }
    }
}
