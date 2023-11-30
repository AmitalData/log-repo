using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel
{
   public class SharedLogisticsPM
    {

        [Key]
       public string InvitationEmail { get; set; }
        public string SystemURL { get; set; }
        public string ResetPasswordURL { get; set; }
        public string AndroidAppLink { get; set; }
        public string IOSAppLink { get; set; }
        public string InvitationPassword { get; set; }
        public string InviteeName { get; set; }
        public string URLprivateCargoTracking { get; set; }
        public string BrandingURL { get; set; }
        public string BrandingURLButton { get; set; }
        public string ResetPasswordButton { get; set; }
    }
}
   

