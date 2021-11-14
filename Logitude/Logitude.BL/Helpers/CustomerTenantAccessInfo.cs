using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class CustomerTenantAccessInfo
    {
        public bool HasAccess { get; set; }
        public int  CustomerTenant { get; set; }

        public bool IsExportActivated { get; set; }
        public bool IsCustomsActivated { get; set; }


    }
}
