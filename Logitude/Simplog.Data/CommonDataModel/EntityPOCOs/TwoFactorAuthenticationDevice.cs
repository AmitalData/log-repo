using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class TwoFactorAuthenticationDevice
    {
        [Key]
        public string Id { get; set; }

        public string TwoFactorkey { get; set; }

        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string DeviceDescription { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public string AuthenticationCode { get; set; }
        public bool InActive { get; set; }

        public DateTime CodeExpirationDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsVerified { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    

    }


}
