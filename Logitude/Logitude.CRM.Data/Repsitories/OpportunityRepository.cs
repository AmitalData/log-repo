 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class OpportunityRepository:IRepository<Opportunity>
   {
		public List<Opportunity> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }
       
        public IQueryable<Opportunity> GetAllFromIdList(List<string> ids, int tenant)
        {
            IQueryable<Opportunity> entities = (from a in context.Opportunities where a.Tenant == tenant && ids.Contains(a.Id) select a);
            return entities;
        }

        public IQueryable<Opportunity> GetAllForReport(int tenant)
        {
            return from a in context.Opportunities.Include("Customer") where a.Tenant == tenant select a;
        }


        public Opportunity GetOppByCustomerIdAndTenant(string customerId, int tenant)
        {
            return (from a in context.Opportunities
                    where a.CustomerId == customerId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public IQueryable<Opportunity> GetOppListByCustomerIdAndTenant(string customerId, int tenant)
        {
            return from a in context.Opportunities
                   where a.CustomerId == customerId && a.Tenant == tenant
                   select a;
        }

        partial void OnEntityUpdate(Opportunity entity)
        {
            UpdateAnalyticTable(entity);
        }


        partial void OnEntityAdd(Opportunity entity)
        {
            AddToAnalyticTable(entity);
        }

        private void UpdateAnalyticTable(Opportunity entity)
        {
            var opportunityAnalytic = Map<OpportunityAnalytic>(entity);
            if (context.OpportunityAnalytics.Any(e => e.Id == opportunityAnalytic.Id)) context.OpportunityAnalytics.Attach(opportunityAnalytic);
            else AddToAnalyticTable(entity);
        }

        private void AddToAnalyticTable(Opportunity entity)
        {
            var opportunityAnalytic = Map<OpportunityAnalytic>(entity);
            context.OpportunityAnalytics.Add(opportunityAnalytic);
        }

        private T Map<T>(Opportunity from) where T : new()
        {
            var toPropes = typeof(T).GetProperties();
            var fromPropes = from.GetType().GetProperties().ToDictionary(e => e.Name, e => e);
            var to = new T();
            foreach (var item in toPropes)
            {
                if (fromPropes.ContainsKey(item.Name))
                {
                    item.SetValue(to, fromPropes[item.Name].GetValue(from));
                }
            }
            return to;
        }

    }

}
   