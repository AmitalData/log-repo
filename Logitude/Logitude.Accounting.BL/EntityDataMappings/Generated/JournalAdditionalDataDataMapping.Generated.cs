
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
   
   public partial class JournalAdditionalDataDataMapping: IMapping<JournalAdditionalDataPM, JournalAdditionalData>,IMappingEncodeBase64NVARCHARFields<JournalAdditionalDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         JournalId, 
	         TaxReportId, 
	         TaxReportTransmitStatusCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         JournalId, 
	         TaxReportId, 
	         TaxReportTransmitStatusCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(JournalAdditionalDataPM entityPM, JournalAdditionalData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportId))
            {
				entityPOCO.TaxReportId = entityPM.TaxReportId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportTransmitStatusCode))
            {
				entityPOCO.TaxReportTransmitStatusCode = entityPM.TaxReportTransmitStatusCode;
			}
			}

		public void POCOToPM(JournalAdditionalDataPM entityPM, JournalAdditionalData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JournalId))
            {
					entityPM.JournalId = entityPOCO.JournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportId))
            {
					entityPM.TaxReportId = entityPOCO.TaxReportId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportTransmitStatusCode))
            {
					entityPM.TaxReportTransmitStatusCode = entityPOCO.TaxReportTransmitStatusCode;
            }

		}

		public void PMToOldPM(JournalAdditionalDataPM entityPM, JournalAdditionalDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportId))
            {
                oldEntityPM.TaxReportId = entityPM.TaxReportId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportTransmitStatusCode))
            {
                oldEntityPM.TaxReportTransmitStatusCode = entityPM.TaxReportTransmitStatusCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(JournalAdditionalDataPM entityPM)
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
	 