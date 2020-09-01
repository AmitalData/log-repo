
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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs; 
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL.EntityDataMappings
{
   
   public partial class CargoTrackingShipmentMasterDataMapping: IMapping<CargoTrackingShipmentMasterPM, CargoTrackingShipmentMaster>,IMappingEncodeBase64NVARCHARFields<CargoTrackingShipmentMasterPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Master, 
	         MainCarriageATD, 
	         MainCarriageETD, 
	         MainCarriageATA, 
	         MainCarriageETA, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Master, 
	         MainCarriageATD, 
	         MainCarriageETD, 
	         MainCarriageATA, 
	         MainCarriageETA, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingShipmentMasterPM entityPM, CargoTrackingShipmentMaster entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
				entityPOCO.Master = entityPM.Master;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageATD))
            {
				entityPOCO.MainCarriageATD = entityPM.MainCarriageATD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETD))
            {
				entityPOCO.MainCarriageETD = entityPM.MainCarriageETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageATA))
            {
				entityPOCO.MainCarriageATA = entityPM.MainCarriageATA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETA))
            {
				entityPOCO.MainCarriageETA = entityPM.MainCarriageETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(CargoTrackingShipmentMasterPM entityPM, CargoTrackingShipmentMaster entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Master))
            {
					entityPM.Master = entityPOCO.Master;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageATD))
            {
					entityPM.MainCarriageATD = entityPOCO.MainCarriageATD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageETD))
            {
					entityPM.MainCarriageETD = entityPOCO.MainCarriageETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageATA))
            {
					entityPM.MainCarriageATA = entityPOCO.MainCarriageATA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageETA))
            {
					entityPM.MainCarriageETA = entityPOCO.MainCarriageETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(CargoTrackingShipmentMasterPM entityPM, CargoTrackingShipmentMasterPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
                oldEntityPM.Master = entityPM.Master;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageATD))
            {
                oldEntityPM.MainCarriageATD = entityPM.MainCarriageATD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETD))
            {
                oldEntityPM.MainCarriageETD = entityPM.MainCarriageETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageATA))
            {
                oldEntityPM.MainCarriageATA = entityPM.MainCarriageATA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETA))
            {
                oldEntityPM.MainCarriageETA = entityPM.MainCarriageETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingShipmentMasterPM entityPM)
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
	 