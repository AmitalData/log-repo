using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerCompetitorRepository : IRepository<CustomerCompetitor>
    {
        ICommonDataContext commonDataContext;

        public CustomerCompetitorRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerCompetitorRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerCompetitorRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CustomerCompetitor> GetCustomerCompetitors(int tenant)
        {
            return (from record in context.CustomerCompetitors
                    where record.Tenant == tenant
                    select record);
        }

        public CustomerCompetitor GetSingleCustomerCompetitor(string customerId, string competitorId, int tenant)
        {
            return (from a in commonDataContext.CustomerCompetitors
                    where a.CustomerId == customerId && a.CompetitorId == competitorId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CustomerCompetitor entity)
        {
            context.CustomerCompetitors.Add(entity);
        }

        public void Remove(CustomerCompetitor entity)
        {
            context.CustomerCompetitors.Attach(entity);
            context.CustomerCompetitors.Remove(entity);
        }

        public void Update(CustomerCompetitor entity)
        {
            try
            {
                context.CustomerCompetitors.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerCompetitor> All()
        {
            return context.CustomerCompetitors.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerCompetitor> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerCompetitor GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}