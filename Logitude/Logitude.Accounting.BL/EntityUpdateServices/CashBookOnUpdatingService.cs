using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class CashBookOnUpdatingService : ICashBookOnUpdatingUpdateService
    {
        private IAccountingContext _MainContext;
        public CashBookOnUpdatingService(IAccountingContext mainContext)
        {
            this._MainContext = mainContext;
        }
        public virtual DateTime GetCurrentDateTime(CashBookPM entityPM)
        {
            return TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
        }

        public virtual string GetLogContactId(CashBookPM entityPM)
        {
            //ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }
            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            return myLoggedUserId;
        }

        public void OnUpdating(CashBookPM entityPM)
        {
            DateTime todayDateTime = GetCurrentDateTime(entityPM);//TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.Inactive == null)
            {
                entityPM.Inactive = false;
            }

            entityPM.UpdateDate = todayDateTime;

            string myLoggedUserId = GetLogContactId(entityPM);

            entityPM.UpdatedByUserId = myLoggedUserId;


            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;
        }
    }

    public interface ICashBookOnUpdatingUpdateService {
        void OnUpdating(CashBookPM entityPM);
        string GetLogContactId(CashBookPM entityPM);
        DateTime GetCurrentDateTime(CashBookPM entityPM);
    }
}
