using MicrosoftGraphClient.Models;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientProfileService : IGraphClientResourceService
    {
        Profile Get();
    }
}