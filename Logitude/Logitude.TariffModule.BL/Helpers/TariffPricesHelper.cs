using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
    public  class TariffPricesHelper
    {
        public double? grossWeight;
        public double? chargeableWeight;
        public double? volume;
        public string grossWeightUnitCode;
        public string chargeableWeightUnitCode;
        public string volumeUnitCode;
        public string profitCurrencyId;
        public double? profitRate;
        public List<RatesTableList> ratesList;
        private int tenant;

        public TariffPricesHelper(int tenant)
        {
            this.tenant = tenant;
            ratesList = this.GetRates(tenant);
        }

        public double? ComputeGrossWeigh_Kg_Ton(string type)
        {
            double? weigh_Kg = null;
            double? weigh_Ton = null;

            if (grossWeight != null)
            {
                double factorOfConvert = 1;

                if (!string.IsNullOrEmpty(grossWeightUnitCode))
                {
                    switch (grossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = grossWeight * factorOfConvert;
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
        public double? ComputeChargeableWeight_Kg()
        {
            double? weigh_Kg = null;

            if (chargeableWeight != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(chargeableWeightUnitCode))
                {
                    switch (chargeableWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = chargeableWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);
            }
            return weigh_Kg;
        }
        public double? ComputeVolumeInCBM()
        {
            double? volumeInCBM = null;

            if (volume != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(volumeUnitCode))
                {
                    switch (volumeUnitCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }
                        case "CBF": { factorOfConvert = 35.315; break; }
                    }
                }
                volumeInCBM = volume * factorOfConvert;
            }

            if (volumeInCBM != null)
            {
                volumeInCBM = Round(volumeInCBM, 3);
            }
            return volumeInCBM;
        }
        public double? GetCurrencyRate(string currencyId, string localCurrencyId)
        {
            double? myResult = null;

            if (currencyId == localCurrencyId)
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
        public double? CalculateProfitAmount(double? expectedAmount, double? localExpectedAmount, string currencyId)
        {
            if (currencyId == profitCurrencyId)
            {
                return expectedAmount;
            }

            else
            {
                return (localExpectedAmount / profitRate);
            }
        }
        public double? CalculateLocalAmount(double? expectedAmount, double? rate)
        {
            if (expectedAmount != null && rate != null)
            {
                return Round(expectedAmount * rate, 2);
            }

            return null;
        }
        public double? Round(double? value, int digits)
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

        public List<RatesTableList> GetRates(int tenant)
        {
            IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTableRepository = new RatesTableRepository(MyContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);

            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            IQueryable<RatesTableList> entityLists = ratesTableQuery.GetIQueryableEntityList(entityPocos);
            entityLists = entityLists.OrderByDescending(r => r.ValueDate);

            return entityLists.ToList();
        }

        public decimal CalculateLocalAmount(decimal amount, string convertedCurrencyId, string tariffLineCurrencyId)
        {
            var tenantCurrency = GetTenantCurrency();
            decimal amountInTariffCurr, amountInConvertedCurr;

            if (convertedCurrencyId == tariffLineCurrencyId)
            {
                amountInTariffCurr = amount;
            }

            else
            {
                if (tenantCurrency == tariffLineCurrencyId)
                    amountInTariffCurr = amount;
                else
                {
                    RatesTableList rateList = ratesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == tariffLineCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amount * (decimal)rate;

                }

                if (tenantCurrency == convertedCurrencyId)
                    amountInConvertedCurr = amountInTariffCurr;

                else
                {
                    RatesTableList rateList = ratesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == convertedCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amountInTariffCurr / (decimal)rate;
                }
            }

            return amountInTariffCurr;
        }
        private string GetTenantCurrency()
        {
            TenantRepository tRepo = new TenantRepository(tenant);
            Tenant t = tRepo.GetSingleByTenant(tenant);
            var tenantCurrency = (t == null ? null : t.CurrencyId);
            return tenantCurrency;
        }
    }
}
