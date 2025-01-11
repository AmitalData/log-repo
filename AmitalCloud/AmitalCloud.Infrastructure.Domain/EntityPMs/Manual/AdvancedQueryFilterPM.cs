using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class AdvancedQueryFilterPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string QueryCode { get; set; }
        public string ObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public bool CustomPredefined { get; set; }
        public string ObjectFieldName { get; set; }
        public bool IsCustomFilter { get; set; }
        public string QueryUserId { get; set; }
        public string QueryObjectTableName { get; set; }
        public string DataTypeCode { get; set; }
        public bool DisplayInList { get; set; }
        public string ObjectFieldOperator { get; set; }
        public string UserId { get; set; }

        public string ObjectFieldCode { get; set; }
    }
}