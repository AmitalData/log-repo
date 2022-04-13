using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.TariffModule.BL.Helpers
{
    public class SalesLocalChargesGenerator
    {
        private SalesLocalChargesTariffSearchArgs SalesLocalChargesTariffSearchArgs;
        private ICommonDataContext commonContext;
        private Quote quote;
        private int tenant;
        private List<ByPckageType> BCNTGroupedList = new List<ByPckageType>();
        private List<SalesLocalCharges> localCharges;
        private double? TEU;
        private double? valueOfGoods;
        private double? noOfPackages;
        private List<QuotePackage> FCLQuotePackages;
        private string customerGroupExportId;
        private string customerGroupImportId;
        private Customer customer;

        public SalesLocalChargesGenerator(SalesLocalChargesTariffSearchArgs args, int tenant)
        {
            this.tenant = tenant;
            this.SalesLocalChargesTariffSearchArgs = args;
            this.SalesLocalChargesTariffSearchArgs.SalesLocalCharges = new List<SalesLocalCharges>();
            this.commonContext = CommonDataContext.GetContext(tenant);

            this.GetQuote();
            this.SetQuotePropeaties();
            this.FillBCNTGroupedList();
        }

        public SalesLocalChargesTariffSearchArgs GenerateSaleLocalCharges()
        {
            List<Tariff> tariffs = this.GetTariffs(tenant);
            List<TariffLine> tariffLines = this.GetTariffLines(tariffs, tenant);

            foreach (TariffLine tariffLine in tariffLines)
            {
                Tariff tariff = tariffs.Where(d => d.Id == tariffLine.TariffId).FirstOrDefault();
                this.CreateQuoteChargeFromCustomChargesLine(tariffLine, tariff);
                this.SalesLocalChargesTariffSearchArgs.SalesLocalCharges.AddRange(this.localCharges);
            }

            return this.SalesLocalChargesTariffSearchArgs;
        }
        private void GetQuote()
        {
            QuoteRepository quoteRepository = new QuoteRepository(tenant);
            quote = quoteRepository.GetSingleQuote(SalesLocalChargesTariffSearchArgs.QuoteId, tenant);
        }
        private void SetQuotePropeaties()
        {
            if (quote == null)
            {
                return;
            }
            GetCustomer();
            GetCustomerGroupExportId();
            GetCustomerGroupImportId();
            TariffPricesHelper.grossWeight = this.quote.GrossWeight;
            TariffPricesHelper.chargeableWeight = this.quote.ChargeableWeight;
            TariffPricesHelper.volume = this.quote.Volume;
            TariffPricesHelper.grossWeightUnitCode = this.quote.GrossWeightUnitCode;
            TariffPricesHelper.chargeableWeightUnitCode = this.quote.ChargeableWeightUnitCode;
            TariffPricesHelper.volumeUnitCode = this.quote.VolumeUnitCode;
            this.valueOfGoods = this.quote.ValueOfGoods;
            this.noOfPackages = MethodHelper.IsFCLEntity(quote.TransportModeId, quote.ShipmentTypeId) ? this.quote.NumberOfContainers : this.quote.NumberOfPackages;
            this.TEU = this.quote.TEU;
            TariffPricesHelper.profitCurrencyId = this.quote.ProfitCurrencyId;
            TariffPricesHelper.profitRate = this.quote.ProfitExchangeRate;
            this.GetQuotePackages();

        }

        private void GetCustomer()
        {
            CustomerRepository customerRepository = new CustomerRepository(tenant);
            customer = customerRepository.GetSingleCustomer(quote.CustomerId, tenant);
        }

        private void GetCustomerGroupExportId()
        {
            customerGroupExportId = customer.ExportLocalCustomerGroupId;
            if (string.IsNullOrEmpty(customerGroupExportId))
            {
                customerGroupExportId = this.GetGeneralCustomerGroup();
            }
        }
        private void GetCustomerGroupImportId()
        {
            customerGroupImportId = customer.ImportLocalCustomerGroupId;
            if (string.IsNullOrEmpty(customerGroupImportId))
            {
                customerGroupImportId = this.GetGeneralCustomerGroup();
            }
        }
        private string GetGeneralCustomerGroup()
        {
            CustomerGroupRepository customerGroupRepository = new CustomerGroupRepository(tenant);
            var generalCustomerGroup = customerGroupRepository.GetGeneralCustomerGroup(tenant);
            return generalCustomerGroup;
        }

        private void GetQuotePackages()
        {
            QuotePackageRepository quotePackageRepository = new QuotePackageRepository(tenant);
            this.FCLQuotePackages = quotePackageRepository.GetQuotePackagesForQuoteTenant(SalesLocalChargesTariffSearchArgs.QuoteId, tenant)
                .Where(a => a.PackageType != null && a.PackageType.IsContainer).ToList();
        }
        private void FillBCNTGroupedList()
        {
            if (this.FCLQuotePackages.Count > 0)
            {
                foreach (QuotePackage item in this.FCLQuotePackages)
                {
                    var itemGrouped = BCNTGroupedList.Where(f => f.PackageTypeId == item.PackageTypeId).FirstOrDefault();
                    if (itemGrouped == null)
                    {
                        itemGrouped = new ByPckageType();
                        itemGrouped.PackageTypeId = item.PackageTypeId;
                        itemGrouped.Quantity = item.Quantity;

                        if (itemGrouped.Quantity == null)
                        {
                            itemGrouped.Quantity = 0;
                        }

                        BCNTGroupedList.Add(itemGrouped);
                    }

                    else
                    {
                        if (item.Quantity != null)
                        {
                            itemGrouped.Quantity += item.Quantity;
                        }
                    }
                }
            }
        }
       
        private List<Tariff> GetTariffs(int tenant)
        {
            TariffRepository tariffRepository = new TariffRepository(tenant);
            IQueryable<Tariff> tariffs = tariffRepository.GetActiveSalesLocalChargesTariffs(tenant);
            List<Tariff> myResult = new List<Tariff>();
            var yyy = tariffs.ToList();
            if (IsExportQuote())
            {
                Tariff exportTariff = tariffs.Where(d => d.CustomerGroupId == this.customerGroupExportId && d.TypeCode == "ECS").FirstOrDefault();
                if (exportTariff != null)
                {
                    myResult.Add(exportTariff);
                }
            }

            if (IsImportQuote())
            {
                Tariff importTariff = tariffs.Where(d => d.CustomerGroupId == this.customerGroupImportId && d.TypeCode == "ICS").FirstOrDefault();
                if (importTariff != null)
                {
                    myResult.Add(importTariff);
                }
            }

            return myResult;
        }

        private bool IsExportQuote()
        {
            if (this.quote.DirectionId == "E" )
            {
                return true;
            }
            if (this.quote.DirectionId == "D" )
            {
                return true;
            }
            if (this.quote.DirectionId == "R")
            {
                return true;
            }

            return false;
        }

        private bool IsImportQuote()
        {
            if (this.quote.DirectionId == "I")
            {
                return true;
            }
            if (this.quote.DirectionId == "D")
            {
                return true;
            }
            if (this.quote.DirectionId == "R")
            {
                return true;
            }

            return false;
        }
        private List<TariffLine> GetTariffLines(List<Tariff> tariffs, int tenant)
        {
            List<TariffLine> myResult = new List<TariffLine>();
            TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(tenant);
            TariffLineRepository tariffLineRepository = new TariffLineRepository(tenant);
            DateTime? dateFilter  = SalesLocalChargesTariffSearchArgs.BetweenDate;
            foreach (Tariff tariff in tariffs)
            {
                IQueryable<TariffVersion> tariffVersions = tariffVersionRepository.GetActiveVersionsByTariffId(tariff.Id, tenant);
                List<int> versionIds = tariffVersions.Select(a => a.Version).ToList();
                IQueryable<TariffLine> tariffLines = tariffLineRepository.GetAllTariffLinesByTariffIdAndVersions(tariff.Id, versionIds, tenant);

                tariffLines = tariffLines.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)
                               && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)) : true));

                TariffLine customChargesLine = this.FilterTariffLines(tariffLines);

                if (customChargesLine != null)
                {
                    myResult.Add(customChargesLine);
                }
            }

            return myResult;
        }
     
        private TariffLine FilterTariffLines(IQueryable<TariffLine> tariffLines)
        {
            if (IsImportQuote())
            {
                return FilterTariffLinesBasedOnFromCountries(tariffLines);
            }

            else if (IsExportQuote())
            {
                return FilterTariffLinesBasedOnToCountries(tariffLines);
            }

            return null;
        }

        private TariffLine FilterTariffLinesBasedOnFromCountries(IQueryable<TariffLine> tariffLines)
        {
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.FromCountryId == SalesLocalChargesTariffSearchArgs.FromCountryId);
            return filteredLines.FirstOrDefault();
        }
        private TariffLine FilterTariffLinesBasedOnToCountries(IQueryable<TariffLine> tariffLines)
        {
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.ToCountryId == SalesLocalChargesTariffSearchArgs.ToCountryId);
            return filteredLines.FirstOrDefault();
        }
        private void CreateQuoteChargeFromCustomChargesLine(TariffLine tariffLine, Tariff tariff)
        {
            this.localCharges = new List<SalesLocalCharges>();

            for (int i = 1; i <= 10; i++)
            {
                string measurementId = (string)tariff.GetType().GetProperty("Surcharge" + i + "UOM").GetValue(tariff);
                Measurement measurement = this.commonContext.Measurements.Where(p => p.Tenant == tenant && p.Id == measurementId).FirstOrDefault();
                if (measurement == null)
                {
                    return;
                }

                if (measurement.Code == "BCNT")
                {
                    this.HandleMeasurement_BCNT(tariff, tariffLine, i);
                }

                else
                {
                    this.HandleMeasurement_NotBCNT(measurement, tariff, tariffLine, i);
                }
            }
        }
        private void HandleMeasurement_BCNT(Tariff tariff, TariffLine tariffLine, int index)
        {
            string chargeId = (string)tariff.GetType().GetProperty("Surcharge" + index + "Id").GetValue(tariff);
            ChargesType chargesType = this.commonContext.ChargesTypes.Where(p => p.Tenant == tariff.Tenant && p.Id == chargeId).FirstOrDefault();
            if (chargesType != null)
            {
                foreach (ByPckageType byPckageType in BCNTGroupedList)
                {
                    Measurement measurement = null;
                    PackageType packageType = this.commonContext.PackageTypes.Where(p => p.Tenant == tenant && p.Id == byPckageType.PackageTypeId).FirstOrDefault();
                    if (packageType != null)
                    {
                        measurement = this.commonContext.Measurements.Where(p => p.Tenant == tenant && p.Id == packageType.MeasurementId).FirstOrDefault();
                    }

                    if (measurement != null)
                    {
                        decimal? tariffChargePrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + index + "Price").GetValue(tariffLine);
                        decimal? tariffChargeMinPrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + index + "MinPrice").GetValue(tariffLine);
                        double? price = null;
                        double? minAmount = null;

                        if (tariffChargePrice != null)
                        {
                            price = Convert.ToDouble(tariffChargePrice);
                        }

                        if (tariffChargeMinPrice != null)
                        {
                            minAmount = Convert.ToDouble(tariffChargeMinPrice);
                        }

                        if (price != null)
                        {
                            SalesLocalCharges localCharge = new SalesLocalCharges()
                            {
                                TariffId = tariff.Id,
                                TariffNumber = tariff.TariffNumber,
                                TariffLineId = tariffLine.Id,
                                VersionId = tariffLine.Version,
                                ChargeTypeCode = chargesType.Code,
                                ChargeTypeName = chargesType.EnglishName,
                                ChargeTypeId = chargesType.Id,
                                UnitOfMesurmentCode = measurement.Code,
                                UnitOfMesurmentId = measurement.Id,
                                IsDifferentCurrency = tariffLine.IsDifferentCurrenciesPerCharge,
                                Notes = tariffLine.Notes,
                                SellerId = tariff.SellerId,
                                SellerName = tariff.Seller?.EnglishName,
                                CurrencyId = tariffLine.CurrencyId != null ? tariffLine.CurrencyId : tariff.CurrencyId,
                                Quantity = byPckageType.Quantity,
                            };

                            if (tariffLine.IsDifferentCurrenciesPerCharge)
                            {
                                localCharge.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + index + "CurrencyId").GetValue(tariffLine);
                            }

                            Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == localCharge.CurrencyId).FirstOrDefault();
                            localCharge.CurrencyCode = currency?.Code;
                            localCharge.Rate = TariffPricesHelper.GetCurrencyRate(localCharge.CurrencyId, SalesLocalChargesTariffSearchArgs.LocalCurrencyId);
                            localCharge.Price = TariffPricesHelper.Round(price, 3);

                            double? expectedAmount = 0;

                            if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                            {
                                expectedAmount = localCharge.Quantity * (price / 100);
                            }
                            else
                            {
                                expectedAmount = price * localCharge.Quantity;
                            }

                            if (minAmount != null)
                            {
                                if (minAmount > expectedAmount)
                                {
                                    expectedAmount = minAmount;
                                    localCharge.MinAmount = minAmount;
                                }
                            }

                            localCharge.ExpectedAmount = expectedAmount == null ? 0 : expectedAmount.Value;
                            localCharge.LocalExpectedAmount = TariffPricesHelper.CalculateLocalAmount(expectedAmount, localCharge.Rate);
                            localCharge.ProfitExpectedAmount = TariffPricesHelper.CalculateProfitAmount(expectedAmount, localCharge.LocalExpectedAmount, localCharge.CurrencyId);
                            localCharges.Add(localCharge);
                        }
                    }
                }
            }
        }
        private void HandleMeasurement_NotBCNT(Measurement measurement, Tariff tariff, TariffLine tariffLine, int index)
        {
            string chargeId = (string)tariff.GetType().GetProperty("Surcharge" + index + "Id").GetValue(tariff);
            ChargesType chargesType = this.commonContext.ChargesTypes.Where(p => p.Tenant == tariff.Tenant && p.Id == chargeId).FirstOrDefault();
            if (chargesType != null)
            {
                decimal? tariffChargePrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + index + "Price").GetValue(tariffLine);
                decimal? tariffChargeMinPrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + index + "MinPrice").GetValue(tariffLine);
                double? price = null;
                double? minAmount = null;

                if (tariffChargePrice != null)
                {
                    price = Convert.ToDouble(tariffChargePrice);
                }

                if (tariffChargeMinPrice != null)
                {
                    minAmount = Convert.ToDouble(tariffChargeMinPrice);
                }

                if (price != null)
                {
                    SalesLocalCharges localCharge = new SalesLocalCharges()
                    {
                        TariffId = tariff.Id,
                        TariffNumber = tariff.TariffNumber,
                        TariffLineId = tariffLine.Id,
                        VersionId = tariffLine.Version,
                        ChargeTypeCode = chargesType.Code,
                        ChargeTypeName = chargesType.EnglishName,
                        ChargeTypeId = chargesType.Id,
                        UnitOfMesurmentCode = measurement.Code,
                        UnitOfMesurmentId = measurement.Id,
                        IsDifferentCurrency = tariffLine.IsDifferentCurrenciesPerCharge,
                        Notes = tariffLine.Notes,
                        SellerId = tariff.SellerId,
                        SellerName = tariff.Seller?.EnglishName,
                    };

                    localCharge.CurrencyId = tariffLine.CurrencyId != null ? tariffLine.CurrencyId : tariff.CurrencyId;
                    if (tariffLine.IsDifferentCurrenciesPerCharge)
                    {
                        localCharge.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + index + "CurrencyId").GetValue(tariffLine);
                    }

                    Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == localCharge.CurrencyId).FirstOrDefault();
                    localCharge.CurrencyCode = currency?.Code;

                    switch (measurement.Code)
                    {
                        case "FIXD": { localCharge.Quantity = 1; break; }
                        case "BTEU": { localCharge.Quantity = this.TEU; break; }
                        case "PRVL": { localCharge.Quantity = this.valueOfGoods; break; }
                        case "PRFR": { localCharge.Quantity = SalesLocalChargesTariffSearchArgs.FriehgtAmount; break; }
                        case "QTY": { localCharge.Quantity = this.noOfPackages; break; }
                        case "GRWT": { localCharge.Quantity = TariffPricesHelper.grossWeight; break; }
                        case "CHWT": { localCharge.Quantity = TariffPricesHelper.chargeableWeight; break; }
                        case "VOLU": { localCharge.Quantity = TariffPricesHelper.volume; break; }
                        case "PFCL": { localCharge.Quantity = SalesLocalChargesTariffSearchArgs.ForiegnChargesAmount; break; }
                        case "GWTN": { localCharge.Quantity = TariffPricesHelper.ComputeGrossWeigh_Kg_Ton("ton"); break; }
                        case "CWKG": { localCharge.Quantity = TariffPricesHelper.ComputeChargeableWeight_Kg(); break; }
                        case "GWKG": { localCharge.Quantity = TariffPricesHelper.ComputeGrossWeigh_Kg_Ton("kg"); break; }
                        case "VCBM": { localCharge.Quantity = TariffPricesHelper.ComputeVolumeInCBM(); break; }
                        default: { break; }
                    }

                    localCharge.Rate = TariffPricesHelper.GetCurrencyRate(localCharge.CurrencyId, SalesLocalChargesTariffSearchArgs.LocalCurrencyId);
                    localCharge.Price = TariffPricesHelper.Round(price, 3);

                    double? expectedAmount = 0;

                    if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                    {
                        expectedAmount = localCharge.Quantity * (price / 100);
                    }
                    else
                    {
                        expectedAmount = price * localCharge.Quantity;
                    }

                    if (minAmount != null)
                    {
                        if (minAmount > expectedAmount)
                        {
                            expectedAmount = minAmount;
                            localCharge.MinAmount = minAmount;
                        }
                    }

                    localCharge.ExpectedAmount = expectedAmount == null ? 0 : expectedAmount.Value;
                    localCharge.LocalExpectedAmount = TariffPricesHelper.CalculateLocalAmount(expectedAmount, localCharge.Rate);
                    localCharge.ProfitExpectedAmount = TariffPricesHelper.CalculateProfitAmount(expectedAmount, localCharge.LocalExpectedAmount, localCharge.CurrencyId);
                    localCharges.Add(localCharge);
                }
            }
        }
       

    }

    public class SalesLocalChargesTariffSearchArgs
    {
        public string QuoteId { get; set; }
        public string FromCountryId { get; set; }
        public string ToCountryId { get; set; }
        public DateTime? BetweenDate { get; set; }
        public double? FriehgtAmount { get; set; }
        public double? ForiegnChargesAmount { get; set; }
        public string LocalCurrencyId { get; set; }
        public List<SalesLocalCharges> SalesLocalCharges { get; set; }
    }

    public class SalesLocalCharges
    {
        public string ChargeTypeId { get; set; }
        public string ChargeTypeCode { get; set; }
        public string ChargeTypeName { get; set; }
        public string UnitOfMesurmentId { get; set; }
        public string UnitOfMesurmentCode { get; set; }
        public string TariffId { get; set; }
        public string TariffNumber { get; set; }
        public int VersionId { get; set; }
        public string TariffLineId { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public double? ExpectedAmount { get; set; }
        public double? LocalExpectedAmount { get; set; }
        public double? ProfitExpectedAmount { get; set; }
        public double? MinAmount { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public bool IsDifferentCurrency { get; set; }
        public string Notes { get; set; }
        public double? Rate { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
    }
}