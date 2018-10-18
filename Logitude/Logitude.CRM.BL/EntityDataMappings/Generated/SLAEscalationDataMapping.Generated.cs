
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class SLAEscalationDataMapping: IMapping<SLAEscalationPM, SLAEscalation>,IMappingEncodeBase64NVARCHARFields<SLAEscalationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAHeaderId, 
	         LineNumber, 
	         EscalationFor, 
	         EscalationActionTimeIndicator, 
	         EscalationTime, 
	         EscalationTimeUnit, 
	         EscalaitonTimeInMinutes,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAHeaderId, 
	         LineNumber, 
	         EscalationFor, 
	         EscalationActionTimeIndicator, 
	         EscalationTime, 
	         EscalationTimeUnit, 
	         EscalaitonTimeInMinutes, 
	         TimeIndicator, 
	         TimeUnitName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAHeaderId))
            {
				entityPOCO.SLAHeaderId = entityPM.SLAHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationFor))
            {
				entityPOCO.EscalationFor = entityPM.EscalationFor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationActionTimeIndicator))
            {
				entityPOCO.EscalationActionTimeIndicator = entityPM.EscalationActionTimeIndicator;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationTime))
            {
				entityPOCO.EscalationTime = entityPM.EscalationTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationTimeUnit))
            {
				entityPOCO.EscalationTimeUnit = entityPM.EscalationTimeUnit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalaitonTimeInMinutes))
            {
				entityPOCO.EscalaitonTimeInMinutes = entityPM.EscalaitonTimeInMinutes;
			}
			}

		public void POCOToPM(SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SLAHeaderId))
            {
					entityPM.SLAHeaderId = entityPOCO.SLAHeaderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationFor))
            {
					entityPM.EscalationFor = entityPOCO.EscalationFor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationActionTimeIndicator))
            {
					entityPM.EscalationActionTimeIndicator = entityPOCO.EscalationActionTimeIndicator;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationTime))
            {
					entityPM.EscalationTime = entityPOCO.EscalationTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalationTimeUnit))
            {
					entityPM.EscalationTimeUnit = entityPOCO.EscalationTimeUnit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EscalaitonTimeInMinutes))
            {
					entityPM.EscalaitonTimeInMinutes = entityPOCO.EscalaitonTimeInMinutes;
            }

		}

		public void PMToOldPM(SLAEscalationPM entityPM, SLAEscalationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAHeaderId))
            {
                oldEntityPM.SLAHeaderId = entityPM.SLAHeaderId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationFor))
            {
                oldEntityPM.EscalationFor = entityPM.EscalationFor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationActionTimeIndicator))
            {
                oldEntityPM.EscalationActionTimeIndicator = entityPM.EscalationActionTimeIndicator;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationTime))
            {
                oldEntityPM.EscalationTime = entityPM.EscalationTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalationTimeUnit))
            {
                oldEntityPM.EscalationTimeUnit = entityPM.EscalationTimeUnit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EscalaitonTimeInMinutes))
            {
                oldEntityPM.EscalaitonTimeInMinutes = entityPM.EscalaitonTimeInMinutes;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SLAEscalationPM entityPM)
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
	 