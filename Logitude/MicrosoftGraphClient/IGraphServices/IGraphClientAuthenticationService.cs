using MicrosoftGraphClient.IGraphServices.Base;
using MicrosoftGraphClient.Models.AuthenticationService;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientAuthenticationService : IGraphClientResourceService<IGraphClientAuthenticationService>
    {
        RefreshTokenResponse RefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}