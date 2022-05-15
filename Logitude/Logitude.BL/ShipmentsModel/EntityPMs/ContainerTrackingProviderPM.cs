using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerTrackingProviderPM
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string CallbackURL { get; set; }
        public string APIKey { get; set; }
        public string ProviderURL { get; set; }
    }
}