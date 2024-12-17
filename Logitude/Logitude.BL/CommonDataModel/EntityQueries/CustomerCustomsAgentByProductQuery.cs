using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerCustomsAgentByProductQuery
    {
         CustomerCustomsAgentByProductRepository repository;

        public CustomerCustomsAgentByProductQuery()
        {
            repository = new CustomerCustomsAgentByProductRepository(); 
            
        }

        public CustomerCustomsAgentByProductQuery(int tenant)
        {
            repository = new CustomerCustomsAgentByProductRepository(tenant);
        }

        public CustomerCustomsAgentByProductQuery(CustomerCustomsAgentByProductRepository repository)
        {
            this.repository = repository;
        }

        public CustomerCustomsAgentByProductPM GetSinglePM(string producttypecode, string customerId, int tenant)
        {
            CustomerCustomsAgentByProductPM result = null;
            CustomerCustomsAgentByProduct entityPoco = repository.GetSingleCustomerCustomsAgentByProduct(producttypecode, customerId, tenant);

            if (entityPoco != null)
            {
                result = new CustomerCustomsAgentByProductPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    Tenant = entityPoco.Tenant,
                    CustomsAgentId = entityPoco.CustomsAgentId,
                    CustomerId = entityPoco.CustomerId,
                    CustomsAgentName =entityPoco.CustomsAgentCard != null ? entityPoco.CustomsAgentCard.EnglishName : null,
          
                };
            }

            return result;
        }

        public List<CustomerCustomsAgentByProductPM> GetCustomerCustomsAgentByProductPMs(int tenant,string customerId)
        {
            List<CustomerCustomsAgentByProduct> pocos = repository.GetCustomerCustomsAgentByProductforCustomer(tenant, customerId).ToList();

            List<CustomerCustomsAgentByProductPM> entityPMs = (from a in pocos
                                                           select new CustomerCustomsAgentByProductPM()
                                                           {
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               CustomerId = a.CustomerId,
                                                               Tenant = a.Tenant,
                                                               CustomsAgentId = a.CustomsAgentId,
                                                               CustomsAgentName = a.CustomsAgentCard != null ? a.CustomsAgentCard.EnglishName : null,
                                                             
                                                           }).ToList();
            return entityPMs;
        }

        public IQueryable<CustomerCustomsAgentByProductList> GetIQueryableEntityList(IQueryable<CustomerCustomsAgentByProduct> iQueryable,int tenant)
        {
            IQueryable<CustomerCustomsAgentByProductList> result = from entity in iQueryable
                                             select new CustomerCustomsAgentByProductList()
                                             {
                                                 ProductTypeCode = entity.ProductTypeCode,
                                                 Tenant = entity.Tenant,
                                                 CustomsAgentId = entity.CustomsAgentId,
                                                 CustomerId = entity.CustomerId,
                                                 
                                             };
            return result;
        }
    }
}
