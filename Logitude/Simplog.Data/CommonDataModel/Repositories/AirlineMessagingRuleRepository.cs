using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AirlineMessagingRuleRepository : IRepository<AirlineMessagingRule>
    {
        ICommonDataContext commonDataContext;

        public AirlineMessagingRuleRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AirlineMessagingRuleRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public AirlineMessagingRuleRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<AirlineMessagingRule> GetAirlineMessagingRules(int tenant)
        {
            return (from record in context.AirlineMessagingRules.Include("RuleField") where record.Tenant == tenant select record);
        }

        public AirlineMessagingRule GetSingleAirlineMessagingRule(string id, int tenant)
        {
            return (from record in context.AirlineMessagingRules.Include("RuleField") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<AirlineMessagingRule> GetAirlineMessagingRulesForAirline(string airlineCode, string messageType, int tenant)
        {
            return (from record in context.AirlineMessagingRules.Include("Airline").Include("RuleField") 
                    where record.Tenant == tenant && record.Airline.Code == airlineCode && record.MessageTypeCode == messageType && !record.InActive
                    select record);
        }

        public void Add(AirlineMessagingRule entity)
        {
            context.AirlineMessagingRules.Add(entity);
        }

        public void Remove(AirlineMessagingRule entity)
        {
            context.AirlineMessagingRules.Attach(entity);
            context.AirlineMessagingRules.Remove(entity);
        }

        public void Update(AirlineMessagingRule entity)
        {
            context.AirlineMessagingRules.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AirlineMessagingRule> All()
        {
            return context.AirlineMessagingRules.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AirlineMessagingRule> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AirlineMessagingRule GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}