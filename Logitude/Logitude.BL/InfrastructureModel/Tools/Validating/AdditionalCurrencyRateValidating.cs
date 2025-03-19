using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public partial class AdditionalCurrencyRateValidating
    {
        private const double RateMinValue = 0.5;
        private const double RateMaxValue = 1.5;

        public static void Validate(AdditionalCurrencyRatePM entityPM)
        {
            ValidationResult result = IsRateValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
        }

        private static ValidationResult IsRateValid(AdditionalCurrencyRatePM additionalCurrencyRatePM)
        {
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