using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentPayable
    {
        static List<ShipmentPayablePM> shipmentpayablePM = new List<ShipmentPayablePM>();

        public static List<ShipmentPayablePM> ShipmentPayables(int quantity, int unitPrice)
        {
            for(int i = 0; i < 2; i++)
            {
                shipmentpayablePM.Add(ShipmentPayableItem(quantity, unitPrice));
            }
            return shipmentpayablePM;
        }
        /*
         Should Rate, Quantity and UnitPrice be dynamic ?
         Also, from where we should get a Profit exchange rate ?
         */
        public static ShipmentPayablePM ShipmentPayableItem(int quantity, int unitPrice)
        {
            ShipmentPayablePM ShipmentPayable = new ShipmentPayablePM();
            ShipmentPayable.ChargesTypeId = ShipmentVariables.ChargeTypeAFTId;
            ShipmentPayable.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            ShipmentPayable.VatTypeId = ShipmentVariables.VATTypeZeroId;
            ShipmentPayable.ShipmentPayableLineStatusCode = "OAMT";

            ShipmentPayable.CurrencyId = CorePreparationVariables.ProfitCurrencyId; ;
            ShipmentPayable.Rate = CorePreparationVariables.ProfitCurrencyRate;

            ShipmentPayable.ProfitCurrencyExchangeRate = CorePreparationVariables.ProfitCurrencyRate;
            ShipmentPayable.Quantity = quantity;
            ShipmentPayable.UnitPrice = unitPrice;
            ShipmentPayable.ChangeSetOp = ChangeSetOperation.Insert;
            ShipmentPayable.OpenAmount = ShipmentPayable.Quantity * ShipmentPayable.UnitPrice;
            ShipmentPayable.OpenAmountInLocalCurrency = ShipmentPayable.OpenAmount * CorePreparationVariables.ProfitCurrencyRate;
            ShipmentPayable.OpenAmountInProfitCurrency = ShipmentPayable.OpenAmountInLocalCurrency / CorePreparationVariables.ProfitCurrencyRate;
            return ShipmentPayable;

        }
    }
}
