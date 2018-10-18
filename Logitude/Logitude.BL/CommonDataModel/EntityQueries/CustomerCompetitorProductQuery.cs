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
    public class CustomerCompetitorProductQuery
    {             
        CustomerCompetitorProductRepository repository;

        public CustomerCompetitorProductQuery()
        {
            repository = new CustomerCompetitorProductRepository();
        }

        public CustomerCompetitorProductQuery(int tenant)
        {
            repository = new CustomerCompetitorProductRepository(tenant);
        }

        public CustomerCompetitorProductQuery(CustomerCompetitorProductRepository repository)
        {
            this.repository = repository;
        }

        public List<CustomerCompetitorProductPM> GetCustomerCompetitorProductsByCompetitorId(string customerId,string competitorId, int tenant)
        {
            List<CustomerCompetitorProductPM> result =

                (from a in repository.context.CustomerCompetitorProducts.Include("ProductType").Include("Customer").Include("Competitor")
                 where a.Tenant == tenant && a.CompetitorId == competitorId && a.CustomerId == customerId
                 select new CustomerCompetitorProductPM()
                 {
                     CustomerId = a.CustomerId,
                     CompetitorId = a.CompetitorId,
                     ProductTypeCode = a.ProductTypeCode,
                     Tenant = a.Tenant,
                     CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                 }).ToList();

            return result;
        }

        public IQueryable<CustomerCompetitorProductList> GetIQueryableEntityList(IQueryable<CustomerCompetitorProduct> iQueryable)
        {
            IQueryable<CustomerCompetitorProductList> result =

                from a in iQueryable.Include("ProductType").Include("Customer").Include("Competitor")
                select new CustomerCompetitorProductList()
                {
                    CustomerId = a.CustomerId,
                    ProductTypeCode = a.ProductTypeCode,
                    Tenant = a.Tenant,
                    CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
                    CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                    ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                };

            return result;
        }
    }
}