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
    }
}
