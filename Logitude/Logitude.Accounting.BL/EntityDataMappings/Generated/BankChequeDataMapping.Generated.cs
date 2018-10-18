
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
   
   public partial class BankChequeDataMapping: IMapping<BankChequePM, BankCheque>
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
	         BankAccountId, 
	         FirstNumber, 
	         CurrentChequeNumber, 
	         LastNumber, 
	         Inactive, 
	         IsEnded,
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
	         BankAccountId, 
	         FirstNumber, 
	         CurrentChequeNumber, 
	         LastNumber, 
	         Inactive, 
	         IsEnded,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BankChequePM entityPM, BankCheque entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
                entityPOCO.BankAccountId = entityPM.BankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstNumber))
            {
                entityPOCO.FirstNumber = entityPM.FirstNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentChequeNumber))
            {
                entityPOCO.CurrentChequeNumber = entityPM.CurrentChequeNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastNumber))
            {
                entityPOCO.LastNumber = entityPM.LastNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                entityPOCO.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEnded))
            {
                entityPOCO.IsEnded = entityPM.IsEnded;
            }
			
		  BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  		}

		public void POCOToPM(BankChequePM entityPM, BankCheque entityPOCO)
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
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankAccountId))
            {
                entityPM.BankAccountId = entityPOCO.BankAccountId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstNumber))
            {
                entityPM.FirstNumber = entityPOCO.FirstNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrentChequeNumber))
            {
                entityPM.CurrentChequeNumber = entityPOCO.CurrentChequeNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastNumber))
            {
                entityPM.LastNumber = entityPOCO.LastNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
                entityPM.Inactive = entityPOCO.Inactive;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsEnded))
            {
                entityPM.IsEnded = entityPOCO.IsEnded;
            }
			
		}

		public void PMToOldPM(BankChequePM entityPM, BankChequePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccountId))
            {
                oldEntityPM.BankAccountId = entityPM.BankAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstNumber))
            {
                oldEntityPM.FirstNumber = entityPM.FirstNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentChequeNumber))
            {
                oldEntityPM.CurrentChequeNumber = entityPM.CurrentChequeNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastNumber))
            {
                oldEntityPM.LastNumber = entityPM.LastNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEnded))
            {
                oldEntityPM.IsEnded = entityPM.IsEnded;
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
		
		private void BuildSearchFieldsGenerated(BankChequePM entityPM, BankCheque entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 