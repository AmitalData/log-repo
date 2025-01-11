using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ContainerTrackingProviderPM : EntityPM
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