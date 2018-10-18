
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
   
   public partial class CashBookLineDataMapping: IMapping<CashBookLinePM, CashBookLine>,IMappingEncodeBase64NVARCHARFields<CashBookLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CashBookId, 
	         Tenant, 
	         ARPChequeId, 
	         IsDeposited, 
	         SearchFields,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CashBookId, 
	         Tenant, 
	         ARPChequeId, 
	         ChequeNumber, 
	         IsDeposited, 
	         DueDate, 
	         LocalAmount, 
	         Currency, 
	         ForeignAmount, 
	         AccountNumber, 
	         Bank, 
	         Branch, 
	         ARPaymentNumber, 
	         ARPaymentId, 
	         SearchFields, 
	         ARPChequeStatusName, 
	         ARPChequeStatusCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CashBookLinePM entityPM, CashBookLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeposited))
            {
				entityPOCO.IsDeposited = entityPM.IsDeposited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CashBookLinePM entityPM, CashBookLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CashBookId))
            {
					entityPM.CashBookId = entityPOCO.CashBookId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ARPChequeId))
            {
					entityPM.ARPChequeId = entityPOCO.ARPChequeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDeposited))
            {
					entityPM.IsDeposited = entityPOCO.IsDeposited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

		}

		public void PMToOldPM(CashBookLinePM entityPM, CashBookLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeposited))
            {
                oldEntityPM.IsDeposited = entityPM.IsDeposited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CashBookLinePM entityPM)
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
		
		private void BuildSearchFieldsGenerated(CashBookLinePM entityPM, CashBookLine entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 