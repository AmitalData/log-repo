using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerForwarderByProductQuery
    {
          CustomerForwarderByProductRepository repository;

        public CustomerForwarderByProductQuery()
        {
            repository = new CustomerForwarderByProductRepository(); 
            
        }

        public CustomerForwarderByProductQuery(int tenant)
        {
            repository = new CustomerForwarderByProductRepository(tenant);
        }

        public CustomerForwarderByProductQuery(CustomerForwarderByProductRepository repository)
        {
            this.repository = repository;
        }

        public CustomerForwarderByProductPM GetSinglePM(string producttypecode, string customerId, int tenant)
        {
            CustomerForwarderByProductPM result = null;
            CustomerForwarderByProduct entityPoco = repository.GetSingleCustomerForwarderByProduct(producttypecode, customerId, tenant);

            if (entityPoco != null)
            {
                result = new CustomerForwarderByProductPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    Tenant = entityPoco.Tenant,
                    ForwarderId = entityPoco.ForwarderId,
                    CustomerId = entityPoco.CustomerId,
                    ForwarderName = entityPoco.ForwarderCard != null? entityPoco.ForwarderCard.EnglishName : null,
          
                };
            }

            return result;
        }

        public List<CustomerForwarderByProductPM> GetCustomerForwarderByProductPMs(int tenant,string customerId)
        {
            List<CustomerForwarderByProduct> pocos = repository.GetCustomerForwarderByProductforCustomer(tenant, customerId).ToList();

            List<CustomerForwarderByProductPM> entityPMs = (from a in pocos
                                                           select new CustomerForwarderByProductPM()
                                                           {
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               CustomerId = a.CustomerId,
                                                               Tenant = a.Tenant,
                                                               ForwarderId = a.ForwarderId,
                                                               ForwarderName = a.ForwarderCard != null ? a.ForwarderCard.EnglishName : null,
                                                             
                                                           }).ToList();
            return entityPMs;
        }

        public IQueryable<CustomerForwarderByProductList> GetIQueryableEntityList(IQueryable<CustomerForwarderByProduct> iQueryable,int tenant)
        {
            IQueryable<CustomerForwarderByProductList> result = from entity in iQueryable
                                             select new CustomerForwarderByProductList()
                                             {
                                                 ProductTypeCode = entity.ProductTypeCode,
                                                 Tenant = entity.Tenant,
                                                 ForwarderId = entity.ForwarderId,
                                                 CustomerId = entity.CustomerId,
                                                 
                                             };
            return result;
        }
    }
}
