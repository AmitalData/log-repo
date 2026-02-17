
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class PaymentChequeLineDataMapping: IMapping<PaymentChequeLinePM, PaymentChequeLine>,IMappingEncodeBase64NVARCHARFields<PaymentChequeLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         PaymentChequeId, 
	         Line, 
	         Notes, 
	         Amount, 
	         SequenceNumeric,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         PaymentChequeId, 
	         Line, 
	         Notes, 
	         Amount, 
	         SequenceNumeric,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PaymentChequeLinePM entityPM, PaymentChequeLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
				entityPOCO.Amount = entityPM.Amount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			}

		public void POCOToPM(PaymentChequeLinePM entityPM, PaymentChequeLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentChequeId))
            {
					entityPM.PaymentChequeId = entityPOCO.PaymentChequeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Amount))
            {
					entityPM.Amount = entityPOCO.Amount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

		}

		public void PMToOldPM(PaymentChequeLinePM entityPM, PaymentChequeLinePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
                oldEntityPM.Amount = entityPM.Amount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PaymentChequeLinePM entityPM)
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
	 