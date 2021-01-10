using Logitude.Test.Base.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SecurityTests.Models.CompanyAddressSetting
{
    public class AddressSettingSecurityAccessStepsContext
    {
        public AddressSettingSecurityAccessStepsContext()
        {
            FirstUserAddressSetting = new AddressPM();
            SecondUserAddressSetting = new AddressPM();
            FirstUserTenant = new TenantPM();
            SecondUserTenant = new TenantPM();
        }

        public AddressPM FirstUserAddressSetting { get; set; }
        public AddressPM SecondUserAddressSetting { get; set; }

        public TenantPM FirstUserTenant { get; set; }
        public TenantPM SecondUserTenant { get; set; }

        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
    }
}
