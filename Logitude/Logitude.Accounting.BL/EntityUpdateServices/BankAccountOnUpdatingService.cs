using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankAccountOnUpdatingService : IBankAccountOnUpdatingUpdateService
    {
        private IAccountingContext _MainContext;
        public BankAccountOnUpdatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        public void OnUpdating(BankAccountPM entityPM, BankAccount entityPOCO)
        {
            bool useLocal = true;

            entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);//DateTime.Now;
            // users:
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.UpdatedByUserId = user.Id;

                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = user.Id;
                }
            }
           

            entityPM.SearchFields = entityPM.AccountNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.BranchNumber;

        }

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }


        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        public virtual ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

    }

    public interface IBankAccountOnUpdatingUpdateService
    {
        ContactPM GetLoggedContact(int tenant);
        void OnUpdating(BankAccountPM entityPM, BankAccount entityPOCO);
       
        DateTime GetCurrentDateTime(int tenant);
    }
}
