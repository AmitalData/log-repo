
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
   
   public partial class AccountingEntitiesJournalDataMapping: IMapping<AccountingEntitiesJournalPM, AccountingEntitiesJournal>,IMappingEncodeBase64NVARCHARFields<AccountingEntitiesJournalPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AccountingEntityId, 
	         AccountingEntityCode, 
	         Action, 
	         ChildEntityId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         AccountingEntityId, 
	         AccountingEntityCode, 
	         Action, 
	         ChildEntityId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(AccountingEntitiesJournalPM entityPM, AccountingEntitiesJournal entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityId))
            {
				entityPOCO.AccountingEntityId = entityPM.AccountingEntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityCode))
            {
				entityPOCO.AccountingEntityCode = entityPM.AccountingEntityCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Action))
            {
				entityPOCO.Action = entityPM.Action;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChildEntityId))
            {
				entityPOCO.ChildEntityId = entityPM.ChildEntityId;
			}
			}

		public void POCOToPM(AccountingEntitiesJournalPM entityPM, AccountingEntitiesJournal entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingEntityId))
            {
					entityPM.AccountingEntityId = entityPOCO.AccountingEntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountingEntityCode))
            {
					entityPM.AccountingEntityCode = entityPOCO.AccountingEntityCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Action))
            {
					entityPM.Action = entityPOCO.Action;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChildEntityId))
            {
					entityPM.ChildEntityId = entityPOCO.ChildEntityId;
            }

		}

		public void PMToOldPM(AccountingEntitiesJournalPM entityPM, AccountingEntitiesJournalPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityId))
            {
                oldEntityPM.AccountingEntityId = entityPM.AccountingEntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountingEntityCode))
            {
                oldEntityPM.AccountingEntityCode = entityPM.AccountingEntityCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Action))
            {
                oldEntityPM.Action = entityPM.Action;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChildEntityId))
            {
                oldEntityPM.ChildEntityId = entityPM.ChildEntityId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(AccountingEntitiesJournalPM entityPM)
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
	 