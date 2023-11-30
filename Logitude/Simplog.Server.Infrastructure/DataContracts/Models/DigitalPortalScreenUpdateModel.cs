using System;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalPortalScreenUpdateModel
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ScreenCode { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string DraftContent { get; set; }
        public bool IsDraft { get; set; }
        public string ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public bool IsList { get; set; }
    }
}
