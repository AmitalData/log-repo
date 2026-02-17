using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.CRMModel.DomainServices;
using WebFreight.Web.Security;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OpportunityWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OpportunityWcfService.svc or OpportunityWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OpportunityWcfService : IOpportunityWcfService
    {

        public List<Logitude.CRM.Data.EntityLists.OpportunityList> GetOpportunityList(string email, string searchText, int tenant, int skip, int take, DataContracts.OpportunityApiFilters filters, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);//UPDATE//READ

                List<OpportunityList> result = new List<OpportunityList>();
                ICRMContext crmcontext = CRMContext.GetContext(tenant);
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commoncontext);
                CardContactRepository cardContactRepository = new CardContactRepository(commoncontext);
                OpportunityRepository opportunityRepository = new OpportunityRepository(crmcontext);

                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);

                QueryOperations queryOperations = new QueryOperations() { PageIndex = skip, PageSize = take, };
                queryOperations.SetFilter("SearchFields", searchText, false, "Contains", null, true);
                if (!string.IsNullOrEmpty(filters.CustomerId))
                {
                    queryOperations.SetFilter("CustomerId", filters.CustomerId, false, "Equals", null, true);
                } 
                queryOperations.SetFilter("IsClosed", !filters.IsOpen, false, "Equals", null, true);
                //queryOperations.SetFilter("IsClosed", !filters.IsOpen, false, "Equals", null, true);


                if (filters.MyOpportunities)
                {
                    queryOperations.SetFilter("OwnerId", (contact != null ? contact.Id : null), false, "Equals", null, false);
                    //CreatedByUserId
                }
                MemoryStream memorystream = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                serializer.Serialize(memorystream, queryOperations);

                CRMDomainService service = new CRMDomainService();
                List<OpportunityList> opportunities = service.GetOpportunityFiltersHybrid(memorystream.ToArray(), tenant);

                result = opportunities;


                return result;
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        public CustomerList GetCustomerListByOpportunityId(string opportunityId, int tenant, ref Response response)
        {
            try
            {
                CustomerList list = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }


                OpportunityRepository opp_Repository = new OpportunityRepository(tenant);
                Opportunity opportunity = opp_Repository.GetSingle(opportunityId, tenant);
                if (opportunity != null)
                {
                    CustomerQuery query = new CustomerQuery(tenant);
                    list = query.GetSingleCustomerList(opportunity.CustomerId, tenant);

                }

                return list;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        public OpportunityList GetOpportunityListById(string id, int tenant, ref Response response)
        {
            try
            {
                OpportunityList list = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }


                ICRMContext crmContext = CRMContext.GetContext(tenant);
                OpportunityListQueryService query = new OpportunityListQueryService(crmContext);
                list = query.GetSingle(id);
                
                return list;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }
    }
}
