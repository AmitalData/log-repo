using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class ApiCredintialsPM
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UsedFor { get; set; }
        public string HashedPrimaryAccessKey { get; set; }
        public string HashedSeconderyAccessKey { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string AllowedIPs { get; set; }
        public int? TokenExpirationTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string maskedPrimaryAccessKey { get; set; }
        public string maskedSeconderyAccessKey { get; set; }
        //public string ComputingPartnerId { get; set; }
    }
}
