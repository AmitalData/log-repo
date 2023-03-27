using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.GraphAPI;
using MicrosoftGraphClient.Models.GraphAPI;
using MicrosoftGraphClient.Models.AuthenticationService;
using System.Collections.Generic;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientAuthenticationService : GraphClientResourceService<IGraphClientAuthenticationService>, IGraphClientAuthenticationService
    {
        public GraphClientAuthenticationService() : base(GraphClientApiUrls.Authentication, null) { }

        protected override IGraphClientAuthenticationService GetInstance()
        {
            return this;
        }

        public RefreshTokenResponse RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            Dictionary<string, string> requestParameters = new Dictionary<string, string>();
            requestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            requestHeaders.Add("Origin", "");
            requestParameters.Add("grant_type", "refresh_token");
            requestParameters.Add("client_id", refreshTokenRequest.ClientId ?? "");
            requestParameters.Add("refresh_token", refreshTokenRequest.RefreshToken ?? "");
            requestParameters.Add("scope", refreshTokenRequest.Scopes != null && refreshTokenRequest.Scopes.Length > 0 ? string.Join(" ", refreshTokenRequest.Scopes) : "");
            return GraphAPICaller.Call<RefreshTokenResponse>(new GraphAPICallerParams
            {
                Url = Url + "/token",
                Method = Method.POST,
                RequestHeaders = requestHeaders,
                RequestParameters = requestParameters
            });
        }
    }
}