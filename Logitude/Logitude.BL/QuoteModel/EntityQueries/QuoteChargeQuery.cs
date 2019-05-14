using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteChargeQuery
    {
        QuoteChargeRepository repository;
        public QuoteChargeQuery()
        {
            repository = new QuoteChargeRepository(); 
        }
        public QuoteChargeQuery(int tenant)
        {
            repository = new QuoteChargeRepository(tenant);
        }
        public QuoteChargeQuery(QuoteChargeRepository quoteChargeRepository)
        {
            repository = quoteChargeRepository;
        }
        
        public QuoteChargePM GetSinglePM(string id, int tenant)
        {
            QuoteChargePM myResult = (from a in repository.context.QuoteCharges.Include("ChargesType").Include("VendorCard").Include("CostMeasurement").Include("SaleMeasurement").Include("CostCurrency").Include("Currency").Include("Quote").Include("VatType")
                                            where a.Tenant == tenant && a.Id == id
                                            select new QuoteChargePM()
                                            {
                                                Id = a.Id,
                                                ChargesTypeId = a.ChargesTypeId,
                                                ChargesTypeCode = a.ChargesType == null ? "" : a.ChargesType.Code,
                                                ChargesTypeName = a.ChargesType == null ? "" : a.ChargesType.EnglishName,
                                                ChargesTypeDescription = a.ChargesType == null ? "" : a.ChargesType.Description,
                                                ChargesTypeLocalName = a.ChargesType == null ? "" : a.ChargesType.LocalName,
                                                ChargesGroupCode = a.ChargesType == null ? "" : a.ChargesType.ChargesGroupCode,
                                                ViewOrder = a.ChargesType == null ? 0 : a.ChargesType.ViewOrder,
                                                VendorId = a.VendorId,
                                                VendorName = a.VendorCard == null ? String.Empty : a.VendorCard.EnglishName,
                                                CostMeasurementId = a.CostMeasurementId,
                                                CostMeasurementCode = a.CostMeasurement == null ? "" : a.CostMeasurement.Code,
                                                CostMeasurementShortName = a.CostMeasurement == null ? "" : a.CostMeasurement.ShortName,
                                                SaleMeasurementId = a.SaleMeasurementId,
                                                SaleMeasurementCode = a.SaleMeasurement == null ? "" : a.SaleMeasurement.Code,
                                                SaleMeasurementShortName = a.SaleMeasurement == null ? "" : a.SaleMeasurement.ShortName,
                                                SaleMeasurementLocalName = a.SaleMeasurement == null ? "" : a.SaleMeasurement.LocalName,

                                                CostCurrencyId = a.CostCurrencyId,
                                                CostCurrencyCode = a.CostCurrency == null ? "" : a.CostCurrency.Code,
                                                CostExchangeRate = a.CostExchangeRate,
                                                CostIsFixedRate = a.CostIsFixedRate,
                                                SaleCurrencyId = a.SaleCurrencyId,
                                                SaleCurrencyCode = a.Currency == null ? "" : a.Currency.Code,
                                                SaleExchangeRate = a.SaleExchangeRate,
                                                SaleIsFixedRate = a.SaleIsFixedRate,
                                                Notes = a.Notes,
                                                QuoteId = a.QuoteId,
                                                Tenant = a.Tenant,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                UpdateDate = a.UpdateDate,
                                                ValueDate = a.ValueDate,
                                                SaleContainerType1UnitPrice = a.SaleContainerType1UnitPrice,
                                                SaleContainerType2UnitPrice = a.SaleContainerType2UnitPrice,
                                                SaleContainerType3UnitPrice = a.SaleContainerType3UnitPrice,
                                                SaleContainerType4UnitPrice = a.SaleContainerType4UnitPrice,
                                                SaleContainerType5UnitPrice = a.SaleContainerType5UnitPrice,
                                                ContainerType1MarkUpTypeCode = a.ContainerType1MarkUpTypeCode,
                                                ContainerType2MarkUpTypeCode = a.ContainerType2MarkUpTypeCode,
                                                ContainerType3MarkUpTypeCode = a.ContainerType3MarkUpTypeCode,
                                                ContainerType4MarkUpTypeCode = a.ContainerType4MarkUpTypeCode,
                                                ContainerType5MarkUpTypeCode = a.ContainerType5MarkUpTypeCode,
                                                ContainerType1MarkUpValue = a.ContainerType1MarkUpValue,
                                                ContainerType2MarkUpValue = a.ContainerType2MarkUpValue,
                                                ContainerType3MarkUpValue = a.ContainerType3MarkUpValue,
                                                ContainerType4MarkUpValue = a.ContainerType4MarkUpValue,
                                                ContainerType5MarkUpValue = a.ContainerType5MarkUpValue,
                                                CostContainerType1UnitPrice = a.CostContainerType1UnitPrice,
                                                CostContainerType2UnitPrice = a.CostContainerType2UnitPrice,
                                                CostContainerType3UnitPrice = a.CostContainerType3UnitPrice,
                                                CostContainerType4UnitPrice = a.CostContainerType4UnitPrice,
                                                CostContainerType5UnitPrice = a.CostContainerType5UnitPrice,
                                                IsAllIN = a.IsAllIN,
                                                MarkUpTypeCode = a.MarkUpTypeCode,
                                                MarkUpValue = a.MarkUpValue,
                                                QuoteTypeCode = a.Quote.QuoteTypeCode,
                                                CostUnitPrice = a.CostUnitPrice,
                                                CostQuantity = a.CostQuantity,
                                                SaleQuantity = a.SaleQuantity,
                                                SaleTotalAmount = a.SaleTotalAmount,
                                                SaleTotalAmountLocal = a.SaleTotalAmountLocal,
                                                SaleUnitPrice = a.SaleUnitPrice,
                                                CostTotalAmount = a.CostTotalAmount,
                                                CostTotalAmountLocal = a.CostTotalAmountLocal,
                                                CostMaxAmount = a.CostMaxAmount,
                                                CostMinAmount = a.CostMinAmount,
                                                SaleMinAmount = a.SaleMinAmount,
                                                IsChargeBySteps = a.IsChargeBySteps,
                                                VatTypeId = a.VatTypeId,
                                                VatPercentage = a.VatPercentage,
                                                VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                                VatIsMultiPercentage = a.VatType == null ? false : a.VatType.IsMultiPercentage,
                                                SaleUnitPriceInSaleCurrency = a.SaleUnitPriceInSaleCurrency,
                                                SaleUnitPrice1InSaleCurrency = a.SaleUnitPrice1InSaleCurrency,
                                                SaleUnitPrice2InSaleCurrency = a.SaleUnitPrice2InSaleCurrency,
                                                SaleUnitPrice3InSaleCurrency = a.SaleUnitPrice3InSaleCurrency,
                                                SaleUnitPrice4InSaleCurrency = a.SaleUnitPrice4InSaleCurrency,
                                                SaleUnitPrice5InSaleCurrency = a.SaleUnitPrice5InSaleCurrency,
                                                SaleAmountInSaleCurrency = a.SaleAmountInSaleCurrency,
                                                SaleMaxAmount = a.SaleMaxAmount,
                                            }).FirstOrDefault();

            return myResult;
        }

        public List<QuoteChargePM> GetQuoteChargesPMsByQuoteId(string id, int tenant)
        {
            List<QuoteChargePM> myResult = (from a in repository.context.QuoteCharges.Include("ChargesType").Include("VendorCard").Include("CostMeasurement").Include("SaleMeasurement").Include("CostCurrency").Include("Currency").Include("Quote").Include("VatType")
                                          where a.Tenant == tenant && a.QuoteId == id
                                          select new QuoteChargePM()
                                          {
                                              Id = a.Id,
                                              ChargesTypeId = a.ChargesTypeId,
                                              ChargesTypeCode = a.ChargesType == null ? "" : a.ChargesType.Code,
                                              ChargesTypeName = a.ChargesType == null ? "" : a.ChargesType.EnglishName,
                                              ChargesTypeDescription = a.ChargesType == null ? "" : a.ChargesType.Description,
                                              ChargesTypeLocalName = a.ChargesType == null ? "" : a.ChargesType.LocalName,
                                              ChargesGroupCode = a.ChargesType == null ? "" : a.ChargesType.ChargesGroupCode,
                                              ViewOrder = a.ChargesType == null ? 0 : a.ChargesType.ViewOrder,
                                              VendorId = a.VendorId,
                                              VendorName = a.VendorCard == null ? String.Empty : a.VendorCard.EnglishName,
                                              CostMeasurementId = a.CostMeasurementId,
                                              CostMeasurementCode = a.CostMeasurement == null ? "" : a.CostMeasurement.Code,
                                              CostMeasurementShortName = a.CostMeasurement == null ? "" : a.CostMeasurement.ShortName,
                                              SaleMeasurementId = a.SaleMeasurementId,
                                              SaleMeasurementCode = a.SaleMeasurement == null ? "" : a.SaleMeasurement.Code,
                                              SaleMeasurementShortName = a.SaleMeasurement == null ? "" : a.SaleMeasurement.ShortName,
                                              SaleMeasurementLocalName = a.SaleMeasurement == null ? "" : a.SaleMeasurement.LocalName,
                                              CostCurrencyId = a.CostCurrencyId,
                                              CostCurrencyCode = a.CostCurrency == null ? "" : a.CostCurrency.Code,
                                              CostExchangeRate = a.CostExchangeRate,
                                              CostIsFixedRate = a.CostIsFixedRate,
                                              SaleCurrencyId = a.SaleCurrencyId,
                                              SaleCurrencyCode = a.Currency == null ? "" : a.Currency.Code,
                                              SaleExchangeRate = a.SaleExchangeRate,
                                              SaleIsFixedRate = a.SaleIsFixedRate,
                                              Notes = a.Notes,
                                              QuoteId = a.QuoteId,
                                              Tenant = a.Tenant,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              UpdateDate = a.UpdateDate,
                                              ValueDate = a.ValueDate,
                                              SaleContainerType1UnitPrice = a.SaleContainerType1UnitPrice,
                                              SaleContainerType2UnitPrice = a.SaleContainerType2UnitPrice,
                                              SaleContainerType3UnitPrice = a.SaleContainerType3UnitPrice,
                                              SaleContainerType4UnitPrice = a.SaleContainerType4UnitPrice,
                                              SaleContainerType5UnitPrice = a.SaleContainerType5UnitPrice,
                                              ContainerType1MarkUpTypeCode = a.ContainerType1MarkUpTypeCode,
                                              ContainerType2MarkUpTypeCode = a.ContainerType2MarkUpTypeCode,
                                              ContainerType3MarkUpTypeCode = a.ContainerType3MarkUpTypeCode,
                                              ContainerType4MarkUpTypeCode = a.ContainerType4MarkUpTypeCode,
                                              ContainerType5MarkUpTypeCode = a.ContainerType5MarkUpTypeCode,
                                              ContainerType1MarkUpValue = a.ContainerType1MarkUpValue,
                                              ContainerType2MarkUpValue = a.ContainerType2MarkUpValue,
                                              ContainerType3MarkUpValue = a.ContainerType3MarkUpValue,
                                              ContainerType4MarkUpValue = a.ContainerType4MarkUpValue,
                                              ContainerType5MarkUpValue = a.ContainerType5MarkUpValue,
                                              CostContainerType1UnitPrice = a.CostContainerType1UnitPrice,
                                              CostContainerType2UnitPrice = a.CostContainerType2UnitPrice,
                                              CostContainerType3UnitPrice = a.CostContainerType3UnitPrice,
                                              CostContainerType4UnitPrice = a.CostContainerType4UnitPrice,
                                              CostContainerType5UnitPrice = a.CostContainerType5UnitPrice,
                                              IsAllIN = a.IsAllIN,
                                              MarkUpTypeCode = a.MarkUpTypeCode,
                                              MarkUpValue = a.MarkUpValue,
                                              QuoteTypeCode = a.Quote.QuoteTypeCode,
                                              CostUnitPrice = a.CostUnitPrice,
                                              CostQuantity = a.CostQuantity,
                                              SaleQuantity = a.SaleQuantity,
                                              SaleTotalAmount = a.SaleTotalAmount,
                                              SaleTotalAmountLocal = a.SaleTotalAmountLocal,
                                              SaleUnitPrice = a.SaleUnitPrice,
                                              CostTotalAmount = a.CostTotalAmount,
                                              CostTotalAmountLocal = a.CostTotalAmountLocal,
                                              CostMaxAmount = a.CostMaxAmount,
                                              CostMinAmount = a.CostMinAmount,
                                              SaleMinAmount = a.SaleMinAmount,
                                              IsChargeBySteps = a.IsChargeBySteps,
                                              VatTypeId = a.VatTypeId,
                                              VatPercentage = a.VatPercentage,
                                              VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                              VatIsMultiPercentage = a.VatType == null ? false : a.VatType.IsMultiPercentage,
                                              SaleUnitPriceInSaleCurrency = a.SaleUnitPriceInSaleCurrency,
                                              SaleUnitPrice1InSaleCurrency = a.SaleUnitPrice1InSaleCurrency,
                                              SaleUnitPrice2InSaleCurrency = a.SaleUnitPrice2InSaleCurrency,
                                              SaleUnitPrice3InSaleCurrency = a.SaleUnitPrice3InSaleCurrency,
                                              SaleUnitPrice4InSaleCurrency = a.SaleUnitPrice4InSaleCurrency,
                                              SaleUnitPrice5InSaleCurrency = a.SaleUnitPrice5InSaleCurrency,
                                              SaleAmountInSaleCurrency = a.SaleAmountInSaleCurrency,
                                              SaleMaxAmount = a.SaleMaxAmount,
                                          }).ToList();

            QuotePriceStepsRepository quotePriceStepsRepository = new QuotePriceStepsRepository(this.repository.context);
            QuotePriceStepsQuery quotePriceStepsQuery = new QuotePriceStepsQuery(quotePriceStepsRepository);

            QuoteTotalVATRepository quoteTotalVATRepository = new QuoteTotalVATRepository(this.repository.context);

            List<QuoteTotalVAT> totalVats = quoteTotalVATRepository.GetTotalVATs(id, tenant).ToList();

            foreach (QuoteChargePM quoteChargePM in myResult)
            {
                if (quoteChargePM.VatTypeId != null)
                {
                    QuoteTotalVAT singleTotalVat = totalVats.Where(d => d.QuoteId == id && d.VatTypeId == quoteChargePM.VatTypeId).FirstOrDefault();
                    if (singleTotalVat != null)
                    {
                        quoteChargePM.ExternalVATCard = singleTotalVat.ExternalVATCard;
                        quoteChargePM.ExternalTAXItemId = singleTotalVat.ExternalTAXItemId;
                    }

                    if (quoteChargePM.SaleTotalAmount != null && quoteChargePM.VatPercentage != null)
                    {
                        quoteChargePM.VatAmount = quoteChargePM.SaleTotalAmount * quoteChargePM.VatPercentage / 100;
                    }
                }

                quoteChargePM.QuoteChargePriceSteps = quotePriceStepsQuery.GetQuotePriceStepPMsByQuoteChargeId(id, quoteChargePM.Id, tenant);

                #region Cost In Sale Currency
                if (quoteChargePM.CostCurrencyId == quoteChargePM.SaleCurrencyId)
                {
                    quoteChargePM.CostUnitPriceInSaleCurrency = quoteChargePM.CostUnitPrice;
                    quoteChargePM.CostUnitPrice1InSaleCurrency = quoteChargePM.CostContainerType1UnitPrice;
                    quoteChargePM.CostUnitPrice2InSaleCurrency = quoteChargePM.CostContainerType2UnitPrice;
                    quoteChargePM.CostUnitPrice3InSaleCurrency = quoteChargePM.CostContainerType3UnitPrice;
                    quoteChargePM.CostUnitPrice4InSaleCurrency = quoteChargePM.CostContainerType4UnitPrice;
                    quoteChargePM.CostUnitPrice5InSaleCurrency = quoteChargePM.CostContainerType5UnitPrice;
                    quoteChargePM.CostAmountInSaleCurrency = quoteChargePM.CostTotalAmount;
                }

                else
                {
                    double? costPriceInLocal = quoteChargePM.CostUnitPrice * quoteChargePM.CostExchangeRate;
                    double? costPrice1InLocal = quoteChargePM.CostContainerType1UnitPrice * quoteChargePM.CostExchangeRate;
                    double? costPrice2InLocal = quoteChargePM.CostContainerType2UnitPrice * quoteChargePM.CostExchangeRate;
                    double? costPrice3InLocal = quoteChargePM.CostContainerType3UnitPrice * quoteChargePM.CostExchangeRate;
                    double? costPrice4InLocal = quoteChargePM.CostContainerType4UnitPrice * quoteChargePM.CostExchangeRate;
                    double? costPrice5InLocal = quoteChargePM.CostContainerType5UnitPrice * quoteChargePM.CostExchangeRate;

                    double? costPriceInSale = costPriceInLocal / quoteChargePM.SaleExchangeRate;
                    double? costPrice1InSale = costPrice1InLocal / quoteChargePM.SaleExchangeRate;
                    double? costPrice2InSale = costPrice2InLocal / quoteChargePM.SaleExchangeRate;
                    double? costPrice3InSale = costPrice3InLocal / quoteChargePM.SaleExchangeRate;
                    double? costPrice4InSale = costPrice4InLocal / quoteChargePM.SaleExchangeRate;
                    double? costPrice5InSale = costPrice5InLocal / quoteChargePM.SaleExchangeRate;
                    double? costAmountInSaleCurrency = quoteChargePM.CostTotalAmountLocal / quoteChargePM.SaleExchangeRate;

                    quoteChargePM.CostUnitPriceInSaleCurrency = MethodHelper.Round(costPriceInSale, 3);
                    quoteChargePM.CostUnitPrice1InSaleCurrency = MethodHelper.Round(costPrice1InSale, 3);
                    quoteChargePM.CostUnitPrice2InSaleCurrency = MethodHelper.Round(costPrice2InSale, 3);
                    quoteChargePM.CostUnitPrice3InSaleCurrency = MethodHelper.Round(costPrice3InSale, 3);
                    quoteChargePM.CostUnitPrice4InSaleCurrency = MethodHelper.Round(costPrice4InSale, 3);
                    quoteChargePM.CostUnitPrice5InSaleCurrency = MethodHelper.Round(costPrice5InSale, 3);
                    quoteChargePM.CostAmountInSaleCurrency = MethodHelper.Round(costAmountInSaleCurrency, 3);
                }
                #endregion
            }
         
            return myResult.OrderBy(d => d.ViewOrder).ThenBy(d => d.ChargesTypeCode).ToList();
        }

    }
}