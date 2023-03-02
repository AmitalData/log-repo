
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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class WorkFlowInstanceDataMapping: IMapping<WorkFlowInstancePM, WorkFlowInstance>,IMappingEncodeBase64NVARCHARFields<WorkFlowInstancePM>
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
	         SearchFields, 
	         StatusCode, 
	         StartTime, 
	         EndTime, 
	         BusinessKey, 
	         Duration, 
	         WorkFlowVersionId, 
	         RetryAttemptsNumber, 
	         WorkflowId, 
	         NumberOfActivities,
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
	         SearchFields, 
	         StatusCode, 
	         StartTime, 
	         EndTime, 
	         BusinessKey, 
	         Duration, 
	         StatusName, 
	         WorkFlowVersionId, 
	         WorkFlowVersionNumber, 
	         RetryAttemptsNumber, 
	         WorkflowId, 
	         CreatedByUserName, 
	         UpdatedByUserName, 
	         NumberOfActivities,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartTime))
            {
				entityPOCO.StartTime = entityPM.StartTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndTime))
            {
				entityPOCO.EndTime = entityPM.EndTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessKey))
            {
				entityPOCO.BusinessKey = entityPM.BusinessKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
				entityPOCO.Duration = entityPM.Duration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowVersionId))
            {
				entityPOCO.WorkFlowVersionId = entityPM.WorkFlowVersionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetryAttemptsNumber))
            {
				entityPOCO.RetryAttemptsNumber = entityPM.RetryAttemptsNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowId))
            {
				entityPOCO.WorkflowId = entityPM.WorkflowId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfActivities))
            {
				entityPOCO.NumberOfActivities = entityPM.NumberOfActivities;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartTime))
            {
					entityPM.StartTime = entityPOCO.StartTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndTime))
            {
					entityPM.EndTime = entityPOCO.EndTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessKey))
            {
					entityPM.BusinessKey = entityPOCO.BusinessKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Duration))
            {
					entityPM.Duration = entityPOCO.Duration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkFlowVersionId))
            {
					entityPM.WorkFlowVersionId = entityPOCO.WorkFlowVersionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RetryAttemptsNumber))
            {
					entityPM.RetryAttemptsNumber = entityPOCO.RetryAttemptsNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkflowId))
            {
					entityPM.WorkflowId = entityPOCO.WorkflowId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfActivities))
            {
					entityPM.NumberOfActivities = entityPOCO.NumberOfActivities;
            }

		}

		public void PMToOldPM(WorkFlowInstancePM entityPM, WorkFlowInstancePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartTime))
            {
                oldEntityPM.StartTime = entityPM.StartTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndTime))
            {
                oldEntityPM.EndTime = entityPM.EndTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessKey))
            {
                oldEntityPM.BusinessKey = entityPM.BusinessKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
                oldEntityPM.Duration = entityPM.Duration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowVersionId))
            {
                oldEntityPM.WorkFlowVersionId = entityPM.WorkFlowVersionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetryAttemptsNumber))
            {
                oldEntityPM.RetryAttemptsNumber = entityPM.RetryAttemptsNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowId))
            {
                oldEntityPM.WorkflowId = entityPM.WorkflowId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfActivities))
            {
                oldEntityPM.NumberOfActivities = entityPM.NumberOfActivities;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WorkFlowInstancePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(WorkFlowInstancePM entityPM, WorkFlowInstance entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 