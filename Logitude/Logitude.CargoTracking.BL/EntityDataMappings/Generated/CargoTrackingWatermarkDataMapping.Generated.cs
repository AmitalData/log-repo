
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
   
   public partial class CargoTrackingWatermarkDataMapping: IMapping<CargoTrackingWatermarkPM, CargoTrackingWatermark>,IMappingEncodeBase64NVARCHARFields<CargoTrackingWatermarkPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         TableName, 
	         LastUpdateDate, 
	         LastRun,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         TableName, 
	         LastUpdateDate, 
	         LastRun,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingWatermarkPM entityPM, CargoTrackingWatermark entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
				entityPOCO.LastUpdateDate = entityPM.LastUpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRun))
            {
				entityPOCO.LastRun = entityPM.LastRun;
			}
			}

		public void POCOToPM(CargoTrackingWatermarkPM entityPM, CargoTrackingWatermark entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TableName))
            {
					entityPM.TableName = entityPOCO.TableName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateDate))
            {
					entityPM.LastUpdateDate = entityPOCO.LastUpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastRun))
            {
					entityPM.LastRun = entityPOCO.LastRun;
            }

		}

		public void PMToOldPM(CargoTrackingWatermarkPM entityPM, CargoTrackingWatermarkPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
                oldEntityPM.LastUpdateDate = entityPM.LastUpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRun))
            {
                oldEntityPM.LastRun = entityPM.LastRun;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingWatermarkPM entityPM)
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
	 