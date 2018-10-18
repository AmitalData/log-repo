using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class QueryGroupPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int IndexOrder { get; set; }
    }
}