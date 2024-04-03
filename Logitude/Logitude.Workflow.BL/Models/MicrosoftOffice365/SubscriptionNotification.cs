using System;
using Newtonsoft.Json;

namespace Logitude.Workflow.BL.Models.MicrosoftOffice365
{
    public class SubscriptionNotification
    {
        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("subscriptionExpirationDateTime")]
        public DateTime SubscriptionExpirationDateTime { get; set; }

        [JsonProperty("changeType")]
        public string ChangeType { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("resourceData")]
        public SubscriptionNotificationResourceData ResourceData { get; set; }

        [JsonProperty("clientState")]
        public string ClientState { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }
    }
}