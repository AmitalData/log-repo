using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestReportUpdateService
    {
        protected override void OnCreating(InterestReportPM entityPM, EntityPM entityParentPM)
        {
            entityPM.CreateDateTime = DateTime.UtcNow;
            entityPM.InterestReportStatusCode = "1";
            entityPM.ReportNumber = CodeCounter.GetNumber("InterestReport", entityPM.Tenant).ToString();
            GLAccountRepository gLAccountRepository = new GLAccountRepository(entityPM.Tenant);
            GLAccount gLAccount = gLAccountRepository.GetSingle(entityPM.GLAccountId, entityPM.Tenant);
            entityPM.GLAccountInterestCreditLimit = gLAccount.InterestCreditLimit;
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            if (gLAccount.Inactive == true)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotdefined", entityPM.Tenant, showLocals));
            }
        }

        protected override void OnUpdating(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;

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
