using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class TransportModePM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}