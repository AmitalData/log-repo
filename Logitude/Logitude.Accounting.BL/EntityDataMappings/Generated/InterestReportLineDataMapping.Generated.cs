
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
   
   public partial class InterestReportLineDataMapping: IMapping<InterestReportLinePM, InterestReportLine>,IMappingEncodeBase64NVARCHARFields<InterestReportLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         InterestReportId, 
	         InterestTransactionId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         InterestReportId, 
	         InterestTransactionId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterestReportLinePM entityPM, InterestReportLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestTransactionId))
            {
				entityPOCO.InterestTransactionId = entityPM.InterestTransactionId;
			}
			}

		public void POCOToPM(InterestReportLinePM entityPM, InterestReportLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestReportId))
            {
					entityPM.InterestReportId = entityPOCO.InterestReportId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestTransactionId))
            {
					entityPM.InterestTransactionId = entityPOCO.InterestTransactionId;
            }

		}

		public void PMToOldPM(InterestReportLinePM entityPM, InterestReportLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestTransactionId))
            {
                oldEntityPM.InterestTransactionId = entityPM.InterestTransactionId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterestReportLinePM entityPM)
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
	 