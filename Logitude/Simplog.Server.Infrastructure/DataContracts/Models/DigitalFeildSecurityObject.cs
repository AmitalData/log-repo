using System;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalFeildSecurityObject
    {
        public string FieldCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool HasPermission { get; set; }
    }
}
