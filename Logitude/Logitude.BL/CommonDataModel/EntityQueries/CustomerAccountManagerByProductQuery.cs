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
    public class CustomerAccountManagerByProductQuery
    {
         CustomerAccountManagerByProductRepository repository;

        public CustomerAccountManagerByProductQuery()
        {
            repository = new CustomerAccountManagerByProductRepository(); 
            
        }

        public CustomerAccountManagerByProductQuery(int tenant)
        {
            repository = new CustomerAccountManagerByProductRepository(tenant);
        }

        public CustomerAccountManagerByProductQuery(CustomerAccountManagerByProductRepository repository)
        {
            this.repository = repository;
        }

        public CustomerAccountManagerByProductPM GetSinglePM(string producttypecode, string customerId, int tenant)
        {
            CustomerAccountManagerByProductPM result = null;
            CustomerAccountManagerByProduct entityPoco = repository.GetSingleCustomerAccountManagerByProduct(producttypecode, customerId, tenant);

            if (entityPoco != null)
            {
                result = new CustomerAccountManagerByProductPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    Tenant = entityPoco.Tenant,
                    AccountManagerId = entityPoco.AccountManagerId,
                    CustomerId = entityPoco.CustomerId,
                    AccountManagerName = entityPoco.AccountManagerUser != null? entityPoco.AccountManagerUser.Contact.EnglishName : null,
          
                };
            }

            return result;
        }

        public List<CustomerAccountManagerByProductPM> GetCustomerAccountManagerByProductPMs(int tenant,string customerId)
        {
            List<CustomerAccountManagerByProduct> pocos = repository.GetCustomerAccountManagerByProductforCustomer(tenant, customerId).ToList();

            List<CustomerAccountManagerByProductPM> entityPMs = (from a in pocos
                                                           select new CustomerAccountManagerByProductPM()
                                                           {
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               CustomerId = a.CustomerId,
                                                               Tenant = a.Tenant,
                                                               AccountManagerId = a.AccountManagerId,
                                                             AccountManagerName = a.AccountManagerUser != null? a.AccountManagerUser.Contact.EnglishName : null,
                                                           }).ToList();
            return entityPMs;
        }

        public IQueryable<CustomerAccountManagerByProductList> GetIQueryableEntityList(IQueryable<CustomerAccountManagerByProduct> iQueryable,int tenant)
        {
            IQueryable<CustomerAccountManagerByProductList> result = from entity in iQueryable
                                             select new CustomerAccountManagerByProductList()
                                             {
                                                 ProductTypeCode = entity.ProductTypeCode,
                                                 Tenant = entity.Tenant,
                                                 AccountManagerId = entity.AccountManagerId,
                                                 CustomerId = entity.CustomerId,
                                                
                                                 
                                             };
            return result;
        }
    }
}
