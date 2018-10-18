using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class MenuButtonGroupList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tenant { get; set; }
        public string MenuButtonGroupType { get; set; }
        public string ObjectTableId { get; set; }
    }
}