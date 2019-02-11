using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.BL.Validators
{
    public partial class RevaluationValidator
    {
        public static ValidationResult IsRevaluationValid(RevaluationPM myRevaluationPM)
        {
            if (myRevaluationPM != null)
            {
                DateTime now = DateTime.Now;
                if (myRevaluationPM.RevaluationDate.Date > now.Date)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("Revaluations.Q.FutureDateForbidden", myRevaluationPM.Tenant));
                }
            }
            if (String.IsNullOrEmpty(myRevaluationPM.GLAccountId) && String.IsNullOrEmpty(myRevaluationPM.ChartOfAccountsId)
                    && !(myRevaluationPM.RevaluationEnabled.HasValue && myRevaluationPM.RevaluationEnabled.Value == true))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("Revaluations.Q.DataMissing", myRevaluationPM.Tenant));
            }
            if (myRevaluationPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            { 
                IAccountingContext accountingContext = AccountingContext.GetContext(myRevaluationPM.Tenant);
                RevaluationListQueryService revaluationListQueryService = new RevaluationListQueryService(accountingContext);
                List<RevaluationList> revaluations = revaluationListQueryService.GetOpenRevaluationList(myRevaluationPM.Tenant);
                if (revaluations != null && revaluations.Count > 0)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("Revaluations.Q.OpenRevaluations", myRevaluationPM.Tenant));
                }
            }
            return null;
        }
    }
}
