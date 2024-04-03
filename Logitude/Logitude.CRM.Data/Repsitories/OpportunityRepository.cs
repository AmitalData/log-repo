
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
    public partial class OpportunityRepository : IRepository<Opportunity>
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

    }

}
