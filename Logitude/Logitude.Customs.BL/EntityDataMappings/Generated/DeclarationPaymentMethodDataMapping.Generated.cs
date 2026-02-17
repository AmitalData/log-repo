
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
   
   public partial class DeclarationPaymentMethodDataMapping: IMapping<DeclarationPaymentMethodPM, DeclarationPaymentMethod>,IMappingEncodeBase64NVARCHARFields<DeclarationPaymentMethodPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Line, 
	         SequenceNumeric, 
	         PayerActivityTypeCode, 
	         MethodTypeCode, 
	         Amount, 
	         BankCode, 
	         BranchCode, 
	         AccountNumber, 
	         Tenant, 
	         InternalBankId, 
	         CustomsBranchId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Line, 
	         SequenceNumeric, 
	         PayerActivityTypeCode, 
	         MethodTypeCode, 
	         Amount, 
	         BankCode, 
	         BranchCode, 
	         AccountNumber, 
	         Tenant, 
	         InternalBankId, 
	         PayerActivityTypeName, 
	         MethodTypeName, 
	         InternalBankName, 
	         CustomsBranchId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationPaymentMethodPM entityPM, DeclarationPaymentMethod entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
				entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayerActivityTypeCode))
            {
				entityPOCO.PayerActivityTypeCode = entityPM.PayerActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MethodTypeCode))
            {
				entityPOCO.MethodTypeCode = entityPM.MethodTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
				entityPOCO.Amount = entityPM.Amount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankCode))
            {
				entityPOCO.BankCode = entityPM.BankCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchCode))
            {
				entityPOCO.BranchCode = entityPM.BranchCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
				entityPOCO.AccountNumber = entityPM.AccountNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalBankId))
            {
				entityPOCO.InternalBankId = entityPM.InternalBankId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchId))
            {
				entityPOCO.CustomsBranchId = entityPM.CustomsBranchId;
			}
			}

		public void POCOToPM(DeclarationPaymentMethodPM entityPM, DeclarationPaymentMethod entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SequenceNumeric))
            {
					entityPM.SequenceNumeric = entityPOCO.SequenceNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PayerActivityTypeCode))
            {
					entityPM.PayerActivityTypeCode = entityPOCO.PayerActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MethodTypeCode))
            {
					entityPM.MethodTypeCode = entityPOCO.MethodTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Amount))
            {
					entityPM.Amount = entityPOCO.Amount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankCode))
            {
					entityPM.BankCode = entityPOCO.BankCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchCode))
            {
					entityPM.BranchCode = entityPOCO.BranchCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountNumber))
            {
					entityPM.AccountNumber = entityPOCO.AccountNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalBankId))
            {
					entityPM.InternalBankId = entityPOCO.InternalBankId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBranchId))
            {
					entityPM.CustomsBranchId = entityPOCO.CustomsBranchId;
            }

		}

		public void PMToOldPM(DeclarationPaymentMethodPM entityPM, DeclarationPaymentMethodPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SequenceNumeric))
            {
                oldEntityPM.SequenceNumeric = entityPM.SequenceNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PayerActivityTypeCode))
            {
                oldEntityPM.PayerActivityTypeCode = entityPM.PayerActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MethodTypeCode))
            {
                oldEntityPM.MethodTypeCode = entityPM.MethodTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Amount))
            {
                oldEntityPM.Amount = entityPM.Amount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankCode))
            {
                oldEntityPM.BankCode = entityPM.BankCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchCode))
            {
                oldEntityPM.BranchCode = entityPM.BranchCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
                oldEntityPM.AccountNumber = entityPM.AccountNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalBankId))
            {
                oldEntityPM.InternalBankId = entityPM.InternalBankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchId))
            {
                oldEntityPM.CustomsBranchId = entityPM.CustomsBranchId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationPaymentMethodPM entityPM)
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
	 