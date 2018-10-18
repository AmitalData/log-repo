using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class AdvancedQueryFilterList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string ObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public string ObjectTableName { get; set; }
        public string UserId { get; set; }
    }
}