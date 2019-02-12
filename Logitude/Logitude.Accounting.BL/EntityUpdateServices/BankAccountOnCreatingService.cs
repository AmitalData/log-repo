using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Resolvers;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankAccountOnCreatingService: IBankAccountOnCreatingService
    {
        private IAccountingContext _MainContext;
        public BankAccountOnCreatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        public void OnCreating(BankAccountPM entityPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);


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

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        public virtual string IdCounterWrapperGetNumber(int tenant)
        {
            //return (new IdCounterWrapper()).GetNumber(
            //        "BankAccount", Tenant);
            return IdCounterUtilResolver.GetNewIdCounter("BankAccount", tenant);
        }
    }


    public interface IBankAccountOnCreatingService {
        void OnCreating(BankAccountPM entityPM);
        DateTime GetCurrentDateTime(int tenant);
        ContactPM GetLoggedContact(int tenant);
        string IdCounterWrapperGetNumber(int Tenant);
    }
}
