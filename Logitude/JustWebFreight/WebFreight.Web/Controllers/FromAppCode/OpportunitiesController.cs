//using Logitude.CRM.Data;
//using Logitude.CRM.Data.EntityListQueryServices;
//using Logitude.CRM.Data.EntityLists;
//using Simplog.Server.Infrastructure.DataContracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;
//using Logitude.CRM.BL.EntityPMs;
//using Logitude.CRM.BL.EntityQueryServices;

//namespace WebFreight.Web
//{
//    public class OpportunitiesController : System.Web.Http.ApiController
//    {
//        public List<OpportunityList> PostFilteredOpportunities(int tenant, OpportunityFilters filters)
//        {
//            //SecurityUtility.AuthenticationOnTenant(tenant);
                       
//            QueryOperations queryOperations = new QueryOperations();
//            queryOperations.SetFilter("CustomerId", filters.CustomerId, false, "Equals", null, false);
//            queryOperations.SetFilter("IsClosed", filters.IsClosed, false, "Equals", null, false);
//            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, true);

//            if (filters.IsMyOpportunities)
//            {
//                queryOperations.SetFilter("MyOpenOpportunities", true, true, "Contains", null, true);   
//            }

//            ICRMContext crmContext = CRMContext.GetContext(tenant);            
//            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);          
//            return listService.GetList(queryOperations, tenant);
//        }

//        public List<OpportunityList> PostOpportunities(int opportunitiesTenant, OpportunityFilters filters)
//        {
//            QueryOperations queryOperations = new QueryOperations();
//            queryOperations.PageIndex = 0;
//            queryOperations.PageSize = 25;
//            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, true);

//            if (filters.IsMyOpportunities)
//            {
//                queryOperations.SetFilter("MyOpenOpportunities", true, true, "Contains", null, true);
//            }

//            ICRMContext crmContext = CRMContext.GetContext(opportunitiesTenant);
//            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
//            return listService.GetList(queryOperations, opportunitiesTenant);
//        }

//        public OpportunityPM GetSingleOpportunityPM(string singleOpportunityId, int tenant)
//        {
//            //SecurityUtility.AuthenticationOnTenant(tenant);
//            OpportunityQueryService entityQuery = new OpportunityQueryService(tenant);
//            OpportunityPM entityPM = entityQuery.GetSingle(singleOpportunityId, false, false);
            
//            return entityPM;
//        }
//    }
//}