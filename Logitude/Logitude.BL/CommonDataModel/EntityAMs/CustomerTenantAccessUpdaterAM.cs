using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class CustomerTenantAccessUpdaterAM
    {
        public int Tenant { get; set; }
        public int CustomerTenant { get; set; }
        public bool? IsPrivateLabel { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPassedTrialEndDate { get; set; }
    }
}
