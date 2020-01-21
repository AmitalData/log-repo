using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentPayable
    {
     
        public static List<ShipmentPayablePM> ShipmentPayables()
        {
            List<ShipmentPayablePM> shipmentpayablePM = new List<ShipmentPayablePM>();
            shipmentpayablePM.Add(ShipmentPayableItem(ShipmentVariables.ChargeTypeAFTId, ShipmentVariables.MeasurmentGRWTId, ShipmentVariables.CurrencyEURId, ShipmentVariables.VATTypeZeroId, "OAMT"));
            return shipmentpayablePM;
        }

        public static ShipmentPayablePM ShipmentPayableItem(string ChargetypeId, string MeasurementId, string CurrencyId, string VatTypeId, string ShipmentPayableLineStatusCode)
        {
            ShipmentPayablePM ShipmentPayableItem = new ShipmentPayablePM();
            ShipmentPayableItem.ChargesTypeId = ChargetypeId;
            ShipmentPayableItem.MeasurementId = MeasurementId;
            ShipmentPayableItem.CurrencyId = CurrencyId;
            ShipmentPayableItem.VatTypeId = VatTypeId;
            ShipmentPayableItem.ShipmentPayableLineStatusCode = ShipmentPayableLineStatusCode;

            return ShipmentPayableItem;

        }
    }
}
