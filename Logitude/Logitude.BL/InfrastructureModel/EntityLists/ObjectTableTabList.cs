using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class ObjectTableTabList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string Code { get; set; }
        public string TabNameTextCodeId { get; set; }
        public int IndexOrder { get; set; }
        public string TabNameTextCodeDefaultText { get; set; }
        public string TabNameTextCodeCode { get; set; }

    }
}