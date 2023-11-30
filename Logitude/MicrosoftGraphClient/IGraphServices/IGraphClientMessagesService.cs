using MicrosoftGraphClient.IGraphServices.Base;
using MicrosoftGraphClient.Models.MessagesService;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientMessagesService : IGraphClientResourceService<IGraphClientMessagesService>
    {
        Message Get(string messageId);
        string GetEml(string messageId);
    }
}