
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
   
   public partial class ExternalReconciliationLineDataMapping: IMapping<ExternalReconciliationLinePM, ExternalReconciliationLine>,IMappingEncodeBase64NVARCHARFields<ExternalReconciliationLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ReconciliationId, 
	         Line, 
	         LedgerTransactionId, 
	         GroupNumber, 
	         ExternalPageLineId, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ReconciliationId, 
	         Line, 
	         LedgerTransactionId, 
	         GroupNumber, 
	         ExternalPageLineId, 
	         Tenant, 
	         LedgerGLAccountId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ExternalReconciliationLinePM entityPM, ExternalReconciliationLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LedgerTransactionId))
            {
				entityPOCO.LedgerTransactionId = entityPM.LedgerTransactionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupNumber))
            {
				entityPOCO.GroupNumber = entityPM.GroupNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalPageLineId))
            {
				entityPOCO.ExternalPageLineId = entityPM.ExternalPageLineId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(ExternalReconciliationLinePM entityPM, ExternalReconciliationLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReconciliationId))
            {
					entityPM.ReconciliationId = entityPOCO.ReconciliationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LedgerTransactionId))
            {
					entityPM.LedgerTransactionId = entityPOCO.LedgerTransactionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GroupNumber))
            {
					entityPM.GroupNumber = entityPOCO.GroupNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalPageLineId))
            {
					entityPM.ExternalPageLineId = entityPOCO.ExternalPageLineId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(ExternalReconciliationLinePM entityPM, ExternalReconciliationLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LedgerTransactionId))
            {
                oldEntityPM.LedgerTransactionId = entityPM.LedgerTransactionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GroupNumber))
            {
                oldEntityPM.GroupNumber = entityPM.GroupNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalPageLineId))
            {
                oldEntityPM.ExternalPageLineId = entityPM.ExternalPageLineId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ExternalReconciliationLinePM entityPM)
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
	 