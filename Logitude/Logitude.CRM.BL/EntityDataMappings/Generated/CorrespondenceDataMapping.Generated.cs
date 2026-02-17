
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
   
   public partial class CorrespondenceDataMapping: IMapping<CorrespondencePM, Correspondence>,IMappingEncodeBase64NVARCHARFields<CorrespondencePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByContactId, 
	         CreateDate, 
	         Description, 
	         IsInternal, 
	         ObjectTableId, 
	         EntityId, 
	         ActivityTypeCode, 
	         ActivityId, 
	         ActivitySubject, 
	         CCs, 
	         Bcc, 
	         NotifyMe, 
	         NotifyOwner, 
	         InternalUsers, 
	         Direction, 
	         HTMLFullBody, 
	         RightToLeft,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByContactId, 
	         CreateDate, 
	         Description, 
	         IsInternal, 
	         ObjectTableId, 
	         EntityId, 
	         ActivityTypeCode, 
	         ActivityId, 
	         ActivitySubject, 
	         ContactName, 
	         CCs, 
	         Bcc, 
	         NotifyMe, 
	         NotifyOwner, 
	         InternalUsers, 
	         ContactEmail, 
	         Direction, 
	         HTMLFullBody, 
	         RightToLeft,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CorrespondencePM entityPM, Correspondence entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByContactId))
            {
				entityPOCO.CreatedByContactId = entityPM.CreatedByContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInternal))
            {
				entityPOCO.IsInternal = entityPM.IsInternal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTypeCode))
            {
				entityPOCO.ActivityTypeCode = entityPM.ActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
				entityPOCO.ActivityId = entityPM.ActivityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivitySubject))
            {
				entityPOCO.ActivitySubject = entityPM.ActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CCs))
            {
				entityPOCO.CCs = entityPM.CCs;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Bcc))
            {
				entityPOCO.Bcc = entityPM.Bcc;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyMe))
            {
				entityPOCO.NotifyMe = entityPM.NotifyMe;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyOwner))
            {
				entityPOCO.NotifyOwner = entityPM.NotifyOwner;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalUsers))
            {
				entityPOCO.InternalUsers = entityPM.InternalUsers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
				entityPOCO.Direction = entityPM.Direction;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HTMLFullBody))
            {
				entityPOCO.HTMLFullBody = entityPM.HTMLFullBody;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RightToLeft))
            {
				entityPOCO.RightToLeft = entityPM.RightToLeft;
			}
			}

		public void POCOToPM(CorrespondencePM entityPM, Correspondence entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByContactId))
            {
					entityPM.CreatedByContactId = entityPOCO.CreatedByContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsInternal))
            {
					entityPM.IsInternal = entityPOCO.IsInternal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityTypeCode))
            {
					entityPM.ActivityTypeCode = entityPOCO.ActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityId))
            {
					entityPM.ActivityId = entityPOCO.ActivityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivitySubject))
            {
					entityPM.ActivitySubject = entityPOCO.ActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CCs))
            {
					entityPM.CCs = entityPOCO.CCs;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Bcc))
            {
					entityPM.Bcc = entityPOCO.Bcc;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotifyMe))
            {
					entityPM.NotifyMe = entityPOCO.NotifyMe;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotifyOwner))
            {
					entityPM.NotifyOwner = entityPOCO.NotifyOwner;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalUsers))
            {
					entityPM.InternalUsers = entityPOCO.InternalUsers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Direction))
            {
					entityPM.Direction = entityPOCO.Direction;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HTMLFullBody))
            {
					entityPM.HTMLFullBody = entityPOCO.HTMLFullBody;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RightToLeft))
            {
					entityPM.RightToLeft = entityPOCO.RightToLeft;
            }

		}

		public void PMToOldPM(CorrespondencePM entityPM, CorrespondencePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByContactId))
            {
                oldEntityPM.CreatedByContactId = entityPM.CreatedByContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsInternal))
            {
                oldEntityPM.IsInternal = entityPM.IsInternal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityTypeCode))
            {
                oldEntityPM.ActivityTypeCode = entityPM.ActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
                oldEntityPM.ActivityId = entityPM.ActivityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivitySubject))
            {
                oldEntityPM.ActivitySubject = entityPM.ActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CCs))
            {
                oldEntityPM.CCs = entityPM.CCs;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Bcc))
            {
                oldEntityPM.Bcc = entityPM.Bcc;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyMe))
            {
                oldEntityPM.NotifyMe = entityPM.NotifyMe;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyOwner))
            {
                oldEntityPM.NotifyOwner = entityPM.NotifyOwner;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalUsers))
            {
                oldEntityPM.InternalUsers = entityPM.InternalUsers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
                oldEntityPM.Direction = entityPM.Direction;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HTMLFullBody))
            {
                oldEntityPM.HTMLFullBody = entityPM.HTMLFullBody;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RightToLeft))
            {
                oldEntityPM.RightToLeft = entityPM.RightToLeft;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CorrespondencePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ActivitySubject)) //T4 find type == nText 
            {
                entityPM.ActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.HTMLFullBody)) //T4 find type == nText 
            {
                entityPM.HTMLFullBody = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.HTMLFullBody));
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
	 