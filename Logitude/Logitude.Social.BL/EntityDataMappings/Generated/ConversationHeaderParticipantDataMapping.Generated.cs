
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
   
   public partial class ConversationHeaderParticipantDataMapping: IMapping<ConversationHeaderParticipantPM, ConversationHeaderParticipant>,IMappingEncodeBase64NVARCHARFields<ConversationHeaderParticipantPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ConversationHeaderId, 
	         ParticipantUserId, 
	         CreateDate, 
	         LeaveDate, 
	         IsLeft, 
	         Replied, 
	         IsRead, 
	         LastReadDate, 
	         IsDelete, 
	         DeleteDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ConversationHeaderId, 
	         ParticipantUserId, 
	         CreateDate, 
	         LeaveDate, 
	         IsLeft, 
	         Replied, 
	         IsRead, 
	         LastReadDate, 
	         IsDelete, 
	         DeleteDate, 
	         ParticipantName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConversationHeaderParticipantPM entityPM, ConversationHeaderParticipant entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConversationHeaderId))
            {
				entityPOCO.ConversationHeaderId = entityPM.ConversationHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipantUserId))
            {
				entityPOCO.ParticipantUserId = entityPM.ParticipantUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeaveDate))
            {
				entityPOCO.LeaveDate = entityPM.LeaveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLeft))
            {
				entityPOCO.IsLeft = entityPM.IsLeft;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Replied))
            {
				entityPOCO.Replied = entityPM.Replied;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRead))
            {
				entityPOCO.IsRead = entityPM.IsRead;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastReadDate))
            {
				entityPOCO.LastReadDate = entityPM.LastReadDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDelete))
            {
				entityPOCO.IsDelete = entityPM.IsDelete;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeleteDate))
            {
				entityPOCO.DeleteDate = entityPM.DeleteDate;
			}
			}

		public void POCOToPM(ConversationHeaderParticipantPM entityPM, ConversationHeaderParticipant entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParticipantUserId))
            {
					entityPM.ParticipantUserId = entityPOCO.ParticipantUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeaveDate))
            {
					entityPM.LeaveDate = entityPOCO.LeaveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsLeft))
            {
					entityPM.IsLeft = entityPOCO.IsLeft;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Replied))
            {
					entityPM.Replied = entityPOCO.Replied;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRead))
            {
					entityPM.IsRead = entityPOCO.IsRead;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastReadDate))
            {
					entityPM.LastReadDate = entityPOCO.LastReadDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDelete))
            {
					entityPM.IsDelete = entityPOCO.IsDelete;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeleteDate))
            {
					entityPM.DeleteDate = entityPOCO.DeleteDate;
            }

		}

		public void PMToOldPM(ConversationHeaderParticipantPM entityPM, ConversationHeaderParticipantPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParticipantUserId))
            {
                oldEntityPM.ParticipantUserId = entityPM.ParticipantUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeaveDate))
            {
                oldEntityPM.LeaveDate = entityPM.LeaveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsLeft))
            {
                oldEntityPM.IsLeft = entityPM.IsLeft;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Replied))
            {
                oldEntityPM.Replied = entityPM.Replied;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRead))
            {
                oldEntityPM.IsRead = entityPM.IsRead;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastReadDate))
            {
                oldEntityPM.LastReadDate = entityPM.LastReadDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDelete))
            {
                oldEntityPM.IsDelete = entityPM.IsDelete;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeleteDate))
            {
                oldEntityPM.DeleteDate = entityPM.DeleteDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConversationHeaderParticipantPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
	 