namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class AddCustomFieldRequest
    {
        public string ObjectTableId { get; set; }
        public string ProfileId { get; set; }
        public string FieldCode { get; set; }
        public string DefaultText { get; set; }
        public string TextCode { get; set; }
        public string CreatedBy { get; set; }
    }
}
