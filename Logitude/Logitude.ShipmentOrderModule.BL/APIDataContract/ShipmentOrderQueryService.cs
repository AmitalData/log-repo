using Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
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

            var temp = query.GetSinglePMByOrderNumber(orderNumber, Tenant);
            if (temp == null)
                throw new ApplicationException("ShipmentOrder with orderNumber " + orderNumber + " doesn't exist");

            return ShipmentOrderDataMapping(temp, Tenant);

        }

        public ShipmentOrderPM GetSinglePMByOrderNumber(string orderNumber, int Tenant)
        {

            var temp = query.GetSinglePMByOrderNumber(orderNumber, Tenant);
            if (temp == null)
                throw new ApplicationException("ShipmentOrder with orderNumber " + orderNumber + " doesn't exist");

            return temp;

        }

        public string GetIdByOrderNumber(string orderNumber, int Tenant)
        {

            var id = query.GetIdByOrderNumber(orderNumber, Tenant);
            if (string.IsNullOrEmpty(id))
                throw new ApplicationException("ShipmentOrder with orderNumber " + orderNumber + " doesn't exist");
            return id;
        }


    }
}
