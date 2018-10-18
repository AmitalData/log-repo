using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.Validators
{
    public partial class AccountingPeriodValidator
    {
        // server side validations
        public static ValidationResult IsAccountingPeriodValid(AccountingPeriodPM period)
        {
            if (period.OpenMonth <= 0 || period.OpenMonth > 12)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AccountingPeriods.O.WrongOpenMonth", period.Tenant));
            }

            if (period.ClosedMonth.HasValue && (period.ClosedMonth.Value <= 0 || period.ClosedMonth.Value > 12))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AccountingPeriods.O.WrongClosedMonth", period.Tenant));
            }
            DateTime now = DateTime.Now;
            if (period.Year > now.Year || (period.Year == now.Year && (period.OpenMonth > now.Month || (period.ClosedMonth.HasValue && period.ClosedMonth.Value > now.Month))))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AccountingPeriods.O.FutureMonthForbidden", period.Tenant));
            }
            if (period.ClosedMonth.HasValue && period.ClosedMonth.Value > period.OpenMonth)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("AccountingPeriods.O.ClosedAfterOpen", period.Tenant));
            }

            return null;
        }
    }
}