using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoTenantMilestoneDefinitions
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public bool IsCustomerView { get; set; }
    }
}
