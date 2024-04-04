using System;

namespace WebFreight.Web.Helpers.AmitalAPI.Structs
{
    public class AmitalApiClient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tenant { get; set; }
        public string AzureApiRegisterName { get; set; }
        public string AzureClientId { get; set; }
        public string Token { get; set; }
        public string AzureManagedApplObjId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime SecretExpired { get; set; }
        public string SecretValue { get; set; }
        public bool Active { get; set; }
    }
}
