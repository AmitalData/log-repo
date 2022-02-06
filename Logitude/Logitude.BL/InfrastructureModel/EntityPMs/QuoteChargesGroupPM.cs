using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class QuoteChargesGroupPM
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public int Tenant { get; set; }
        public string LocalName { get; set; }
        public int ViewOrder { get; set; }
    }
}
