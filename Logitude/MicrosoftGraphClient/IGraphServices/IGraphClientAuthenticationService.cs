using MicrosoftGraphClient.Models.AuthenticationService;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientAuthenticationService
    {
        RefreshAccessTokenResponse RefreshAccessToken(RefreshAccessTokenRequest refreshAccessTokenRequest);
    }
}