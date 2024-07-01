using Intuit.Ipp.Core.Configuration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web;
using WebFreight.Web.Services;

public class WarmService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task MakeParallelRequestsAsync(WarmModel[] requests, string token)
    {
        var tasks = new List<Task>();
        if (_httpClient.DefaultRequestHeaders.Contains("Token"))
        {
            _httpClient.DefaultRequestHeaders.Remove("Token");
        }
        _httpClient.DefaultRequestHeaders.Add("Token", token);

        foreach (var request in requests)
        {
            tasks.Add(Task.Run(async () =>
            {
                try
                {

                    switch (request.Method.ToLower())
                    {
                        case "get":
                            {
                                var queryString = QueryStringHelper.ToQueryString(request.Model);
                                var requestUrl = request.Url + queryString;
                                var response = await _httpClient.GetAsync(requestUrl);
                                response.EnsureSuccessStatusCode();
                                var responseString = await response.Content.ReadAsStringAsync();
                                break;
                            }
                        case "post":
                            {
                                var json = JsonConvert.SerializeObject(request.Model);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                var response = await _httpClient.PostAsync(request.Url, content);
                                response.EnsureSuccessStatusCode();
                                var responseString = await response.Content.ReadAsStringAsync();
                                break;
                            }
                    }
                    //Logger.Log($"Success: {url} - Response: {content}");
                }
                catch (Exception ex)
                {
                    // Logger.LogException(ex);
                    // Logger.Log($"Failure: {url}");
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
}
