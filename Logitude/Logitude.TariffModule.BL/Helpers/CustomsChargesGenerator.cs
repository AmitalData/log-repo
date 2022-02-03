using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.Helpers
{
    public class CustomsChargesGenerator
    {
        private CustomsChargesTariffSearchArgs customsChargesTariffSearchArgs;
        private List<RatesTableList> ratesList;
        private ICommonDataContext commonContext;
        private int tenant;

        public CustomsChargesGenerator(CustomsChargesTariffSearchArgs args, int tenant)
        {
            this.tenant = tenant;
            this.customsChargesTariffSearchArgs = args;
            this.customsChargesTariffSearchArgs.CustomsChargesPayables = new List<CustomsChargesPayable>();
            this.ratesList = this.GetRates(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);
        }

        public CustomsChargesTariffSearchArgs GeneratePayables()
        {
            IQueryable<Tariff> tariffs = this.GetTariffs(tenant);
            List<TariffLine> tariffLines = this.GetTariffLines(tariffs, tenant);
            
            foreach (TariffLine tariffLine in tariffLines)
            {
                Tariff tariff = tariffs.Where(d => d.Id == tariffLine.TariffId).FirstOrDefault();
                this.customsChargesTariffSearchArgs.CustomsChargesPayables.AddRange(this.CreatePayablesFromCustomChargesLine(tariffLine, tariff));
            }

            return this.customsChargesTariffSearchArgs;
        }

        private List<RatesTableList> GetRates(int tenant)
        {
            IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTableRepository = new RatesTableRepository(MyContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);

            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            IQueryable<RatesTableList> entityLists = ratesTableQuery.GetIQueryableEntityList(entityPocos);
            entityLists = entityLists.OrderByDescending(r => r.ValueDate);

            return entityLists.ToList();
        }
        private IQueryable<Tariff> GetTariffs(int tenant)
        {
            TariffRepository tariffRepository = new TariffRepository(tenant);
            IQueryable<Tariff> tariffs = tariffRepository.GetActiveCustomsChargesTariffs(tenant);

            if (!string.IsNullOrEmpty(customsChargesTariffSearchArgs.CustomAgentExportId))
            {
                tariffs = tariffs.Where(d => d.CustomsBrokerId == customsChargesTariffSearchArgs.CustomAgentExportId);
            }

            if (!string.IsNullOrEmpty(customsChargesTariffSearchArgs.CustomAgentImportId))
            {
                tariffs = tariffs.Where(d => d.CustomsBrokerId == customsChargesTariffSearchArgs.CustomAgentImportId);
            }

            return tariffs;
        }
        private List<TariffLine> GetTariffLines(IQueryable<Tariff> tariffs, int tenant)
        {
            List<TariffLine> myResult = new List<TariffLine>();
            TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(tenant);
            TariffLineRepository tariffLineRepository = new TariffLineRepository(tenant);
            DateTime? dateFilter = this.GetCustomsChargesDateFilter(tenant);
            foreach (Tariff tariff in tariffs)
            {
                IQueryable<TariffVersion> tariffVersions = tariffVersionRepository.GetActiveVersionsByTariffId(tariff.Id, tenant);
                List<int> versionIds = tariffVersions.Select(a => a.Version).ToList();
                IQueryable<TariffLine> tariffLines = tariffLineRepository.GetAllTariffLinesByTariffIdAndVersions(tariff.Id, versionIds, tenant);

                tariffLines = tariffLines.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)
                               && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(dateFilter)) : true));

                myResult.Add(this.FilterTariffLinesBasedOnCountries(tariffLines));
            }

            return myResult;
        }        
        private DateTime? GetCustomsChargesDateFilter(int tenant)
        {
            DateTime? date = TenantServerConfigration.GetCurrentDateTime(tenant);

            if (customsChargesTariffSearchArgs.MainCarriageATD != null)
            {
                date = customsChargesTariffSearchArgs.MainCarriageATD;
            }

            else if (customsChargesTariffSearchArgs.MainCarriageETD != null)
            {
                date = customsChargesTariffSearchArgs.MainCarriageETD;
            }

            return date;
        }
        private TariffLine FilterTariffLinesBasedOnCountries(IQueryable<TariffLine> tariffLines)
        {
            List<TariffLine> test = tariffLines.ToList();
            IQueryable<TariffLine> filteredLines = tariffLines.Where(p => p.FromCountryId == customsChargesTariffSearchArgs.FromCountryId && p.ToCountryId == customsChargesTariffSearchArgs.ToCountryId);

            if (filteredLines.Count() == 0)
            {
                filteredLines = tariffLines.Where(p => p.FromCountryId == customsChargesTariffSearchArgs.FromCountryId && p.IsToAllOtherCountries);
            }

            if (filteredLines.Count() == 0)
            {
                filteredLines = tariffLines.Where(p => p.IsFromAllOtherCountries && p.ToCountryId == customsChargesTariffSearchArgs.ToCountryId);
            }

            if (filteredLines.Count() == 0)
            {
                filteredLines = tariffLines.Where(p => p.IsFromAllOtherCountries && p.IsToAllOtherCountries);
            }

            return filteredLines.FirstOrDefault();
        }
        private List<CustomsChargesPayable> CreatePayablesFromCustomChargesLine(TariffLine tariffLine, Tariff tariff)
        {
            List<CustomsChargesPayable> payables = new List<CustomsChargesPayable>();
            for (int i = 1; i <= 10; i++)
            {
                string chargeId = (string)tariff.GetType().GetProperty("Surcharge" + i + "Id").GetValue(tariff);
                string measurementId = (string)tariff.GetType().GetProperty("Surcharge" + i + "UOM").GetValue(tariff);

                Measurement measurement = this.commonContext.Measurements.Where(p => p.Tenant == tariff.Tenant && p.Id == measurementId).FirstOrDefault();
                ChargesType chargesType = this.commonContext.ChargesTypes.Where(p => p.Tenant == tariff.Tenant && p.Id == chargeId).FirstOrDefault();

                if (measurement != null && chargesType != null)
                {
                    decimal? tariffChargePrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + i + "Price").GetValue(tariffLine);
                    decimal? tariffChargeMinPrice = (decimal?)tariffLine.GetType().GetProperty("Surcharge" + i + "MinPrice").GetValue(tariffLine);

                    double? price = null;
                    double? minPrice = null;

                    if(tariffChargePrice != null)
                    {
                        price = Convert.ToDouble(tariffChargePrice);
                    }

                    if (tariffChargeMinPrice != null)
                    {
                        minPrice = Convert.ToDouble(tariffChargeMinPrice);
                    }

                    if (price != null)
                    {
                        CustomsChargesPayable payable = new CustomsChargesPayable()
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
                        };

                        payable.CurrencyId = tariffLine.CurrencyId != null ? tariffLine.CurrencyId : tariff.CurrencyId;
                        if (tariffLine.IsDifferentCurrenciesPerCharge)
                        {
                            payable.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + i + "CurrencyId").GetValue(tariffLine);
                        }

                        Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == payable.CurrencyId).FirstOrDefault();
                        payable.CurrencyCode = currency?.Code;

                        switch (measurement.Code)
                        {
                            case "FIXD": { payable.Quantity = 1; break; }
                            case "BTEU": { payable.Quantity = customsChargesTariffSearchArgs.TEU; break; }
                            case "PRVL": { payable.Quantity = customsChargesTariffSearchArgs.ValueOfGoods; break; }
                            case "PRFR": { payable.Quantity = customsChargesTariffSearchArgs.FriehgtAmount; break; }
                            case "QTY": { payable.Quantity = customsChargesTariffSearchArgs.NoOfPackages; break; }
                            case "GRWT": { payable.Quantity = customsChargesTariffSearchArgs.GrossWeight; break; }
                            case "CHWT": { payable.Quantity = customsChargesTariffSearchArgs.ChargeableWeight; break; }
                            case "VOLU": { payable.Quantity = customsChargesTariffSearchArgs.Volume; break; }
                            case "PFCL": { payable.Quantity = customsChargesTariffSearchArgs.ForiegnChargesAmount; break; }
                            case "GWTN": { payable.Quantity = this.ComputeGrossWeigh_Kg_Ton("ton"); break; }
                            case "CWKG": { payable.Quantity = this.ComputeChargeableWeight_Kg(); break; }
                            case "GWKG": { payable.Quantity = this.ComputeGrossWeigh_Kg_Ton("kg"); break; }
                            case "VCBM": { payable.Quantity = this.ComputeVolumeInCBM(); break; }
                            default: { break; }
                        }

                        payable.Rate = this.GetCurrencyRate(payable.CurrencyId);
                        payable.Price = Round(price, 3);

                        double? expectedAmount = 0;
                        double? minAmount = 0;

                        if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                        {
                            expectedAmount = payable.Quantity * (price / 100);
                        }
                        else
                        {
                            expectedAmount = price * payable.Quantity;
                        }

                        if (tariffChargeMinPrice != null)
                        {
                            if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                            {
                                minAmount = payable.Quantity * (minPrice / 100);
                            }
                            else
                            {
                                minAmount = minPrice * payable.Quantity;
                            }
                        }

                        payable.ExpectedAmount = expectedAmount == null ? 0 : expectedAmount.Value;
                        payable.LocalExpectedAmount = this.CalculateLocalAmount(expectedAmount, payable.Rate);
                        payable.ProfitExpectedAmount = this.CalculateProfitAmount(expectedAmount, payable.LocalExpectedAmount, payable.CurrencyId);
                        payable.MinAmount = minAmount;
                        payables.Add(payable);
                    }
                }
            }

            return payables;
        }

        private double? ComputeGrossWeigh_Kg_Ton(string type)
        {
            double? weigh_Kg = null;
            double? weigh_Ton = null;

            if (customsChargesTariffSearchArgs.GrossWeight != null)
            {
                double factorOfConvert = 1;

                if (!string.IsNullOrEmpty(customsChargesTariffSearchArgs.GrossWeightUnitCode))
                {
                    switch (customsChargesTariffSearchArgs.GrossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = customsChargesTariffSearchArgs.GrossWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);

                weigh_Ton = weigh_Kg / 1000;
            }

            if (weigh_Ton != null)
            {
                weigh_Ton = Round(weigh_Ton, 3);
            }

            if (type == "kg")
                return weigh_Kg;
            return weigh_Ton;
        }
        private double? ComputeChargeableWeight_Kg()
        {
            double? weigh_Kg = null;

            if (customsChargesTariffSearchArgs.ChargeableWeight != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(customsChargesTariffSearchArgs.ChargeableWeightUnitCode))
                {
                    switch (customsChargesTariffSearchArgs.ChargeableWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = customsChargesTariffSearchArgs.ChargeableWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);
            }
            return weigh_Kg;
        }
        private double? ComputeVolumeInCBM()
        {
            double? volumeInCBM = null;

            if (customsChargesTariffSearchArgs.Volume != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(customsChargesTariffSearchArgs.VolumeUnitCode))
                {
                    switch (customsChargesTariffSearchArgs.VolumeUnitCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }
                        case "CBF": { factorOfConvert = 35.315; break; }
                    }
                }
                volumeInCBM = customsChargesTariffSearchArgs.Volume * factorOfConvert;
            }

            if (volumeInCBM != null)
            {
                volumeInCBM = Round(volumeInCBM, 3);
            }
            return volumeInCBM;
        }
        private double? GetCurrencyRate(string currencyId)
        {
            double? myResult = null;

            if (currencyId == customsChargesTariffSearchArgs.LocalCurrencyId)
            {
                myResult = 1;
            }

            else
            {
                RatesTableList lastRate = ratesList.Where(f => f.ForeignCurrencyId == currencyId).FirstOrDefault();
                if (lastRate != null)
                {
                    myResult = Round(lastRate.Rate, 5);
                }
            }

            return myResult;
        }
        private double? CalculateProfitAmount(double? expectedAmount, double? localExpectedAmount, string currencyId)
        {
            double? myResult = null;

            if (currencyId == customsChargesTariffSearchArgs.ProfitCurrencyId)
            {
                myResult = expectedAmount;
            }

            else
            {
                myResult = (localExpectedAmount / customsChargesTariffSearchArgs.ProfitRate);
            }

            return myResult;
        }
        private double? CalculateLocalAmount(double? expectedAmount, double? rate)
        {
            double? myResult = null;

            if (expectedAmount != null && rate != null)
            {
                myResult = Round(expectedAmount * rate, 2);
            }

            return myResult;
        }
        private double? Round(double? value, int digits)
        {
            double? myValue = null;

            if (value != null)
            {
                myValue = Convert.ToDouble(value);
            }

            double? myResult = myValue;

            if (myValue != null && digits >= 1 && digits <= 15)
            {
                string mySTR = String.Format("{0:N" + digits + "}", myValue);

                myResult = Convert.ToDouble(mySTR);
            }

            return myResult;
        }
    }

    public class CustomsChargesTariffSearchArgs
    {
        public string FromCountryId { get; set; }
        public string ToCountryId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitRate { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public string CustomAgentExportId { get; set; }
        public string CustomAgentImportId { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? Volume { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public double? TEU { get; set; }
        public double? ValueOfGoods { get; set; }
        public double? FriehgtAmount { get; set; }
        public double? ForiegnChargesAmount { get; set; }
        public double? NoOfPackages { get; set; }
        public List<CustomsChargesPayable> CustomsChargesPayables { get; set; }
    }

    public class CustomsChargesPayable
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
    }
}
