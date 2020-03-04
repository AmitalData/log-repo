using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class RuleUpdateHistoryRepository : IRepository<RuleUpdateHistory>
    {
        IWebFreightContext webFreightContext;
        public RuleUpdateHistoryRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public RuleUpdateHistoryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public RuleUpdateHistoryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }



        public IQueryable<RuleUpdateHistory> GetRuleUpdateHistories(int tenant)
        {
            IQueryable<RuleUpdateHistory> result = (from a in context.RuleUpdateHistories
                                          where a.Tenant == tenant
                                          select a);


            return result;
        }




        public RuleUpdateHistory GetSingleRuleUpdateHistory(string id, int tenant)
        {
            return context.RuleUpdateHistories.Where(d => d.Tenant == tenant && d.Id == id).FirstOrDefault();
        }

        public RuleUpdateHistory GetRuleUpdateHistoryByRuleCode(string ruleCode, int tenant)
        {
            return context.RuleUpdateHistories.Where(d => d.Tenant == tenant && d.RuleCode == ruleCode).FirstOrDefault();
        }

        

        public void Add(RuleUpdateHistory entity)
        {
            this.context.RuleUpdateHistories.Add(entity);
        }

        public void Remove(RuleUpdateHistory entity)
        {
            this.context.RuleUpdateHistories.Remove(entity);
        }

        public void Update(RuleUpdateHistory entity)
        {
            this.context.RuleUpdateHistories.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<RuleUpdateHistory> All()
        {
            return this.context.RuleUpdateHistories.ToList();
        }

        public IWebFreightContext context
        {
            get { return this.webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.webFreightContext.SaveChanges();
        }


        public List<RuleUpdateHistory> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RuleUpdateHistory GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}