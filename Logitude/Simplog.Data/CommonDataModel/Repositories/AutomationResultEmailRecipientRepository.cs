using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AutomationResultEmailRecipientRepository : IRepository<AutomationResultEmailRecipient>
    {
        ICommonDataContext commonDataContext;

        public AutomationResultEmailRecipientRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AutomationResultEmailRecipientRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public AutomationResultEmailRecipientRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AutomationResultEmailRecipient GetSingleAutomationResultEmailRecipient(string id, int tenant)
        {
            return (from a in this.context.AutomationResultEmailRecipients
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public IQueryable<AutomationResultEmailRecipient> GetAutomationResultEmailRecipient()
        {
            return this.context.AutomationResultEmailRecipients;
        }

        public List<AutomationResultEmailRecipient> GetAutomationResultEmailRecipientByAutomationId(string automationId, int tenant)
        {
            List<AutomationResultEmailRecipient> automationResultEmailRecipientes = (from a in this.context.AutomationResultEmailRecipients
                                                                                       where a.Tenant == tenant && a.AutomationsId == automationId  && a.RecipientType !="Fixed"
                                                                                     select a).ToList();                                                                 
            return automationResultEmailRecipientes;

        }

        public IQueryable<AutomationResultEmailRecipient> GetAutomationResultEmailRecipients(int tenant)
        {
            IQueryable<AutomationResultEmailRecipient> automationResultEmailRecipientes = (from a in this.context.AutomationResultEmailRecipients
                                                                                     where a.Tenant == tenant
                                                                                     select a);                                                                 
            return automationResultEmailRecipientes;

        }


        

        public void Add(AutomationResultEmailRecipient entity)
        {
            this.context.AutomationResultEmailRecipients.Add(entity);
        }

        public void Remove(AutomationResultEmailRecipient entity)
        {

            this.context.AutomationResultEmailRecipients.Attach(entity);

            this.context.AutomationResultEmailRecipients.Remove(entity);
        }

        public void Update(AutomationResultEmailRecipient entity)
        {
            this.context.AutomationResultEmailRecipients.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<AutomationResultEmailRecipient> All()
        {
            return this.context.AutomationResultEmailRecipients.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<AutomationResultEmailRecipient> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AutomationResultEmailRecipient GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}