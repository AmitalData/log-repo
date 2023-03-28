using MicrosoftGraphClient.Models.AuthenticationService;

namespace MicrosoftGraphClient.GraphServices.Base
{
    public abstract class GraphClientResourceService<T>
    {
        protected string Url { get; set; }
        protected string AccessToken { get; set; }

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
            AccessToken = new GraphClientAuthenticationService().RefreshAccessToken(refreshAccessTokenRequest)?.AccessToken;
            return GetInstance();
        }
    }
}