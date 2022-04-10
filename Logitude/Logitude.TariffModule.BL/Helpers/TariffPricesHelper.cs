using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
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
        public static double? grossWeight;
        public static double? chargeableWeight;
        public static double? volume;
        public static string grossWeightUnitCode;
        public static string chargeableWeightUnitCode;
        public static string volumeUnitCode;
        public static string profitCurrencyId;
        public static double? profitRate;
        public static List<RatesTableList> ratesList;

        public TariffPricesHelper(int tenant)
        {
            ratesList = this.GetRates(tenant);
        }

        public static double? ComputeGrossWeigh_Kg_Ton(string type)
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
        public static double? ComputeChargeableWeight_Kg()
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
        public static double? ComputeVolumeInCBM()
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
        public static double? GetCurrencyRate(string currencyId, string localCurrencyId)
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
        public static double? CalculateProfitAmount(double? expectedAmount, double? localExpectedAmount, string currencyId)
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
        public static double? CalculateLocalAmount(double? expectedAmount, double? rate)
        {
            if (expectedAmount != null && rate != null)
            {
                return Round(expectedAmount * rate, 2);
            }

            return null;
        }
        public static double? Round(double? value, int digits)
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
    }
}
