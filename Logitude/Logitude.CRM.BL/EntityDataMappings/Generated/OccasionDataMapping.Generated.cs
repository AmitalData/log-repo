
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
   
   public partial class OccasionDataMapping: IMapping<OccasionPM, Occasion>,IMappingEncodeBase64NVARCHARFields<OccasionPM>
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
	         Name, 
	         StartDateTime, 
	         EndDateTime, 
	         Goal, 
	         Location, 
	         OwnerId, 
	         IndustryId, 
	         OccasionTypeId, 
	         OccasionStatusId, 
	         ParticipatedCustomers, 
	         ParticipatedContacts, 
	         InvitedCustomers, 
	         InvitedContacts,
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
	         Name, 
	         StartDateTime, 
	         EndDateTime, 
	         Goal, 
	         Location, 
	         OwnerId, 
	         IndustryId, 
	         OccasionTypeId, 
	         OccasionStatusId, 
	         CreatedByContactName, 
	         UpdatedByUserName, 
	         TypeName, 
	         OwnerName, 
	         OccasionStatusName, 
	         IndustryName, 
	         ParticipatedCustomers, 
	         ParticipatedContacts, 
	         InvitedCustomers, 
	         InvitedContacts, 
	         IsAllAdded,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OccasionPM entityPM, Occasion entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDateTime))
            {
				entityPOCO.StartDateTime = entityPM.StartDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDateTime))
            {
				entityPOCO.EndDateTime = entityPM.EndDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Goal))
            {
				entityPOCO.Goal = entityPM.Goal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
				entityPOCO.Location = entityPM.Location;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndustryId))
            {
				entityPOCO.IndustryId = entityPM.IndustryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionTypeId))
            {
				entityPOCO.OccasionTypeId = entityPM.OccasionTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionStatusId))
            {
				entityPOCO.OccasionStatusId = entityPM.OccasionStatusId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipatedCustomers))
            {
				entityPOCO.ParticipatedCustomers = entityPM.ParticipatedCustomers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipatedContacts))
            {
				entityPOCO.ParticipatedContacts = entityPM.ParticipatedContacts;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvitedCustomers))
            {
				entityPOCO.InvitedCustomers = entityPM.InvitedCustomers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvitedContacts))
            {
				entityPOCO.InvitedContacts = entityPM.InvitedContacts;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(OccasionPM entityPM, Occasion entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDateTime))
            {
					entityPM.StartDateTime = entityPOCO.StartDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDateTime))
            {
					entityPM.EndDateTime = entityPOCO.EndDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Goal))
            {
					entityPM.Goal = entityPOCO.Goal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Location))
            {
					entityPM.Location = entityPOCO.Location;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IndustryId))
            {
					entityPM.IndustryId = entityPOCO.IndustryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OccasionTypeId))
            {
					entityPM.OccasionTypeId = entityPOCO.OccasionTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OccasionStatusId))
            {
					entityPM.OccasionStatusId = entityPOCO.OccasionStatusId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParticipatedCustomers))
            {
					entityPM.ParticipatedCustomers = entityPOCO.ParticipatedCustomers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParticipatedContacts))
            {
					entityPM.ParticipatedContacts = entityPOCO.ParticipatedContacts;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvitedCustomers))
            {
					entityPM.InvitedCustomers = entityPOCO.InvitedCustomers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvitedContacts))
            {
					entityPM.InvitedContacts = entityPOCO.InvitedContacts;
            }

		}

		public void PMToOldPM(OccasionPM entityPM, OccasionPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDateTime))
            {
                oldEntityPM.StartDateTime = entityPM.StartDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDateTime))
            {
                oldEntityPM.EndDateTime = entityPM.EndDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Goal))
            {
                oldEntityPM.Goal = entityPM.Goal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Location))
            {
                oldEntityPM.Location = entityPM.Location;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndustryId))
            {
                oldEntityPM.IndustryId = entityPM.IndustryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionTypeId))
            {
                oldEntityPM.OccasionTypeId = entityPM.OccasionTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OccasionStatusId))
            {
                oldEntityPM.OccasionStatusId = entityPM.OccasionStatusId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipatedCustomers))
            {
                oldEntityPM.ParticipatedCustomers = entityPM.ParticipatedCustomers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipatedContacts))
            {
                oldEntityPM.ParticipatedContacts = entityPM.ParticipatedContacts;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvitedCustomers))
            {
                oldEntityPM.InvitedCustomers = entityPM.InvitedCustomers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvitedContacts))
            {
                oldEntityPM.InvitedContacts = entityPM.InvitedContacts;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OccasionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Goal)) //T4 find type == nText 
            {
                entityPM.Goal = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Goal));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Location)) //T4 find type == nText 
            {
                entityPM.Location = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Location));
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
		
		private void BuildSearchFieldsGenerated(OccasionPM entityPM, Occasion entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 