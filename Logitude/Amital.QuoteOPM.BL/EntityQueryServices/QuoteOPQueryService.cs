using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.BL.BusinessUnitFilters;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityQueryServices
{
    public partial class QuoteOPQueryService
    {

        public List<ChartingDataClass> GetStageFunnelData(string ownerId, string businessUnitId, int tenant, string RecordsTypeCode)
        {
            var dataSourceQuery =
                (from d in repository.GetAllIncludeStage(tenant)
                 where d.Tenant == tenant
                 && !d.IsClosed
                 && !d.IsCancelled
                 && d.StageId != null
                 && d.Stage.Rank > 0
                 select d);

            var filter = new QuoteOPBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            List<ChartingDataClass> result =
                (from d in dataSourceQuery
                 group d by new { d.StageId, d.Stage.Name, d.Stage.Rank } into g
                 select new ChartingDataClass()
                 {
                     Id = g.Key.StageId,
                     LabelProperty = g.Key.Name,
                     DecimalProperty = g.Count(),
                     IntegerProperty = g.Key.Rank,
                     GroupedId = g.Key.StageId,
                 }).ToList();

            return result;
        }

        public override void GetComposition(EntityKeyFields entityKeys, QuoteOPPM entityPM)
        {
            IQuoteOPMContext context = MainContext as QuoteOPMContext;
            var quoteOPKeys = entityKeys as QuoteOPKeys;
            QuoteOPComputedFieldQueryService quoteOPComputedFieldQueryService = new QuoteOPComputedFieldQueryService(context);
            //entityPM.Quote = quoteOPComputedFieldQueryService.GetMulti(quoteOPKeys, true);
            QuoteOPPackageQueryService quoteOPPackageQueryService = new QuoteOPPackageQueryService(context);
            entityPM.QuotePackages = quoteOPPackageQueryService.GetMulti(quoteOPKeys, true);


            QuoteOPChargeQueryService quoteOPChargeQueryService = new QuoteOPChargeQueryService(context);
            entityPM.QuoteCharges = quoteOPChargeQueryService.GetMulti(quoteOPKeys, true);


            QuoteOPTotalVATQueryService quoteOPTotalVATQueryService = new QuoteOPTotalVATQueryService(context);
            entityPM.TotalVATs = quoteOPTotalVATQueryService.GetMulti(quoteOPKeys, true);


            QuoteOPDocumentVersionQueryService quoteOPDocumentVersionQueryService = new QuoteOPDocumentVersionQueryService(context);
            entityPM.QuoteDocumentVersions = quoteOPDocumentVersionQueryService.GetMulti(quoteOPKeys, true);

            QuoteOPPropertiesQueryService quoteOPPropertiesQueryService = new QuoteOPPropertiesQueryService(context);
            entityPM.QuoteProperties = quoteOPPropertiesQueryService.GetMulti(quoteOPKeys, true);

            SummeryFields(entityPM);

            CalcTotalReceivablesAmount(entityPM);

            CalcQuoteCostCharges(entityPM);
            QuoteSalesTotals(entityPM);
            TotalVATPerQuote(entityPM, context);

            string GetTotalContainers()
            {// c# 7.3 -my first Local functions !!!  
                string totalContainers = "";
                string container = "";
                string containerTypeCode = "";
                int tenant = entityPM.Tenant;
                Simplog.Data.CommonDataModel.EntityPOCOs.PackageType packageType;
                if (entityPM.PackageType1Id != null)
                {
                    packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType1Id, entityPM.Tenant, true);

                    containerTypeCode = packageType.Code;

                    if (containerTypeCode.Length > 2)
                    {
                        containerTypeCode = packageType.Code.Insert(2, "'");
                    }

                    container = entityPM.PackageType1Quantity + " x " + containerTypeCode;
                    totalContainers = totalContainers + container;
                }

                if (entityPM.PackageType2Id != null)
                {
                    packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType2Id, tenant, true);

                    containerTypeCode = packageType.Code;

                    if (containerTypeCode.Length > 2)
                    {
                        containerTypeCode = packageType.Code.Insert(2, "'");
                    }

                    container = entityPM.PackageType2Quantity + " x " + containerTypeCode;
                    totalContainers = totalContainers + ", " + container;
                }

                if (entityPM.PackageType3Id != null)
                {
                    packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType3Id, tenant, true);

                    containerTypeCode = packageType.Code;

                    if (containerTypeCode.Length > 2)
                    {
                        containerTypeCode = packageType.Code.Insert(2, "'");
                    }

                    container = entityPM.PackageType3Quantity + " x " + containerTypeCode;
                    totalContainers = totalContainers + ", " + container;
                }

                if (entityPM.PackageType4Id != null)
                {
                    packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType4Id, tenant, true);

                    containerTypeCode = packageType.Code;

                    if (containerTypeCode.Length > 2)
                    {
                        containerTypeCode = packageType.Code.Insert(2, "'");
                    }

                    container = entityPM.PackageType4Quantity + " x " + containerTypeCode;
                    totalContainers = totalContainers + ", " + container;
                }

                if (entityPM.PackageType5Id != null)
                {
                    packageType = PackageTypeRepository.GetSinglePackageType(entityPM.PackageType5Id, tenant, true);

                    containerTypeCode = packageType.Code;

                    if (containerTypeCode.Length > 2)
                    {
                        containerTypeCode = packageType.Code.Insert(2, "'");
                    }

                    container = entityPM.PackageType5Quantity + " x " + containerTypeCode;
                    totalContainers = totalContainers + ", " + container;
                }
                return totalContainers;
            }
            entityPM.TotalContainers = GetTotalContainers();




            if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
            {
                entityPM.ETDLabel = "ETD Origin City";
            }
            else
            {
                entityPM.ETDLabel = "ETD Port of Loading";
            }

            if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
            {
                entityPM.ETALabel = "ETA Delivery City";
            }
            else
            {
                entityPM.ETALabel = "ETA Port of Destination";
            }

            GetTotalSaleIncludingVAT(entityPM);

            base.GetComposition(entityKeys, entityPM);
        }

        private static void GetTotalSaleIncludingVAT(QuoteOPPM entityPM)
        {
            double? mySaleAmountLocal = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(s => s.SaleTotalAmountLocal);
            double? myTotalVATLocal = entityPM.TotalVATs.Sum(s => s.LocalCurrencyVATAmount);
            double? mySaleAmount = 0;
            double? myTotalVAT = entityPM.TotalVATs.Sum(s => s.QuoteCurrencyVATAmount);

            mySaleAmountLocal = MethodHelper.Round(mySaleAmountLocal, 2);
            mySaleAmount = MethodHelper.Round(mySaleAmountLocal / entityPM.ExchangeRate, 2);
            myTotalVATLocal = MethodHelper.Round(myTotalVATLocal, 2);
            myTotalVAT = MethodHelper.Round(myTotalVAT, 2);

            entityPM.TotalSaleIncludingVATAmountInSaleCurrency = mySaleAmount.GetValueOrDefault() + myTotalVAT.GetValueOrDefault();
            entityPM.TotalSaleIncludingVATAmountInLocalCurrency = mySaleAmountLocal.GetValueOrDefault() + myTotalVATLocal.GetValueOrDefault();
        }

        private static void TotalVATPerQuote(QuoteOPPM entityPM, IQuoteOPMContext context)
        {
            #region TotalVATPerQuote
            if (entityPM.IsChargesByVAT)
            {
                if (entityPM.QuoteCharges.Count > 0)
                {
                    entityPM.TotalVATPerQuote = (from item in /*myQuotesContext*/context.QuoteOPTotalVATs.Include("VatType")
                                                 where item.Tenant == entityPM.Tenant && item.QuoteOPId == entityPM.Id
                                                 select new QuoteOPVATsTotalPM()
                                                 {
                                                     Id = item.Id,
                                                     VatTypeId = item.VatOPTypeId,
                                                     //VatTypeName = item.VatOPType == null ? "" : item.VatType.EnglishName,
                                                     VatPercentage = item.VatPercent,
                                                     AmountInSaleCurrency = item.QuoteCurrencyVATAmount,
                                                     AmountInLocalCurrency = item.LocalCurrencyVATAmount,
                                                 }).ToList();
                }
            }
            #endregion
        }

        private static void QuoteSalesTotals(QuoteOPPM entityPM)
        {
            #region Quote Sales Totals
            if (entityPM.IsSaleCurrencySameAsCost)
            {
                entityPM.QuoteSalesTotals = (from d in entityPM.QuoteCharges
                                             group d by d.SaleCurrencyCode into g
                                             select new QuoteOPSalesTotalPM()
                                             {
                                                 CurrencyCode = g.Key,
                                                 Amount = g.Sum(s => s.SaleTotalAmount),
                                             }).ToList();
            }

            else
            {
                entityPM.QuoteSalesTotals.Add(new QuoteOPSalesTotalPM() { CurrencyCode = entityPM.SaleCurrencyCode, Amount = entityPM.SaleTotalAmountInSaleCurrency });
            }

            string mySalesTotalAmounts = "";
            foreach (QuoteOPSalesTotalPM item in entityPM.QuoteSalesTotals)
            {
                double? myAmountField = 0;
                if (item.Amount != null)
                {
                    myAmountField = item.Amount;
                }

                string myLineText = String.Format("{0:#,0.00}", myAmountField) + " " + item.CurrencyCode;
                if (string.IsNullOrEmpty(mySalesTotalAmounts))
                {
                    mySalesTotalAmounts = myLineText;
                }

                else
                {
                    mySalesTotalAmounts += Environment.NewLine;
                    mySalesTotalAmounts += myLineText;
                }
            }

            entityPM.SalesTotalAmounts = mySalesTotalAmounts;
            #endregion
        }

        private void CalcQuoteCostCharges(QuoteOPPM entityPM)
        {
            entityPM.QuotationSaleCharges = new List<QuoteOPSaleChargePM>();
            #region QuoteCostCharges || QuoteSaleCharges
            foreach (QuoteOPChargePM item in entityPM.QuoteCharges)
            {
                bool isCostChargeAddable = true;

                if (entityPM.TransportModeId.ToUpper() == "A"
                    ||
                    (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
                    ||
                    (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
                    )
                {
                    if (item.CostUnitPrice == null)
                    {
                        isCostChargeAddable = false;
                    }
                }

                else
                {
                    if (item.CostUnitPrice == null
                        && item.CostContainerType1UnitPrice == null
                        && item.CostContainerType2UnitPrice == null
                        && item.CostContainerType3UnitPrice == null
                        && item.CostContainerType4UnitPrice == null
                        && item.CostContainerType5UnitPrice == null
                        )
                    {
                        isCostChargeAddable = false;
                    }
                }

                if (isCostChargeAddable)
                {
                    QuoteOPCostChargePM costChargePM = new QuoteOPCostChargePM()
                    {
                        Id = item.Id,
                        Tenant = item.Tenant,
                        QuoteId = item.QuoteOPId,
                        ChargesTypeId = item.ChargesTypeId,
                        ChargesTypeCode = item.ChargesTypeCode,
                        ChargesTypeName = item.ChargesTypeName,
                        ChargesGroupCode = item.ChargesGroupCode,
                        CurrencyId = item.CostCurrencyId,
                        CurrencyCode = item.CostCurrencyCode,
                        SaleExchangeRate = item.SaleExchangeRate.GetValueOrDefault(),
                        MarkUpTypeCode = item.MarkUpTypeCode,
                        MarkUpValue = item.MarkUpValue.GetValueOrDefault(),
                        Notes = item.Notes,
                        UpdatedByUserId = item.UpdatedByUserId,
                        UpdateDate = item.UpdateDate,
                        ValueDate = item.ValueDate,
                        QuoteTypeCode = item.QuoteTypeCode,
                        ContainerType1MarkUpTypeCode = item.ContainerType1MarkUpTypeCode,
                        ContainerType2MarkUpTypeCode = item.ContainerType2MarkUpTypeCode,
                        ContainerType3MarkUpTypeCode = item.ContainerType3MarkUpTypeCode,
                        ContainerType4MarkUpTypeCode = item.ContainerType4MarkUpTypeCode,
                        ContainerType5MarkUpTypeCode = item.ContainerType5MarkUpTypeCode,
                        ContainerType1MarkUpValue = item.ContainerType1MarkUpValue.GetValueOrDefault(),
                        ContainerType2MarkUpValue = item.ContainerType2MarkUpValue.GetValueOrDefault(),
                        ContainerType3MarkUpValue = item.ContainerType3MarkUpValue.GetValueOrDefault(),
                        ContainerType4MarkUpValue = item.ContainerType4MarkUpValue.GetValueOrDefault(),
                        ContainerType5MarkUpValue = item.ContainerType5MarkUpValue.GetValueOrDefault(),
                        CostMeasurementId = item.CostMeasurementId,
                        CostMeasurementCode = item.CostMeasurementCode,
                        CostMeasurementShortName = item.CostMeasurementShortName,
                        CostQuantity = item.CostQuantity.GetValueOrDefault(),
                        CostUnitPrice = item.CostUnitPrice.GetValueOrDefault(),
                        CostTotalAmount = item.CostTotalAmount.GetValueOrDefault(),
                        CostTotalAmountLocal = item.CostTotalAmountLocal.GetValueOrDefault(),
                        CostContainerType1UnitPrice = item.CostContainerType1UnitPrice.GetValueOrDefault(),
                        CostContainerType2UnitPrice = item.CostContainerType2UnitPrice.GetValueOrDefault(),
                        CostContainerType3UnitPrice = item.CostContainerType3UnitPrice.GetValueOrDefault(),
                        CostContainerType4UnitPrice = item.CostContainerType4UnitPrice.GetValueOrDefault(),
                        CostContainerType5UnitPrice = item.CostContainerType5UnitPrice.GetValueOrDefault(),
                        VatTypeId = item.VatTypeId,
                        VatPercentage = item.VatPercentage.GetValueOrDefault(),
                        VatTypeName = item.VatTypeName,
                        UOMPercentage = item.CostMeasurementCode == "PRVL" || item.CostMeasurementCode == "PRFR" ? "%" : "",
                        CostMinAmount = item.CostMinAmount.GetValueOrDefault(),
                        CostMaxAmount = item.CostMaxAmount.GetValueOrDefault(),
                        SaleMinAmount = item.SaleMinAmount.GetValueOrDefault(),
                        SaleMaxAmount = item.SaleMaxAmount.GetValueOrDefault(),
                        IsRegionalTax = item.IsRegionalTax,
                        MarkUpText = this.GetMarkUpText(item.MarkUpValue, item.MarkUpTypeCode),
                        ContainerType1MarkUpText = this.GetMarkUpText(item.ContainerType1MarkUpValue, item.ContainerType1MarkUpTypeCode),
                        ContainerType2MarkUpText = this.GetMarkUpText(item.ContainerType2MarkUpValue, item.ContainerType2MarkUpTypeCode),
                        ContainerType3MarkUpText = this.GetMarkUpText(item.ContainerType3MarkUpValue, item.ContainerType3MarkUpTypeCode),
                        ContainerType4MarkUpText = this.GetMarkUpText(item.ContainerType4MarkUpValue, item.ContainerType4MarkUpTypeCode),
                        ContainerType5MarkUpText = this.GetMarkUpText(item.ContainerType5MarkUpValue, item.ContainerType5MarkUpTypeCode),
                    };

                    entityPM.QuoteCostCharges.Add(costChargePM);
                }

                bool isSaleChargeAddable = !(item.IsAllIN);
                bool isSaleChargeQuotationAddable = true;


                if (entityPM.TransportModeId.ToUpper() == "A"
                    ||
                    (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
                    ||
                    (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
                    )
                {
                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteOPChargePriceSteps.Count == 0)
                        {
                            isSaleChargeAddable = false;
                            isSaleChargeQuotationAddable = false;
                        }
                    }

                    else
                    {
                        if (item.SaleUnitPrice == null)
                        {
                            isSaleChargeAddable = false;
                            isSaleChargeQuotationAddable = false;
                        }
                    }
                }

                else
                {
                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteOPChargePriceSteps.Count == 0)
                        {
                            isSaleChargeAddable = false;
                            isSaleChargeQuotationAddable = false;
                        }
                    }

                    else
                    {
                        if (item.SaleUnitPrice == null
                            && item.SaleContainerType1UnitPrice == null
                            && item.SaleContainerType2UnitPrice == null
                            && item.SaleContainerType3UnitPrice == null
                            && item.SaleContainerType4UnitPrice == null
                            && item.SaleContainerType5UnitPrice == null
                            )
                        {
                            isSaleChargeAddable = false;
                            isSaleChargeQuotationAddable = false;
                        }
                    }
                }



                if (isSaleChargeAddable || isSaleChargeQuotationAddable)
                {
                    QuoteOPSaleChargePM saleChargePM = MapChargePMToSaleChargePM(item);
                    if (item.IsChargeBySteps)
                    {
                        if (item.QuoteOPChargePriceSteps.Count > 0)
                        {
                            string myPriceBreaks = "";

                            foreach (QuoteOPPriceStepsPM itemStep in item.QuoteOPChargePriceSteps)
                            {
                                string formattedValue = "";
                                if (itemStep.SaleUnitPrice != null)
                                {
                                    formattedValue = itemStep.SaleUnitPrice.Value.ToString();
                                    if (formattedValue.Contains("."))
                                    {
                                        string[] digits = formattedValue.Split('.');
                                        if (digits[1] == "0" || digits[1] == "00")
                                        {
                                            formattedValue = digits[0];
                                        }

                                        if (string.IsNullOrEmpty(formattedValue))
                                        {
                                            formattedValue = itemStep.SaleUnitPrice.Value.ToString("#,##0." + new string('0', 2));
                                        }
                                    }
                                }
                                string stepUOM = GetPriceBreakWeightUnitCodeByCostMeasurementCode(item.CostMeasurementCode, entityPM);
                                if (string.IsNullOrEmpty(myPriceBreaks))
                                {
                                    myPriceBreaks += "+" + itemStep.Step + " " + stepUOM + ": " + formattedValue;
                                }

                                else
                                {
                                    myPriceBreaks += "\r";//Environment.NewLine;
                                    myPriceBreaks += "+" + itemStep.Step + " " + stepUOM + ": " + formattedValue;
                                }

                            }

                            saleChargePM.PriceBreaks = myPriceBreaks;
                        }
                    }

                    if (isSaleChargeAddable) entityPM.QuoteSaleCharges.Add(saleChargePM);
                    else if (isSaleChargeQuotationAddable) entityPM.QuotationSaleCharges.Add(saleChargePM);

                }
            }
            #endregion
        }

        private static void CalcTotalReceivablesAmount(QuoteOPPM entityPM)
        {
            entityPM.TotalReceivablesAmount = 0;

            foreach (QuoteOPChargePM receviable in entityPM.QuoteCharges)
            {
                if (receviable.SaleTotalAmountLocal != null)
                {
                    entityPM.TotalReceivablesAmount += receviable.SaleTotalAmountLocal.GetValueOrDefault();
                }
            }
        }

        private static void SummeryFields(QuoteOPPM entityPM)
        {
            #region Summery Fields
            if (entityPM.QuoteCharges.Count > 0)
            {
                entityPM.CostTotalAmountInLocalCurrency = entityPM.QuoteCharges.Sum(d => d.CostTotalAmountLocal.GetValueOrDefault());
                entityPM.CostTotalAmountInSaleCurrency = entityPM.QuoteCharges.Sum(d => d.CostAmountInSaleCurrency.GetValueOrDefault());
                entityPM.SaleTotalAmountInLocalCurrency = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleTotalAmountLocal.GetValueOrDefault());
                entityPM.SaleTotalAmountInSaleCurrency = entityPM.QuoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleAmountInSaleCurrency);
                var sum = entityPM.SaleTotalAmountInSaleCurrency.GetValueOrDefault() - entityPM.CostTotalAmountInSaleCurrency;
                entityPM.EstimateProfitInSaleCurrency = (double)MethodHelper.Round(sum, 2);
            }
            #endregion
        }

        private string GetPriceBreakWeightUnitCodeByCostMeasurementCode(string costMeasurementCode, QuoteOPPM quotePM)
        {
            string weightUnitCode = "";
            switch (costMeasurementCode)
            {
                case "GRWT": { weightUnitCode = quotePM.GrossWeightUnitCode; break; }
                case "CHWT": { weightUnitCode = quotePM.ChargeableWeightUnitCode; break; }
                case "PDCW": { weightUnitCode = quotePM.PickupDeliveryCWeightUnitCode; break; }
                case "VOLU": { weightUnitCode = quotePM.VolumeUnitCode; break; }
                case "BTEU": { weightUnitCode = "TEU"; break; }
                case "PRVL": { weightUnitCode = "Value of Goods"; break; }
                case "PRFR": { weightUnitCode = "Freight Value"; break; }
                case "GWTN": { weightUnitCode = "Ton"; break; }
                case "QTY": { weightUnitCode = "pieces"; break; }
                case "CWKG": { weightUnitCode = "KG"; break; }
                case "GWKG": { weightUnitCode = "KG"; break; }
                case "VCBM": { weightUnitCode = "CBM"; break; }
            }
            return weightUnitCode.ToLower();
        }

        private QuoteOPSaleChargePM MapChargePMToSaleChargePM(QuoteOPChargePM item)
        {
            var saleChargePM = new QuoteOPSaleChargePM()
            {
                Id = item.Id,
                Tenant = item.Tenant,
                QuoteId = item.QuoteOPId,
                ChargesTypeId = item.ChargesTypeId,
                ChargesTypeCode = item.ChargesTypeCode,
                ChargesTypeName = item.ChargesTypeName,
                ChargesTypeDescription = item.ChargesTypeDescription,
                ChargesGroupCode = item.ChargesGroupCode,
                CurrencyId = item.SaleCurrencyId,
                CurrencyCode = item.SaleCurrencyCode,
                SaleExchangeRate = item.SaleExchangeRate.GetValueOrDefault(),
                MarkUpTypeCode = item.MarkUpTypeCode,
                MarkUpValue = item.MarkUpValue.GetValueOrDefault(),
                ChargesTypeLocalName = item.ChargesTypeLocalName,
                Notes = item.Notes,
                UpdatedByUserId = item.UpdatedByUserId,
                UpdateDate = item.UpdateDate,
                ValueDate = item.ValueDate,
                IsAllIN = item.IsAllIN ? "True" : "False",
                QuoteTypeCode = item.QuoteTypeCode,
                ContainerType1MarkUpTypeCode = item.ContainerType1MarkUpTypeCode,
                ContainerType2MarkUpTypeCode = item.ContainerType2MarkUpTypeCode,
                ContainerType3MarkUpTypeCode = item.ContainerType3MarkUpTypeCode,
                ContainerType4MarkUpTypeCode = item.ContainerType4MarkUpTypeCode,
                ContainerType5MarkUpTypeCode = item.ContainerType5MarkUpTypeCode,
                ContainerType1MarkUpValue = item.ContainerType1MarkUpValue.GetValueOrDefault(),
                ContainerType2MarkUpValue = item.ContainerType2MarkUpValue.GetValueOrDefault(),
                ContainerType3MarkUpValue = item.ContainerType3MarkUpValue.GetValueOrDefault(),
                ContainerType4MarkUpValue = item.ContainerType4MarkUpValue.GetValueOrDefault(),
                ContainerType5MarkUpValue = item.ContainerType5MarkUpValue.GetValueOrDefault(),
                SaleMeasurementId = item.SaleMeasurementId,
                SaleMeasurementCode = item.SaleMeasurementCode,
                SaleMeasurementShortName = item.SaleMeasurementShortName,
                //SaleMeasurementLocalName = item.SaleMeasurementLocalName,
                SaleQuantity = item.SaleQuantity.GetValueOrDefault(),
                SaleUnitPrice = item.SaleUnitPrice.GetValueOrDefault(),
                SaleTotalAmount = item.SaleTotalAmount.GetValueOrDefault(),
                SaleTotalAmountLocal = item.SaleTotalAmountLocal.GetValueOrDefault(),
                SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice.GetValueOrDefault(),
                SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice.GetValueOrDefault(),
                SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice.GetValueOrDefault(),
                SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice.GetValueOrDefault(),
                SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice.GetValueOrDefault(),
                VatTypeId = item.VatTypeId,
                VatPercentage = item.VatPercentage.GetValueOrDefault(),
                VatTypeName = item.VatTypeName,
                VatAmount = item.VatAmount,
                UOMPercentage = item.SaleMeasurementCode == "PRVL" || item.SaleMeasurementCode == "PRFR" ? "%" : "",
                SaleUnitPriceInSaleCurrency = item.SaleUnitPriceInSaleCurrency.GetValueOrDefault(),
                SaleUnitPrice1InSaleCurrency = item.SaleUnitPrice1InSaleCurrency.GetValueOrDefault(),
                SaleUnitPrice2InSaleCurrency = item.SaleUnitPrice2InSaleCurrency.GetValueOrDefault(),
                SaleUnitPrice3InSaleCurrency = item.SaleUnitPrice3InSaleCurrency.GetValueOrDefault(),
                SaleUnitPrice4InSaleCurrency = item.SaleUnitPrice4InSaleCurrency.GetValueOrDefault(),
                SaleUnitPrice5InSaleCurrency = item.SaleUnitPrice5InSaleCurrency.GetValueOrDefault(),
                SaleAmountInSaleCurrency = item.SaleAmountInSaleCurrency.GetValueOrDefault(),
                CostMinAmount = item.CostMinAmount.GetValueOrDefault(),
                CostMaxAmount = item.CostMaxAmount.GetValueOrDefault(),
                SaleMinAmount = item.SaleMinAmount.GetValueOrDefault(),
                SaleMaxAmount = item.SaleMaxAmount.GetValueOrDefault(),
                IsRegionalTax = item.IsRegionalTax,
                MarkUpText = this.GetMarkUpText(item.MarkUpValue, item.MarkUpTypeCode),
                ContainerType1MarkUpText = this.GetMarkUpText(item.ContainerType1MarkUpValue, item.ContainerType1MarkUpTypeCode),
                ContainerType2MarkUpText = this.GetMarkUpText(item.ContainerType2MarkUpValue, item.ContainerType2MarkUpTypeCode),
                ContainerType3MarkUpText = this.GetMarkUpText(item.ContainerType3MarkUpValue, item.ContainerType3MarkUpTypeCode),
                ContainerType4MarkUpText = this.GetMarkUpText(item.ContainerType4MarkUpValue, item.ContainerType4MarkUpTypeCode),
                ContainerType5MarkUpText = this.GetMarkUpText(item.ContainerType5MarkUpValue, item.ContainerType5MarkUpTypeCode),
                //IsChargeBySteps = item.IsChargeBySteps,
                SalesWithVATAmount = item.SaleTotalAmount.GetValueOrDefault() + item.VatAmount,
            };
            return saleChargePM;
        }

        private string GetMarkUpText(double? value, string typeCode)
        {
            string myResult = "0.00";

            if (value != null)
            {
                if (value != 0)
                {
                    myResult = value.ToString();

                    if (typeCode == "P")
                    {
                        myResult += " %";
                    }
                }
            }

            return myResult;
        }
    }
}
