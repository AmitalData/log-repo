
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
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs; 
using Logitude.Social.Data;

namespace Logitude.Social.BL.EntityDataMappings
{
   
   public partial class ConversationHeaderDataMapping: IMapping<ConversationHeaderPM, ConversationHeader>,IMappingEncodeBase64NVARCHARFields<ConversationHeaderPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByUserId, 
	         CreateDate, 
	         ObjectTableId, 
	         EntityId, 
	         IsWaitingForResponse, 
	         EntityDescription,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByUserId, 
	         CreateDate, 
	         ObjectTableId, 
	         EntityId, 
	         IsWaitingForResponse, 
	         EntityDescription, 
	         LasMessageUserId, 
	         LasMessageUserName, 
	         LasMessageBody, 
	         MessageParticipants, 
	         LastMessageDate, 
	         MessageParticipantsCount, 
	         IsRead, 
	         IsLeft, 
	         LeaveDate, 
	         IsReplied, 
	         UserImageDetailId, 
	         FirstImageDetailId, 
	         FirstMessageUserId, 
	         NumberUnreadComment, 
	         FirstMessageBody,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConversationHeaderPM entityPM, ConversationHeader entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsWaitingForResponse))
            {
				entityPOCO.IsWaitingForResponse = entityPM.IsWaitingForResponse;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityDescription))
            {
				entityPOCO.EntityDescription = entityPM.EntityDescription;
			}
			}

		public void POCOToPM(ConversationHeaderPM entityPM, ConversationHeader entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsWaitingForResponse))
            {
					entityPM.IsWaitingForResponse = entityPOCO.IsWaitingForResponse;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityDescription))
            {
					entityPM.EntityDescription = entityPOCO.EntityDescription;
            }

		}

		public void PMToOldPM(ConversationHeaderPM entityPM, ConversationHeaderPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsWaitingForResponse))
            {
                oldEntityPM.IsWaitingForResponse = entityPM.IsWaitingForResponse;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityDescription))
            {
                oldEntityPM.EntityDescription = entityPM.EntityDescription;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConversationHeaderPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.EntityDescription)) //T4 find type == nText 
            {
                entityPM.EntityDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.EntityDescription));
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
			  
   }
}
	 