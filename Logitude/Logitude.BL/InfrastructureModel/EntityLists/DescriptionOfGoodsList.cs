using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class DescriptionOfGoodsList
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