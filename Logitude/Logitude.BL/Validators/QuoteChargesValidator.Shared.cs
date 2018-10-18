
using System.ComponentModel.DataAnnotations;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class QuoteChargesValidator
    {
        public static ValidationResult IsQuoteChargeValid(QuoteChargePM quoteChargePM, ValidationContext context)
        {
            if (quoteChargePM.QuoteTypeCode=="A")
            {
                if (quoteChargePM.SaleExchangeRate == null)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("QuoteCharge.M.ExchangeRateIsRequired", quoteChargePM.Tenant));
                }
            }
            
            return ValidationResult.Success;

        }
    }
}