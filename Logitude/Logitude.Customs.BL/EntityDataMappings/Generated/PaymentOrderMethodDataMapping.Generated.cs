
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
   
   public partial class PaymentOrderMethodDataMapping: IMapping<PaymentOrderMethodPM, PaymentOrderMethod>,IMappingEncodeBase64NVARCHARFields<PaymentOrderMethodPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         Tenant, 
	         Line, 
	         TypeCode, 
	         Amount, 
	         BankCode, 
	         BranchCode, 
	         AccountNumber, 
	         PaymentMethodStatusCode, 
	         InternalBankId, 
	         CustomerActivityTypeCode, 
	         CustomsBranchId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         PaymentOrderId, 
	         Tenant, 
	         Line, 
	         TypeCode, 
	         Amount, 
	         BankCode, 
	         BranchCode, 
	         AccountNumber, 
	         PaymentMethodStatusCode, 
	         InternalBankId, 
	         TypeName, 
	         BankName, 
	         PaymentMethodStatusName, 
	         CustomerActivityTypeCode, 
	         CustomerActivityTypeName, 
	         CustomsBranchId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(PaymentOrderMethodPM entityPM, PaymentOrderMethod entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentMethodStatusCode))
            {
				entityPOCO.PaymentMethodStatusCode = entityPM.PaymentMethodStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalBankId))
            {
				entityPOCO.InternalBankId = entityPM.InternalBankId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerActivityTypeCode))
            {
				entityPOCO.CustomerActivityTypeCode = entityPM.CustomerActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchId))
            {
				entityPOCO.CustomsBranchId = entityPM.CustomsBranchId;
			}
			}

		public void POCOToPM(PaymentOrderMethodPM entityPM, PaymentOrderMethod entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentOrderId))
            {
					entityPM.PaymentOrderId = entityPOCO.PaymentOrderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentMethodStatusCode))
            {
					entityPM.PaymentMethodStatusCode = entityPOCO.PaymentMethodStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InternalBankId))
            {
					entityPM.InternalBankId = entityPOCO.InternalBankId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerActivityTypeCode))
            {
					entityPM.CustomerActivityTypeCode = entityPOCO.CustomerActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBranchId))
            {
					entityPM.CustomsBranchId = entityPOCO.CustomsBranchId;
            }

		}

		public void PMToOldPM(PaymentOrderMethodPM entityPM, PaymentOrderMethodPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentMethodStatusCode))
            {
                oldEntityPM.PaymentMethodStatusCode = entityPM.PaymentMethodStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InternalBankId))
            {
                oldEntityPM.InternalBankId = entityPM.InternalBankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerActivityTypeCode))
            {
                oldEntityPM.CustomerActivityTypeCode = entityPM.CustomerActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchId))
            {
                oldEntityPM.CustomsBranchId = entityPM.CustomsBranchId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(PaymentOrderMethodPM entityPM)
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
	 