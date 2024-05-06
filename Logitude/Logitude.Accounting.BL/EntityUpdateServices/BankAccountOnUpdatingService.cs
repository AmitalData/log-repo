using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Resolvers;
using Microsoft.Practices.Unity;
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
            if (!entityPM.ChequeCounter.HasValue)
            {
                var nextSerial = entityPM.ChequeCounterSerials.FirstOrDefault(c => !c.Inactive);
                if (nextSerial != null)
                {
                    entityPM.ChequeCounter = nextSerial.ChequeCounterBegin;
                    entityPM.ChequeCounterSeriesID = nextSerial.SeriesId;
                }
            }
            else
            {
                var currentSerial = entityPM.ChequeCounterSerials.FirstOrDefault(c => c.SeriesId == entityPM.ChequeCounterSeriesID);
                if (currentSerial.Inactive)
                {
                    var nextSerial = entityPM.ChequeCounterSerials.FirstOrDefault(c => !c.Inactive && c.SeriesId > entityPM.ChequeCounterSeriesID);
                    if (nextSerial != null)
                    {
                        entityPM.ChequeCounter = nextSerial.ChequeCounterBegin;
                        entityPM.ChequeCounterSeriesID = nextSerial.SeriesId;
                    }
                }
                else
                {
                    if (entityPM.ChequeCounter > currentSerial.ChequeCounterEnd)
                    {
                        var nextSerial = entityPM.ChequeCounterSerials.FirstOrDefault(c => !c.Inactive && c.SeriesId > entityPM.ChequeCounterSeriesID);
                        if (nextSerial != null)
                        {
                            entityPM.ChequeCounter = nextSerial.ChequeCounterBegin;
                            entityPM.ChequeCounterSeriesID = nextSerial.SeriesId;
                        }
                    }
                }
            }
        }

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return DateTimeUtilResolver.GetDateCurrentDateTime(tenant);//TenantServerConfigration.GetCurrentDateTime(tenant);
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


    }

    public interface IBankAccountOnUpdatingUpdateService
    {
        ContactPM GetLoggedContact(int tenant);
        void OnUpdating(BankAccountPM entityPM, BankAccount entityPOCO);
       
        DateTime GetCurrentDateTime(int tenant);
    }
}
