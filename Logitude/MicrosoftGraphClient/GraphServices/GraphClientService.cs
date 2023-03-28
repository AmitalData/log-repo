using MicrosoftGraphClient.IGraphServices;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientService : IGraphClientService
    {
        public IGraphClientAuthenticationService Authentication()
        {
            return new GraphClientAuthenticationService();
        }

        public IGraphClientProfileService Profile()
        {
            return new GraphClientProfileService();
        }

        public IGraphClientMessagesService Messages()
        {
            return new GraphClientMessagesService();
        }

        public IGraphClientSubscriptionsService Subscriptions()
        {
            return new GraphClientSubscriptionsService();
        }
    }
}