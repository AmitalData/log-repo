
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
   
   public partial class CargoTrackingShipmentSearchDataMapping: IMapping<CargoTrackingShipmentSearchPM, CargoTrackingShipmentSearch>,IMappingEncodeBase64NVARCHARFields<CargoTrackingShipmentSearchPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         ShipmentDate, 
	         Id, 
	         ShipmentId, 
	         IsPublic, 
	         ReferenceType, 
	         ReferenceFromShipmentId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         ShipmentDate, 
	         Id, 
	         ShipmentId, 
	         IsPublic, 
	         ReferenceType, 
	         ReferenceFromShipmentId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingShipmentSearchPM entityPM, CargoTrackingShipmentSearch entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentDate))
            {
				entityPOCO.ShipmentDate = entityPM.ShipmentDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPublic))
            {
				entityPOCO.IsPublic = entityPM.IsPublic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceType))
            {
				entityPOCO.ReferenceType = entityPM.ReferenceType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceFromShipmentId))
            {
				entityPOCO.ReferenceFromShipmentId = entityPM.ReferenceFromShipmentId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CargoTrackingShipmentSearchPM entityPM, CargoTrackingShipmentSearch entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentDate))
            {
					entityPM.ShipmentDate = entityPOCO.ShipmentDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPublic))
            {
					entityPM.IsPublic = entityPOCO.IsPublic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferenceType))
            {
					entityPM.ReferenceType = entityPOCO.ReferenceType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferenceFromShipmentId))
            {
					entityPM.ReferenceFromShipmentId = entityPOCO.ReferenceFromShipmentId;
            }

		}

		public void PMToOldPM(CargoTrackingShipmentSearchPM entityPM, CargoTrackingShipmentSearchPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentDate))
            {
                oldEntityPM.ShipmentDate = entityPM.ShipmentDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPublic))
            {
                oldEntityPM.IsPublic = entityPM.IsPublic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceType))
            {
                oldEntityPM.ReferenceType = entityPM.ReferenceType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceFromShipmentId))
            {
                oldEntityPM.ReferenceFromShipmentId = entityPM.ReferenceFromShipmentId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingShipmentSearchPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(CargoTrackingShipmentSearchPM entityPM, CargoTrackingShipmentSearch entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 