using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.SubscriptionsService
{
    public class Subscription : CreateSubscription
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("creatorId")]
        public string CreatorId { get; set; }

        [JsonProperty("notificationContentType")]
        public string NotificationContentType { get; set; }
    }
}