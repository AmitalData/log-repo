using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.TariffModule.BL.Helpers
{
    public class SalesLocalChargesGenerator
    {
        private SalesLocalChargesTariffSearchArgs salesLocalChargesTariffSearchArgs;
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
        private Card card;
        private TariffPricesHelper tariffPricesHelper;
        public SalesLocalChargesGenerator(SalesLocalChargesTariffSearchArgs args, int tenant)
        {
            this.tenant = tenant;
            tariffPricesHelper = new TariffPricesHelper(this.tenant);
            this.salesLocalChargesTariffSearchArgs = args;
            this.salesLocalChargesTariffSearchArgs.SalesLocalCharges = new List<SalesLocalCharges>();
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
                this.CreateQuoteChargeFromLocalChargesLine(tariffLine, tariff);
                this.salesLocalChargesTariffSearchArgs.SalesLocalCharges.AddRange(this.localCharges);
            }

            salesLocalChargesTariffSearchArgs.Error = CheckIfTariffsFounded();         
            return this.salesLocalChargesTariffSearchArgs;
        }
        private string CheckIfTariffsFounded()
        {
            string error = null;
            if(salesLocalChargesTariffSearchArgs.SalesLocalCharges != null && salesLocalChargesTariffSearchArgs.SalesLocalCharges.Count() != 0)
            {
                return null;
            }
          
            var customerGroupId = IsExportQuote() ? customerGroupExportId : customerGroupImportId;
            var customerGroupName = GetCustomerGroupName(customerGroupId);

            error = (customerGroupName == "General" || IsDropDomesticQuote()) ? "No matching tariff found." : "No matching tariff found for " + customerGroupName + " customer group.";
            return error;
        }
  
        private void GetQuote()
        {
            QuoteRepository quoteRepository = new QuoteRepository(tenant);
            quote = quoteRepository.GetSingleQuote(salesLocalChargesTariffSearchArgs.QuoteId, tenant);

            if(salesLocalChargesTariffSearchArgs.IsFromUpdateSalesMessage)
            {
                UpdateQuoteCharges();
            }
        }
        private void UpdateQuoteCharges()
        {
            QuoteChargeRepository quoteChargeRepository = new QuoteChargeRepository(tenant);
            var quoteCharges = quoteChargeRepository.GetQuoteReceivablesByQuoteId(salesLocalChargesTariffSearchArgs.QuoteId,tenant);

            foreach (var item in quoteCharges)
            {
                item.SaleTariffId = null;
                item.SaleTariffNumber = null;
                item.SaleTariffLineId = null;
                quoteChargeRepository.Update(item);
            }
            quoteChargeRepository.SubmitChanges();

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
            tariffPricesHelper.grossWeight = this.quote.GrossWeight;
            tariffPricesHelper.chargeableWeight = this.quote.ChargeableWeight;
            tariffPricesHelper.volume = this.quote.Volume;
            tariffPricesHelper.grossWeightUnitCode = this.quote.GrossWeightUnitCode;
            tariffPricesHelper.chargeableWeightUnitCode = this.quote.ChargeableWeightUnitCode;
            tariffPricesHelper.volumeUnitCode = this.quote.VolumeUnitCode;
            this.valueOfGoods = this.quote.ValueOfGoods;
            this.noOfPackages = MethodHelper.IsFCLEntity(quote.TransportModeId, quote.ShipmentTypeId) ? this.quote.NumberOfContainers : this.quote.NumberOfPackages;
            this.TEU = this.quote.TEU;
            tariffPricesHelper.profitCurrencyId = this.quote.ProfitCurrencyId;
            tariffPricesHelper.profitRate = this.quote.ProfitExchangeRate;
        }

        private void GetCustomer()
        {
            CardRepository cardRepository = new CardRepository(tenant);
            card = cardRepository.GetSingleCard(quote.CustomerId, tenant);
        }

        private void GetCustomerGroupExportId()
        {
            customerGroupExportId = card?.ExportLocalCustomerGroupId;
            if (string.IsNullOrEmpty(customerGroupExportId))
            {
                customerGroupExportId = this.GetGeneralCustomerGroup();
            }
        }
        private void GetCustomerGroupImportId()
        {
            customerGroupImportId = card?.ImportLocalCustomerGroupId;
            if (string.IsNullOrEmpty(customerGroupImportId))
            {
                customerGroupImportId = this.GetGeneralCustomerGroup();
            }
        }
        private string GetGeneralCustomerGroup()
        {
            CustomerGroupRepository customerGroupRepository = new CustomerGroupRepository(tenant);
            var generalCustomerGroup = customerGroupRepository.GetGeneralCustomerGroupId(tenant);
            return generalCustomerGroup;
        }
        private string GetCustomerGroupName(string customerGroupId)
        {
            CustomerGroupRepository customerGroupRepository = new CustomerGroupRepository(tenant);
            var customerGroupName = customerGroupRepository.GetGeneralCustomerGroupName(tenant, customerGroupId);
            return customerGroupName;
        }

        private void FillBCNTGroupedList()
        {
            for (int i = 1; i <= 5; i++)
            {
                PropertyInfo packageTypePropInfo = quote.GetType().GetProperty("PackageType" + i + "Id");
                var packageTypePricevalue = (string)packageTypePropInfo.GetValue(quote);

                PropertyInfo packageTypeQuantityPropInfo = quote.GetType().GetProperty("PackageType" + i + "Quantity");
                var packageTypeQuantityPropValue = (int?)packageTypeQuantityPropInfo.GetValue(quote);

                if (packageTypePricevalue != null)
                {
                    var itemGrouped = BCNTGroupedList.Where(f => f.PackageTypeId == packageTypePricevalue).FirstOrDefault();
                    if (itemGrouped == null)
                    {
                        itemGrouped = new ByPckageType();
                        itemGrouped.PackageTypeId = packageTypePricevalue;
                        itemGrouped.Quantity = packageTypeQuantityPropValue;

                        if (itemGrouped.Quantity == null)
                        {
                            itemGrouped.Quantity = 0;
                        }

                        BCNTGroupedList.Add(itemGrouped);
                    }

                    else
                    {
                        if (packageTypeQuantityPropValue != null)
                        {
                            itemGrouped.Quantity += packageTypeQuantityPropValue;
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
            if (IsExportQuote()|| IsDropDomesticQuote())
            {
                Tariff exportTariff = tariffs.Where(d => d.CustomerGroupId == this.customerGroupExportId && d.TypeCode == "ECS").FirstOrDefault();
                if (exportTariff != null)
                {
                    myResult.Add(exportTariff);
                }
            }

            if (IsImportQuote() || IsDropDomesticQuote())
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
            return false;
        }
        private bool IsDropDomesticQuote()
        {
            if (this.quote.DirectionId == "R")
            {
                return true;
            }
            if (this.quote.DirectionId == "D")
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
            return false;
        }
        private List<TariffLine> GetTariffLines(List<Tariff> tariffs, int tenant)
        {
            List<TariffLine> myResult = new List<TariffLine>();
            TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(tenant);
            TariffLineRepository tariffLineRepository = new TariffLineRepository(tenant);
            DateTime? dateFilter = this.GetFilterDate();
            foreach (Tariff tariff in tariffs)
            {
                IQueryable<TariffVersion> tariffVersions = tariffVersionRepository.GetActiveVersionsByTariffId(tariff.Id, tenant);
                List<int> versionIds = tariffVersions.Select(a => a.Version).ToList();
                IQueryable<TariffLine> tariffLines = tariffLineRepository.GetAllTariffLinesByTariffIdAndVersions(tariff.Id, versionIds, tenant);

                tariffLines = tariffLines.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)
                               && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)) : true));

                TariffLine localChargesLine = this.FilterTariffLines(tariffLines);

                if (localChargesLine != null)
                {
                    myResult.Add(localChargesLine);
                }
            }

            return myResult;
        }
        private DateTime? GetFilterDate()
        {
            DateTime? betweenDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            if (this.quote.DirectionId == "I" && this.quote.ETA != null)
            {
                betweenDate = this.quote.ETA;
            }
            else if(this.quote.ETD != null)
            {
                betweenDate = this.quote.ETD;
            }

            return betweenDate;
        }
        private TariffLine FilterTariffLines(IQueryable<TariffLine> tariffLines)
        {
            if (IsDropDomesticQuote())
            {
                return FilterTariffLinesBasedOnBothCountries(tariffLines); ;
            }
       
            else if (IsImportQuote())
            {
                return FilterTariffLinesBasedOnFromCountries(tariffLines);
            }

            else if (IsExportQuote())
            {
                return FilterTariffLinesBasedOnToCountries(tariffLines);
            }

            return null;
        }
        private TariffLine FilterTariffLinesBasedOnBothCountries(IQueryable<TariffLine> tariffLines)
        {
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.FromCountryId == salesLocalChargesTariffSearchArgs.FromCountryId);
            if (filteredLines.Count() == 0)
            {
                filteredLines = tariffLines.Where(p => p.ToCountryId == salesLocalChargesTariffSearchArgs.ToCountryId);
            }
            return filteredLines.FirstOrDefault();
        }
        private TariffLine FilterTariffLinesBasedOnFromCountries(IQueryable<TariffLine> tariffLines)
        {
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.FromCountryId == salesLocalChargesTariffSearchArgs.FromCountryId);
            return filteredLines.FirstOrDefault();
        }
        private TariffLine FilterTariffLinesBasedOnToCountries(IQueryable<TariffLine> tariffLines)
        {
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.ToCountryId == salesLocalChargesTariffSearchArgs.ToCountryId);
            return filteredLines.FirstOrDefault();
        }
        private void CreateQuoteChargeFromLocalChargesLine(TariffLine tariffLine, Tariff tariff)
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
                    this.HandleMeasurement_BCNT(tariff, tariffLine, measurement, i);
                }

                else
                {
                    this.HandleMeasurement_NotBCNT(measurement, tariff, tariffLine, i);
                }
            }
        }
        private void HandleMeasurement_BCNT(Tariff tariff, TariffLine tariffLine, Measurement measurement, int index)
        {
            string chargeId = (string)tariff.GetType().GetProperty("Surcharge" + index + "Id").GetValue(tariff);
            ChargesType chargesType = this.commonContext.ChargesTypes.Where(p => p.Tenant == tariff.Tenant && p.Id == chargeId).FirstOrDefault();
            if (chargesType != null)
            {
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
                        };

                        if (tariffLine.IsDifferentCurrenciesPerCharge)
                        {
                            localCharge.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + index + "CurrencyId").GetValue(tariffLine);
                        }

                        Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == localCharge.CurrencyId).FirstOrDefault();
                        localCharge.CurrencyCode = currency?.Code;
                        localCharge.Rate = tariffPricesHelper.GetCurrencyRate(localCharge.CurrencyId, salesLocalChargesTariffSearchArgs.LocalCurrencyId);
                        localCharge.Price = tariffPricesHelper.Round(price, 3);

                        double? saleAmount = 0;

                        if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                        {
                            saleAmount = localCharge.Quantity * (price / 100);
                        }
                        else
                        {
                            saleAmount = price * localCharge.Quantity;
                        }

                        decimal? minPriceSurcharge = null;
                        decimal? actualMinimumPrice = null;
                        if (minAmount != null)
                        {
                            actualMinimumPrice = (decimal)minAmount;
                            minPriceSurcharge = tariffPricesHelper.CalculateLocalAmount(actualMinimumPrice.Value, tariff.CurrencyId, tariffLine.CurrencyId);
                        }
                        localCharge.MinPrice = minPriceSurcharge;
                        localCharge.ActualMinPrice = actualMinimumPrice;
                        localCharge.SaleTotalAmount = saleAmount == null ? 0 : saleAmount.Value;
                        localCharges.Add(localCharge);
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
                        case "PRFR": { localCharge.Quantity = salesLocalChargesTariffSearchArgs.FriehgtAmount; break; }
                        case "QTY": { localCharge.Quantity = this.noOfPackages; break; }
                        case "GRWT": { localCharge.Quantity = tariffPricesHelper.grossWeight; break; }
                        case "CHWT": { localCharge.Quantity = tariffPricesHelper.chargeableWeight; break; }
                        case "VOLU": { localCharge.Quantity = tariffPricesHelper.volume; break; }
                        case "PFCL": { localCharge.Quantity = salesLocalChargesTariffSearchArgs.ForiegnChargesAmount; break; }
                        case "GWTN": { localCharge.Quantity = tariffPricesHelper.ComputeGrossWeigh_Kg_Ton("ton"); break; }
                        case "CWKG": { localCharge.Quantity = tariffPricesHelper.ComputeChargeableWeight_Kg(); break; }
                        case "GWKG": { localCharge.Quantity = tariffPricesHelper.ComputeGrossWeigh_Kg_Ton("kg"); break; }
                        case "VCBM": { localCharge.Quantity = tariffPricesHelper.ComputeVolumeInCBM(); break; }
                        default: { break; }
                    }

                    localCharge.Rate = tariffPricesHelper.GetCurrencyRate(localCharge.CurrencyId, salesLocalChargesTariffSearchArgs.LocalCurrencyId);
                    localCharge.Price = tariffPricesHelper.Round(price, 3);

                    double? saleAmount = 0;

                    if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                    {
                        saleAmount = localCharge.Quantity * (price / 100);
                    }
                    else
                    {
                        saleAmount = price * localCharge.Quantity;
                    }

                    decimal? minPriceSurcharge = null;
                    decimal? actualMinimumPrice = null;
                    if (minAmount != null)
                    {
                        actualMinimumPrice = (decimal)minAmount;
                        minPriceSurcharge = tariffPricesHelper.CalculateLocalAmount(actualMinimumPrice.Value, tariff.CurrencyId, tariffLine.CurrencyId);
                    }
                    localCharge.MinPrice = minPriceSurcharge;
                    localCharge.ActualMinPrice = actualMinimumPrice;
                    localCharge.SaleTotalAmount = saleAmount == null ? 0 : saleAmount.Value;
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
        public string Error { get; set; }
        public bool IsFromUpdateSalesMessage { get; set; }
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
        public double? SaleTotalAmount { get; set; }
        public double? MinAmount { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public bool IsDifferentCurrency { get; set; }
        public string Notes { get; set; }
        public double? Rate { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? ActualMinPrice { get; set; }
    }
}