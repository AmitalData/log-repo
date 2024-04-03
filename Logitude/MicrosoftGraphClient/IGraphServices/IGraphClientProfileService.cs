using MicrosoftGraphClient.IGraphServices.Base;
using MicrosoftGraphClient.Models.ProfileService;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientProfileService : IGraphClientResourceService<IGraphClientProfileService>
    {
        Profile Get();
    }
}