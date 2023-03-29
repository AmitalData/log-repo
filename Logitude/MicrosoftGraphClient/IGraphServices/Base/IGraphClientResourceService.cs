using MicrosoftGraphClient.Models.AuthenticationService;

namespace MicrosoftGraphClient.IGraphServices.Base
{
    public interface IGraphClientResourceService<T>
    {
        T SetAccessToken(string accessToken);
        T SetAccessToken(RefreshAccessTokenRequest refreshAccessTokenRequest);
        RefreshAccessTokenResponse GetRefreshAccessTokenResponse();
    }
}