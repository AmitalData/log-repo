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
    public partial class AutomaticReconcileMethodValidator
    {

        public static ValidationResult IsAutomaticReconcileMethodValid(AutomaticReconcileMethodPM myAutomaticReconcileMethodPM)
        {
            bool valid = true;

            bool exists = CheckCode(myAutomaticReconcileMethodPM.Code, myAutomaticReconcileMethodPM.Id, myAutomaticReconcileMethodPM.Tenant);
            if (exists == true)
            {
                valid = false;
                return new ValidationResult(TextCodesTranslator.TranslateText("AutomaticReconcileMethod.O.CodeAlreadyExists", myAutomaticReconcileMethodPM.Tenant));
            }

            // unique methods
            bool isexist = CheckMethods(myAutomaticReconcileMethodPM.AutomaticReconcile1,
                myAutomaticReconcileMethodPM.AutomaticReconcile2,
                myAutomaticReconcileMethodPM.AutomaticReconcile3, 
                myAutomaticReconcileMethodPM.Tenant);
            if (isexist == true)
            {
                valid = false;
                return new ValidationResult(TextCodesTranslator.TranslateText("AutomaticReconcileMethod.O.uniqueMethods", myAutomaticReconcileMethodPM.Tenant));
            }

            return null;
        }

        // server side validations
        public static bool CheckCode(string code, string id, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodQueryService automaticReconcileMethodQuery = new AutomaticReconcileMethodQueryService(accountingContext);
            return automaticReconcileMethodQuery.CheckWhetherCodeExists(code, id, tenant);
        }

        public static bool CheckMethods(string method1, string method2, string method3, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            AutomaticReconcileMethodQueryService automaticReconcileMethodQuery = new AutomaticReconcileMethodQueryService(accountingContext);
            return automaticReconcileMethodQuery.CheckUniqueMethods(method1, method2, method3, tenant);
        }
    }
}
