
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
   
   public partial class OpportunityProductDataMapping: IMapping<OpportunityProductPM, OpportunityProduct>,IMappingEncodeBase64NVARCHARFields<OpportunityProductPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         OpportunityProductTypeCode, 
	         Tenant, 
	         Notes, 
	         ChargeableWeight, 
	         TEU, 
	         NumberOfShipments, 
	         Revenue, 
	         PrepaidCollectId, 
	         NotesRightToLeft,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         OpportunityProductTypeCode, 
	         Tenant, 
	         Notes, 
	         ChargeableWeight, 
	         TEU, 
	         NumberOfShipments, 
	         OpportunityProductTypeName, 
	         Revenue, 
	         PrepaidCollectId, 
	         PrepaidCollectName, 
	         NotesRightToLeft,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OpportunityProductPM entityPM, OpportunityProduct entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
				entityPOCO.ChargeableWeight = entityPM.ChargeableWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
				entityPOCO.TEU = entityPM.TEU;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
				entityPOCO.NumberOfShipments = entityPM.NumberOfShipments;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Revenue))
            {
				entityPOCO.Revenue = entityPM.Revenue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrepaidCollectId))
            {
				entityPOCO.PrepaidCollectId = entityPM.PrepaidCollectId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesRightToLeft))
            {
				entityPOCO.NotesRightToLeft = entityPM.NotesRightToLeft;
			}
			}

		public void POCOToPM(OpportunityProductPM entityPM, OpportunityProduct entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityProductTypeCode))
            {
					entityPM.OpportunityProductTypeCode = entityPOCO.OpportunityProductTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeight))
            {
					entityPM.ChargeableWeight = entityPOCO.ChargeableWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TEU))
            {
					entityPM.TEU = entityPOCO.TEU;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfShipments))
            {
					entityPM.NumberOfShipments = entityPOCO.NumberOfShipments;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Revenue))
            {
					entityPM.Revenue = entityPOCO.Revenue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrepaidCollectId))
            {
					entityPM.PrepaidCollectId = entityPOCO.PrepaidCollectId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotesRightToLeft))
            {
					entityPM.NotesRightToLeft = entityPOCO.NotesRightToLeft;
            }

		}

		public void PMToOldPM(OpportunityProductPM entityPM, OpportunityProductPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
                oldEntityPM.ChargeableWeight = entityPM.ChargeableWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
                oldEntityPM.TEU = entityPM.TEU;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfShipments))
            {
                oldEntityPM.NumberOfShipments = entityPM.NumberOfShipments;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Revenue))
            {
                oldEntityPM.Revenue = entityPM.Revenue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrepaidCollectId))
            {
                oldEntityPM.PrepaidCollectId = entityPM.PrepaidCollectId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesRightToLeft))
            {
                oldEntityPM.NotesRightToLeft = entityPM.NotesRightToLeft;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OpportunityProductPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
	 