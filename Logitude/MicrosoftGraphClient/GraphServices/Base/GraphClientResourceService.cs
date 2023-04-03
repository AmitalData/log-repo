using MicrosoftGraphClient.Models.AuthenticationService;

namespace MicrosoftGraphClient.GraphServices.Base
{
    public abstract class GraphClientResourceService<T>
    {
        protected string Url { get; set; }
        protected string AccessToken { get; set; }
        protected RefreshAccessTokenResponse RefreshAccessTokenResponse { get; set; }

        public GraphClientResourceService(string url)
        {
            Url = url;
        }

        protected abstract T GetInstance();

        public T SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
            return GetInstance();
        }

        public T SetAccessToken(RefreshAccessTokenRequest refreshAccessTokenRequest)
        {
            RefreshAccessTokenResponse = new GraphClientAuthenticationService().RefreshAccessToken(refreshAccessTokenRequest);
            AccessToken = RefreshAccessTokenResponse?.AccessToken;
            return GetInstance();
        }

        public RefreshAccessTokenResponse GetRefreshAccessTokenResponse()
        {
            return RefreshAccessTokenResponse;
        }
    }
}