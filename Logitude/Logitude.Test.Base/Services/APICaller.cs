using Logitude.Test.Base.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Logitude.Test.Base.Services
{
    public class APICaller
    {
        public static ApiResponse<T> CallPost<T>(object requestBody, string url, string token)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.POST,
                RequestBody = requestBody,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request);
        }

        public static ApiResponse<T> CallPut<T>(object requestBody, string url, string token)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.PUT,
                RequestBody = requestBody,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request);
        }

        public static ApiResponse<T> CallGet<T>(string url, string token)
        {
            ApiRequestParameters request = new ApiRequestParameters()
            {
                Method = Method.GET,
                Token = token,
                Url = url
            };

            return CallAPIProcess<T>(request);
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

        
        private static ApiResponse<T> CallAPIProcess<T>(ApiRequestParameters requestParameters)
        {
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

            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            response.StatusCode = restResponse.StatusCode;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.Data = restResponse.Data;
            }
            else
            {
                JObject jObject = JObject.Parse(restResponse.Content);
                response.ErrorMessage = jObject["ErrorMessage"].ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                throw new Exception(response.ErrorMessage);
            }

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