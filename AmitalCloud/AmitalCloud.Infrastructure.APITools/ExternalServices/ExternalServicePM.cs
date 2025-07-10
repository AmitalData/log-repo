namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public enum ServiceTypeEnum { DCA = 1, Sign = 2 }
    public class ExternalServicePM
    {



        public string Id { get; set; }
        public ServiceTypeEnum ServiceType { get; set; }
        public int Tenant { get; set; }

        public string MoreParam { get; set; }

        public string ServiceAddressUrl { get; set; }
        public int TimeoutInSec { get; set; }
    }

}
