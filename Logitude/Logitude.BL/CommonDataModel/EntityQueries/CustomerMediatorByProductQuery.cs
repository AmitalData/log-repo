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
    public class CustomerMediatorByProductQuery
    {
        CustomerMediatorByProductRepository repository;



        public CustomerMediatorByProductQuery(int tenant)
        {
            repository = new CustomerMediatorByProductRepository(tenant);
        }

        public CustomerMediatorByProductQuery(CustomerMediatorByProductRepository repository)
        {
            this.repository = repository;
        }

        public CustomerMediatorByProductPM GetSinglePM(string producttypecode, string customerId, int tenant)
        {
            CustomerMediatorByProductPM result = null;
            CustomerMediatorByProduct entityPoco = repository.GetSingleCustomerMediatorByProduct(producttypecode, customerId, tenant);

            if (entityPoco != null)
            {
                result = new CustomerMediatorByProductPM()
                {
                    ProductTypeCode = entityPoco.ProductTypeCode,
                    Tenant = entityPoco.Tenant,
                    MediatorId = entityPoco.MediatorId,
                    CustomerId = entityPoco.CustomerId,
                    MediatorName = entityPoco.MediatorCard != null ? entityPoco.MediatorCard.EnglishName : null,
          
                };
            }

            return result;
        }

        public List<CustomerMediatorByProductPM> GetCustomerMediatorByProductPMs(int tenant,string customerId)
        {
            List<CustomerMediatorByProduct> pocos = repository.GetCustomerMediatorByProductforCustomer(tenant, customerId).ToList();

            List<CustomerMediatorByProductPM> entityPMs = (from a in pocos
                                                           select new CustomerMediatorByProductPM()
                                                           {
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               CustomerId = a.CustomerId,
                                                               Tenant = a.Tenant,
                                                               MediatorId = a.MediatorId,
                                                               MediatorName = a.MediatorCard != null ? a.MediatorCard.EnglishName : null,
                                                           }).ToList();
            return entityPMs;

        }

        public IQueryable<CustomerMediatorByProductList> GetIQueryableEntityList(IQueryable<CustomerMediatorByProduct> iQueryable,int tenant)
        {
            IQueryable<CustomerMediatorByProductList> result = from entity in iQueryable
                                             select new CustomerMediatorByProductList()
                                             {
                                                 ProductTypeCode = entity.ProductTypeCode,
                                                 Tenant = entity.Tenant,
                                                 MediatorId = entity.MediatorId,
                                                 CustomerId = entity.CustomerId,
                                                 
                                             };
            return result;
        }


    }
}
