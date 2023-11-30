
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
   
   public partial class WorkFlowDataMapping: IMapping<WorkFlowPM, WorkFlow>,IMappingEncodeBase64NVARCHARFields<WorkFlowPM>
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
	         Name, 
	         Description, 
	         StatusCode, 
	         FlowJson, 
	         Entity, 
	         Trigger, 
	         RetriesNumber, 
	         RetriesDelay, 
	         WorkFlowTriggerTypeCode, 
	         WorkFlowNumber,
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
	         Name, 
	         Description, 
	         StatusCode, 
	         StatusName, 
	         CreatedByUserName, 
	         UpdatedByUserName, 
	         FlowJson, 
	         Entity, 
	         Trigger, 
	         RetriesNumber, 
	         RetriesDelay, 
	         WorkFlowTriggerTypeCode, 
	         WorkFlowTriggerTypeName, 
	         WorkFlowNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(WorkFlowPM entityPM, WorkFlow entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetriesNumber))
            {
				entityPOCO.RetriesNumber = entityPM.RetriesNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetriesDelay))
            {
				entityPOCO.RetriesDelay = entityPM.RetriesDelay;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowTriggerTypeCode))
            {
				entityPOCO.WorkFlowTriggerTypeCode = entityPM.WorkFlowTriggerTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowNumber))
            {
				entityPOCO.WorkFlowNumber = entityPM.WorkFlowNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(WorkFlowPM entityPM, WorkFlow entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RetriesNumber))
            {
					entityPM.RetriesNumber = entityPOCO.RetriesNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RetriesDelay))
            {
					entityPM.RetriesDelay = entityPOCO.RetriesDelay;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkFlowTriggerTypeCode))
            {
					entityPM.WorkFlowTriggerTypeCode = entityPOCO.WorkFlowTriggerTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkFlowNumber))
            {
					entityPM.WorkFlowNumber = entityPOCO.WorkFlowNumber;
            }

		}

		public void PMToOldPM(WorkFlowPM entityPM, WorkFlowPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetriesNumber))
            {
                oldEntityPM.RetriesNumber = entityPM.RetriesNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RetriesDelay))
            {
                oldEntityPM.RetriesDelay = entityPM.RetriesDelay;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowTriggerTypeCode))
            {
                oldEntityPM.WorkFlowTriggerTypeCode = entityPM.WorkFlowTriggerTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkFlowNumber))
            {
                oldEntityPM.WorkFlowNumber = entityPM.WorkFlowNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(WorkFlowPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FlowJson)) //T4 find type == nText 
            {
                entityPM.FlowJson = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FlowJson));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WorkFlowNumber)) //T4 find type == nText 
            {
                entityPM.WorkFlowNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WorkFlowNumber));
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
		
		private void BuildSearchFieldsGenerated(WorkFlowPM entityPM, WorkFlow entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 