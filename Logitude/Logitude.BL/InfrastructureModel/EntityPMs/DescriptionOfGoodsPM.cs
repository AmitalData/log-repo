using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DescriptionOfGoodsPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string DescriptionOfGood { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
    }
}