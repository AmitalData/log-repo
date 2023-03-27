using MicrosoftGraphClient.IGraphServices;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientService : IGraphClientService
    {
        public IGraphClientAuthenticationService Authentication()
        {
            return new GraphClientAuthenticationService();
        }

        public IGraphClientProfileService Profile(string token = null)
        {
            return new GraphClientProfileService(token);
        }

        public IGraphClientMessagesService Messages(string token = null)
        {
            return new GraphClientMessagesService(token);
        }

        public IGraphClientSubscriptionsService Subscriptions(string token = null)
        {
            return new GraphClientSubscriptionsService(token);
        }
    }
}