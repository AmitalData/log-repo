using System;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public partial class AdditionalCurrencyRateValidating
    {
        private const double RateMinValue = 0.5;
        private const double RateMaxValue = 1.5;
        public AdditionalCurrencyRateRepository repository;
        private const double CountMaxValue = 4;

        public static void Validate(AdditionalCurrencyRatePM entityPM, AdditionalCurrencyRateRepository entityRepository)
        {
            ValidationResult result = IsRateValid(entityPM, entityRepository);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
        }

        private static ValidationResult IsRateValid(AdditionalCurrencyRatePM additionalCurrencyRatePM, AdditionalCurrencyRateRepository entityRepository)
        {

            if (additionalCurrencyRatePM != null && entityRepository!=null)
            {
               var count = entityRepository.GetCountByTenant(additionalCurrencyRatePM.Tenant);
                if (count >= CountMaxValue)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("AdditionalCurrencyRate.O.MaxRateCount", 0, true));
                }
            }
            if (additionalCurrencyRatePM.RateCoefficient == null)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AdditionalCurrencyRate.O.RateRequired", 0, true));
            }
            else if (additionalCurrencyRatePM.RateCoefficient < RateMinValue || additionalCurrencyRatePM.RateCoefficient > RateMaxValue)
            {
                string errorMessage = TextCodesTranslator.TranslateText("AdditionalCurrencyRate.O.RateRange", 0, true);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = errorMessage.Replace("{min}", RateMinValue.ToString()).Replace("{max}", RateMaxValue.ToString());
                }
                return new ValidationResult(errorMessage);
            }

            return null;
        }
    }
}