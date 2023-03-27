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
        public GraphClientSubscriptionsService(string token = null) : base(GraphClientApiUrls.Subscriptions, token) { }

        protected override IGraphClientSubscriptionsService GetInstance()
        {
            return this;
        }

        public Subscription Create(CreateSubscriptionRequest createSubscription)
        {
            return GraphAPICaller.Call<Subscription>(new GraphAPICallerParams { Token = Token, Url = Url, Method = Method.POST, RequestBody = createSubscription });
        }
    }
}