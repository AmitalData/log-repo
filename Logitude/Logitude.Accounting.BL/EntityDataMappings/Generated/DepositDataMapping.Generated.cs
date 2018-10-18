
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class DepositDataMapping: IMapping<DepositPM, Deposit>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         DepositNumber, 
	         DepositDate, 
	         DepositCurrencyId, 
	         LocalDepositAmount, 
	         ForeignAmount, 
	         DepositBankAccountId, 
	         CashBookId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         DepositNumber, 
	         DepositDate, 
	         DepositCurrencyId, 
	         LocalDepositAmount, 
	         ForeignAmount, 
	         DepositBankAccountId, 
	         CashBookId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DepositPM entityPM, Deposit entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                entityPOCO.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                entityPOCO.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                entityPOCO.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositNumber))
            {
                entityPOCO.DepositNumber = entityPM.DepositNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositDate))
            {
                entityPOCO.DepositDate = entityPM.DepositDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositCurrencyId))
            {
                entityPOCO.DepositCurrencyId = entityPM.DepositCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalDepositAmount))
            {
                entityPOCO.LocalDepositAmount = entityPM.LocalDepositAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                entityPOCO.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositBankAccountId))
            {
                entityPOCO.DepositBankAccountId = entityPM.DepositBankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CashBookId))
            {
                entityPOCO.CashBookId = entityPM.CashBookId;
            }
			
		  BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  		}

		public void POCOToPM(DepositPM entityPM, Deposit entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
                entityPM.Id = entityPOCO.Id;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
                entityPM.Tenant = entityPOCO.Tenant;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
                entityPM.CreateDate = entityPOCO.CreateDate;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
                entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
                entityPM.UpdateDate = entityPOCO.UpdateDate;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
                entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
                entityPM.SearchFields = entityPOCO.SearchFields;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositNumber))
            {
                entityPM.DepositNumber = entityPOCO.DepositNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositDate))
            {
                entityPM.DepositDate = entityPOCO.DepositDate;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositCurrencyId))
            {
                entityPM.DepositCurrencyId = entityPOCO.DepositCurrencyId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalDepositAmount))
            {
                entityPM.LocalDepositAmount = entityPOCO.LocalDepositAmount;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignAmount))
            {
                entityPM.ForeignAmount = entityPOCO.ForeignAmount;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositBankAccountId))
            {
                entityPM.DepositBankAccountId = entityPOCO.DepositBankAccountId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CashBookId))
            {
                entityPM.CashBookId = entityPOCO.CashBookId;
            }
			
		}

		public void PMToOldPM(DepositPM entityPM, DepositPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositNumber))
            {
                oldEntityPM.DepositNumber = entityPM.DepositNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositDate))
            {
                oldEntityPM.DepositDate = entityPM.DepositDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositCurrencyId))
            {
                oldEntityPM.DepositCurrencyId = entityPM.DepositCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalDepositAmount))
            {
                oldEntityPM.LocalDepositAmount = entityPM.LocalDepositAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                oldEntityPM.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositBankAccountId))
            {
                oldEntityPM.DepositBankAccountId = entityPM.DepositBankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CashBookId))
            {
                oldEntityPM.CashBookId = entityPM.CashBookId;
            }
			
		}

	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(DepositPM entityPM, Deposit entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 