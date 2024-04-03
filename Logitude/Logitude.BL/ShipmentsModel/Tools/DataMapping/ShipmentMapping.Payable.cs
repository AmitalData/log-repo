using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        internal static void MapPayable(ShipmentPayablePM itemPM, ShipmentPayable itemPoco,string loggedContactId, Tenant loggedTenant, bool isNewEntity)
        {
            if (loggedTenant.CurrencyId == itemPM.CurrencyId)
            {
                itemPM.Rate = 1;
                itemPM.ExpectedAmountLocal = itemPM.ExpectedAmount;
            }

            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
                itemPoco.CreatedByUserId = loggedContactId;
                itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }

            itemPoco.ValueDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdateByUserId = loggedContactId;
            itemPM.Quantity = MethodHelper.Round(itemPM.Quantity, 3);
            itemPM.UnitPrice = MethodHelper.Round(itemPM.UnitPrice, 3);
            itemPM.Rate = MethodHelper.Round(itemPM.Rate, 5);
            itemPM.ProfitCurrencyExchangeRate = MethodHelper.Round(itemPM.ProfitCurrencyExchangeRate, 5);
            itemPM.ExpectedAmount = MethodHelper.Round(itemPM.ExpectedAmount, 2);
            itemPM.ExpectedAmountLocal = MethodHelper.Round(itemPM.ExpectedAmountLocal, 2);
            itemPM.ExpectedAmountInProfitCurrency = MethodHelper.Round(itemPM.ExpectedAmountInProfitCurrency, 2);
            itemPM.AccountedAmount = MethodHelper.Round(itemPM.AccountedAmount, 2);
            itemPM.AccountedAmountInLocalCurrency = MethodHelper.Round(itemPM.AccountedAmountInLocalCurrency, 2);
            itemPM.AccountedAmountInProfitCurrency = MethodHelper.Round(itemPM.AccountedAmountInProfitCurrency, 2);
            itemPM.OpenAmount = MethodHelper.Round(itemPM.OpenAmount, 2);
            itemPM.OpenAmountInLocalCurrency = MethodHelper.Round(itemPM.OpenAmountInLocalCurrency, 2);
            itemPM.OpenAmountInProfitCurrency = MethodHelper.Round(itemPM.OpenAmountInProfitCurrency, 2);
            itemPM.CorrectionAmount = MethodHelper.Round(itemPM.CorrectionAmount, 2);
            itemPM.ProratedAmountInLocalCurrency = MethodHelper.Round(itemPM.ProratedAmountInLocalCurrency, 2);
            itemPM.ProratedAmountInProfitCurrency = MethodHelper.Round(itemPM.ProratedAmountInProfitCurrency, 2);

            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.UnitPrice = itemPM.UnitPrice;
            itemPoco.Rate = itemPM.Rate;
            itemPoco.ProfitCurrencyExchangeRate = itemPM.ProfitCurrencyExchangeRate;
            itemPoco.ExpectedAmount = itemPM.ExpectedAmount;
            itemPoco.ExpectedAmountLocal = itemPM.ExpectedAmountLocal;
            itemPoco.ExpectedAmountInProfitCurrency = itemPM.ExpectedAmountInProfitCurrency;
            itemPoco.AccountedAmount = itemPM.AccountedAmount;
            itemPoco.AccountedAmountInLocalCurrency = itemPM.AccountedAmountInLocalCurrency;
            itemPoco.AccountedAmountInProfitCurrency = itemPM.AccountedAmountInProfitCurrency;
            itemPoco.OpenAmount = itemPM.OpenAmount;
            itemPoco.OpenAmountInLocalCurrency = itemPM.OpenAmountInLocalCurrency;
            itemPoco.OpenAmountInProfitCurrency = itemPM.OpenAmountInProfitCurrency;
            itemPoco.CorrectionAmount = itemPM.CorrectionAmount;
            itemPoco.ShipmentPayableLineStatusCode = itemPM.ShipmentPayableLineStatusCode;
            itemPoco.ShipmentPayableAmountTypeCode = itemPM.ShipmentPayableAmountTypeCode;
            itemPoco.ChargesTypeId = itemPM.ChargesTypeId;
            itemPoco.CurrencyId = itemPM.CurrencyId;
            itemPoco.MeasurementId = itemPM.MeasurementId;
            itemPoco.AWBPrint = itemPM.AWBPrint;
            itemPoco.VendorId = itemPM.VendorId;
            itemPoco.DueTypeCode = itemPM.DueTypeCode;
            itemPoco.IsFromQuote = itemPM.IsFromQuote;
            itemPoco.PrepaidCollectId = itemPM.PrepaidCollectId;
            itemPoco.MinAmount = itemPM.MinAmount;
            itemPoco.MaxAmount = itemPM.MaxAmount;
            itemPoco.IsEditedByUser = itemPM.IsEditedByUser;
            itemPoco.ShipmentPayableParentId = itemPM.ShipmentPayableParentId;
            itemPoco.CorrectionByUserId = itemPM.CorrectionByUserId;
            itemPoco.CorrectionDate = itemPM.CorrectionDate;
            itemPoco.IATACodeId = itemPM.IATACodeId;
            itemPoco.QuoteCostMinAmount = itemPM.QuoteCostMinAmount;
            itemPoco.QuoteCostMaxAmount = itemPM.QuoteCostMaxAmount;
            itemPoco.QuoteChargeId = itemPM.QuoteChargeId;
            itemPoco.IsChargeBySteps = itemPM.IsChargeBySteps;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.VatTypeId = itemPM.VatTypeId;
            itemPoco.IsBackToBack = itemPM.IsBackToBack;
            itemPoco.ReceivableId = itemPM.ReceivableId;
            itemPoco.TariffId = itemPM.TariffId;
            itemPoco.TariffNumber = itemPM.TariffNumber;
            itemPoco.TariffLineId = itemPM.TariffLineId;
            itemPoco.TariffVersion = itemPM.TariffVersion;
            itemPoco.IsCustomsChargesTariff = itemPM.IsCustomsChargesTariff;
            itemPoco.VatAmountLocal = itemPM.VatAmountLocal;
            itemPoco.VatAmountProfit = itemPM.VatAmountProfit;
            itemPoco.ProratedAmountInLocalCurrency = itemPM.ProratedAmountInLocalCurrency;
            itemPoco.ProratedAmountInProfitCurrency = itemPM.ProratedAmountInProfitCurrency;
        }
    }
}
