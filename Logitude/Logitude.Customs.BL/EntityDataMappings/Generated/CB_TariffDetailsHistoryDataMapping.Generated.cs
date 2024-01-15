
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
   
   public partial class CB_TariffDetailsHistoryDataMapping: IMapping<CB_TariffDetailsHistoryPM, CB_TariffDetailsHistory>,IMappingEncodeBase64NVARCHARFields<CB_TariffDetailsHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TariffID, 
	         QuotaID, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         WithinQuota_ComputMethDataID, 
	         WithoutQuota_ComputMethDataID, 
	         ChangeRequestTypePriority,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TariffID, 
	         QuotaID, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         WithinQuota_ComputMethDataID, 
	         WithoutQuota_ComputMethDataID, 
	         ChangeRequestTypePriority,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_TariffDetailsHistoryPM entityPM, CB_TariffDetailsHistory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffID))
            {
				entityPOCO.TariffID = entityPM.TariffID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaID))
            {
				entityPOCO.QuotaID = entityPM.QuotaID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
				entityPOCO.EntityStatusID = entityPM.EntityStatusID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithinQuota_ComputMethDataID))
            {
				entityPOCO.WithinQuota_ComputMethDataID = entityPM.WithinQuota_ComputMethDataID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithoutQuota_ComputMethDataID))
            {
				entityPOCO.WithoutQuota_ComputMethDataID = entityPM.WithoutQuota_ComputMethDataID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			}

		public void POCOToPM(CB_TariffDetailsHistoryPM entityPM, CB_TariffDetailsHistory entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffID))
            {
					entityPM.TariffID = entityPOCO.TariffID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotaID))
            {
					entityPM.QuotaID = entityPOCO.QuotaID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityStatusID))
            {
					entityPM.EntityStatusID = entityPOCO.EntityStatusID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WithinQuota_ComputMethDataID))
            {
					entityPM.WithinQuota_ComputMethDataID = entityPOCO.WithinQuota_ComputMethDataID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WithoutQuota_ComputMethDataID))
            {
					entityPM.WithoutQuota_ComputMethDataID = entityPOCO.WithoutQuota_ComputMethDataID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

		}

		public void PMToOldPM(CB_TariffDetailsHistoryPM entityPM, CB_TariffDetailsHistoryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffID))
            {
                oldEntityPM.TariffID = entityPM.TariffID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaID))
            {
                oldEntityPM.QuotaID = entityPM.QuotaID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
                oldEntityPM.EntityStatusID = entityPM.EntityStatusID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithinQuota_ComputMethDataID))
            {
                oldEntityPM.WithinQuota_ComputMethDataID = entityPM.WithinQuota_ComputMethDataID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WithoutQuota_ComputMethDataID))
            {
                oldEntityPM.WithoutQuota_ComputMethDataID = entityPM.WithoutQuota_ComputMethDataID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_TariffDetailsHistoryPM entityPM)
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
	 