using AmitalCloud.Infrastructure.Data.BaseClasses;
using AmitalCloud.Infrastructure.Data.Context;using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class GeneralLockRepository : Repository<GeneralLock>
    {
        IAmitalCloudContext currentContext;

        public GeneralLockRepository() : this(0)
        {
        }
        public GeneralLockRepository(int tenant) : this(AmitalCloudContext.GetContext(tenant))
        {
        }


        public GeneralLockRepository(IAmitalCloudContext context) : base(context)
        {
            currentContext = context;
        }

        public GeneralLock GetSingleGeneralLock(string generalKey, int tenant)
        {


            var poco = (from a in context.GeneralLocks
                        where a.GeneralKey == generalKey && a.Tenant == tenant
                        select a).FirstOrDefault();


            return poco;
        }
        public GeneralLock GetSingleGeneralLockNOWAIT(string generalKey, int tenant)
        {
            throw new NotImplementedException();
            //return GetListNOWAITWhere(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant).FirstOrDefault(); ;
        }
        public void FastDelete(string generalKey, int tenant)
        {
            DeleteWhere(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant);
        }
        public void FastDeleteIfCreated15MinOld(string generalKey, int tenant)
        {
            DateTime createdAtb4_15min = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(-15);
                DeleteWhere(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant
                && rec.CreatedAt < createdAtb4_15min
                );
        }
        public IQueryable<GeneralLock> GetGeneralLocks()
        {
            return context.GeneralLocks;
        }
        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }





    }

}
