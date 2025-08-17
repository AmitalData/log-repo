
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
    public class CustomerDepositionRepository : IRepository<CustomerDeposition>
    {
        ICommonDataContext commonDataContext;

  

        public CustomerDepositionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerDepositionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CustomerDeposition GetSinglePM(string id, int tenant)
        {
            return (from a in context.CustomerDepositions where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }


        public CustomerDeposition GetSingleCustomerDeposition(string id, int tenant)
        {
            return (from a in context.CustomerDepositions where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }


        public IQueryable<CustomerDeposition> GetCustomerDepositions(int tenant)
        {
            return from a in context.CustomerDepositions
                   where a.Tenant == tenant
                   select a;
        }

        public void Add(CustomerDeposition entity)
        {
            context.CustomerDepositions.Add(entity);
        }

        public void Remove(CustomerDeposition entity)
        {
            context.CustomerDepositions.Attach(entity);
            context.CustomerDepositions.Remove(entity);
        }

        public void Update(CustomerDeposition entity)
        {
            context.CustomerDepositions.Attach(entity);
            context.SetAsModified(entity);
        }
        
        public List<CustomerDeposition> All()
        {
            return context.CustomerDepositions.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomerDeposition> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomerDeposition GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }


        public int? GetLastCustomerDepositionCreated(List<int> customerTenantIds)
        {
            int? tenant = null;

            CustomerDeposition customerDeposition = (from a in context.CustomerDepositions
                    where customerTenantIds.Contains(a.Tenant)
                    select a).OrderByDescending(d => d.CreateDate).FirstOrDefault();

            if (customerDeposition != null)
            {
                tenant = customerDeposition.Tenant;
            }

            return tenant;

        }

        public CustomerDeposition GetCustomerDepositionByCustomsShipperIdAndDepositionNumber(string customsShipperId, string depositionNumber, int tenant)
        {
            return (from a in context.CustomerDepositions where a.CustomsShipperId == customsShipperId && a.DepositionNumber == depositionNumber && a.Tenant == tenant select a).FirstOrDefault();
        }



          public CustomerDeposition GetValidityCustomerDepositionByCustomsShipperId(string customsShipperId, int tenant)
        {
            return (from a in context.CustomerDepositions where a.CustomsShipperId == customsShipperId &&  a.Tenant == tenant select a).OrderByDescending(d=>d.ValidityEndDate).FirstOrDefault();
        }



    }
}

