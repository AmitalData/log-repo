using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Data.Helpers;
using System.Collections.Generic;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    public class IntegrationShipmentReceivable
    {
        static List<ShipmentReceivablePM> shipmentReceivablePM = new List<ShipmentReceivablePM>();
        public static List<ShipmentReceivablePM> ShipmentReceivables()
        {
            for(int i = 0; i < 2; i++)
            {
                shipmentReceivablePM.Add(CreateShipmentReceivableItem());
            }
            return shipmentReceivablePM;
        }

        public static ShipmentReceivablePM CreateShipmentReceivableItem()
        {
            ShipmentReceivablePM ShipmentReceivable = new ShipmentReceivablePM();

            ShipmentReceivable.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ShipmentReceivable.CreateDate = TenantServerConfigration.GetCurrentDateTime(IntegrationTestLoginParameters.Tenant);
            ShipmentReceivable.ChargesTypeId = ShipmentVariables.ChargeTypeAFTId;
            ShipmentReceivable.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            ShipmentReceivable.CurrencyId = ShipmentVariables.CurrencyEURId;
            ShipmentReceivable.VatTypeId = ShipmentVariables.VATTypeZeroId;
            ShipmentReceivable.ShipmentReceivableLineStatusCode = "OAMT";
            ShipmentReceivable.ProfitCurrencyExchangeRate = 3.8;
            ShipmentReceivable.Rate = 1;
            ShipmentReceivable.Quantity = 3;
            ShipmentReceivable.UnitPrice = 4;
            ShipmentReceivable.TotalAmount = ShipmentReceivable.Quantity * ShipmentReceivable.UnitPrice;
            ShipmentReceivable.TotalAmountLocal = ShipmentReceivable.TotalAmount * ShipmentReceivable.Rate;
            ShipmentReceivable.AmountInProfitCurrency = ShipmentReceivable.TotalAmountLocal / ShipmentReceivable.ProfitCurrencyExchangeRate;
            return ShipmentReceivable;
        }

    }
}
