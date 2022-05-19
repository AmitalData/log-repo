using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerTrackingProviderPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SourceCode { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string CallbackURL { get; set; }
        public string APIKey { get; set; }
        public string ProviderURL { get; set; }
        public string LogitudeToken { get; set; }
    }
}