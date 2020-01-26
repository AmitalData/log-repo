using Logitude.BL.ShipmentsModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentPayable
    {
        static List<ShipmentPayablePM> shipmentpayablePM = new List<ShipmentPayablePM>();

        public static List<ShipmentPayablePM> ShipmentPayables()
        {
            for(int i = 0; i < 2; i++)
            {
                shipmentpayablePM.Add(ShipmentPayableItem());
            }
            return shipmentpayablePM;
        }
        /*
         Should Rate, Quantity and UnitPrice be dynamic ?
         Also, from where we should get a Profit exchange rate ?
         */
        public static ShipmentPayablePM ShipmentPayableItem()
        {
            ShipmentPayablePM ShipmentPayable = new ShipmentPayablePM();
            ShipmentPayable.ChargesTypeId = ShipmentVariables.ChargeTypeAFTId;
            ShipmentPayable.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            ShipmentPayable.CurrencyId = ShipmentVariables.CurrencyEURId;
            ShipmentPayable.VatTypeId = ShipmentVariables.VATTypeZeroId;
            ShipmentPayable.ShipmentPayableLineStatusCode = "OAMT";
            ShipmentPayable.ProfitCurrencyExchangeRate = 3.8;
            ShipmentPayable.Rate = 1;
            ShipmentPayable.Quantity = 5;
            ShipmentPayable.UnitPrice = 2;
            ShipmentPayable.OpenAmount = ShipmentPayable.Quantity * ShipmentPayable.UnitPrice;
            ShipmentPayable.OpenAmountInLocalCurrency = ShipmentPayable.OpenAmount * ShipmentPayable.Rate;
            ShipmentPayable.OpenAmountInProfitCurrency = ShipmentPayable.OpenAmountInLocalCurrency / ShipmentPayable.ProfitCurrencyExchangeRate;
            return ShipmentPayable;

        }
    }
}
