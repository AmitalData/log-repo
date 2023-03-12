namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class AddCustomFieldRequest
    {
        public string ObjectTableId { get; set; }
        public string ParentObjectTableId { get; set; }
        public string ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public string FieldCode { get; set; }
        public string DefaultText { get; set; }
        public string DisplayText { get; set; }
        public string TextCode { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string LanguageCode { get; set; }
        public bool IsList { get; set; }
        public bool IsPm { get; set; }
    }
}
