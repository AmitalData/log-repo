using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    public class IntegrationShipmentReceivable
    {
        static List<ShipmentReceivablePM> shipmentReceivablePM = new List<ShipmentReceivablePM>();
        public static List<ShipmentReceivablePM> ShipmentReceivables(int quantity, int unitPrice)
        {
            for(int i = 0; i < 2; i++)
            {
                shipmentReceivablePM.Add(CreateShipmentReceivableItem( quantity,  unitPrice));
            }
            return shipmentReceivablePM;
        }

        public static ShipmentReceivablePM CreateShipmentReceivableItem(int quantity, int unitPrice)
        {
            ShipmentReceivablePM ShipmentReceivable = new ShipmentReceivablePM();

            ShipmentReceivable.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            //ShipmentReceivable.CreateDate = TenantServerConfigration.GetCurrentDateTime(IntegrationTestLoginParameters.Tenant);
            ShipmentReceivable.ChargesTypeId = ShipmentVariables.ChargeTypeAFTId;
            ShipmentReceivable.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            ShipmentReceivable.CurrencyId = CorePreparationVariables.ProfitCurrencyId;
            ShipmentReceivable.Rate = CorePreparationVariables.ProfitCurrencyRate;
            ShipmentReceivable.ProfitCurrencyExchangeRate = CorePreparationVariables.ProfitCurrencyRate;
            ShipmentReceivable.VatTypeId = ShipmentVariables.VATTypeZeroId;
            ShipmentReceivable.ShipmentReceivableLineStatusCode = "OAMT";
            ShipmentReceivable.Quantity = quantity;
            ShipmentReceivable.UnitPrice = unitPrice;
            ShipmentReceivable.ChangeSetOp = ChangeSetOperation.Insert;
            ShipmentReceivable.TotalAmount = ShipmentReceivable.Quantity * ShipmentReceivable.UnitPrice;
            ShipmentReceivable.TotalAmountLocal = ShipmentReceivable.TotalAmount * CorePreparationVariables.ProfitCurrencyRate;
            ShipmentReceivable.AmountInProfitCurrency = ShipmentReceivable.TotalAmountLocal / CorePreparationVariables.ProfitCurrencyRate;
            return ShipmentReceivable;
        }

    }
}
