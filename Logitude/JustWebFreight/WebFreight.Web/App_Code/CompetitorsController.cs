//using Logitude.CRM.Data;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.CommonDataModel.Repositories;
//using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Server.Infrastructure.DataContracts;
//using Simplog.Server.Infrastructure.Helpers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Reflection;
//using System.Web.Http;
//using System.Xml.Serialization;
//using Logitude.BL.CommonDataModel.EntityLists;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.App_Code
//{
//    public class CompetitorsController : ApiController
//    {
//        public List<CustomerCompetitorPM> GetCustomerCompetitorsByCustomerId(string customerId, int tenant)
//        {
//            CustomerCompetitorRepository repository;
//            repository = new CustomerCompetitorRepository(tenant);

//            List<CustomerCompetitorPM> result =

//                (from a in repository.context.CustomerCompetitors.Include("Competitor").Include("Customer")
//                 where a.Tenant == tenant && a.CustomerId == customerId
//                 select new CustomerCompetitorPM()
//                 {
//                     CustomerId = a.CustomerId,
//                     CompetitorId = a.CompetitorId,
//                     Tenant = a.Tenant,
//                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
//                     CompetitorName = a.Competitor != null ? a.Competitor.Name : null,
//                     CompetitorWebsite = a.Competitor != null ? a.Competitor.Website : null,
//                     CompetitorStrengths = a.Competitor != null ? a.Competitor.Strengths : null,
//                     CompetitorWeaknesses = a.Competitor != null ? a.Competitor.Weaknesses : null,
//                     CompetitorOpportunity = a.Competitor != null ? a.Competitor.Opportunity : null,
//                     CompetitorThreat = a.Competitor != null ? a.Competitor.Threat : null,
//                 }).ToList();
           

//            CustomerCompetitorProductQuery productsQuery = new CustomerCompetitorProductQuery(tenant);

//            foreach (CustomerCompetitorPM item in result)
//            {
//                item.CustomerCompetitorProducts = productsQuery.GetCustomerCompetitorProductsByCompetitorId(item.CustomerId, item.CompetitorId, tenant).ToList();
//            }

//            return result;
//        }
//    }

//}