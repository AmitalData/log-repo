using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AutomationHistoryRepository : IRepository<AutomationHistory>
    {
        ICommonDataContext commonDataContext;

        public AutomationHistoryRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AutomationHistoryRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public AutomationHistoryRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AutomationHistory GetSingleAutomationHistory(int  version,string automationId,int tenant)
        {
            return (from a in this.context.AutomationHistorys
                    where a.Version == version && a.AutomationsId == automationId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public string GetAutomationXMLFromAutomationHistoryByDate(DateTime? updatedate , string automationId,  int tenant)
        {
            return (from a in this.context.AutomationHistorys
                    where a.Tenant == tenant && a.AutomationsId == automationId && a.CreateDate <= updatedate
                    orderby a.CreateDate descending
                    select a.AutomationXML).FirstOrDefault();
        }




        public void Add(AutomationHistory entity)
        {
            this.context.AutomationHistorys.Add(entity);
        }

        public void Remove(AutomationHistory entity)
        {

            this.context.AutomationHistorys.Attach(entity);

            this.context.AutomationHistorys.Remove(entity);
        }

        public void Update(AutomationHistory entity)
        {
            this.context.AutomationHistorys.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<AutomationHistory> All()
        {
            return this.context.AutomationHistorys.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<AutomationHistory> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AutomationHistory GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}