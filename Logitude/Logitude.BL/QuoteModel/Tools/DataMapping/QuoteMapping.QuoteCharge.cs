using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.Helpers;

using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.Helpers;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        public static void MapQuoteCharge(QuoteChargePM itemPM, QuoteCharge itemPoco, bool isNewEntity, QuotePM iQuotePM)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.QuoteId = itemPM.QuoteId;
            }

            itemPoco.ValueDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdatedByUserId = itemPM.UpdatedByUserId;
            itemPoco.Notes = itemPM.Notes;
            itemPoco.ChargesTypeId = itemPM.ChargesTypeId;
            itemPoco.VendorId = itemPM.VendorId;
            itemPoco.IsAllIN = itemPM.IsAllIN;
            itemPoco.IsChargeBySteps = itemPM.IsChargeBySteps;
            itemPoco.CostMinAmount = itemPM.CostMinAmount;
            itemPoco.CostMaxAmount = itemPM.CostMaxAmount;
            itemPoco.SaleMinAmount = itemPM.SaleMinAmount;
            itemPoco.SaleMaxAmount = itemPM.SaleMaxAmount;

            itemPoco.CostCurrencyId = itemPM.CostCurrencyId;
            itemPoco.CostExchangeRate = itemPM.CostExchangeRate;
            itemPoco.CostIsFixedRate = itemPM.CostIsFixedRate;
            itemPoco.CostMeasurementId = itemPM.CostMeasurementId;
            itemPoco.CostQuantity = itemPM.CostQuantity;
            itemPoco.CostUnitPrice = itemPM.CostUnitPrice;            
            itemPoco.CostContainerType1UnitPrice = itemPM.CostContainerType1UnitPrice;
            itemPoco.CostContainerType2UnitPrice = itemPM.CostContainerType2UnitPrice;
            itemPoco.CostContainerType3UnitPrice = itemPM.CostContainerType3UnitPrice;
            itemPoco.CostContainerType4UnitPrice = itemPM.CostContainerType4UnitPrice;
            itemPoco.CostContainerType5UnitPrice = itemPM.CostContainerType5UnitPrice;
            itemPoco.CostTotalAmount = itemPM.CostTotalAmount;
            itemPoco.CostTotalAmountLocal = itemPM.CostTotalAmountLocal;                      
            
            itemPoco.SaleCurrencyId = itemPM.SaleCurrencyId;            
            itemPoco.SaleExchangeRate = itemPM.SaleExchangeRate;
            itemPoco.SaleIsFixedRate = itemPM.SaleIsFixedRate;
            itemPoco.SaleMeasurementId = itemPM.SaleMeasurementId;
            itemPoco.SaleQuantity = itemPM.SaleQuantity;
            itemPoco.SaleUnitPrice = itemPM.SaleUnitPrice;
            itemPoco.SaleContainerType1UnitPrice = itemPM.SaleContainerType1UnitPrice;
            itemPoco.SaleContainerType2UnitPrice = itemPM.SaleContainerType2UnitPrice;
            itemPoco.SaleContainerType3UnitPrice = itemPM.SaleContainerType3UnitPrice;
            itemPoco.SaleContainerType4UnitPrice = itemPM.SaleContainerType4UnitPrice;
            itemPoco.SaleContainerType5UnitPrice = itemPM.SaleContainerType5UnitPrice;
            itemPoco.SaleTotalAmount = itemPM.SaleTotalAmount;
            itemPoco.SaleTotalAmountLocal = itemPM.SaleTotalAmountLocal;

            itemPoco.MarkUpValue = itemPM.MarkUpValue;
            itemPoco.MarkUpTypeCode = itemPM.MarkUpTypeCode;
            itemPoco.ContainerType1MarkUpValue = itemPM.ContainerType1MarkUpValue;
            itemPoco.ContainerType2MarkUpValue = itemPM.ContainerType2MarkUpValue;
            itemPoco.ContainerType3MarkUpValue = itemPM.ContainerType3MarkUpValue;
            itemPoco.ContainerType4MarkUpValue = itemPM.ContainerType4MarkUpValue;
            itemPoco.ContainerType5MarkUpValue = itemPM.ContainerType5MarkUpValue;
            itemPoco.ContainerType1MarkUpTypeCode = itemPM.ContainerType1MarkUpTypeCode;
            itemPoco.ContainerType2MarkUpTypeCode = itemPM.ContainerType2MarkUpTypeCode;
            itemPoco.ContainerType3MarkUpTypeCode = itemPM.ContainerType3MarkUpTypeCode;
            itemPoco.ContainerType4MarkUpTypeCode = itemPM.ContainerType4MarkUpTypeCode;
            itemPoco.ContainerType5MarkUpTypeCode = itemPM.ContainerType5MarkUpTypeCode;
            itemPoco.VatTypeId = itemPM.VatTypeId;
            itemPoco.VatPercentage = itemPM.VatPercentage;

            // New Fields Calculation
            itemPoco.SaleUnitPriceInSaleCurrency = itemPM.SaleUnitPriceInSaleCurrency;
            itemPoco.SaleUnitPrice1InSaleCurrency = itemPM.SaleUnitPrice1InSaleCurrency;
            itemPoco.SaleUnitPrice2InSaleCurrency = itemPM.SaleUnitPrice2InSaleCurrency;
            itemPoco.SaleUnitPrice3InSaleCurrency = itemPM.SaleUnitPrice3InSaleCurrency;
            itemPoco.SaleUnitPrice4InSaleCurrency = itemPM.SaleUnitPrice4InSaleCurrency;
            itemPoco.SaleUnitPrice5InSaleCurrency = itemPM.SaleUnitPrice5InSaleCurrency;
            itemPoco.SaleAmountInSaleCurrency = itemPM.SaleAmountInSaleCurrency;
            itemPoco.IsCostAllIn = itemPM.IsCostAllIn;
            itemPoco.TariffId = itemPM.TariffId;
            itemPoco.TariffNumber = itemPM.TariffNumber;
            itemPoco.TariffLineId = itemPM.TariffLineId;
            itemPoco.TariffVersion = itemPM.TariffVersion;
            itemPoco.IsRegionalTax = itemPM.IsRegionalTax;
        }
    }
}
