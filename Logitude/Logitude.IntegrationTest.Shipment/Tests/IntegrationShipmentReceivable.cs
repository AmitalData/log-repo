using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Data.Helpers;
using System.Collections.Generic;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    public class IntegrationShipmentReceivable
    {
    
        public static List<ShipmentReceivablePM> ShipmentReceivables()
        {
            List<ShipmentReceivablePM> shipmentReceivablePM = new List<ShipmentReceivablePM>();
            shipmentReceivablePM.Add(CreateShipmentReceivableItem());
            return shipmentReceivablePM;
        }

        public static ShipmentReceivablePM CreateShipmentReceivableItem()
        {
            ShipmentReceivablePM ShipmentReceivable = new ShipmentReceivablePM();

            ShipmentReceivable.CreatedByUserId = IntegrationTestLoginParameters.LoginUserId;
            ShipmentReceivable.CreateDate = TenantServerConfigration.GetCurrentDateTime(IntegrationTestLoginParameters.Tenant);
            ShipmentReceivable.Quantity = 10;
            ShipmentReceivable.ChargesTypeId = ShipmentVariables.ChargeTypeAFTId;
            ShipmentReceivable.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            ShipmentReceivable.CurrencyId = ShipmentVariables.CurrencyEURId;
            ShipmentReceivable.VatTypeId = ShipmentVariables.VATTypeZeroId;
            ShipmentReceivable.ShipmentReceivableLineStatusCode = "OAMT";
            /*
             if (loggedTenant.CurrencyId == itemPM.CurrencyId)
            {
                itemPM.Rate = 1;
                itemPM.TotalAmountLocal = itemPM.TotalAmount;
            }

            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
                itemPoco.CreatedByUserId = loggedContactId;
                itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }

            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdateByUserId = loggedContactId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.ChargesTypeId = itemPM.ChargesTypeId;
            itemPoco.CurrencyId = itemPM.CurrencyId;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.Rate = itemPM.Rate;
            itemPoco.ShipmentReceivableLineStatusCode = itemPM.ShipmentReceivableLineStatusCode;
            itemPoco.TotalAmount = itemPM.TotalAmount;
            itemPoco.TotalAmountLocal = itemPM.TotalAmountLocal;
            itemPoco.MeasurementId = itemPM.MeasurementId;
            itemPoco.UnitPrice = itemPM.UnitPrice;
            itemPoco.PayableLocal = itemPM.PayableLocal;
            itemPoco.PrepaidCollectId = itemPM.PrepaidCollectId;
            itemPoco.AWBPrint = itemPM.AWBPrint;
            itemPoco.DueTypeCode = itemPM.DueTypeCode;
            itemPoco.ARInvoiceLineId = itemPM.ARInvoiceLineId;
            itemPoco.IsFixedPrice = itemPM.IsFixedPrice;
            itemPoco.IsFromQuote = itemPM.IsFromQuote;
            itemPoco.IsExchangeRateFixed = itemPM.IsExchangeRateFixed;
            itemPoco.ProfitCurrencyExchangeRate = itemPM.ProfitCurrencyExchangeRate;
            itemPoco.AmountInProfitCurrency = itemPM.AmountInProfitCurrency;
            itemPoco.ARInvoiceId = itemPM.ARInvoiceId;
            itemPoco.IATACodeId = itemPM.IATACodeId;
            itemPoco.QuoteChargeId = itemPM.QuoteChargeId;
            itemPoco.IsChargeBySteps = itemPM.IsChargeBySteps;
            itemPoco.VatTypeId = itemPM.VatTypeId;
            itemPoco.IsBackToBack = itemPM.IsBackToBack;
            itemPoco.IsExpense = itemPM.IsExpense;
            itemPoco.ShipmentReceivableParentId = itemPM.ShipmentReceivableParentId;
            itemPoco.QuoteSaleMinAmount = itemPM.QuoteSaleMinAmount;
            itemPoco.QuoteSaleMaxAmount = itemPM.QuoteSaleMaxAmount;
             */
            return ShipmentReceivable;

        }

    }
}
