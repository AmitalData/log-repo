
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
   
   public partial class ARPaymentsJournalDataMapping: IMapping<ARPaymentsJournalPM, ARPaymentsJournal>,IMappingEncodeBase64NVARCHARFields<ARPaymentsJournalPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         PaymentId, 
	         IsVoided,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         PaymentId, 
	         IsVoided,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ARPaymentsJournalPM entityPM, ARPaymentsJournal entityPOCO)
        {
			 }

		public void POCOToPM(ARPaymentsJournalPM entityPM, ARPaymentsJournal entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentId))
            {
					entityPM.PaymentId = entityPOCO.PaymentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsVoided))
            {
					entityPM.IsVoided = entityPOCO.IsVoided;
            }

		}

		public void PMToOldPM(ARPaymentsJournalPM entityPM, ARPaymentsJournalPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
		}

	    public void EncodeBase64NVARCHARFields(ARPaymentsJournalPM entityPM)
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
	 