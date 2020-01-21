using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    public class IntegrationShipmentReceivable
    {
    
        public static List<ShipmentReceivablePM> ShipmentReceivables()
        {
            List<ShipmentReceivablePM> shipmentReceivablePM = new List<ShipmentReceivablePM>();
            shipmentReceivablePM.Add(ShipmentReceivableItem(ShipmentVariables.ChargeTypeAFTId, ShipmentVariables.MeasurmentGRWTId, ShipmentVariables.CurrencyEURId, ShipmentVariables.VATTypeZeroId, "OAMT"));
            return shipmentReceivablePM;
        }

        public static ShipmentReceivablePM ShipmentReceivableItem(string ChargetypeId, string MeasurementId, string CurrencyId, string VatTypeId,string ShipmentReceivableLineStatusCode)
        {
            ShipmentReceivablePM ShipmentReceivableItem = new ShipmentReceivablePM();
            ShipmentReceivableItem.ChargesTypeId = ChargetypeId;
            ShipmentReceivableItem.MeasurementId = MeasurementId;
            ShipmentReceivableItem.CurrencyId = CurrencyId;
            ShipmentReceivableItem.VatTypeId = VatTypeId;
            ShipmentReceivableItem.ShipmentReceivableLineStatusCode = ShipmentReceivableLineStatusCode;

            return ShipmentReceivableItem;

        }

    }
}
