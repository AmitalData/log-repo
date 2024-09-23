using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Simplog.Data.Typesense
{
    public class TClient
    {
        public readonly string apiKey;
        public readonly string baseUrl;

        public TClient(string apiKey, string baseUrl)
        {
            this.apiKey = apiKey;
            this.baseUrl = baseUrl;
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("X-TYPESENSE-API-KEY", apiKey);

            return client;
        }

        public async Task<List<T>> SearchAsync<T>(string tableName, string[] column, string value, string filterBy = null)
            => await SearchAsync<T>(tableName, string.Join(",", column), value, filterBy);

        public async Task<List<T>> SearchAsync<T>(string tableName, string column, string value, string filterBy = null)
        {
            string url = $"collections/{tableName}/documents/search";
            url += $"?q={value}&query_by={column}";
            if (filterBy != null)
                url += $"&filter_by={filterBy}";

            url += "&max_candidates=10000000&exhaustive_search=1&per_page=250";

            HttpClient client = GetClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(content);

            SearchResponse<T> result = JsonConvert.DeserializeObject<SearchResponse<T>>(content);
            List<T> responseList = result.hits.Select(x => x.document).ToList();

            client.Dispose();

            return responseList;
        }
        
        public async Task CreateTableAsync(CreateTableRequest schema)
        {
            string url = $"collections";
            string json = JsonConvert.SerializeObject(schema);

            HttpClient client = GetClient();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception(responseContent);

            client.Dispose();
        }

        public async Task DropTableAsync(string tableName)
        {
            string url = $"collections/{tableName}";

            HttpClient client = GetClient();
            HttpResponseMessage response = await client.DeleteAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(content);

            client.Dispose();
        }
        
        public async Task UpsertAsync(string tableName, object record)
        {
            string url = $"collections/{tableName}/documents";
            string json = JsonConvert.SerializeObject(record);

            HttpClient client = GetClient();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception(responseContent);

            client.Dispose();
        }        

        public async Task InsertAsync<T>(string tableName, IEnumerable<T> records)
        {
            string url = $"collections/{tableName}/documents/import?action=create";
            string data = string.Join(Environment.NewLine, records.Select(x => JsonConvert.SerializeObject(x)));

            HttpClient client = GetClient();            
            HttpContent content = new StringContent(data, System.Text.Encoding.UTF8, "text/plain");
            HttpResponseMessage response = await client.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            client.Dispose();
        }
    }
}