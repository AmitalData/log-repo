using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class QueryColumnPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QueryId { get; set; }
        public string ObjectFieldId { get; set; }
        public int IndexOrder { get; set; }
        public double ColumnWidth { get; set; }
        public string ObjectFieldName { get; set; }
        public string QueryCode { get; set; }
        public string QueryObjectTableName { get; set; }
        public string DataTemplateName { get; set; }
        public string ColumnHeaderTemplateName { get; set; }
        public string ConverterName { get; set; }
        public string ObjectFieldListLabelTextCodeCode { get; set; }
        public string ObjectFieldFieldLableTextCodeDefaultText { get; set; }
        public string ObjectFieldFieldLableTextCodeCode { get; set; }
        public string ObjectFieldDataTypeCode { get; set; }
        public bool DisplayInList { get; set; }
        public string UserId { get; set; }
        public int UpdatedByTenant { get; set; }
        public string ObjectFieldFullNameTextCodeCode { get; set; }
    }
}