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
    public class CustomerSalesmanByProductQuery
    {
          CustomerSalesmanByProductRepository repository;



        public CustomerSalesmanByProductQuery(int tenant)
        {
            repository = new CustomerSalesmanByProductRepository(tenant);
        }

        public CustomerSalesmanByProductQuery(CustomerSalesmanByProductRepository repository)
        {
            this.repository = repository;
        }

       

        public CustomerSalesmanByProductPM GetSinglePM(string producttypecode, string customerId, int tenant)
        {
            CustomerSalesmanByProductPM result = null;
            CustomerSalesmanByProduct entityPoco = repository.GetSingleCustomerSalesmanByProduct(producttypecode, customerId, tenant);

            if (entityPoco != null)
            {
                result = new CustomerSalesmanByProductPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    Tenant = entityPoco.Tenant,
                    SalesmanUserId = entityPoco.SalesmanUserId,
                    CustomerId = entityPoco.CustomerId,
                    SalesmanUserName = entityPoco.SalesmanUser != null ? entityPoco.SalesmanUser.Contact.EnglishName : null,
          
                };
            }

            return result;
        }

        public List<CustomerSalesmanByProductPM> GetCustomerSalesmanByProductPMs(int tenant,string customerId)
        {
            List<CustomerSalesmanByProduct> pocos = repository.GetCustomerSalesmanByProductforCustomer(tenant, customerId).ToList();

            List<CustomerSalesmanByProductPM> entityPMs = (from a in pocos
                                                           select new CustomerSalesmanByProductPM()
                                                           {
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               CustomerId = a.CustomerId,
                                                               Tenant = a.Tenant,
                                                               SalesmanUserId = a.SalesmanUserId,
                                                             SalesmanUserName = a.SalesmanUser != null ? a.SalesmanUser.Contact.EnglishName : null,
                                                           }).ToList();
            return entityPMs;
        }

        public IQueryable<CustomerSalesmanByProductList> GetIQueryableEntityList(IQueryable<CustomerSalesmanByProduct> iQueryable,int tenant)
        {
            IQueryable<CustomerSalesmanByProductList> result = from entity in iQueryable
                                             select new CustomerSalesmanByProductList()
                                             {
                                                 ProductTypeCode = entity.ProductTypeCode,
                                                 Tenant = entity.Tenant,
                                                 SalesmanUserId = entity.SalesmanUserId,
                                                 CustomerId = entity.CustomerId,
                                                 
                                             };
            return result;
        }
    }
}
