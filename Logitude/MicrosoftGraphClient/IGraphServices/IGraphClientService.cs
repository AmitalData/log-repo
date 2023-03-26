namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientService
    {
        IGraphClientProfileService Profile(string token = null);
        IGraphClientMessagesService Messages(string token = null);
        IGraphClientSubscriptionsService Subscriptions(string token = null);
    }
}