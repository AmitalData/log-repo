using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
  public  class TenantLoginPolicy
    {
        [Key]
        public int Tenant { get; set; }
        public string LoginPolicyCode { get; set; }
        public bool IsEnabledForSpecificUsers { get; set; }
        public string TwoFactorInternalIPs { get; set; }
        public bool KeepUserLoggedIn { get; set; }
        public bool ExcludeInternalIPs { get; set; }
        public string AllowedIPs { get; set; }
        public decimal SessionTimeout  { get; set; }
        [ForeignKey("LoginPolicyCode")]
        public virtual LoginPolicy LoginPolicy { get; set; }
    }
}
