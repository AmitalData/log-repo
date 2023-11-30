using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class CustomerTenantAccessRequestAM
    {
        public int PartnerTenant { get; set; }
        public int CustomerTenant { get; set; }

        public bool IsExportActivated { get; set; }
        public bool IsCustomsActivated { get; set; }
        //public string StatusCode { get; set; }
    }
}
