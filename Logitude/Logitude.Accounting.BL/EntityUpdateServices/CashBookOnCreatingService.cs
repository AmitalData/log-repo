using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
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
    public class CashBookOnCreatingService : ICashBookOnCreatingUpdateService
    {
        private IAccountingContext _MainContext;
        public CashBookOnCreatingService(IAccountingContext mainContext)
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

        public virtual string GetObjectTableId(CashBookPM entityPM)
        {
            throw new NotImplementedException();
        }

        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "CashBook", Tenant);
        }

        public void OnCreating(CashBookPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant); //IdCounter.GetNumber("CashBook", entityPM.Tenant);

            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;
            entityPM.Inactive = false;

            DateTime todayDateTime = GetCurrentDateTime(entityPM);

            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;


            string myLoggedUserId= GetLogContactId(entityPM);
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.CreatedByUserId == null)
            {
                entityPM.CreatedByUserId = myLoggedUserId;
            }

        }
    }

    public interface ICashBookOnCreatingUpdateService
    {
        void OnCreating(CashBookPM entityPM, EntityPM entityParentPM);
        string GetObjectTableId(CashBookPM entityPM);
        string GetLogContactId(CashBookPM entityPM);
        DateTime GetCurrentDateTime(CashBookPM entityPM);
    }
}
