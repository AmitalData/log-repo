using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Test.Base.Services
{
    public class APICaller
    {
        public static ApiResponse<T> CallPost<T>(object requestBody, string url, string token, int retries = 0)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.POST,
                RequestBody = requestBody,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request, retries);
        }

        public static ApiResponse<T> CallPut<T>(object requestBody, string url, string token, int retries = 0)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.PUT,
                RequestBody = requestBody,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request, retries);
        }

        public static ApiResponse<T> CallGet<T>(string url, string token, int retries = 0)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.GET,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request, retries);
        }

        public static ApiResponse<T> CallGetByFilters<T>(string url, string token, ApiQueryFilters apiQueryFilters)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.GET,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request, apiQueryFilters);
        }

        
        private static ApiResponse<T> CallAPIProcess<T>(ApiRequestParameters requestParameters, int retries = 0)// needs refactoring
        {
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);

            string restClientUrl = GetRequestUrl(requestParameters.Url);
            RestClient restClient = new RestClient(restClientUrl);
            RestRequest restRequest = new RestRequest(requestParameters.Method) { RequestFormat = DataFormat.Json };
            var response = new ApiResponse<T>();

            if (!string.IsNullOrEmpty(requestParameters.Token))
            {
                restRequest.AddHeader("Token", requestParameters.Token);
            }
            if (requestParameters.RequestBody != null)
            {
                restRequest.AddJsonBody(JsonConvert.SerializeObject(requestParameters.RequestBody));
            }

            var attempts = 0;
            IRestResponse<T> restResponse;
            var exceptions = new List<Exception>();
            do
            {
                try
                {
                    restResponse = restClient.Execute<T>(restRequest);
                    response.StatusCode = restResponse.StatusCode;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Data = restResponse.Data;
                        break;
                    }
                    else
                    {
                        JObject jObject = JObject.Parse(restResponse.Content);
                        response.ErrorMessage = jObject["ErrorMessage"].ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                        throw new Exception(response.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    if (attempts == retries)
                        throw new AggregateException(exceptions);

                    attempts++;
                    Task.Delay(pauseBetweenFailures).Wait();
                    pauseBetweenFailures = TimeSpan.FromTicks(pauseBetweenFailures.Add(pauseBetweenFailures).Ticks * (attempts + 1));
                    if (pauseBetweenFailures.CompareTo(TimeSpan.FromMinutes(5)) > 0)
                        pauseBetweenFailures = TimeSpan.FromMinutes(5);
                }
            } while (true);

            return response;
        }

        private static ApiResponse<T> CallAPIProcess<T>(ApiRequestParameters requestParameters, ApiQueryFilters apiQueryFilters)
        {
            string restClientUrl = GetRequestUrl(requestParameters.Url);
            restClientUrl += GetQueryStringFromApiQueryFilters(apiQueryFilters);

            RestClient restClient = new RestClient(restClientUrl);
            RestRequest restRequest = new RestRequest(requestParameters.Method) { RequestFormat = DataFormat.Json };
            var response = new ApiResponse<T>();

            if (!string.IsNullOrEmpty(requestParameters.Token))
            {
                restRequest.AddHeader("Token", requestParameters.Token);
            }
            if (requestParameters.RequestBody != null)
            {
                restRequest.AddJsonBody(JsonConvert.SerializeObject(requestParameters.RequestBody));
            }

            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            response.StatusCode = restResponse.StatusCode;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JObject jObject = JObject.Parse(restResponse.Content);
                string result = jObject["Result"].ToString();
                response.Data = JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                JObject jObject = JObject.Parse(restResponse.Content);
                response.ErrorMessage = jObject["ErrorMessage"].ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                throw new Exception(response.ErrorMessage);
            }

            return response;
        }

        private static string GetRequestUrl(string url)
        {
            string apiUrl = Settings.ServerUrl;

            if (String.IsNullOrEmpty(url))
            {
                return null;
            }
            return (apiUrl.EndsWith("/") ? apiUrl.TrimEnd('/') : apiUrl) + (!url.StartsWith("/") ? ("/" + url) : url);
        }

        private static string GetQueryStringFromApiQueryFilters(ApiQueryFilters apiQueryFilters)
        {
            try
            {
                string apiQueryFiltersJson = JsonConvert.SerializeObject(apiQueryFilters);
                IDictionary<string, string> apiQueryFiltersDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(apiQueryFiltersJson);
                IEnumerable<string> apiQueryFiltersQueryStringList = apiQueryFiltersDictionary.Where(x => !String.IsNullOrEmpty(x.Value))
                                                                                              .Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value));
                string apiQueryFiltersQueryString = "?" + string.Join("&", apiQueryFiltersQueryStringList);
                return apiQueryFiltersQueryString;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}