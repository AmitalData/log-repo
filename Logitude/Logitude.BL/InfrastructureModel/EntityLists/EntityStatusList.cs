using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class EntityStatusList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string ObjectTableId { get; set; }
        public int StatusWeight { get; set; }
        public string ObjectTableName { get; set; }
        public bool InActive { get; set; }
        public string Code { get; set; }
        //public int IndexOrder { get; set; }
        public string SearchFields { get; set; }
        public string DisplayName { get; set; }
        public int? StatusLocalWeight { get; set; }
        public string EntityStatusTypeCode { get; set; }
        public bool AllowPartial { get; set; }
    }
}