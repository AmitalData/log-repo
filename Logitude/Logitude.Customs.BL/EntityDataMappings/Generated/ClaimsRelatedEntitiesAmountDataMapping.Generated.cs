
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
   
   public partial class ClaimsRelatedEntitiesAmountDataMapping: IMapping<ClaimsRelatedEntitiesAmountPM, ClaimsRelatedEntitiesAmount>,IMappingEncodeBase64NVARCHARFields<ClaimsRelatedEntitiesAmountPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         LineNo, 
	         PaymentTypeCode, 
	         Amount,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         LineNo, 
	         PaymentTypeCode, 
	         PaymentTypeName, 
	         Amount,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClaimsRelatedEntitiesAmountPM entityPM, ClaimsRelatedEntitiesAmount entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTypeCode))
            {
				entityPOCO.PaymentTypeCode = entityPM.PaymentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
				entityPOCO.Amount = entityPM.Amount;
			}
			}

		public void POCOToPM(ClaimsRelatedEntitiesAmountPM entityPM, ClaimsRelatedEntitiesAmount entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClaimId))
            {
					entityPM.ClaimId = entityPOCO.ClaimId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CounterKey))
            {
					entityPM.CounterKey = entityPOCO.CounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNo))
            {
					entityPM.LineNo = entityPOCO.LineNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentTypeCode))
            {
					entityPM.PaymentTypeCode = entityPOCO.PaymentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Amount))
            {
					entityPM.Amount = entityPOCO.Amount;
            }

		}

		public void PMToOldPM(ClaimsRelatedEntitiesAmountPM entityPM, ClaimsRelatedEntitiesAmountPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentTypeCode))
            {
                oldEntityPM.PaymentTypeCode = entityPM.PaymentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
                oldEntityPM.Amount = entityPM.Amount;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClaimsRelatedEntitiesAmountPM entityPM)
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
	 