using System;
using Newtonsoft.Json;

namespace MicrosoftGraphClient.Models.SubscriptionsService
{
    public class CreateSubscriptionRequest
    {
        [JsonProperty("changeType")]
        public string ChangeType { get; set; }

        [JsonProperty("notificationUrl")]
        public string NotificationUrl { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset? ExpirationDateTime { get; set; }

        [JsonProperty("clientState")]
        public string ClientState { get; set; }

        [JsonProperty("latestSupportedTlsVersion")]
        public string LatestSupportedTlsVersion { get; set; }

        [JsonProperty("includeResourceData")]
        public bool IncludeResourceData { get; set; }
    }
}