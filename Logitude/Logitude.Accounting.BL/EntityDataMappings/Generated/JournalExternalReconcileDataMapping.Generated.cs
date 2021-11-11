
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
   
   public partial class JournalExternalReconcileDataMapping: IMapping<JournalExternalReconcilePM, JournalExternalReconcile>,IMappingEncodeBase64NVARCHARFields<JournalExternalReconcilePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         JournalId, 
	         Line, 
	         Tenant, 
	         LedgerTransactionId, 
	         ReconcileExternalPageLineId, 
	         SkipAccountsValidation,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         JournalId, 
	         Line, 
	         Tenant, 
	         LedgerTransactionId, 
	         ReconcileExternalPageLineId, 
	         SkipAccountsValidation,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(JournalExternalReconcilePM entityPM, JournalExternalReconcile entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LedgerTransactionId))
            {
				entityPOCO.LedgerTransactionId = entityPM.LedgerTransactionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReconcileExternalPageLineId))
            {
				entityPOCO.ReconcileExternalPageLineId = entityPM.ReconcileExternalPageLineId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SkipAccountsValidation))
            {
				entityPOCO.SkipAccountsValidation = entityPM.SkipAccountsValidation;
			}
			}

		public void POCOToPM(JournalExternalReconcilePM entityPM, JournalExternalReconcile entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JournalId))
            {
					entityPM.JournalId = entityPOCO.JournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LedgerTransactionId))
            {
					entityPM.LedgerTransactionId = entityPOCO.LedgerTransactionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReconcileExternalPageLineId))
            {
					entityPM.ReconcileExternalPageLineId = entityPOCO.ReconcileExternalPageLineId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SkipAccountsValidation))
            {
					entityPM.SkipAccountsValidation = entityPOCO.SkipAccountsValidation;
            }

		}

		public void PMToOldPM(JournalExternalReconcilePM entityPM, JournalExternalReconcilePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LedgerTransactionId))
            {
                oldEntityPM.LedgerTransactionId = entityPM.LedgerTransactionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReconcileExternalPageLineId))
            {
                oldEntityPM.ReconcileExternalPageLineId = entityPM.ReconcileExternalPageLineId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SkipAccountsValidation))
            {
                oldEntityPM.SkipAccountsValidation = entityPM.SkipAccountsValidation;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(JournalExternalReconcilePM entityPM)
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
	 