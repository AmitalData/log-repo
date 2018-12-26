using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankDepositOnCreatingService: IBankDepositOnCreatingService
    {
        private IAccountingContext _MainContext;
        public BankDepositOnCreatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        // Main Method
        public void OnCreating(BankDepositPM entityPM)
        {
            // Id 
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);

            // code
            if (entityPM.DepositNumber == 0) entityPM.DepositNumber = CodeCounterWrapperGetNumber(entityPM.Tenant);

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

            // dates
            entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);
            entityPM.CreateDate = GetCurrentDateTime(entityPM.Tenant);

            // LOGIC
            foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            {
                item.DepositId = entityPM.Id;
            }

            // Activity log
            //if (user != null)
            //{
            //    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            //    ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
            //    ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", user.Id);
            //}



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

        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "BankDeposit", Tenant);
        }

        public virtual int CodeCounterWrapperGetNumber(int Tenant)
        {
            return (new CodeCounterWrapper(false)).GetNumber(
                    "BankDeposit", Tenant);
        }

    }


    public interface IBankDepositOnCreatingService {
        void OnCreating(BankDepositPM entityPM);
        DateTime GetCurrentDateTime(int tenant);
        ContactPM GetLoggedContact(int tenant);
        string IdCounterWrapperGetNumber(int Tenant);
        int CodeCounterWrapperGetNumber(int Tenant);
    }
}
