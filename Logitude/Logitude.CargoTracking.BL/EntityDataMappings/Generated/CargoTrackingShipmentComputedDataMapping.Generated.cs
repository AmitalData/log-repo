
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
   
   public partial class CargoTrackingShipmentComputedDataMapping: IMapping<CargoTrackingShipmentComputedPM, CargoTrackingShipmentComputed>,IMappingEncodeBase64NVARCHARFields<CargoTrackingShipmentComputedPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         FirstPickupATD, 
	         FinalDeliveryATA, 
	         FinalDeliveryETA, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         FirstPickupATD, 
	         FinalDeliveryATA, 
	         FinalDeliveryETA, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingShipmentComputedPM entityPM, CargoTrackingShipmentComputed entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstPickupATD))
            {
				entityPOCO.FirstPickupATD = entityPM.FirstPickupATD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDeliveryATA))
            {
				entityPOCO.FinalDeliveryATA = entityPM.FinalDeliveryATA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDeliveryETA))
            {
				entityPOCO.FinalDeliveryETA = entityPM.FinalDeliveryETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(CargoTrackingShipmentComputedPM entityPM, CargoTrackingShipmentComputed entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstPickupATD))
            {
					entityPM.FirstPickupATD = entityPOCO.FirstPickupATD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalDeliveryATA))
            {
					entityPM.FinalDeliveryATA = entityPOCO.FinalDeliveryATA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalDeliveryETA))
            {
					entityPM.FinalDeliveryETA = entityPOCO.FinalDeliveryETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(CargoTrackingShipmentComputedPM entityPM, CargoTrackingShipmentComputedPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstPickupATD))
            {
                oldEntityPM.FirstPickupATD = entityPM.FirstPickupATD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDeliveryATA))
            {
                oldEntityPM.FinalDeliveryATA = entityPM.FinalDeliveryATA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDeliveryETA))
            {
                oldEntityPM.FinalDeliveryETA = entityPM.FinalDeliveryETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingShipmentComputedPM entityPM)
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
	 