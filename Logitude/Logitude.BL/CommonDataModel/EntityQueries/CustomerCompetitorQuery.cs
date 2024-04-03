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
using Logitude.BL.CommonDataModel.EntityDws;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerCompetitorQuery
    {                
        CustomerCompetitorRepository repository;

        public CustomerCompetitorQuery()
        {
            repository = new CustomerCompetitorRepository();
        }

        public CustomerCompetitorQuery(int tenant)
        {
            repository = new CustomerCompetitorRepository(tenant);
        }

        public CustomerCompetitorQuery(CustomerCompetitorRepository repository)
        {
            this.repository = repository;
        }

        public List<CustomerCompetitorPM> GetCustomerCompetitorsByCustomerId(string customerId, int tenant)
        {
            List<CustomerCompetitorPM> result =

                (from a in repository.context.CustomerCompetitors.Include("Competitor").Include("Customer")
                 where a.Tenant == tenant && a.CustomerId == customerId
                 select new CustomerCompetitorPM()
                 {
                     CustomerId = a.CustomerId,
                      CompetitorId = a.CompetitorId,
                     Tenant = a.Tenant,   
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
                 }).ToList();


            CustomerCompetitorProductQuery productsQuery = new CustomerCompetitorProductQuery(tenant);

            foreach (CustomerCompetitorPM item in result)
            {
                item.CustomerCompetitorProducts = productsQuery.GetCustomerCompetitorProductsByCompetitorId(item.CustomerId, item.CompetitorId, tenant).ToList();
            }

            return result;
        }



        public List<CustomerCompetitorPM> GetCustomerCompetitorsByListsCustomerIds(List< string> customerIds, int tenant)
        {
            List<CustomerCompetitorPM> result =

                (from a in repository.context.CustomerCompetitors.Include("Competitor").Include("Customer")
                 where a.Tenant == tenant && customerIds.Contains(a.CustomerId)
                 select new CustomerCompetitorPM()
                 {
                     CustomerId = a.CustomerId,
                     CompetitorId = a.CompetitorId,
                     Tenant = a.Tenant,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
                 }).ToList();


            CustomerCompetitorProductQuery productsQuery = new CustomerCompetitorProductQuery(tenant);

            foreach (CustomerCompetitorPM item in result)
            {
                item.CustomerCompetitorProducts = productsQuery.GetCustomerCompetitorProductsByCompetitorId(item.CustomerId, item.CompetitorId, tenant).ToList();
            }

            return result;
        }



        public IQueryable<CustomerCompetitorList> GetIQueryableEntityList(IQueryable<CustomerCompetitor> iQueryable)
        {
            IQueryable<CustomerCompetitorList> result =

                from a in iQueryable.Include("Competitor").Include("Customer")
                select new CustomerCompetitorList()
                {
                    CustomerId = a.CustomerId,
                    CompetitorId = a.CompetitorId, 
                    Tenant = a.Tenant,
                    CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                    CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
                    
                };

            return result;
        }


        public List<CustomerCompetitorDW> GetCustomerCompetitorsDW(int tenant)
        {
            List<CustomerCompetitorDW> result = (from a in repository.context.CustomerCompetitors.Include("Competitor").Include("Customer")
                                                 where a.Tenant == tenant
                                                 select new CustomerCompetitorDW()
                                                 {
                                                     CustomerId = a.CustomerId,
                                                     CompetitorId = a.CompetitorId,
                                                     Tenant = a.Tenant,
                                                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                                                     CompetitorName = a.Competitor != null ? a.Competitor.Name : null,

                                                 }).ToList();

            return result;
        }


    }
}