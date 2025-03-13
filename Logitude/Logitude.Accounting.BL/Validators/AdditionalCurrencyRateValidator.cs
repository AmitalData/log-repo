using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.Validators
{
    public partial class AdditionalCurrencyRateValidator
    {
        private const double RateMinValue = 0.5;
        private const double RateMaxValue = 1.5;

        public static ValidationResult IsRateValid(AdditionalCurrencyRatePM additionalCurrencyRatePM)
        {
            if (additionalCurrencyRatePM.Rate == null)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AdditionalCurrencyRate.O.RateRequired", 0, true));
            }
            else if (additionalCurrencyRatePM.Rate < RateMinValue || additionalCurrencyRatePM.Rate > RateMaxValue)
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