using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1
{
    public partial class ShipmentOrderQueryService
    {

        public ShipmentOrder GetByOrderNumber(string orderNumber, int Tenant)
        {
            try
            {
                var temp = query.GetSinglePMByOrderNumber(orderNumber, Tenant);
                if (temp == null)
                    throw new ApplicationException("ShipmentOrder with orderNumber " + orderNumber + " doesn't exist");

                return ShipmentOrderDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
