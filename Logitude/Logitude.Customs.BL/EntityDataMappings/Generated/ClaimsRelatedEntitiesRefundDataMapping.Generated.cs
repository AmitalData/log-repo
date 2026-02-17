
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
   
   public partial class ClaimsRelatedEntitiesRefundDataMapping: IMapping<ClaimsRelatedEntitiesRefundPM, ClaimsRelatedEntitiesRefund>,IMappingEncodeBase64NVARCHARFields<ClaimsRelatedEntitiesRefundPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         RefundQuntityLineNo, 
	         InvoiceNumber, 
	         SequenceNumeric, 
	         RefundQuntity,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         RefundQuntityLineNo, 
	         InvoiceNumber, 
	         SequenceNumeric, 
	         RefundQuntity,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClaimsRelatedEntitiesRefundPM entityPM, ClaimsRelatedEntitiesRefund entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
				entityPOCO.InvoiceNumber = entityPM.InvoiceNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RefundQuntity))
            {
				entityPOCO.RefundQuntity = entityPM.RefundQuntity;
			}
			}

		public void POCOToPM(ClaimsRelatedEntitiesRefundPM entityPM, ClaimsRelatedEntitiesRefund entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RefundQuntityLineNo))
            {
					entityPM.RefundQuntityLineNo = entityPOCO.RefundQuntityLineNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceNumber))
            {
					entityPM.InvoiceNumber = entityPOCO.InvoiceNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RefundQuntity))
            {
					entityPM.RefundQuntity = entityPOCO.RefundQuntity;
            }

		}

		public void PMToOldPM(ClaimsRelatedEntitiesRefundPM entityPM, ClaimsRelatedEntitiesRefundPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceNumber))
            {
                oldEntityPM.InvoiceNumber = entityPM.InvoiceNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RefundQuntity))
            {
                oldEntityPM.RefundQuntity = entityPM.RefundQuntity;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClaimsRelatedEntitiesRefundPM entityPM)
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
	 