
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OccasionInviteeDataMapping: IMapping<OccasionInviteePM, OccasionInvitee>,IMappingEncodeBase64NVARCHARFields<OccasionInviteePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AddedDate, 
	         AddedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         Notes, 
	         OccasionId, 
	         ContactId, 
	         Invited, 
	         Participated,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AddedDate, 
	         AddedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         Notes, 
	         OccasionId, 
	         ContactId, 
	         AddedByUserName, 
	         UpdatedByUserName, 
	         OccasionName, 
	         ContactName, 
	         Invited, 
	         Participated, 
	         ContactPhone, 
	         ContactEmail, 
	         ContactTel, 
	         ContactPosition, 
	         ContactMobile,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedDate))
            {
				entityPOCO.AddedDate = entityPM.AddedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
				entityPOCO.AddedByUserId = entityPM.AddedByUserId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionId))
            {
				entityPOCO.OccasionId = entityPM.OccasionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
				entityPOCO.ContactId = entityPM.ContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Invited))
            {
				entityPOCO.Invited = entityPM.Invited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Participated))
            {
				entityPOCO.Participated = entityPM.Participated;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedDate))
            {
					entityPM.AddedDate = entityPOCO.AddedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedByUserId))
            {
					entityPM.AddedByUserId = entityPOCO.AddedByUserId;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OccasionId))
            {
					entityPM.OccasionId = entityPOCO.OccasionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactId))
            {
					entityPM.ContactId = entityPOCO.ContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Invited))
            {
					entityPM.Invited = entityPOCO.Invited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Participated))
            {
					entityPM.Participated = entityPOCO.Participated;
            }

		}

		public void PMToOldPM(OccasionInviteePM entityPM, OccasionInviteePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedDate))
            {
                oldEntityPM.AddedDate = entityPM.AddedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
                oldEntityPM.AddedByUserId = entityPM.AddedByUserId;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionId))
            {
                oldEntityPM.OccasionId = entityPM.OccasionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
                oldEntityPM.ContactId = entityPM.ContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Invited))
            {
                oldEntityPM.Invited = entityPM.Invited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Participated))
            {
                oldEntityPM.Participated = entityPM.Participated;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OccasionInviteePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
		
		private void BuildSearchFieldsGenerated(OccasionInviteePM entityPM, OccasionInvitee entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 