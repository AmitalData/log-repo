
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
   
   public partial class WorkFlowInstanceActivityDataMapping: IMapping<WorkFlowInstanceActivityPM, WorkFlowInstanceActivity>,IMappingEncodeBase64NVARCHARFields<WorkFlowInstanceActivityPM>
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
	         Sequence, 
	         ActionName, 
	         StartTime, 
	         Duration, 
	         StatusCode, 
	         WorkflowInstanceId, 
	         EndTime, 
	         ErrorMessage, 
	         Result, 
	         ActionType,
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
	         Sequence, 
	         ActionName, 
	         StartTime, 
	         Duration, 
	         StatusCode, 
	         StatusName, 
	         WorkflowInstanceId, 
	         EndTime, 
	         ErrorMessage, 
	         Result,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WorkFlowInstanceActivityPM entityPM, WorkFlowInstanceActivity entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sequence))
            {
				entityPOCO.Sequence = entityPM.Sequence;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActionName))
            {
				entityPOCO.ActionName = entityPM.ActionName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartTime))
            {
				entityPOCO.StartTime = entityPM.StartTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
				entityPOCO.Duration = entityPM.Duration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowInstanceId))
            {
				entityPOCO.WorkflowInstanceId = entityPM.WorkflowInstanceId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndTime))
            {
				entityPOCO.EndTime = entityPM.EndTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
				entityPOCO.ErrorMessage = entityPM.ErrorMessage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Result))
            {
				entityPOCO.Result = entityPM.Result;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WorkFlowInstanceActivityPM entityPM, WorkFlowInstanceActivity entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Sequence))
            {
					entityPM.Sequence = entityPOCO.Sequence;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActionName))
            {
					entityPM.ActionName = entityPOCO.ActionName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartTime))
            {
					entityPM.StartTime = entityPOCO.StartTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Duration))
            {
					entityPM.Duration = entityPOCO.Duration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkflowInstanceId))
            {
					entityPM.WorkflowInstanceId = entityPOCO.WorkflowInstanceId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndTime))
            {
					entityPM.EndTime = entityPOCO.EndTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorMessage))
            {
					entityPM.ErrorMessage = entityPOCO.ErrorMessage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Result))
            {
					entityPM.Result = entityPOCO.Result;
            }

		}

		public void PMToOldPM(WorkFlowInstanceActivityPM entityPM, WorkFlowInstanceActivityPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sequence))
            {
                oldEntityPM.Sequence = entityPM.Sequence;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActionName))
            {
                oldEntityPM.ActionName = entityPM.ActionName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartTime))
            {
                oldEntityPM.StartTime = entityPM.StartTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Duration))
            {
                oldEntityPM.Duration = entityPM.Duration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowInstanceId))
            {
                oldEntityPM.WorkflowInstanceId = entityPM.WorkflowInstanceId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndTime))
            {
                oldEntityPM.EndTime = entityPM.EndTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
                oldEntityPM.ErrorMessage = entityPM.ErrorMessage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Result))
            {
                oldEntityPM.Result = entityPM.Result;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WorkFlowInstanceActivityPM entityPM)
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
		
		private void BuildSearchFieldsGenerated(WorkFlowInstanceActivityPM entityPM, WorkFlowInstanceActivity entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 