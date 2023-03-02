
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
   
   public partial class WorkFlowVersionDataMapping: IMapping<WorkFlowVersionPM, WorkFlowVersion>,IMappingEncodeBase64NVARCHARFields<WorkFlowVersionPM>
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
	         VersionNumber, 
	         Description, 
	         StatusCode, 
	         WorkflowId, 
	         FlowJson, 
	         Entity, 
	         Trigger, 
	         ActivatedDate,
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
	         VersionNumber, 
	         Description, 
	         StatusName, 
	         StatusCode, 
	         WorkflowId, 
	         FlowJson, 
	         Entity, 
	         Trigger, 
	         ActivatedDate, 
	         CreatedByUserName, 
	         UpdatedByUserName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionNumber))
            {
				entityPOCO.VersionNumber = entityPM.VersionNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowId))
            {
				entityPOCO.WorkflowId = entityPM.WorkflowId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlowJson))
            {
				entityPOCO.FlowJson = entityPM.FlowJson;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Entity))
            {
				entityPOCO.Entity = entityPM.Entity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Trigger))
            {
				entityPOCO.Trigger = entityPM.Trigger;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivatedDate))
            {
				entityPOCO.ActivatedDate = entityPM.ActivatedDate;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VersionNumber))
            {
					entityPM.VersionNumber = entityPOCO.VersionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkflowId))
            {
					entityPM.WorkflowId = entityPOCO.WorkflowId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlowJson))
            {
					entityPM.FlowJson = entityPOCO.FlowJson;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Entity))
            {
					entityPM.Entity = entityPOCO.Entity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Trigger))
            {
					entityPM.Trigger = entityPOCO.Trigger;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivatedDate))
            {
					entityPM.ActivatedDate = entityPOCO.ActivatedDate;
            }

		}

		public void PMToOldPM(WorkFlowVersionPM entityPM, WorkFlowVersionPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionNumber))
            {
                oldEntityPM.VersionNumber = entityPM.VersionNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkflowId))
            {
                oldEntityPM.WorkflowId = entityPM.WorkflowId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlowJson))
            {
                oldEntityPM.FlowJson = entityPM.FlowJson;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Entity))
            {
                oldEntityPM.Entity = entityPM.Entity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Trigger))
            {
                oldEntityPM.Trigger = entityPM.Trigger;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivatedDate))
            {
                oldEntityPM.ActivatedDate = entityPM.ActivatedDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WorkFlowVersionPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FlowJson)) //T4 find type == nText 
            {
                entityPM.FlowJson = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FlowJson));
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
		
		private void BuildSearchFieldsGenerated(WorkFlowVersionPM entityPM, WorkFlowVersion entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 