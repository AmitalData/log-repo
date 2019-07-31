
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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.EntityDataMappings
{
   
   public partial class TMEmployeeTimeDataMapping: IMapping<TMEmployeeTimePM, TMEmployeeTime>,IMappingEncodeBase64NVARCHARFields<TMEmployeeTimePM>
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
	         EmployeeUserId, 
	         DateOfWork, 
	         Description, 
	         TimeInMinutes, 
	         WINumber, 
	         ProjectId, 
	         LocationCode, 
	         AnalyzeQueueId, 
	         SprintId, 
	         ProratedDuration, 
	         FullDuration, 
	         NeedsProrating,
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
	         EmployeeUserId, 
	         DateOfWork, 
	         Description, 
	         TimeInMinutes, 
	         WINumber, 
	         ProjectId, 
	         LocationCode, 
	         ProjectName, 
	         ProjectDescription, 
	         AnalyzeQueueId, 
	         ProjectId_db, 
	         Description_db, 
	         WINumber_db, 
	         TimeInMinutes_db, 
	         SprintId, 
	         ProratedDuration, 
	         FullDuration, 
	         NeedsProrating, 
	         LocationName, 
	         SprintName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeUserId))
            {
				entityPOCO.EmployeeUserId = entityPM.EmployeeUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateOfWork))
            {
				entityPOCO.DateOfWork = entityPM.DateOfWork;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TimeInMinutes))
            {
				entityPOCO.TimeInMinutes = entityPM.TimeInMinutes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WINumber))
            {
				entityPOCO.WINumber = entityPM.WINumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectId))
            {
				entityPOCO.ProjectId = entityPM.ProjectId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocationCode))
            {
				entityPOCO.LocationCode = entityPM.LocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyzeQueueId))
            {
				entityPOCO.AnalyzeQueueId = entityPM.AnalyzeQueueId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SprintId))
            {
				entityPOCO.SprintId = entityPM.SprintId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProratedDuration))
            {
				entityPOCO.ProratedDuration = entityPM.ProratedDuration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullDuration))
            {
				entityPOCO.FullDuration = entityPM.FullDuration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedsProrating))
            {
				entityPOCO.NeedsProrating = entityPM.NeedsProrating;
			}
			}

		public void POCOToPM(TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmployeeUserId))
            {
					entityPM.EmployeeUserId = entityPOCO.EmployeeUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DateOfWork))
            {
					entityPM.DateOfWork = entityPOCO.DateOfWork;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TimeInMinutes))
            {
					entityPM.TimeInMinutes = entityPOCO.TimeInMinutes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WINumber))
            {
					entityPM.WINumber = entityPOCO.WINumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProjectId))
            {
					entityPM.ProjectId = entityPOCO.ProjectId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocationCode))
            {
					entityPM.LocationCode = entityPOCO.LocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnalyzeQueueId))
            {
					entityPM.AnalyzeQueueId = entityPOCO.AnalyzeQueueId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SprintId))
            {
					entityPM.SprintId = entityPOCO.SprintId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProratedDuration))
            {
					entityPM.ProratedDuration = entityPOCO.ProratedDuration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullDuration))
            {
					entityPM.FullDuration = entityPOCO.FullDuration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NeedsProrating))
            {
					entityPM.NeedsProrating = entityPOCO.NeedsProrating;
            }

		}

		public void PMToOldPM(TMEmployeeTimePM entityPM, TMEmployeeTimePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeUserId))
            {
                oldEntityPM.EmployeeUserId = entityPM.EmployeeUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateOfWork))
            {
                oldEntityPM.DateOfWork = entityPM.DateOfWork;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TimeInMinutes))
            {
                oldEntityPM.TimeInMinutes = entityPM.TimeInMinutes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WINumber))
            {
                oldEntityPM.WINumber = entityPM.WINumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectId))
            {
                oldEntityPM.ProjectId = entityPM.ProjectId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocationCode))
            {
                oldEntityPM.LocationCode = entityPM.LocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnalyzeQueueId))
            {
                oldEntityPM.AnalyzeQueueId = entityPM.AnalyzeQueueId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SprintId))
            {
                oldEntityPM.SprintId = entityPM.SprintId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProratedDuration))
            {
                oldEntityPM.ProratedDuration = entityPM.ProratedDuration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullDuration))
            {
                oldEntityPM.FullDuration = entityPM.FullDuration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedsProrating))
            {
                oldEntityPM.NeedsProrating = entityPM.NeedsProrating;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TMEmployeeTimePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
	 