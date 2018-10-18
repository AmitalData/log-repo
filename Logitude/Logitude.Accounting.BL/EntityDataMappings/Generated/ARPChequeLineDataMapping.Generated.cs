
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
   
   public partial class ARPChequeLineDataMapping: IMapping<ARPChequeLinePM, ARPChequeLine>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         PaymentId, 
	         LineNumber, 
	         ChequeNumber, 
	         ValueDate, 
	         CurrencyId, 
	         LocalAmount, 
	         ForeignAmount, 
	         BankId, 
	         BankBranch, 
	         BankAccount,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         PaymentId, 
	         PaymentNumber, 
	         LineNumber, 
	         ChequeNumber, 
	         ValueDate, 
	         CurrencyId, 
	         CurrencyCode, 
	         CurrencyName, 
	         LocalAmount, 
	         ForeignAmount, 
	         BankId, 
	         BankNumber, 
	         BankName, 
	         BankBranch, 
	         BankAccount,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ARPChequeLinePM entityPM, ARPChequeLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                entityPOCO.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentId))
            {
                entityPOCO.PaymentId = entityPM.PaymentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                entityPOCO.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeNumber))
            {
                entityPOCO.ChequeNumber = entityPM.ChequeNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
                entityPOCO.ValueDate = entityPM.ValueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                entityPOCO.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
                entityPOCO.LocalAmount = entityPM.LocalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                entityPOCO.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankId))
            {
                entityPOCO.BankId = entityPM.BankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankBranch))
            {
                entityPOCO.BankBranch = entityPM.BankBranch;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccount))
            {
                entityPOCO.BankAccount = entityPM.BankAccount;
            }
			
		  BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  		}

		public void POCOToPM(ARPChequeLinePM entityPM, ARPChequeLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
                entityPM.Id = entityPOCO.Id;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
                entityPM.Tenant = entityPOCO.Tenant;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
                entityPM.SearchFields = entityPOCO.SearchFields;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentId))
            {
                entityPM.PaymentId = entityPOCO.PaymentId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
                entityPM.LineNumber = entityPOCO.LineNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChequeNumber))
            {
                entityPM.ChequeNumber = entityPOCO.ChequeNumber;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueDate))
            {
                entityPM.ValueDate = entityPOCO.ValueDate;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
                entityPM.CurrencyId = entityPOCO.CurrencyId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalAmount))
            {
                entityPM.LocalAmount = entityPOCO.LocalAmount;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignAmount))
            {
                entityPM.ForeignAmount = entityPOCO.ForeignAmount;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankId))
            {
                entityPM.BankId = entityPOCO.BankId;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankBranch))
            {
                entityPM.BankBranch = entityPOCO.BankBranch;
            }
			
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BankAccount))
            {
                entityPM.BankAccount = entityPOCO.BankAccount;
            }
			
		}

		public void PMToOldPM(ARPChequeLinePM entityPM, ARPChequeLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentId))
            {
                oldEntityPM.PaymentId = entityPM.PaymentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChequeNumber))
            {
                oldEntityPM.ChequeNumber = entityPM.ChequeNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueDate))
            {
                oldEntityPM.ValueDate = entityPM.ValueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalAmount))
            {
                oldEntityPM.LocalAmount = entityPM.LocalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignAmount))
            {
                oldEntityPM.ForeignAmount = entityPM.ForeignAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankId))
            {
                oldEntityPM.BankId = entityPM.BankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankBranch))
            {
                oldEntityPM.BankBranch = entityPM.BankBranch;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BankAccount))
            {
                oldEntityPM.BankAccount = entityPM.BankAccount;
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
		
		private void BuildSearchFieldsGenerated(ARPChequeLinePM entityPM, ARPChequeLine entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 