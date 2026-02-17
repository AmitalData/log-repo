
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
   
   public partial class BankDepositLineDataMapping: IMapping<BankDepositLinePM, BankDepositLine>,IMappingEncodeBase64NVARCHARFields<BankDepositLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         DepositId, 
	         Line, 
	         ARPaymentChequeId, 
	         IsOutOfDeposit, 
	         OutOfDepositeDate, 
	         Notes, 
	         SearchFields,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         DepositId, 
	         Line, 
	         ARPaymentChequeId, 
	         IsOutOfDeposit, 
	         OutOfDepositeDate, 
	         Notes, 
	         ChequeNumber, 
	         DueDate, 
	         LocalAmount, 
	         Currency, 
	         ForeignAmount, 
	         AccountNumber, 
	         Bank, 
	         Branch, 
	         ARPaymentNumber, 
	         CompositId, 
	         ARPaymentId, 
	         SearchFields, 
	         ChequeStatusName, 
	         ChequeStatusCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BankDepositLinePM entityPM, BankDepositLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ARPaymentChequeId))
            {
				entityPOCO.ARPaymentChequeId = entityPM.ARPaymentChequeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOutOfDeposit))
            {
				entityPOCO.IsOutOfDeposit = entityPM.IsOutOfDeposit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutOfDepositeDate))
            {
				entityPOCO.OutOfDepositeDate = entityPM.OutOfDepositeDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(BankDepositLinePM entityPM, BankDepositLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositId))
            {
					entityPM.DepositId = entityPOCO.DepositId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ARPaymentChequeId))
            {
					entityPM.ARPaymentChequeId = entityPOCO.ARPaymentChequeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsOutOfDeposit))
            {
					entityPM.IsOutOfDeposit = entityPOCO.IsOutOfDeposit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutOfDepositeDate))
            {
					entityPM.OutOfDepositeDate = entityPOCO.OutOfDepositeDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

		}

		public void PMToOldPM(BankDepositLinePM entityPM, BankDepositLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ARPaymentChequeId))
            {
                oldEntityPM.ARPaymentChequeId = entityPM.ARPaymentChequeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOutOfDeposit))
            {
                oldEntityPM.IsOutOfDeposit = entityPM.IsOutOfDeposit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutOfDepositeDate))
            {
                oldEntityPM.OutOfDepositeDate = entityPM.OutOfDepositeDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BankDepositLinePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
		
		private void BuildSearchFieldsGenerated(BankDepositLinePM entityPM, BankDepositLine entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 