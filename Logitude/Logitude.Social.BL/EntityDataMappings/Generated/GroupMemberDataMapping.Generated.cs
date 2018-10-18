
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
   
   public partial class GroupMemberDataMapping: IMapping<GroupMemberPM, GroupMember>,IMappingEncodeBase64NVARCHARFields<GroupMemberPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         GroupId, 
	         UserId, 
	         Tenant, 
	         CreateDate, 
	         IsCancelled, 
	         CancelledDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         GroupId, 
	         UserId, 
	         Tenant, 
	         CreateDate, 
	         IsCancelled, 
	         CancelledDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GroupMemberPM entityPM, GroupMember entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledDate))
            {
				entityPOCO.CancelledDate = entityPM.CancelledDate;
			}
			}

		public void POCOToPM(GroupMemberPM entityPM, GroupMember entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupId))
            {
					entityPM.GroupId = entityPOCO.GroupId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelledDate))
            {
					entityPM.CancelledDate = entityPOCO.CancelledDate;
            }

		}

		public void PMToOldPM(GroupMemberPM entityPM, GroupMemberPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelledDate))
            {
                oldEntityPM.CancelledDate = entityPM.CancelledDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GroupMemberPM entityPM)
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
	 