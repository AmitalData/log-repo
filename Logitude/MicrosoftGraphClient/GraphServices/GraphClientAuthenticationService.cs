using RestSharp;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.GraphAPI;
using MicrosoftGraphClient.Models.GraphAPI;
using MicrosoftGraphClient.Models.AuthenticationService;
using System.Collections.Generic;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientAuthenticationService : IGraphClientAuthenticationService
    {
        protected string Url { get; set; }

        public GraphClientAuthenticationService()
        {
            Url = GraphClientApiUrls.Authentication;
        }

        public RefreshAccessTokenResponse RefreshAccessToken(RefreshAccessTokenRequest refreshAccessTokenRequest)
        {
            if(refreshAccessTokenRequest != null)
            {
                Dictionary<string, string> requestHeaders = GetRefreshAccessTokenRequestHeaders();
                Dictionary<string, string> requestParameters = GetRefreshAccessTokenRequestParameters(refreshAccessTokenRequest);
                return GraphAPICaller.Call<RefreshAccessTokenResponse>(new GraphAPICallerParameters
                {
                    Url = Url + "/token",
                    Method = Method.Post,
                    RequestHeaders = requestHeaders,
                    RequestParameters = requestParameters
                });
            }
            return null;
        }

        private Dictionary<string, string> GetRefreshAccessTokenRequestHeaders()
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            requestHeaders.Add("Origin", "");
            return requestHeaders;
        }

        private Dictionary<string, string> GetRefreshAccessTokenRequestParameters(RefreshAccessTokenRequest refreshAccessTokenRequest)
        {
            Dictionary<string, string> requestParameters = new Dictionary<string, string>();
            if(refreshAccessTokenRequest != null)
            {
                requestParameters.Add("grant_type", "refresh_token");
                requestParameters.Add("client_id", refreshAccessTokenRequest.ClientId ?? "");
                requestParameters.Add("refresh_token", refreshAccessTokenRequest.RefreshToken ?? "");
                requestParameters.Add("scope", refreshAccessTokenRequest.Scopes != null && refreshAccessTokenRequest.Scopes.Length > 0 ? string.Join(" ", refreshAccessTokenRequest.Scopes) : "");
            }
            return requestParameters;
        }
    }
}