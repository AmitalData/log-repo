using RestSharp;
using MicrosoftGraphClient.GraphServices.Base;
using MicrosoftGraphClient.IGraphServices;
using MicrosoftGraphClient.Constants;
using MicrosoftGraphClient.Models.SubscriptionsService;
using MicrosoftGraphClient.GraphAPI;

namespace MicrosoftGraphClient.GraphServices
{
    public class GraphClientSubscriptionsService : GraphClientResourceService<IGraphClientSubscriptionsService>, IGraphClientSubscriptionsService
    {
        public GraphClientSubscriptionsService(string token = null) : base(GraphClientApiUrls.Subscriptions, token) { }

        protected override IGraphClientSubscriptionsService GetInstance()
        {
            return this;
        }

        public Subscription Create(CreateSubscription createSubscription)
        {
            return GraphAPICaller.Call<Subscription>(Token, Url, Method.POST, createSubscription);
        }
    }
}