namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalUploaderInfo
    {
        public string Id { get; set; }
        public string CardId { get; set; }
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Notes { get; set; }
        public string ObjectTableName { get; set; }
        public string Base64String { get; set; }
        public string DocumentTypeName { get; set; }
        public int FileSize { get; set; }
        public int Buffersize { get; set; }
        public bool IsApprovalRequired { get; set; }
    }
}
