
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
   
   public partial class SLALineDataMapping: IMapping<SLALinePM, SLALine>,IMappingEncodeBase64NVARCHARFields<SLALinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAHeaderId, 
	         SeverityId, 
	         BusinessHoursId, 
	         FirstResponseTime, 
	         FirstResponseTimeUnit, 
	         FirstResponseTimeInMinute, 
	         ResolveWithinTime, 
	         ResolveWithinTimeUnit, 
	         ResolveWithinTimeInMinute, 
	         FirstResponseEscalate, 
	         ResolveWithinEscalate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAHeaderId, 
	         SeverityId, 
	         BusinessHoursId, 
	         FirstResponseTime, 
	         FirstResponseTimeUnit, 
	         FirstResponseTimeInMinute, 
	         ResolveWithinTime, 
	         ResolveWithinTimeUnit, 
	         ResolveWithinTimeInMinute, 
	         FirstResponseEscalate, 
	         ResolveWithinEscalate, 
	         SeverityName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SLALinePM entityPM, SLALine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAHeaderId))
            {
				entityPOCO.SLAHeaderId = entityPM.SLAHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeverityId))
            {
				entityPOCO.SeverityId = entityPM.SeverityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessHoursId))
            {
				entityPOCO.BusinessHoursId = entityPM.BusinessHoursId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTime))
            {
				entityPOCO.FirstResponseTime = entityPM.FirstResponseTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTimeUnit))
            {
				entityPOCO.FirstResponseTimeUnit = entityPM.FirstResponseTimeUnit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTimeInMinute))
            {
				entityPOCO.FirstResponseTimeInMinute = entityPM.FirstResponseTimeInMinute;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTime))
            {
				entityPOCO.ResolveWithinTime = entityPM.ResolveWithinTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTimeUnit))
            {
				entityPOCO.ResolveWithinTimeUnit = entityPM.ResolveWithinTimeUnit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTimeInMinute))
            {
				entityPOCO.ResolveWithinTimeInMinute = entityPM.ResolveWithinTimeInMinute;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseEscalate))
            {
				entityPOCO.FirstResponseEscalate = entityPM.FirstResponseEscalate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinEscalate))
            {
				entityPOCO.ResolveWithinEscalate = entityPM.ResolveWithinEscalate;
			}
			}

		public void POCOToPM(SLALinePM entityPM, SLALine entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SeverityId))
            {
					entityPM.SeverityId = entityPOCO.SeverityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessHoursId))
            {
					entityPM.BusinessHoursId = entityPOCO.BusinessHoursId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseTime))
            {
					entityPM.FirstResponseTime = entityPOCO.FirstResponseTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseTimeUnit))
            {
					entityPM.FirstResponseTimeUnit = entityPOCO.FirstResponseTimeUnit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseTimeInMinute))
            {
					entityPM.FirstResponseTimeInMinute = entityPOCO.FirstResponseTimeInMinute;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResolveWithinTime))
            {
					entityPM.ResolveWithinTime = entityPOCO.ResolveWithinTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResolveWithinTimeUnit))
            {
					entityPM.ResolveWithinTimeUnit = entityPOCO.ResolveWithinTimeUnit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResolveWithinTimeInMinute))
            {
					entityPM.ResolveWithinTimeInMinute = entityPOCO.ResolveWithinTimeInMinute;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstResponseEscalate))
            {
					entityPM.FirstResponseEscalate = entityPOCO.FirstResponseEscalate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ResolveWithinEscalate))
            {
					entityPM.ResolveWithinEscalate = entityPOCO.ResolveWithinEscalate;
            }

		}

		public void PMToOldPM(SLALinePM entityPM, SLALinePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SeverityId))
            {
                oldEntityPM.SeverityId = entityPM.SeverityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessHoursId))
            {
                oldEntityPM.BusinessHoursId = entityPM.BusinessHoursId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTime))
            {
                oldEntityPM.FirstResponseTime = entityPM.FirstResponseTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTimeUnit))
            {
                oldEntityPM.FirstResponseTimeUnit = entityPM.FirstResponseTimeUnit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseTimeInMinute))
            {
                oldEntityPM.FirstResponseTimeInMinute = entityPM.FirstResponseTimeInMinute;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTime))
            {
                oldEntityPM.ResolveWithinTime = entityPM.ResolveWithinTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTimeUnit))
            {
                oldEntityPM.ResolveWithinTimeUnit = entityPM.ResolveWithinTimeUnit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinTimeInMinute))
            {
                oldEntityPM.ResolveWithinTimeInMinute = entityPM.ResolveWithinTimeInMinute;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstResponseEscalate))
            {
                oldEntityPM.FirstResponseEscalate = entityPM.FirstResponseEscalate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ResolveWithinEscalate))
            {
                oldEntityPM.ResolveWithinEscalate = entityPM.ResolveWithinEscalate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SLALinePM entityPM)
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
	 