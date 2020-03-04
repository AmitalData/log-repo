using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class QueryColumnList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }
        public string ObjectFieldId { get; set; }
        public int IndexOrder { get; set; }
        public double ColumnWidth { get; set; }
        public string ObjectFieldName { get; set; }
        public string UserId { get; set; }
        public string ObjectFieldCode { get; set; }
    }
}