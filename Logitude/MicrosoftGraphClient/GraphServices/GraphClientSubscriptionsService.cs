using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.SubscriptionsService;
using MicrosoftGraphClient.GraphAPI;
using MicrosoftGraphClient.Models.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientSubscriptionsService : GraphClientResourceService<IGraphClientSubscriptionsService>, IGraphClientSubscriptionsService
    {
        public GraphClientSubscriptionsService() : base(GraphClientApiUrls.Subscriptions) { }

        protected override IGraphClientSubscriptionsService GetInstance()
        {
            return this;
        }

        public Subscription Create(CreateSubscriptionRequest createSubscription)
        {
            return GraphAPICaller.Call<Subscription>(new GraphAPICallerParameters { Url = Url, AccessToken = AccessToken, Method = Method.Post, RequestBody = createSubscription });
        }
    }
}