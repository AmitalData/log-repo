namespace Simplog.Server.Infrastructure.DataContracts
{
    public class QueryFilterItem
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public object FieldValue2 { get; set; }
        public object FieldValue3 { get; set; }
        public string Operator { get; set; }
        public bool IsCustom { get; set; }
        public bool DisplayInList { get; set; }
        public bool IsCustomField { get; set; }
        public string FieldDataType { get; set; }

    }
}