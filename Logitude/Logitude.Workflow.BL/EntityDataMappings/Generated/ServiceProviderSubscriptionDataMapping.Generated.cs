
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class ServiceProviderSubscriptionDataMapping: IMapping<ServiceProviderSubscriptionPM, ServiceProviderSubscription>,IMappingEncodeBase64NVARCHARFields<ServiceProviderSubscriptionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         UserEmail, 
	         AccessToken, 
	         RefreshToken, 
	         EmailProvider, 
	         WorkflowNumber, 
	         AdditionalSettings, 
	         SubscriptionExpirationDateTime, 
	         AccessTokenExpirationDateTime, 
	         WebhookParams,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         UserEmail, 
	         AccessToken, 
	         RefreshToken, 
	         EmailProvider, 
	         WorkflowNumber, 
	         AdditionalSettings, 
	         SubscriptionExpirationDateTime, 
	         AccessTokenExpirationDateTime, 
	         WebhookParams,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscription entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserEmail))
            {
				entityPOCO.UserEmail = entityPM.UserEmail;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccessToken))
            {
				entityPOCO.AccessToken = entityPM.AccessToken;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RefreshToken))
            {
				entityPOCO.RefreshToken = entityPM.RefreshToken;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmailProvider))
            {
				entityPOCO.EmailProvider = entityPM.EmailProvider;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowNumber))
            {
				entityPOCO.WorkflowNumber = entityPM.WorkflowNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalSettings))
            {
				entityPOCO.AdditionalSettings = entityPM.AdditionalSettings;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SubscriptionExpirationDateTime))
            {
				entityPOCO.SubscriptionExpirationDateTime = entityPM.SubscriptionExpirationDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccessTokenExpirationDateTime))
            {
				entityPOCO.AccessTokenExpirationDateTime = entityPM.AccessTokenExpirationDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WebhookParams))
            {
				entityPOCO.WebhookParams = entityPM.WebhookParams;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscription entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserEmail))
            {
					entityPM.UserEmail = entityPOCO.UserEmail;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccessToken))
            {
					entityPM.AccessToken = entityPOCO.AccessToken;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RefreshToken))
            {
					entityPM.RefreshToken = entityPOCO.RefreshToken;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmailProvider))
            {
					entityPM.EmailProvider = entityPOCO.EmailProvider;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkflowNumber))
            {
					entityPM.WorkflowNumber = entityPOCO.WorkflowNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionalSettings))
            {
					entityPM.AdditionalSettings = entityPOCO.AdditionalSettings;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SubscriptionExpirationDateTime))
            {
					entityPM.SubscriptionExpirationDateTime = entityPOCO.SubscriptionExpirationDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccessTokenExpirationDateTime))
            {
					entityPM.AccessTokenExpirationDateTime = entityPOCO.AccessTokenExpirationDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WebhookParams))
            {
					entityPM.WebhookParams = entityPOCO.WebhookParams;
            }

		}

		public void PMToOldPM(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscriptionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserEmail))
            {
                oldEntityPM.UserEmail = entityPM.UserEmail;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccessToken))
            {
                oldEntityPM.AccessToken = entityPM.AccessToken;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RefreshToken))
            {
                oldEntityPM.RefreshToken = entityPM.RefreshToken;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmailProvider))
            {
                oldEntityPM.EmailProvider = entityPM.EmailProvider;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowNumber))
            {
                oldEntityPM.WorkflowNumber = entityPM.WorkflowNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AdditionalSettings))
            {
                oldEntityPM.AdditionalSettings = entityPM.AdditionalSettings;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SubscriptionExpirationDateTime))
            {
                oldEntityPM.SubscriptionExpirationDateTime = entityPM.SubscriptionExpirationDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccessTokenExpirationDateTime))
            {
                oldEntityPM.AccessTokenExpirationDateTime = entityPM.AccessTokenExpirationDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WebhookParams))
            {
                oldEntityPM.WebhookParams = entityPM.WebhookParams;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ServiceProviderSubscriptionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.UserEmail)) //T4 find type == nText 
            {
                entityPM.UserEmail = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.UserEmail));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AccessToken)) //T4 find type == nText 
            {
                entityPM.AccessToken = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AccessToken));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RefreshToken)) //T4 find type == nText 
            {
                entityPM.RefreshToken = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RefreshToken));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WorkflowNumber)) //T4 find type == nText 
            {
                entityPM.WorkflowNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WorkflowNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AdditionalSettings)) //T4 find type == nText 
            {
                entityPM.AdditionalSettings = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AdditionalSettings));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WebhookParams)) //T4 find type == nText 
            {
                entityPM.WebhookParams = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WebhookParams));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscription entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 