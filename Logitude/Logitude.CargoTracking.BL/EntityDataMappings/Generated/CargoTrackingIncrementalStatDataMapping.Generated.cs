
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
   
   public partial class CargoTrackingIncrementalStatDataMapping: IMapping<CargoTrackingIncrementalStatPM, CargoTrackingIncrementalStat>,IMappingEncodeBase64NVARCHARFields<CargoTrackingIncrementalStatPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         StartDate, 
	         EndDate, 
	         Shipments, 
	         Cards, 
	         Ports, 
	         Countries, 
	         TransportModes, 
	         ShipmentComputedFields, 
	         ShipmentMasterDatas, 
	         Id, 
	         ErrorLog,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         StartDate, 
	         EndDate, 
	         Shipments, 
	         Cards, 
	         Ports, 
	         Countries, 
	         TransportModes, 
	         ShipmentComputedFields, 
	         ShipmentMasterDatas, 
	         Id, 
	         ErrorLog,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingIncrementalStatPM entityPM, CargoTrackingIncrementalStat entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Shipments))
            {
				entityPOCO.Shipments = entityPM.Shipments;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Cards))
            {
				entityPOCO.Cards = entityPM.Cards;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ports))
            {
				entityPOCO.Ports = entityPM.Ports;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Countries))
            {
				entityPOCO.Countries = entityPM.Countries;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModes))
            {
				entityPOCO.TransportModes = entityPM.TransportModes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentComputedFields))
            {
				entityPOCO.ShipmentComputedFields = entityPM.ShipmentComputedFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentMasterDatas))
            {
				entityPOCO.ShipmentMasterDatas = entityPM.ShipmentMasterDatas;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorLog))
            {
				entityPOCO.ErrorLog = entityPM.ErrorLog;
			}
			}

		public void POCOToPM(CargoTrackingIncrementalStatPM entityPM, CargoTrackingIncrementalStat entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Shipments))
            {
					entityPM.Shipments = entityPOCO.Shipments;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Cards))
            {
					entityPM.Cards = entityPOCO.Cards;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Ports))
            {
					entityPM.Ports = entityPOCO.Ports;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Countries))
            {
					entityPM.Countries = entityPOCO.Countries;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModes))
            {
					entityPM.TransportModes = entityPOCO.TransportModes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentComputedFields))
            {
					entityPM.ShipmentComputedFields = entityPOCO.ShipmentComputedFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentMasterDatas))
            {
					entityPM.ShipmentMasterDatas = entityPOCO.ShipmentMasterDatas;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorLog))
            {
					entityPM.ErrorLog = entityPOCO.ErrorLog;
            }

		}

		public void PMToOldPM(CargoTrackingIncrementalStatPM entityPM, CargoTrackingIncrementalStatPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Shipments))
            {
                oldEntityPM.Shipments = entityPM.Shipments;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Cards))
            {
                oldEntityPM.Cards = entityPM.Cards;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ports))
            {
                oldEntityPM.Ports = entityPM.Ports;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Countries))
            {
                oldEntityPM.Countries = entityPM.Countries;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModes))
            {
                oldEntityPM.TransportModes = entityPM.TransportModes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentComputedFields))
            {
                oldEntityPM.ShipmentComputedFields = entityPM.ShipmentComputedFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentMasterDatas))
            {
                oldEntityPM.ShipmentMasterDatas = entityPM.ShipmentMasterDatas;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorLog))
            {
                oldEntityPM.ErrorLog = entityPM.ErrorLog;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingIncrementalStatPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ErrorLog)) //T4 find type == nText 
            {
                entityPM.ErrorLog = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ErrorLog));
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
	 