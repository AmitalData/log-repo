using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookConnection.Common.Contracts
{
    public interface IShipmentRepo
    {

        ShipmentWcfServiceReference.ShipmentList[] GetShipmentList(OutlookConnection.Common.ShipmentWcfServiceReference.ShipmentApiFilters filters, int tenant, ref OutlookConnection.Common.ShipmentWcfServiceReference.Response response);



    }
}
