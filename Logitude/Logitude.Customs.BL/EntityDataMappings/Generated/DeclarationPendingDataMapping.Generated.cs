
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationPendingDataMapping: IMapping<DeclarationPendingPM, DeclarationPending>,IMappingEncodeBase64NVARCHARFields<DeclarationPendingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeclarationID, 
	         CourierPendingReasonCode, 
	         PendingRemarks, 
	         Status,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         DeclarationID, 
	         CourierPendingReasonCode, 
	         CourierPendingReasonName, 
	         PendingRemarks, 
	         Status,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationPendingPM entityPM, DeclarationPending entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PendingRemarks))
            {
				entityPOCO.PendingRemarks = entityPM.PendingRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Status))
            {
				entityPOCO.Status = entityPM.Status;
			}
			}

		public void POCOToPM(DeclarationPendingPM entityPM, DeclarationPending entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationID))
            {
					entityPM.DeclarationID = entityPOCO.DeclarationID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierPendingReasonCode))
            {
					entityPM.CourierPendingReasonCode = entityPOCO.CourierPendingReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PendingRemarks))
            {
					entityPM.PendingRemarks = entityPOCO.PendingRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Status))
            {
					entityPM.Status = entityPOCO.Status;
            }

		}

		public void PMToOldPM(DeclarationPendingPM entityPM, DeclarationPendingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PendingRemarks))
            {
                oldEntityPM.PendingRemarks = entityPM.PendingRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Status))
            {
                oldEntityPM.Status = entityPM.Status;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationPendingPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.PendingRemarks)) //T4 find type == nText 
            {
                entityPM.PendingRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PendingRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Status)) //T4 find type == nText 
            {
                entityPM.Status = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Status));
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
	 