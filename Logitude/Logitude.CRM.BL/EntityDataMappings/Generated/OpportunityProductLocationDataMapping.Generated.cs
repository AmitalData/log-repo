
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
   
   public partial class OpportunityProductLocationDataMapping: IMapping<OpportunityProductLocationPM, OpportunityProductLocation>,IMappingEncodeBase64NVARCHARFields<OpportunityProductLocationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         OpportunityProductTypeCode, 
	         LineNumber, 
	         Tenant, 
	         CountryId, 
	         TEU, 
	         NumberOfShipments, 
	         ChargeableWeight, 
	         Revenue,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         OpportunityProductTypeCode, 
	         LineNumber, 
	         Tenant, 
	         CountryId, 
	         LocationCode, 
	         LocationName, 
	         TEU, 
	         NumberOfShipments, 
	         ChargeableWeight, 
	         Revenue,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OpportunityProductLocationPM entityPM, OpportunityProductLocation entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryId))
            {
				entityPOCO.CountryId = entityPM.CountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
				entityPOCO.TEU = entityPM.TEU;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
				entityPOCO.NumberOfShipments = entityPM.NumberOfShipments;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
				entityPOCO.ChargeableWeight = entityPM.ChargeableWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Revenue))
            {
				entityPOCO.Revenue = entityPM.Revenue;
			}
			}

		public void POCOToPM(OpportunityProductLocationPM entityPM, OpportunityProductLocation entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityProductTypeCode))
            {
					entityPM.OpportunityProductTypeCode = entityPOCO.OpportunityProductTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryId))
            {
					entityPM.CountryId = entityPOCO.CountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TEU))
            {
					entityPM.TEU = entityPOCO.TEU;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfShipments))
            {
					entityPM.NumberOfShipments = entityPOCO.NumberOfShipments;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeight))
            {
					entityPM.ChargeableWeight = entityPOCO.ChargeableWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Revenue))
            {
					entityPM.Revenue = entityPOCO.Revenue;
            }

		}

		public void PMToOldPM(OpportunityProductLocationPM entityPM, OpportunityProductLocationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryId))
            {
                oldEntityPM.CountryId = entityPM.CountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
                oldEntityPM.TEU = entityPM.TEU;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
                oldEntityPM.NumberOfShipments = entityPM.NumberOfShipments;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
                oldEntityPM.ChargeableWeight = entityPM.ChargeableWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Revenue))
            {
                oldEntityPM.Revenue = entityPM.Revenue;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OpportunityProductLocationPM entityPM)
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
	 