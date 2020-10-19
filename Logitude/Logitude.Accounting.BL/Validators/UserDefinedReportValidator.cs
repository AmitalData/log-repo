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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Server.Infrastructure;
using System.Runtime.InteropServices;

namespace Logitude.Accounting.BL.Validators
{
    public partial class UserDefinedReportValidator
    {
        // server side validations
        public static ValidationResult IsUserDefinedReportValid(UserDefinedReportPM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;

            ValidateCanUpdaeReport(entityPM, showLocals);

            return null;
        }

       

        private static void ValidateCanUpdaeReport(UserDefinedReportPM entityPM, bool showLocals)
        {
            bool IsNotDeletde = false;
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                if (periodPM.ChangeSetOp != ChangeSetOperation.Delete && !periodPM.IsCancelled)
                {
                    IsNotDeletde = true;
                    break;
                }
            }
            if (!IsNotDeletde)
                throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.TheTeportNeedsAtLeastOne", entityPM.Tenant, showLocals));

   
        }

        
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}