
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
   
   public partial class ConversationHeaderMessageDataMapping: IMapping<ConversationHeaderMessagePM, ConversationHeaderMessage>,IMappingEncodeBase64NVARCHARFields<ConversationHeaderMessagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ConversationHeaderId, 
	         CreatedByUserId, 
	         CreateDate, 
	         MessageBody,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ConversationHeaderId, 
	         CreatedByUserId, 
	         CreateDate, 
	         MessageBody, 
	         UserName, 
	         UserImageDetailId, 
	         ImageDetailId, 
	         DefaultColor, 
	         RegardingEntity,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConversationHeaderMessagePM entityPM, ConversationHeaderMessage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConversationHeaderId))
            {
				entityPOCO.ConversationHeaderId = entityPM.ConversationHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageBody))
            {
				entityPOCO.MessageBody = entityPM.MessageBody;
			}
			}

		public void POCOToPM(ConversationHeaderMessagePM entityPM, ConversationHeaderMessage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConversationHeaderId))
            {
					entityPM.ConversationHeaderId = entityPOCO.ConversationHeaderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MessageBody))
            {
					entityPM.MessageBody = entityPOCO.MessageBody;
            }

		}

		public void PMToOldPM(ConversationHeaderMessagePM entityPM, ConversationHeaderMessagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConversationHeaderId))
            {
                oldEntityPM.ConversationHeaderId = entityPM.ConversationHeaderId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageBody))
            {
                oldEntityPM.MessageBody = entityPM.MessageBody;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConversationHeaderMessagePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.MessageBody)) //T4 find type == nText 
            {
                entityPM.MessageBody = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.MessageBody));
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
	 