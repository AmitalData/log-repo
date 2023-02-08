using System;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalFeildSecurityUpdateModel
    {
        public string FieldCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool HasPermission { get; set; }
        public bool IsList { get; set; }
        public bool IsPm { get; set; }
    }
}
