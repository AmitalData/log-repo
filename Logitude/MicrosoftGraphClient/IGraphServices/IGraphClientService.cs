namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientService
    {
        IGraphClientAuthenticationService Authentication();
        IGraphClientProfileService Profile();
        IGraphClientMessagesService Messages();
        IGraphClientSubscriptionsService Subscriptions();
    }
}