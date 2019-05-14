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
        private void GenerateDefaultCharges()
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
                        LastRate freightChargeRate = myQuery.GetLastRecordByValueDate(tenant, loggedTenant.FreightCurrencyId, loggedTenant.CurrencyId, entityPM.OpenDate);
                        LastRate othersChargeRate = myQuery.GetLastRecordByValueDate(tenant, loggedTenant.OtherChargesCurrencyId, loggedTenant.CurrencyId, entityPM.OpenDate);

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
                                    UpdatedByUserId = loggedContact.Id,
                                    CostMeasurementId = chargesType.MeasurementId,
                                    SaleMeasurementId = chargesType.MeasurementId,
                                    MarkUpTypeCode = "F",
                                    MarkUpValue = 0,
                                    QuoteTypeCode = entityPM.QuoteTypeCode,
                                    SaleCurrencyId = entityPM.SaleCurrencyId,
                                    SaleExchangeRate = entityPM.ExchangeRate,
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    IsBackToBack = chargesType.IsBackToBack,
                                };

                                if (chargesType.Measurement != null)
                                {
                                    quoteChargePM.CostMeasurementCode = chargesType.Measurement.Code;
                                    quoteChargePM.SaleMeasurementCode = chargesType.Measurement.Code;
                                    quoteChargePM.CostMeasurementShortName = chargesType.Measurement.ShortName;
                                    quoteChargePM.SaleMeasurementShortName = chargesType.Measurement.ShortName;
                                }

                                if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                                {
                                    quoteChargePM.CostCurrencyId = loggedTenant.FreightCurrencyId;

                                    if (string.IsNullOrEmpty(quoteChargePM.CostCurrencyId))
                                    {
                                        quoteChargePM.CostExchangeRate = null;
                                    }

                                    else if (quoteChargePM.CostCurrencyId == loggedTenant.CurrencyId)
                                    {
                                        quoteChargePM.CostExchangeRate = 1;
                                    }

                                    else if (freightChargeRate != null)
                                    {
                                        quoteChargePM.CostExchangeRate = MethodHelper.Round(freightChargeRate.Rate, 5);
                                    }
                                }

                                else
                                {
                                    quoteChargePM.CostCurrencyId = loggedTenant.OtherChargesCurrencyId;

                                    if (string.IsNullOrEmpty(quoteChargePM.CostCurrencyId))
                                    {
                                        quoteChargePM.CostExchangeRate = null;
                                    }

                                    else if (quoteChargePM.CostCurrencyId == loggedTenant.CurrencyId)
                                    {
                                        quoteChargePM.CostExchangeRate = 1;
                                    }

                                    else if (othersChargeRate != null)
                                    {
                                        quoteChargePM.CostExchangeRate = MethodHelper.Round(othersChargeRate.Rate, 5);
                                    }
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
                                    UpdatedByUserId = loggedContact.Id,
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
                                    SaleCurrencyId = entityPM.SaleCurrencyId,
                                    SaleExchangeRate = entityPM.ExchangeRate,
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

                                if (item.ChargesGroupCode == "FRT" || item.ChargesGroupCode == "SCH")
                                {
                                    itemPM.CostCurrencyId = loggedTenant.FreightCurrencyId;

                                    if (string.IsNullOrEmpty(itemPM.CostCurrencyId))
                                    {
                                        itemPM.CostExchangeRate = null;
                                    }

                                    else if (itemPM.CostCurrencyId == loggedTenant.CurrencyId)
                                    {
                                        itemPM.CostExchangeRate = 1;
                                    }

                                    else if (freightChargeRate != null)
                                    {
                                        itemPM.CostExchangeRate = MethodHelper.Round(freightChargeRate.Rate, 5);
                                    }
                                }

                                else
                                {
                                    itemPM.CostCurrencyId = loggedTenant.OtherChargesCurrencyId;

                                    if (string.IsNullOrEmpty(itemPM.CostCurrencyId))
                                    {
                                        itemPM.CostExchangeRate = null;
                                    }

                                    else if (itemPM.CostCurrencyId == loggedTenant.CurrencyId)
                                    {
                                        itemPM.CostExchangeRate = 1;
                                    }

                                    else if (othersChargeRate != null)
                                    {
                                        itemPM.CostExchangeRate = MethodHelper.Round(othersChargeRate.Rate, 5);
                                    }
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
                                        case "QTY": { itemPM.CostQuantity = entityPM.NumberOfPackages; break; }
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
                                        case "QTY": { itemPM.SaleQuantity = entityPM.NumberOfPackages; break; }
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
        private void ComputeChargesAmounts()
        {
            if (isNewEntity)
            {
                if (entityPM.QuoteCharges.Count > 0 && !entityPM.IsHybrid)
                {
                    this.AllPackageTypes = (from d in myCommonContext.PackageTypes
                                            where d.Tenant == this.tenant
                                            select d).ToList();

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.CostMeasurementCode != "PRFR" && d.SaleMeasurementCode != "PRFR"))
                    {
                        this.ComputeLineCostQuantity(item);
                        this.ComputeLineSaleQuantity(item);
                        this.ComputeLineCostTotalAmounts(item);
                        this.ComputeLineSaleTotalAmounts(item);
                    }

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.CostMeasurementCode == "PRFR"))
                    {
                        this.ComputeLineCostQuantity(item);
                        this.ComputeLineCostTotalAmounts(item);
                    }

                    foreach (QuoteChargePM item in entityPM.QuoteCharges.Where(d => d.SaleMeasurementCode == "PRFR"))
                    {
                        this.ComputeLineSaleQuantity(item);
                        this.ComputeLineSaleTotalAmounts(item);
                    }

                    this.ComputeQuoteEstimateProfit();
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
                    if (item.CostMeasurementCode == "PRVL" || item.CostMeasurementCode == "PRFR")
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
                    if (item.SaleMeasurementCode == "PRVL" || item.SaleMeasurementCode == "PRFR")
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

            else
            {
                if (item.CostTotalAmountLocal != null && this.entityPM.ExchangeRate != null)
                {
                    myResult = item.CostTotalAmountLocal / this.entityPM.ExchangeRate;
                }
            }

            item.CostAmountInSaleCurrency = MethodHelper.Round(myResult, 2);
        }
        private void ComputeQuoteEstimateProfit()
        {
            double? myResult = null;

            double? myCostAmountLocal = MethodHelper.Round(this.entityPM.QuoteCharges.Sum(s => s.CostTotalAmountLocal), 2);
            double? mySaleAmountLocal = MethodHelper.Round(this.entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(s => s.SaleTotalAmountLocal), 2);
            double? mySaleProfitLocal = MethodHelper.Round(mySaleAmountLocal - myCostAmountLocal, 2);

            if(this.entityPM.ExchangeRate == null)
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
                bool isSameAmounts = true;

                if (this.entityPM.IsSaleCurrencySameAsCost)
                {
                    if (item.SaleCurrencyId != this.entityPM.SaleCurrencyId)
                    {
                        isSameAmounts = false;
                    }
                }

                if (isSameAmounts)
                {
                    item.SaleUnitPriceInSaleCurrency = item.SaleUnitPrice;
                    item.SaleUnitPrice1InSaleCurrency = item.SaleContainerType1UnitPrice;
                    item.SaleUnitPrice2InSaleCurrency = item.SaleContainerType2UnitPrice;
                    item.SaleUnitPrice3InSaleCurrency = item.SaleContainerType3UnitPrice;
                    item.SaleUnitPrice4InSaleCurrency = item.SaleContainerType4UnitPrice;
                    item.SaleUnitPrice5InSaleCurrency = item.SaleContainerType5UnitPrice;
                    item.SaleAmountInSaleCurrency = item.SaleTotalAmount;
                }

                else
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
        }
    }
}
