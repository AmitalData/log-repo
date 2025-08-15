using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AutomationLastUpdateRepository : IRepository<AutomationLastUpdate>
    {
        ICommonDataContext commonDataContext;

 

        public AutomationLastUpdateRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public AutomationLastUpdateRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AutomationLastUpdate GetSingleAutomationLastUpdate(string objecttableId, int tenant)
        {
            return (from a in this.context.AutomationLastUpdates
                    where a.ObjectTableId == objecttableId &&  a.Tenant == tenant
                    select a).FirstOrDefault();
        }



    

        public void Add(AutomationLastUpdate entity)
        {
            this.context.AutomationLastUpdates.Add(entity);
        }

        public void Remove(AutomationLastUpdate entity)
        {

            this.context.AutomationLastUpdates.Attach(entity);

            this.context.AutomationLastUpdates.Remove(entity);
        }

        public void Update(AutomationLastUpdate entity)
        {
            this.context.AutomationLastUpdates.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<AutomationLastUpdate> All()
        {
            return this.context.AutomationLastUpdates.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<AutomationLastUpdate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AutomationLastUpdate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}