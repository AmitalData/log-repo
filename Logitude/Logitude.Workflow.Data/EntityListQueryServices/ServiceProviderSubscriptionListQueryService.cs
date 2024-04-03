	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityLists;

namespace Logitude.Workflow.Data.EntityListQueryServices
{ 

    public partial class ServiceProviderSubscriptionListQueryService
    {
	    private IQueryable<ServiceProviderSubscriptionList> GetIqueryableList(IQueryable<ServiceProviderSubscription> iQueryable)
        {
		IQueryable<ServiceProviderSubscriptionList> query = (from a in iQueryable
                                            select new ServiceProviderSubscriptionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          UserEmail = a.UserEmail,
					
					                          AccessToken = a.AccessToken,
					
					                          RefreshToken = a.RefreshToken,
					
					                          EmailProvider = a.EmailProvider,
					
					                          WorkflowNumber = a.WorkflowNumber,
					
					                          AdditionalSettings = a.AdditionalSettings,
					
					                          SubscriptionExpirationDateTime = a.SubscriptionExpirationDateTime,
					
					                          AccessTokenExpirationDateTime = a.AccessTokenExpirationDateTime,
					
					                          WebhookParams = a.WebhookParams,
					
		                    	            });
            return query;
		}

		private IQueryable<ServiceProviderSubscription> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ServiceProviderSubscription> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<ServiceProviderSubscription> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ServiceProviderSubscription> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	