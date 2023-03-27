using MicrosoftGraphClient.IGraphServices.Base;
using MicrosoftGraphClient.Models.SubscriptionsService;

namespace MicrosoftGraphClient.IGraphServices
{
    public interface IGraphClientSubscriptionsService : IGraphClientResourceService<IGraphClientSubscriptionsService>
    {
        Subscription Create(CreateSubscriptionRequest createSubscription);
    }
}