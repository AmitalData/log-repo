
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
   
   public partial class PaymentOrderLineDataMapping: IMapping<PaymentOrderLinePM, PaymentOrderLine>,IMappingEncodeBase64NVARCHARFields<PaymentOrderLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         Tenant, 
	         ParagraphTypeCode, 
	         Amount,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         Tenant, 
	         ParagraphTypeCode, 
	         Amount, 
	         ParagraphTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PaymentOrderLinePM entityPM, PaymentOrderLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
				entityPOCO.Amount = entityPM.Amount;
			}
			}

		public void POCOToPM(PaymentOrderLinePM entityPM, PaymentOrderLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentOrderId))
            {
					entityPM.PaymentOrderId = entityPOCO.PaymentOrderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParagraphTypeCode))
            {
					entityPM.ParagraphTypeCode = entityPOCO.ParagraphTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Amount))
            {
					entityPM.Amount = entityPOCO.Amount;
            }

		}

		public void PMToOldPM(PaymentOrderLinePM entityPM, PaymentOrderLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
                oldEntityPM.Amount = entityPM.Amount;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PaymentOrderLinePM entityPM)
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
	 