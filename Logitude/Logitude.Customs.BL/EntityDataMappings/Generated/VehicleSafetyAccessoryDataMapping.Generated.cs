
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
   
   public partial class VehicleSafetyAccessoryDataMapping: IMapping<VehicleSafetyAccessoryPM, VehicleSafetyAccessory>,IMappingEncodeBase64NVARCHARFields<VehicleSafetyAccessoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         VehicleId, 
	         LineNumber, 
	         VehicleSafetyAccessoryCode, 
	         VehicleSafAccessoryInstlTypCod,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         VehicleId, 
	         LineNumber, 
	         VehicleSafetyAccessoryCode, 
	         VehicleSafAccessoryInstlTypCod, 
	         VehicleSafAccessoryInstlTypName, 
	         VehicleSafetyAccessoryName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(VehicleSafetyAccessoryPM entityPM, VehicleSafetyAccessory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleSafetyAccessoryCode))
            {
				entityPOCO.VehicleSafetyAccessoryCode = entityPM.VehicleSafetyAccessoryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleSafAccessoryInstlTypCod))
            {
				entityPOCO.VehicleSafAccessoryInstlTypCod = entityPM.VehicleSafAccessoryInstlTypCod;
			}
			}

		public void POCOToPM(VehicleSafetyAccessoryPM entityPM, VehicleSafetyAccessory entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleId))
            {
					entityPM.VehicleId = entityPOCO.VehicleId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleSafetyAccessoryCode))
            {
					entityPM.VehicleSafetyAccessoryCode = entityPOCO.VehicleSafetyAccessoryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VehicleSafAccessoryInstlTypCod))
            {
					entityPM.VehicleSafAccessoryInstlTypCod = entityPOCO.VehicleSafAccessoryInstlTypCod;
            }

		}

		public void PMToOldPM(VehicleSafetyAccessoryPM entityPM, VehicleSafetyAccessoryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleSafetyAccessoryCode))
            {
                oldEntityPM.VehicleSafetyAccessoryCode = entityPM.VehicleSafetyAccessoryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VehicleSafAccessoryInstlTypCod))
            {
                oldEntityPM.VehicleSafAccessoryInstlTypCod = entityPM.VehicleSafAccessoryInstlTypCod;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(VehicleSafetyAccessoryPM entityPM)
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
	 