using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public partial class QuoteService
    {
        List<PackageType> AllPackageTypes = new List<PackageType>();
        public void GenerateDefaultCharges()
        {
            if (isNewEntity || entityPM.ConvertToFCL || entityPM.ConvertToLCL)
            {
                if ((entityPM.QuoteCharges.Count() == 0 && !entityPM.IsHybrid))
                {
                    IQueryable<ChargesType> iQueryable_ChargeTypes = (from d in myCommonContext.ChargesTypes
                                                                      where d.Tenant == tenant
                                                                      && d.InActive == false
                                                                      && d.IsAutoDisplayInQuote == true
                                                                      select d);

                    switch (entityPM.TransportModeId.ToUpper())
                    {
                        case "A":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsAir);
                                break;
                            }

                        case "O":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsOcean);
                                break;
                            }

                        case "I":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsInland);
                                break;
                            }
                    }

                    switch (entityPM.DirectionId.ToUpper())
                    {
                        case "E":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsExport);
                                break;
                            }

                        case "I":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsImport);
                                break;
                            }

                        case "D":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsDomestic);
                                break;
                            }

                        case "R":
                            {
                                iQueryable_ChargeTypes = iQueryable_ChargeTypes.Where(r => r.IsDrop);
                                break;
                            }
                    }

                    List<ChargesType> list_ChargeTypes = new List<ChargesType>();
                    if (this.isLCLQuote)
                    {
                        list_ChargeTypes = iQueryable_ChargeTypes.OrderBy(d => d.ViewOrder).ToList();
                    }

                    else
                    {
                        var byContainerTypeId = (from d in myCommonContext.Measurements where d.Tenant == tenant && d.Code == "BCNT" select d.Id).FirstOrDefault();
                        list_ChargeTypes = iQueryable_ChargeTypes.Where(d => d.ContainerMeasurementId == byContainerTypeId || d.ContainerMeasurementId == null).OrderBy(d => d.ViewOrder).ToList();
                    }

                    if (list_ChargeTypes.Count > 0)
                    {
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        List<CurrencyRate> AllRates = new List<CurrencyRate>();

                        AllRates.Add(this.GetCurrencyRate(loggedTenant, loggedTenant.FreightCurrencyId, myQuery));
                        AllRates.Add(this.GetCurrencyRate(loggedTenant, loggedTenant.OtherChargesCurrencyId, myQuery));

                        if (this.isLCLQuote)
                        {
                            #region
                            foreach (ChargesType chargesType in list_ChargeTypes)
                            {
                                QuoteChargePM quoteChargePM = new QuoteChargePM()
                                {
                                    Tenant = tenant,
                                    QuoteId = entityPM.Id,
                                    ChargesTypeId = chargesType.Id,
                                    ChargesTypeCode = chargesType.Code,
                                    ChargesTypeName = chargesType.EnglishName,
                                    ChargesGroupCode = chargesType.ChargesGroupCode,
                                    UpdatedByUserId = initializer.LoggedContactId,
                                    CostMeasurementId = chargesType.MeasurementId,
                                    SaleMeasurementId = chargesType.MeasurementId,
                                    MarkUpTypeCode = "F",
                                    MarkUpValue = 0,
                                    QuoteTypeCode = entityPM.QuoteTypeCode,
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    IsBackToBack = chargesType.IsBackToBack,
                                };

                                if (chargesType.MeasurementId != null)
                                {
                                    quoteChargePM.CostMeasurementId = chargesType.MeasurementId;
                                    quoteChargePM.SaleMeasurementId = chargesType.MeasurementId;
                                    Measurement iMeasurement = (from d in myCommonContext.Measurements where d.Id == chargesType.MeasurementId select d).FirstOrDefault();

                                    if (iMeasurement != null)
                                    {
                                        quoteChargePM.CostMeasurementCode = iMeasurement.Code;
                                        quoteChargePM.SaleMeasurementCode = iMeasurement.Code;
                                        quoteChargePM.CostMeasurementShortName = iMeasurement.ShortName;
                                        quoteChargePM.SaleMeasurementShortName = iMeasurement.ShortName;
                                    }
                                }


                                if (!string.IsNullOrEmpty(chargesType.PayablesDefaultCurrencyId))
                                {
                                    quoteChargePM.CostCurrencyId = chargesType.PayablesDefaultCurrencyId;
                                }

                                else
                                {
                                    if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                                    {
                                        quoteChargePM.CostCurrencyId = loggedTenant.FreightCurrencyId;
                                    }

                                    else
                                    {
                                        quoteChargePM.CostCurrencyId = loggedTenant.OtherChargesCurrencyId;
                                    }
                                }

                                if (quoteChargePM.CostCurrencyId != null)
                                {
                                    CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == quoteChargePM.CostCurrencyId).FirstOrDefault();
                                    if (iCurrencyRate == null)
                                    {
                                        iCurrencyRate = this.GetCurrencyRate(loggedTenant, quoteChargePM.CostCurrencyId, myQuery);
                                        AllRates.Add(iCurrencyRate);
                                    }

                                    if (iCurrencyRate != null)
                                    {
                                        quoteChargePM.CostExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                                    }
                                }

                                if (entityPM.IsSaleCurrencySameAsCost)
                                {
                                    quoteChargePM.SaleCurrencyId = quoteChargePM.CostCurrencyId;
                                    quoteChargePM.SaleExchangeRate = quoteChargePM.CostExchangeRate;
                                }

                                else if (entityPM.IsMultiCurrency)
                                {
                                    if (!string.IsNullOrEmpty(chargesType.ReceivablesDefaultCurrencyId))
                                    {
                                        quoteChargePM.SaleCurrencyId = chargesType.ReceivablesDefaultCurrencyId;
                                    }

                                    else
                                    {
                                        if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                                        {
                                            quoteChargePM.SaleCurrencyId = loggedTenant.FreightCurrencyId;
                                        }

                                        else
                                        {
                                            quoteChargePM.SaleCurrencyId = loggedTenant.OtherChargesCurrencyId;
                                        }
                                    }

                                    // Get Rate
                                    if (quoteChargePM.SaleCurrencyId != null)
                                    {
                                        CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == quoteChargePM.SaleCurrencyId).FirstOrDefault();
                                        if (iCurrencyRate == null)
                                        {
                                            iCurrencyRate = this.GetCurrencyRate(loggedTenant, quoteChargePM.SaleCurrencyId, myQuery);
                                            AllRates.Add(iCurrencyRate);
                                        }

                                        if (iCurrencyRate != null)
                                        {
                                            quoteChargePM.SaleExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                                        }
                                    }
                                }

                                else
                                {
                                    quoteChargePM.SaleCurrencyId = entityPM.SaleCurrencyId;
                                    quoteChargePM.SaleExchangeRate = entityPM.ExchangeRate;
                                }

                                if (entityPM.QuoteTypeCode == "A")
                                {
                                    switch (quoteChargePM.CostMeasurementCode)
                                    {
                                        case "GRWT": { quoteChargePM.CostQuantity = entityPM.GrossWeight; break; }
                                        case "CHWT": { quoteChargePM.CostQuantity = entityPM.ChargeableWeight; break; }
                                        case "VOLU": { quoteChargePM.CostQuantity = entityPM.Volume; break; }
                                        case "FIXD": { quoteChargePM.CostQuantity = 1; break; }
                                        case "BCNT": { quoteChargePM.CostQuantity = null; break; }
                                        case "BTEU": { quoteChargePM.CostQuantity = entityPM.TEU; break; }
                                        case "PRVL": { quoteChargePM.CostQuantity = entityPM.ValueOfGoods; break; }
                                        case "QTY": { quoteChargePM.CostQuantity = entityPM.NumberOfPackages; break; }
                                        case "CWKG": { quoteChargePM.CostQuantity = entityPM.ChargeableWeightInKG; break; }
                                        case "GWKG": { quoteChargePM.CostQuantity = entityPM.GrossWeightInKG; break; }
                                        case "PDCW": { quoteChargePM.CostQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                        default: { break; }
                                    }

                                    switch (quoteChargePM.SaleMeasurementCode)
                                    {
                                        case "GRWT": { quoteChargePM.SaleQuantity = entityPM.GrossWeight; break; }
                                        case "CHWT": { quoteChargePM.SaleQuantity = entityPM.ChargeableWeight; break; }
                                        case "VOLU": { quoteChargePM.SaleQuantity = entityPM.Volume; break; }
                                        case "FIXD": { quoteChargePM.SaleQuantity = 1; break; }
                                        case "BCNT": { quoteChargePM.SaleQuantity = null; break; }
                                        case "BTEU": { quoteChargePM.SaleQuantity = entityPM.TEU; break; }
                                        case "PRVL": { quoteChargePM.SaleQuantity = entityPM.ValueOfGoods; break; }
                                        case "QTY": { quoteChargePM.SaleQuantity = entityPM.NumberOfPackages; break; }
                                        case "CWKG": { quoteChargePM.SaleQuantity = entityPM.ChargeableWeightInKG; break; }
                                        case "GWKG": { quoteChargePM.SaleQuantity = entityPM.GrossWeightInKG; break; }
                                        case "PDCW": { quoteChargePM.SaleQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                        default: { break; }
                                    }

                                    if (entityPM.IsChargesByVAT)
                                    {
                                        quoteChargePM.VatTypeId = chargesType.VatTypeId;

                                        if (quoteChargePM.VatTypeId != null)
                                        {
                                            VatType myVatType = this.allVatTypes.Where(d => d.Id == quoteChargePM.VatTypeId).FirstOrDefault();
                                            if (myVatType != null)
                                            {
                                                quoteChargePM.VatTypeName = myVatType.EnglishName;
                                                quoteChargePM.VatIsMultiPercentage = myVatType.IsMultiPercentage;

                                                if (myVatType.IsMultiPercentage)
                                                {

                                                }

                                                else
                                                {
                                                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == quoteChargePM.VatTypeId).FirstOrDefault();
                                                    if (myPercentagePM != null)
                                                    {
                                                        quoteChargePM.VatPercentage = myPercentagePM.Percentage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                entityPM.QuoteCharges.Add(quoteChargePM);
                            }
                            #endregion
                        }

                        else
                        {
                            #region
                            foreach (ChargesType item in list_ChargeTypes)
                            {
                                QuoteChargePM itemPM = new QuoteChargePM()
                                {
                                    Tenant = tenant,
                                    QuoteId = entityPM.Id,
                                    ChargesTypeId = item.Id,
                                    ChargesTypeCode = item.Code,
                                    ChargesTypeName = item.EnglishName,
                                    UpdatedByUserId = initializer.LoggedContactId,
                                    ChargesGroupCode = item.ChargesGroupCode,
                                    MarkUpTypeCode = "F",
                                    ContainerType1MarkUpTypeCode = "F",
                                    ContainerType2MarkUpTypeCode = "F",
                                    ContainerType3MarkUpTypeCode = "F",
                                    ContainerType4MarkUpTypeCode = "F",
                                    ContainerType5MarkUpTypeCode = "F",
                                    MarkUpValue = 0,
                                    ContainerType1MarkUpValue = 0,
                                    ContainerType2MarkUpValue = 0,
                                    ContainerType3MarkUpValue = 0,
                                    ContainerType4MarkUpValue = 0,
                                    ContainerType5MarkUpValue = 0,
                                    QuoteTypeCode = entityPM.QuoteTypeCode,
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    IsBackToBack = item.IsBackToBack,
                                };

                                Measurement iMeasurement = null;
                                if (item.ContainerMeasurementId != null)
                                {
                                    itemPM.CostMeasurementId = item.ContainerMeasurementId;
                                    itemPM.SaleMeasurementId = item.ContainerMeasurementId;
                                    iMeasurement = (from d in myCommonContext.Measurements where d.Id == item.ContainerMeasurementId select d).FirstOrDefault();
                                }

                                else if (item.MeasurementId != null)
                                {
                                    itemPM.CostMeasurementId = item.MeasurementId;
                                    itemPM.SaleMeasurementId = item.MeasurementId;
                                    iMeasurement = (from d in myCommonContext.Measurements where d.Id == item.MeasurementId select d).FirstOrDefault();
                                }

                                if (iMeasurement != null)
                                {
                                    itemPM.CostMeasurementCode = iMeasurement.Code;
                                    itemPM.SaleMeasurementCode = iMeasurement.Code;
                                    itemPM.CostMeasurementShortName = iMeasurement.ShortName;
                                    itemPM.SaleMeasurementShortName = iMeasurement.ShortName;
                                }

                                if (!string.IsNullOrEmpty(item.PayablesDefaultCurrencyId))
                                {
                                    itemPM.CostCurrencyId = item.PayablesDefaultCurrencyId;
                                }

                                else
                                {
                                    if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH")
                                    {
                                        itemPM.CostCurrencyId = loggedTenant.FreightCurrencyId;
                                    }

                                    else
                                    {
                                        itemPM.CostCurrencyId = loggedTenant.OtherChargesCurrencyId;

                                    }
                                }

                                if (itemPM.CostCurrencyId != null)
                                {
                                    CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == itemPM.CostCurrencyId).FirstOrDefault();
                                    if (iCurrencyRate == null)
                                    {
                                        iCurrencyRate = this.GetCurrencyRate(loggedTenant, itemPM.CostCurrencyId, myQuery);
                                        AllRates.Add(iCurrencyRate);
                                    }

                                    if (iCurrencyRate != null)
                                    {
                                        itemPM.CostExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                                    }
                                }

                                if (entityPM.IsSaleCurrencySameAsCost)
                                {
                                    itemPM.SaleCurrencyId = itemPM.CostCurrencyId;
                                    itemPM.SaleExchangeRate = itemPM.CostExchangeRate;
                                }

                                else if (entityPM.IsMultiCurrency)
                                {
                                    if (!string.IsNullOrEmpty(item.ReceivablesDefaultCurrencyId))
                                    {
                                        itemPM.SaleCurrencyId = item.ReceivablesDefaultCurrencyId;
                                    }

                                    else
                                    {
                                        if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH")
                                        {
                                            itemPM.SaleCurrencyId = loggedTenant.FreightCurrencyId;
                                        }

                                        else
                                        {
                                            itemPM.SaleCurrencyId = loggedTenant.OtherChargesCurrencyId;
                                        }
                                    }

                                    // Get Rate
                                    if (itemPM.SaleCurrencyId != null)
                                    {
                                        CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == itemPM.SaleCurrencyId).FirstOrDefault();
                                        if (iCurrencyRate == null)
                                        {
                                            iCurrencyRate = this.GetCurrencyRate(loggedTenant, itemPM.SaleCurrencyId, myQuery);
                                            AllRates.Add(iCurrencyRate);
                                        }

                                        if (iCurrencyRate != null)
                                        {
                                            itemPM.SaleExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                                        }
                                    }
                                }

                                else
                                {
                                    itemPM.SaleCurrencyId = entityPM.SaleCurrencyId;
                                    itemPM.SaleExchangeRate = entityPM.ExchangeRate;
                                }

                                if (entityPM.QuoteTypeCode == "A")
                                {
                                    switch (itemPM.CostMeasurementCode)
                                    {
                                        case "GRWT": { itemPM.CostQuantity = entityPM.GrossWeight; break; }
                                        case "CHWT": { itemPM.CostQuantity = entityPM.ChargeableWeight; break; }
                                        case "VOLU": { itemPM.CostQuantity = entityPM.Volume; break; }
                                        case "FIXD": { itemPM.CostQuantity = 1; break; }
                                        case "BCNT": { itemPM.CostQuantity = null; break; }
                                        case "BTEU": { itemPM.CostQuantity = entityPM.TEU; break; }
                                        case "PRVL": { itemPM.CostQuantity = entityPM.ValueOfGoods; break; }
                                        case "QTY": { itemPM.CostQuantity = entityPM.NumberOfContainers; break; }
                                        case "CWKG": { itemPM.CostQuantity = entityPM.ChargeableWeightInKG; break; }
                                        case "GWKG": { itemPM.CostQuantity = entityPM.GrossWeightInKG; break; }
                                        case "PDCW": { itemPM.CostQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                        default: { break; }
                                    }

                                    switch (itemPM.SaleMeasurementCode)
                                    {
                                        case "GRWT": { itemPM.SaleQuantity = entityPM.GrossWeight; break; }
                                        case "CHWT": { itemPM.SaleQuantity = entityPM.ChargeableWeight; break; }
                                        case "VOLU": { itemPM.SaleQuantity = entityPM.Volume; break; }
                                        case "FIXD": { itemPM.SaleQuantity = 1; break; }
                                        case "BCNT": { itemPM.SaleQuantity = null; break; }
                                        case "BTEU": { itemPM.SaleQuantity = entityPM.TEU; break; }
                                        case "PRVL": { itemPM.SaleQuantity = entityPM.ValueOfGoods; break; }
                                        case "QTY": { itemPM.SaleQuantity = entityPM.NumberOfContainers; break; }
                                        case "CWKG": { itemPM.SaleQuantity = entityPM.ChargeableWeightInKG; break; }
                                        case "GWKG": { itemPM.SaleQuantity = entityPM.GrossWeightInKG; break; }
                                        case "PDCW": { itemPM.SaleQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                        default: { break; }
                                    }

                                    if (entityPM.IsChargesByVAT)
                                    {
                                        itemPM.VatTypeId = item.VatTypeId;

                                        if (itemPM.VatTypeId != null)
                                        {
                                            VatType myVatType = this.allVatTypes.Where(d => d.Id == itemPM.VatTypeId).FirstOrDefault();
                                            if (myVatType != null)
                                            {
                                                itemPM.VatTypeName = myVatType.EnglishName;
                                                itemPM.VatIsMultiPercentage = myVatType.IsMultiPercentage;

                                                if (myVatType.IsMultiPercentage)
                                                {

                                                }

                                                else
                                                {
                                                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemPM.VatTypeId).FirstOrDefault();
                                                    if (myPercentagePM != null)
                                                    {
                                                        itemPM.VatPercentage = myPercentagePM.Percentage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                entityPM.QuoteCharges.Add(itemPM);
                            }
                            #endregion
                        }
                    }
                }
            }
        }
        public void ComputeChargesAmounts()
        {
            if (isNewEntity)
            {
                if (entityPM.QuoteCharges.Count > 0 && !entityPM.IsHybrid)
                {
                    this.AllPackageTypes = (from d in myCommonContext.PackageTypes
                                            where d.Tenant == this.tenant
                                            select d).ToList();

                    if (!this.entityPM.IsCopyExchangeRates)
                    {
                        this.GetAllItemsExchangeRate();
                    }

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.CostMeasurementCode != "PRFR" && d.SaleMeasurementCode != "PRFR"))
                    {
                        this.ComputeLineCostQuantity(item);
                        this.ComputeLineSaleQuantity(item);
                        this.ComputeLineCostUnitPrice(item);
                        this.ComputeLineSaleUnitPrice(item);
                        this.ComputeLineCostTotalAmounts(item);
                        this.ComputeLineSaleTotalAmounts(item);
                    }

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.CostMeasurementCode == "PRFR"))
                    {
                        this.ComputeLineCostQuantity(item);
                        this.ComputeLineCostUnitPrice(item);
                        this.ComputeLineCostTotalAmounts(item);
                    }

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.SaleMeasurementCode == "PRFR"))
                    {
                        this.ComputeLineSaleQuantity(item);
                        this.ComputeLineSaleUnitPrice(item);
                        this.ComputeLineSaleTotalAmounts(item);
                    }

                    QuoteChargePM itemFreight = entityPM.QuoteCharges.Where(d => d.ChargesGroupCode == "FRT").FirstOrDefault();
                    if (itemFreight != null)
                    {
                        foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.ChargesGroupCode != "FRT" && d.IsAllIN))
                        {
                            this.ComputeLineAllInAmount(item, itemFreight);
                        }
                    }

                    this.ComputeQuoteEstimateProfit();
                }
            }
        }



        private void GetAllItemsExchangeRate()
        {
            RatesTableQuery myQuery = new RatesTableQuery(tenant);
            List<CurrencyRate> AllRates = new List<CurrencyRate>();
            foreach (QuoteChargePM item in entityPM.QuoteCharges)
            {
                if (item.CostCurrencyId != null)
                {
                    CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == item.CostCurrencyId).FirstOrDefault();
                    if (iCurrencyRate == null)
                    {
                        iCurrencyRate = this.GetCurrencyRate(loggedTenant, item.CostCurrencyId, myQuery);
                        AllRates.Add(iCurrencyRate);
                    }

                    if (iCurrencyRate != null)
                    {
                        item.CostExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                    }
                }

                if (item.SaleCurrencyId != null)
                {
                    CurrencyRate iCurrencyRate = AllRates.Where(d => d.Id == item.SaleCurrencyId).FirstOrDefault();
                    if (iCurrencyRate == null)
                    {
                        iCurrencyRate = this.GetCurrencyRate(loggedTenant, item.SaleCurrencyId, myQuery);
                        AllRates.Add(iCurrencyRate);
                    }

                    if (iCurrencyRate != null)
                    {
                        item.SaleExchangeRate = MethodHelper.Round(iCurrencyRate.Rate, 5);
                    }
                }
            }
        }

        private void ComputeLineCostQuantity(QuoteChargePM item)
        {
            double? myResult = null;

            if (this.isAdhoc)
            {
                switch (item.CostMeasurementCode)
                {
                    case "GRWT": { myResult = entityPM.GrossWeight; break; }
                    case "CHWT": { myResult = entityPM.ChargeableWeight; break; }
                    case "VOLU": { myResult = entityPM.Volume; break; }
                    case "BTEU": { myResult = entityPM.TEU; break; }
                    case "FIXD": { myResult = 1; break; }
                    case "PRVL": { myResult = entityPM.ValueOfGoods; break; }
                    case "PRFR": { myResult = entityPM.QuoteCharges.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.CostTotalAmount); break; }
                    case "QTY": { myResult = this.isFCLQuote ? entityPM.NumberOfContainers : entityPM.NumberOfPackages; break; }
                    case "CWKG": { myResult = entityPM.ChargeableWeightInKG; break; }
                    case "GWKG": { myResult = entityPM.GrossWeightInKG; break; }
                    case "PDCW": { myResult = entityPM.PickupDeliveryChargeableWeight; break; }
                    case "PFCL": { myResult = entityPM.QuoteCharges.Where(d => d.CostCurrencyId != loggedTenant.CurrencyId && d.CostMeasurementCode != "PFCL" && d.CostMeasurementCode != "PFCL").Sum(s => s.CostTotalAmountLocal); break; }
                    default:
                        {
                            if (this.isFCLQuote)
                            {
                                if (!string.IsNullOrEmpty(item.CostMeasurementId))
                                {
                                    PackageType myPackageType = this.AllPackageTypes.Where(d => d.MeasurementId == item.CostMeasurementId).FirstOrDefault();
                                    if (myPackageType != null)
                                    {
                                        myResult = 0;

                                        if (myPackageType.Id == this.entityPM.PackageType1Id) { myResult = this.entityPM.PackageType1Quantity == null ? myResult : myResult + this.entityPM.PackageType1Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType2Id) { myResult = this.entityPM.PackageType2Quantity == null ? myResult : myResult + this.entityPM.PackageType2Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType3Id) { myResult = this.entityPM.PackageType3Quantity == null ? myResult : myResult + this.entityPM.PackageType3Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType4Id) { myResult = this.entityPM.PackageType4Quantity == null ? myResult : myResult + this.entityPM.PackageType4Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType5Id) { myResult = this.entityPM.PackageType5Quantity == null ? myResult : myResult + this.entityPM.PackageType5Quantity; }

                                        myResult = myResult == 0 ? null : myResult;
                                    }
                                }
                            }

                            break;
                        }
                }
            }

            item.CostQuantity = MethodHelper.Round(myResult, 2);
        }
        private void ComputeLineSaleQuantity(QuoteChargePM item)
        {
            double? myResult = null;

            if (this.isAdhoc)
            {
                switch (item.SaleMeasurementCode)
                {
                    case "GRWT": { myResult = entityPM.GrossWeight; break; }
                    case "CHWT": { myResult = entityPM.ChargeableWeight; break; }
                    case "VOLU": { myResult = entityPM.Volume; break; }
                    case "BTEU": { myResult = entityPM.TEU; break; }
                    case "FIXD": { myResult = 1; break; }
                    case "PRVL": { myResult = entityPM.ValueOfGoods; break; }
                    case "PRFR": { myResult = entityPM.QuoteCharges.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.SaleTotalAmount); break; }
                    case "QTY": { myResult = this.isFCLQuote ? entityPM.NumberOfContainers : entityPM.NumberOfPackages; break; }
                    case "CWKG": { myResult = entityPM.ChargeableWeightInKG; break; }
                    case "GWKG": { myResult = entityPM.GrossWeightInKG; break; }
                    case "PDCW": { myResult = entityPM.PickupDeliveryChargeableWeight; break; }
                    case "PFCL": { myResult = entityPM.QuoteCharges.Where(d => d.SaleCurrencyId != loggedTenant.CurrencyId && d.SaleMeasurementCode != "PFCL" && d.SaleMeasurementCode != "PFCL").Sum(s => s.SaleTotalAmountLocal); break; }
                    default:
                        {
                            if (this.isFCLQuote)
                            {
                                if (!string.IsNullOrEmpty(item.SaleMeasurementId))
                                {
                                    PackageType myPackageType = this.AllPackageTypes.Where(d => d.MeasurementId == item.SaleMeasurementId).FirstOrDefault();
                                    if (myPackageType != null)
                                    {
                                        myResult = 0;

                                        if (myPackageType.Id == this.entityPM.PackageType1Id) { myResult = this.entityPM.PackageType1Quantity == null ? myResult : myResult + this.entityPM.PackageType1Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType2Id) { myResult = this.entityPM.PackageType2Quantity == null ? myResult : myResult + this.entityPM.PackageType2Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType3Id) { myResult = this.entityPM.PackageType3Quantity == null ? myResult : myResult + this.entityPM.PackageType3Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType4Id) { myResult = this.entityPM.PackageType4Quantity == null ? myResult : myResult + this.entityPM.PackageType4Quantity; }
                                        if (myPackageType.Id == this.entityPM.PackageType5Id) { myResult = this.entityPM.PackageType5Quantity == null ? myResult : myResult + this.entityPM.PackageType5Quantity; }

                                        myResult = myResult == 0 ? null : myResult;
                                    }
                                }
                            }

                            break;
                        }
                }
            }

            item.SaleQuantity = MethodHelper.Round(myResult, 2);
        }

        private void ComputeLineCostUnitPrice(QuoteChargePM item)
        {
            if (this.entityPM.IsSaleCurrencySameAsCost)
            {
                item.CostUnitPriceInSaleCurrency = item.CostUnitPrice;
                item.CostUnitPrice1InSaleCurrency = item.CostContainerType1UnitPrice;
                item.CostUnitPrice2InSaleCurrency = item.CostContainerType2UnitPrice;
                item.CostUnitPrice3InSaleCurrency = item.CostContainerType3UnitPrice;
                item.CostUnitPrice4InSaleCurrency = item.CostContainerType4UnitPrice;
                item.CostUnitPrice5InSaleCurrency = item.CostContainerType5UnitPrice;
            }

            else
            {
                double? saleExchangeRate = null;

                if (this.entityPM.IsMultiCurrency)
                {
                    saleExchangeRate = item.SaleExchangeRate;
                }

                else
                {
                    saleExchangeRate = this.entityPM.ExchangeRate;
                }

                item.CostUnitPriceInSaleCurrency = MethodHelper.Round(item.CostUnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
                item.CostUnitPrice1InSaleCurrency = MethodHelper.Round(item.CostContainerType1UnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
                item.CostUnitPrice2InSaleCurrency = MethodHelper.Round(item.CostContainerType2UnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
                item.CostUnitPrice3InSaleCurrency = MethodHelper.Round(item.CostContainerType3UnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
                item.CostUnitPrice4InSaleCurrency = MethodHelper.Round(item.CostContainerType4UnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
                item.CostUnitPrice5InSaleCurrency = MethodHelper.Round(item.CostContainerType5UnitPrice * item.CostExchangeRate / saleExchangeRate, 3);
            }
        }
        private void ComputeLineSaleUnitPrice(QuoteChargePM item)
        {
            if (item.CostUnitPriceInSaleCurrency != null)
            {
                item.SaleUnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPriceInSaleCurrency, item.MarkUpValue, item.MarkUpTypeCode);
            }

            if (item.CostUnitPrice1InSaleCurrency != null)
            {
                item.SaleContainerType1UnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPrice1InSaleCurrency, item.ContainerType1MarkUpValue, item.ContainerType1MarkUpTypeCode);
            }

            if (item.CostUnitPrice2InSaleCurrency != null)
            {
                item.SaleContainerType2UnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPrice2InSaleCurrency, item.ContainerType2MarkUpValue, item.ContainerType2MarkUpTypeCode);
            }

            if (item.CostUnitPrice3InSaleCurrency != null)
            {
                item.SaleContainerType3UnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPrice3InSaleCurrency, item.ContainerType3MarkUpValue, item.ContainerType3MarkUpTypeCode);
            }

            if (item.CostUnitPrice4InSaleCurrency != null)
            {
                item.SaleContainerType4UnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPrice4InSaleCurrency, item.ContainerType4MarkUpValue, item.ContainerType4MarkUpTypeCode);
            }

            if (item.CostUnitPrice5InSaleCurrency != null)
            {
                item.SaleContainerType5UnitPrice = this.ComputeLineSaleUnitPrice(item.CostUnitPrice5InSaleCurrency, item.ContainerType5MarkUpValue, item.ContainerType5MarkUpTypeCode);
            }
        }

        private double? ComputeLineSaleUnitPrice(double? costUnitPriceInSaleCurrency, double? markUpValue, string markUpTypeCode)
        {
            double? myResult = costUnitPriceInSaleCurrency;


            if (costUnitPriceInSaleCurrency != null)
            {
                double markup = markUpValue == null ? 0 : markUpValue.Value;

                if (markUpTypeCode == "P")
                {
                    myResult = costUnitPriceInSaleCurrency + (costUnitPriceInSaleCurrency * (markup / 100));
                }

                else
                {
                    myResult = costUnitPriceInSaleCurrency + markup;
                }
            }

            if (myResult == 0)
            {
                myResult = null;
            }

            return myResult;
        }

        private void ComputeLineCostTotalAmounts(QuoteChargePM item)
        {
            double? myTotalAmount = null;

            if (item.CostMeasurementCode == "BCNT")
            {
                if (this.isFCLQuote)
                {
                    if (item.CostContainerType1UnitPrice != null && this.entityPM.PackageType1Quantity != null)
                    {
                        var R1 = item.CostContainerType1UnitPrice * this.entityPM.PackageType1Quantity;
                        myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
                    }

                    if (item.CostContainerType2UnitPrice != null && this.entityPM.PackageType2Quantity != null)
                    {
                        var R2 = item.CostContainerType2UnitPrice * this.entityPM.PackageType2Quantity;
                        myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
                    }

                    if (item.CostContainerType3UnitPrice != null && this.entityPM.PackageType3Quantity != null)
                    {
                        var R3 = item.CostContainerType3UnitPrice * this.entityPM.PackageType3Quantity;
                        myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
                    }

                    if (item.CostContainerType4UnitPrice != null && this.entityPM.PackageType4Quantity != null)
                    {
                        var R4 = item.CostContainerType4UnitPrice * this.entityPM.PackageType4Quantity;
                        myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
                    }

                    if (item.CostContainerType5UnitPrice != null && this.entityPM.PackageType5Quantity != null)
                    {
                        var R5 = item.CostContainerType5UnitPrice * this.entityPM.PackageType5Quantity;
                        myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
                    }
                }
            }

            else
            {
                if (item.CostQuantity != null && item.CostUnitPrice != null)
                {
                    if (item.CostMeasurementCode == "PRVL" || item.CostMeasurementCode == "PRFR" || item.CostMeasurementCode == "PFCL")
                    {
                        myTotalAmount = item.CostQuantity * item.CostUnitPrice / 100;
                    }

                    else
                    {
                        myTotalAmount = item.CostQuantity * item.CostUnitPrice;
                    }
                }

                /* MinMax */
                if (myTotalAmount != null)
                {
                    if (item.CostMinAmount != null)
                    {
                        if (myTotalAmount < item.CostMinAmount)
                        {
                            myTotalAmount = item.CostMinAmount;
                        }
                    }

                    if (item.CostMaxAmount != null)
                    {
                        if (myTotalAmount > item.CostMaxAmount)
                        {
                            myTotalAmount = item.CostMaxAmount;
                        }
                    }
                }
            }

            // Amounts
            if (myTotalAmount == null)
            {
                item.CostTotalAmount = null;
                item.CostTotalAmountLocal = null;
                item.CostAmountInSaleCurrency = null;
            }

            else
            {
                item.CostTotalAmount = MethodHelper.Round(myTotalAmount, 2);

                if (item.CostExchangeRate == null)
                {
                    item.CostTotalAmountLocal = null;
                }

                else
                {
                    item.CostTotalAmountLocal = MethodHelper.Round(myTotalAmount * item.CostExchangeRate, 2);
                }

                this.ComputeLineCostAmountInSaleCurrency(item);
            }
        }
        private void ComputeLineSaleTotalAmounts(QuoteChargePM item)
        {
            double? myTotalAmount = null;

            if (item.SaleMeasurementCode == "BCNT")
            {
                if (this.isFCLQuote)
                {
                    if (item.SaleContainerType1UnitPrice != null && this.entityPM.PackageType1Quantity != null)
                    {
                        var R1 = item.SaleContainerType1UnitPrice * this.entityPM.PackageType1Quantity;
                        myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
                    }

                    if (item.SaleContainerType2UnitPrice != null && this.entityPM.PackageType2Quantity != null)
                    {
                        var R2 = item.SaleContainerType2UnitPrice * this.entityPM.PackageType2Quantity;
                        myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
                    }

                    if (item.SaleContainerType3UnitPrice != null && this.entityPM.PackageType3Quantity != null)
                    {
                        var R3 = item.SaleContainerType3UnitPrice * this.entityPM.PackageType3Quantity;
                        myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
                    }

                    if (item.SaleContainerType4UnitPrice != null && this.entityPM.PackageType4Quantity != null)
                    {
                        var R4 = item.SaleContainerType4UnitPrice * this.entityPM.PackageType4Quantity;
                        myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
                    }

                    if (item.SaleContainerType5UnitPrice != null && this.entityPM.PackageType5Quantity != null)
                    {
                        var R5 = item.SaleContainerType5UnitPrice * this.entityPM.PackageType5Quantity;
                        myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
                    }
                }
            }

            else
            {
                if (item.SaleUnitPrice != null && item.SaleQuantity != null)
                {
                    if (item.SaleMeasurementCode == "PRVL" || item.SaleMeasurementCode == "PRFR" || item.CostMeasurementCode == "PFCL")
                    {
                        myTotalAmount = item.SaleQuantity * item.SaleUnitPrice / 100;
                    }

                    else
                    {
                        myTotalAmount = item.SaleQuantity * item.SaleUnitPrice;
                    }
                }

                /* MinMax */
                if (myTotalAmount != null)
                {
                    if (item.SaleMinAmount != null)
                    {
                        if (myTotalAmount < item.SaleMinAmount)
                        {
                            myTotalAmount = item.SaleMinAmount;
                        }
                    }

                    if (item.SaleMaxAmount != null)
                    {
                        if (myTotalAmount > item.SaleMaxAmount)
                        {
                            myTotalAmount = item.SaleMaxAmount;
                        }
                    }
                }
            }

            // Amounts
            if (myTotalAmount == null)
            {
                item.SaleTotalAmount = null;
                item.SaleTotalAmountLocal = null;
            }

            else
            {
                item.SaleTotalAmount = MethodHelper.Round(myTotalAmount, 2);

                if (item.SaleExchangeRate == null)
                {
                    item.SaleTotalAmountLocal = null;
                }

                else
                {
                    item.SaleTotalAmountLocal = MethodHelper.Round(myTotalAmount * item.SaleExchangeRate, 2);
                }
            }
        }
        private void ComputeLineCostAmountInSaleCurrency(QuoteChargePM item)
        {
            double? myResult = null;

            if (this.entityPM.IsSaleCurrencySameAsCost)
            {
                if (item.CostTotalAmount != null)
                {
                    myResult = item.CostTotalAmount;
                }
            }

            else if (entityPM.IsMultiCurrency)
            {
                if (item.CostTotalAmountLocal != null && item.SaleExchangeRate != null)
                {
                    myResult = item.CostTotalAmountLocal / item.SaleExchangeRate;
                }
            }

            else
            {
                if (item.CostTotalAmountLocal != null && this.entityPM.ExchangeRate != null)
                {
                    myResult = item.CostTotalAmountLocal / this.entityPM.ExchangeRate;
                }
            }

            item.CostAmountInSaleCurrency = MethodHelper.Round(myResult, 2);
        }

        private void ComputeLineAllInAmount(QuoteChargePM item, QuoteChargePM itemFreight)
        {
            if (item.IsAllIN && itemFreight != null)
            {
                if (this.isLCLQuote)
                {
                    this.ComputeLineAllInAmount_LCL(item, itemFreight);
                }

                else
                {
                    this.ComputeLineAllInAmount_FCL(item, itemFreight);
                }
            }
        }
        private void ComputeLineAllInAmount_LCL(QuoteChargePM item, QuoteChargePM itemFreight)
        {
            if (item.SaleTotalAmountLocal != null || itemFreight.SaleTotalAmountLocal != null)
            {
                double? itemSaleTotalAmountLocal = item.SaleTotalAmountLocal == null ? 0 : item.SaleTotalAmountLocal;
                double? itemFreightSaleTotalAmountLocal = itemFreight.SaleTotalAmountLocal == null ? 0 : itemFreight.SaleTotalAmountLocal;
                itemFreight.SaleTotalAmountLocal = MethodHelper.Round(itemFreightSaleTotalAmountLocal + itemSaleTotalAmountLocal, 2);

                double? itemTotalAmount = null;
                if (itemFreight.SaleExchangeRate != null && itemFreight.SaleExchangeRate != 0)
                {
                    itemTotalAmount = itemFreight.SaleTotalAmountLocal / itemFreight.SaleExchangeRate;
                }

                if (itemFreight.SaleMinAmount != null)
                {
                    if (itemTotalAmount == null || itemTotalAmount < itemFreight.SaleMinAmount)
                    {
                        itemTotalAmount = itemFreight.SaleMinAmount;
                    }
                }

                if (itemFreight.SaleMaxAmount != null)
                {
                    if (itemTotalAmount == null || itemTotalAmount > itemFreight.SaleMaxAmount)
                    {
                        itemTotalAmount = itemFreight.SaleMaxAmount;
                    }
                }

                double? itemUnitPrice = itemFreight.SaleUnitPrice;
                if (itemTotalAmount != null && itemFreight.SaleQuantity != null)
                {
                    itemUnitPrice = itemTotalAmount / itemFreight.SaleQuantity;
                }

                itemFreight.SaleTotalAmount = MethodHelper.Round(itemTotalAmount, 2);
                itemFreight.SaleUnitPrice = MethodHelper.Round(itemUnitPrice, 3);
            }
        }
        private void ComputeLineAllInAmount_FCL(QuoteChargePM item, QuoteChargePM itemFreight)
        {
            if (item.CostMeasurementCode == "BCNT")
            {
                double? itemTotalAmount = null;

                if (entityPM.PackageType1Id != null)
                {
                    double? itemSaleUnitPrice = item.SaleContainerType1UnitPrice == null ? 0 : item.SaleContainerType1UnitPrice;
                    double? itemFreightSaleUnitPrice = itemFreight.SaleContainerType1UnitPrice == null ? 0 : itemFreight.SaleContainerType1UnitPrice;
                    itemFreight.SaleContainerType1UnitPrice = MethodHelper.Round(itemSaleUnitPrice + itemFreightSaleUnitPrice, 3);

                    if (entityPM.PackageType1Quantity != null && itemFreight.SaleContainerType1UnitPrice != null)
                    {
                        double? itemAmount = entityPM.PackageType1Quantity * itemFreight.SaleContainerType1UnitPrice;
                        itemTotalAmount = itemTotalAmount == null ? itemAmount : itemTotalAmount + itemAmount;
                    }
                }

                if (entityPM.PackageType2Id != null)
                {
                    double? itemSaleUnitPrice = item.SaleContainerType2UnitPrice == null ? 0 : item.SaleContainerType2UnitPrice;
                    double? itemFreightSaleUnitPrice = itemFreight.SaleContainerType2UnitPrice == null ? 0 : itemFreight.SaleContainerType2UnitPrice;
                    itemFreight.SaleContainerType2UnitPrice = MethodHelper.Round(itemSaleUnitPrice + itemFreightSaleUnitPrice, 3);

                    if (entityPM.PackageType2Quantity != null && itemFreight.SaleContainerType2UnitPrice != null)
                    {
                        double? itemAmount = entityPM.PackageType2Quantity * itemFreight.SaleContainerType2UnitPrice;
                        itemTotalAmount = itemTotalAmount == null ? itemAmount : itemTotalAmount + itemAmount;
                    }
                }

                if (entityPM.PackageType3Id != null)
                {
                    double? itemSaleUnitPrice = item.SaleContainerType3UnitPrice == null ? 0 : item.SaleContainerType3UnitPrice;
                    double? itemFreightSaleUnitPrice = itemFreight.SaleContainerType3UnitPrice == null ? 0 : itemFreight.SaleContainerType3UnitPrice;
                    itemFreight.SaleContainerType3UnitPrice = MethodHelper.Round(itemSaleUnitPrice + itemFreightSaleUnitPrice, 3);

                    if (entityPM.PackageType3Quantity != null && itemFreight.SaleContainerType3UnitPrice != null)
                    {
                        double? itemAmount = entityPM.PackageType3Quantity * itemFreight.SaleContainerType3UnitPrice;
                        itemTotalAmount = itemTotalAmount == null ? itemAmount : itemTotalAmount + itemAmount;
                    }
                }

                if (entityPM.PackageType4Id != null)
                {
                    double? itemSaleUnitPrice = item.SaleContainerType4UnitPrice == null ? 0 : item.SaleContainerType4UnitPrice;
                    double? itemFreightSaleUnitPrice = itemFreight.SaleContainerType4UnitPrice == null ? 0 : itemFreight.SaleContainerType4UnitPrice;
                    itemFreight.SaleContainerType4UnitPrice = MethodHelper.Round(itemSaleUnitPrice + itemFreightSaleUnitPrice, 3);

                    if (entityPM.PackageType4Quantity != null && itemFreight.SaleContainerType4UnitPrice != null)
                    {
                        double? itemAmount = entityPM.PackageType4Quantity * itemFreight.SaleContainerType4UnitPrice;
                        itemTotalAmount = itemTotalAmount == null ? itemAmount : itemTotalAmount + itemAmount;
                    }
                }

                if (entityPM.PackageType5Id != null)
                {
                    double? itemSaleUnitPrice = item.SaleContainerType5UnitPrice == null ? 0 : item.SaleContainerType5UnitPrice;
                    double? itemFreightSaleUnitPrice = itemFreight.SaleContainerType5UnitPrice == null ? 0 : itemFreight.SaleContainerType5UnitPrice;
                    itemFreight.SaleContainerType5UnitPrice = MethodHelper.Round(itemSaleUnitPrice + itemFreightSaleUnitPrice, 3);

                    if (entityPM.PackageType5Quantity != null && itemFreight.SaleContainerType5UnitPrice != null)
                    {
                        double? itemAmount = entityPM.PackageType5Quantity * itemFreight.SaleContainerType5UnitPrice;
                        itemTotalAmount = itemTotalAmount == null ? itemAmount : itemTotalAmount + itemAmount;
                    }
                }

                if (itemTotalAmount != null)
                {
                    if (itemFreight.SaleMinAmount != null && itemTotalAmount < itemFreight.SaleMinAmount)
                    {
                        itemTotalAmount = itemFreight.SaleMinAmount;
                    }

                    if (itemFreight.SaleMaxAmount != null && itemTotalAmount < itemFreight.SaleMaxAmount)
                    {
                        itemTotalAmount = itemFreight.SaleMaxAmount;
                    }
                }

                itemFreight.SaleTotalAmount = itemTotalAmount == null ? null : MethodHelper.Round(itemTotalAmount, 2);
                itemFreight.SaleTotalAmountLocal = itemTotalAmount == null ? null : MethodHelper.Round(itemTotalAmount * itemFreight.SaleExchangeRate, 2);
            }
        }

        private void ComputeQuoteEstimateProfit()
        {
            double? myResult = null;

            double? myCostAmountLocal = MethodHelper.Round(this.entityPM.QuoteCharges.Sum(s => s.CostTotalAmountLocal), 2);
            double? mySaleAmountLocal = MethodHelper.Round(this.entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(s => s.SaleTotalAmountLocal), 2);
            double? mySaleProfitLocal = MethodHelper.Round(mySaleAmountLocal - myCostAmountLocal, 2);

            if (this.entityPM.ExchangeRate == null)
            {
                myResult = null;
            }

            else
            {
                myResult = mySaleProfitLocal / this.entityPM.ExchangeRate;
            }

            this.entityPM.EstimateProfit = MethodHelper.Round(myResult, 2);
        }
        private void ComputeChargesSaleFieldsInSaleCurrency()
        {
            List<QuoteChargePM> allCharges = new List<QuoteChargePM>();

            if (this.isNewEntity)
            {
                allCharges = this.entityPM.QuoteCharges;
            }

            else
            {
                allCharges = this.quoteChargesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

            foreach (QuoteChargePM item in allCharges)
            {
                item.SaleUnitPriceInSaleCurrency = MethodHelper.Round(item.SaleUnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleUnitPrice1InSaleCurrency = MethodHelper.Round(item.SaleContainerType1UnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleUnitPrice2InSaleCurrency = MethodHelper.Round(item.SaleContainerType2UnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleUnitPrice3InSaleCurrency = MethodHelper.Round(item.SaleContainerType3UnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleUnitPrice4InSaleCurrency = MethodHelper.Round(item.SaleContainerType4UnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleUnitPrice5InSaleCurrency = MethodHelper.Round(item.SaleContainerType5UnitPrice * item.SaleExchangeRate / this.entityPM.ExchangeRate, 3);
                item.SaleAmountInSaleCurrency = MethodHelper.Round(item.SaleTotalAmount * item.SaleExchangeRate / this.entityPM.ExchangeRate, 2);
            }
        }
        private CurrencyRate GetCurrencyRate(Tenant loggedTenant, string iCurrencyId, RatesTableQuery myQuery)
        {
            CurrencyRate iResult = new CurrencyRate() { Id = iCurrencyId };

            if (iCurrencyId == loggedTenant.CurrencyId)
            {
                iResult.Rate = 1;
            }

            else
            {
                LastRate iRate = myQuery.GetLastRecordByValueDate(tenant, iCurrencyId, loggedTenant.CurrencyId, entityPM.OpenDate);
                if (iRate != null)
                {
                    iResult.Rate = iRate.Rate;
                }
            }

            return iResult;
        }
    }


    public class CurrencyRate
    {
        public string Id { get; set; }
        public double? Rate { get; set; }
    }
}
