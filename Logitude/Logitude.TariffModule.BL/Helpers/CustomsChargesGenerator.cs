using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
        private Shipment shipment;
        private int tenant;
        private List<ByPckageType> BCNTGroupedList = new List<ByPckageType>();
        private List<CustomsChargesPayable> payables;
        private string customAgentExportId;
        private string customAgentImportId;
        private double? grossWeight;
        private double? chargeableWeight;
        private double? volume;
        private string grossWeightUnitCode;
        private string chargeableWeightUnitCode;
        private string volumeUnitCode;
        private string profitCurrencyId;
        private double? profitRate;
        private double? TEU;
        private double? valueOfGoods;
        private double? noOfPackages;
        private List<ShipmentPackage> FCLShipmentPackages;

        public CustomsChargesGenerator(CustomsChargesTariffSearchArgs args, int tenant)
        {
            this.tenant = tenant;
            this.customsChargesTariffSearchArgs = args;
            this.customsChargesTariffSearchArgs.CustomsChargesPayables = new List<CustomsChargesPayable>();
            this.ratesList = this.GetRates(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);            

            this.GetShipment();
            this.SetShipmentPropeaties();
            this.FillBCNTGroupedList();            
        }

        public CustomsChargesTariffSearchArgs GeneratePayables()
        {
            List<Tariff> tariffs = this.GetTariffs(tenant);
            List<TariffLine> tariffLines = this.GetTariffLines(tariffs, tenant);
            
            foreach (TariffLine tariffLine in tariffLines)
            {
                Tariff tariff = tariffs.Where(d => d.Id == tariffLine.TariffId).FirstOrDefault();
                this.CreatePayablesFromCustomChargesLine(tariffLine, tariff);
                this.customsChargesTariffSearchArgs.CustomsChargesPayables.AddRange(this.payables);
            }

            return this.customsChargesTariffSearchArgs;
        }
        private void GetShipment()
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            shipment = shipmentRepository.GetSingleShipment(customsChargesTariffSearchArgs.ShipmentId, tenant);
        }
        private void SetShipmentPropeaties()
        {
            if (shipment != null)
            {                
                this.customAgentExportId = this.shipment.CustomAgentExportId;
                this.customAgentImportId = this.shipment.CustomAgentImportId;
                this.grossWeight = this.shipment.GrossWeight;
                this.chargeableWeight = this.shipment.ChargeableWeight;
                this.volume = this.shipment.Volume;
                this.grossWeightUnitCode = this.shipment.GrossWeightUnitCode;
                this.chargeableWeightUnitCode = this.shipment.ChargeableWeightUnitCode;
                this.volumeUnitCode = this.shipment.VolumeUnitCode;
                this.valueOfGoods = this.shipment.ValueOfGoods;
                this.noOfPackages = MethodHelper.IsFCLEntity(shipment.TransportModeId, shipment.ShipmentTypeId) ? this.shipment.NumberOfContainers : this.shipment.NumberOfPackages;
                this.TEU = this.shipment.TEU;                
                this.profitCurrencyId = this.shipment.ProfitCurrencyId;
                this.profitRate = this.shipment.ProfitExchangeRate;
                this.GetShipmentPackages();
            }
        } 
        private void GetShipmentPackages()
        {
            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
            this.FCLShipmentPackages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(customsChargesTariffSearchArgs.ShipmentId, tenant)
                .Where(a => a.PackageType != null && a.PackageType.IsContainer).ToList();
        }
        private void FillBCNTGroupedList()
        {
            if (this.FCLShipmentPackages.Count > 0)
            {
                foreach (ShipmentPackage item in this.FCLShipmentPackages)
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
        private List<Tariff> GetTariffs(int tenant)
        {
            TariffRepository tariffRepository = new TariffRepository(tenant);
            IQueryable<Tariff> tariffs = tariffRepository.GetActiveCustomsChargesTariffs(tenant);
            List<Tariff> myResult = new List<Tariff>();

            if (!string.IsNullOrEmpty(this.customAgentExportId))
            {
                Tariff exportTariff = tariffs.Where(d => d.SellerId == this.customAgentExportId && d.TypeCode == "ECC").FirstOrDefault();
                if (exportTariff != null)
                {
                    myResult.Add(exportTariff);
                }
            }

            if (!string.IsNullOrEmpty(this.customAgentImportId))
            {
                Tariff importTariff = tariffs.Where(d => d.SellerId == this.customAgentImportId && d.TypeCode == "ICC").FirstOrDefault();
                if (importTariff != null)
                {
                    myResult.Add(importTariff);
                }
            }

            return myResult;
        }
        private List<TariffLine> GetTariffLines(List<Tariff> tariffs, int tenant)
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

                TariffLine customChargesLine = this.FilterTariffLinesBasedOnCountries(tariffLines);

                if (customChargesLine != null)
                {
                    myResult.Add(customChargesLine);
                }
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
        private void CreatePayablesFromCustomChargesLine(TariffLine tariffLine, Tariff tariff)
        {
            this.payables = new List<CustomsChargesPayable>();

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
                foreach(ByPckageType byPckageType in BCNTGroupedList)
                {
                    Measurement measurement = null;
                    PackageType packageType = this.commonContext.PackageTypes.Where(p => p.Tenant == tenant && p.Id == byPckageType.PackageTypeId).FirstOrDefault();
                    if(packageType != null)
                    {
                        measurement = this.commonContext.Measurements.Where(p => p.Tenant == tenant && p.Id == packageType.MeasurementId).FirstOrDefault();
                    }
                    
                    if(measurement != null)
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
                                SellerId = tariff.SellerId,
                                SellerName = tariff.Seller?.EnglishName,
                                CurrencyId = tariffLine.CurrencyId != null ? tariffLine.CurrencyId : tariff.CurrencyId,
                                Quantity = byPckageType.Quantity,
                            };

                            if (tariffLine.IsDifferentCurrenciesPerCharge)
                            {
                                payable.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + index + "CurrencyId").GetValue(tariffLine);
                            }

                            Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == payable.CurrencyId).FirstOrDefault();
                            payable.CurrencyCode = currency?.Code;                            
                            payable.Rate = this.GetCurrencyRate(payable.CurrencyId);
                            payable.Price = Round(price, 3);

                            double? expectedAmount = 0;

                            if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                            {
                                expectedAmount = payable.Quantity * (price / 100);
                            }
                            else
                            {
                                expectedAmount = price * payable.Quantity;
                            }

                            if (minAmount != null)
                            {
                                if (minAmount > expectedAmount)
                                {
                                    expectedAmount = minAmount;
                                    payable.MinAmount = minAmount;
                                }
                            }

                            payable.ExpectedAmount = expectedAmount == null ? 0 : expectedAmount.Value;
                            payable.LocalExpectedAmount = this.CalculateLocalAmount(expectedAmount, payable.Rate);
                            payable.ProfitExpectedAmount = this.CalculateProfitAmount(expectedAmount, payable.LocalExpectedAmount, payable.CurrencyId);                            
                            payables.Add(payable);
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
                        SellerId = tariff.SellerId,
                        SellerName = tariff.Seller?.EnglishName,
                    };

                    payable.CurrencyId = tariffLine.CurrencyId != null ? tariffLine.CurrencyId : tariff.CurrencyId;
                    if (tariffLine.IsDifferentCurrenciesPerCharge)
                    {
                        payable.CurrencyId = (string)tariffLine.GetType().GetProperty("Surcharge" + index + "CurrencyId").GetValue(tariffLine);
                    }

                    Currency currency = this.commonContext.Currencies.Where(p => p.Tenant == tariff.Tenant && p.Id == payable.CurrencyId).FirstOrDefault();
                    payable.CurrencyCode = currency?.Code;

                    switch (measurement.Code)
                    {
                        case "FIXD": { payable.Quantity = 1; break; }
                        case "BTEU": { payable.Quantity = this.TEU; break; }
                        case "PRVL": { payable.Quantity = this.valueOfGoods; break; }
                        case "PRFR": { payable.Quantity = customsChargesTariffSearchArgs.FriehgtAmount; break; }
                        case "QTY": { payable.Quantity = this.noOfPackages; break; }
                        case "GRWT": { payable.Quantity = this.grossWeight; break; }
                        case "CHWT": { payable.Quantity = this.chargeableWeight; break; }
                        case "VOLU": { payable.Quantity = this.volume; break; }
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

                    if (measurement.Code == "PRVL" || measurement.Code == "PRFR")
                    {
                        expectedAmount = payable.Quantity * (price / 100);
                    }
                    else
                    {
                        expectedAmount = price * payable.Quantity;
                    }

                    if (minAmount != null)
                    {
                        if (minAmount > expectedAmount)
                        {
                            expectedAmount = minAmount;
                            payable.MinAmount = minAmount;
                        }
                    }

                    payable.ExpectedAmount = expectedAmount == null ? 0 : expectedAmount.Value;
                    payable.LocalExpectedAmount = this.CalculateLocalAmount(expectedAmount, payable.Rate);
                    payable.ProfitExpectedAmount = this.CalculateProfitAmount(expectedAmount, payable.LocalExpectedAmount, payable.CurrencyId);
                    payables.Add(payable);
                }
            }
        }
        private double? ComputeGrossWeigh_Kg_Ton(string type)
        {
            double? weigh_Kg = null;
            double? weigh_Ton = null;

            if (this.grossWeight != null)
            {
                double factorOfConvert = 1;

                if (!string.IsNullOrEmpty(this.grossWeightUnitCode))
                {
                    switch (this.grossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = this.grossWeight * factorOfConvert;
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

            if (this.chargeableWeight != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(this.chargeableWeightUnitCode))
                {
                    switch (this.chargeableWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = this.chargeableWeight * factorOfConvert;
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

            if (this.volume != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(this.volumeUnitCode))
                {
                    switch (this.volumeUnitCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }
                        case "CBF": { factorOfConvert = 35.315; break; }
                    }
                }
                volumeInCBM = this.volume * factorOfConvert;
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
            if (currencyId == this.profitCurrencyId)
            {
                return expectedAmount;
            }

            else
            {
                return (localExpectedAmount / this.profitRate);
            }
        }
        private double? CalculateLocalAmount(double? expectedAmount, double? rate)
        {
            if (expectedAmount != null && rate != null)
            {
                return Round(expectedAmount * rate, 2);
            }

            return null;
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
        public string ShipmentId { get; set; }
        public string FromCountryId { get; set; }
        public string ToCountryId { get; set; }        
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETD { get; set; }               
        public double? FriehgtAmount { get; set; }
        public double? ForiegnChargesAmount { get; set; }
        public string LocalCurrencyId { get; set; }
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
        public string SellerId { get; set; }
        public string SellerName { get; set; }
    }
}
