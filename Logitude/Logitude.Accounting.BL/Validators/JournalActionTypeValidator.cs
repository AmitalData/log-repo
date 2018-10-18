using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.Validators
{
    public partial class JournalActionTypeValidator
    {

        public static ValidationResult IsJournalActionTypeValid(JournalActionTypePM myJournalActionTypePM)
        {
            bool valid = true;

            bool exists = CheckCode(myJournalActionTypePM.Code, myJournalActionTypePM.Id, myJournalActionTypePM.Tenant);
            if (exists == true)
            {
                valid = false;
                return new ValidationResult(TextCodesTranslator.TranslateText("JournalActionType.O.CodeAlreadyExists", myJournalActionTypePM.Tenant));
            }
            return null;
        }

        // server side validations
        public static bool CheckCode(string code, string id, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            JournalActionTypeQueryService journalActionTypeQuery = new JournalActionTypeQueryService(accountingContext);
            return journalActionTypeQuery.CheckWhetherCodeExists(code, id, tenant);
        }
    }
}
