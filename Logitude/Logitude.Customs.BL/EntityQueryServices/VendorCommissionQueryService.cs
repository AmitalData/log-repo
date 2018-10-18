using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class VendorCommissionQueryService
    {

        public VendorCommissionPM GetSingleCommisionByVendorAndCustomer(string vendorId, string customerId, int tenant)
        {
            VendorCommissionPM commisionPM = null;
            VendorCommission commision = repository.GetSingleVendor(vendorId, customerId, tenant);
            if (commision != null)
            {
                commisionPM = new VendorCommissionPM()
                {
                    CommisionPercentage = commision.CommisionPercentage,
                    VendorId = commision.VendorId,
                    CustomerId = commision.CustomerId,
                    Tenant = commision.Tenant,
                };
            }
            return commisionPM;
        }

    }
}
