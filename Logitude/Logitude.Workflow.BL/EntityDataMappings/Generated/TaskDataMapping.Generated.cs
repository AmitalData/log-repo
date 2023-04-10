
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.CustomFields;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class TaskDataMapping: IMapping<TaskPM, Task>,IMappingEncodeBase64NVARCHARFields<TaskPM>
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
	         Subject, 
	         DueDate, 
	         OwnerId, 
	         PriorityId, 
	         StatusId, 
	         EntityId, 
	         TaskTypeId, 
	         EntityObjectTableId, 
	         ClosedByUserId, 
	         ClosedDate, 
	         IsClosed, 
	         IsCancelled, 
	         CheckWithId, 
	         EntityNumber, 
	         IsAssigned, 
	         StartDate,
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
	         Subject, 
	         DueDate, 
	         OwnerId, 
	         PriorityId, 
	         StatusId, 
	         EntityId, 
	         TaskTypeId, 
	         EntityObjectTableId, 
	         ClosedByUserId, 
	         ClosedDate, 
	         IsClosed, 
	         IsCancelled, 
	         CheckWithId, 
	         EntityNumber, 
	         OwnerName, 
	         PriorityName, 
	         StatusName, 
	         TaskTypeName, 
	         EntityObjectTableName, 
	         ClosedByUserName, 
	         CheckWithName, 
	         CreatedByUserName, 
	         UpdatedByUserName, 
	         Fields, 
	         ToDoConditions, 
	         DoneConditions, 
	         Description, 
	         IsAssigned, 
	         StartDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TaskPM entityPM, Task entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
				entityPOCO.Subject = entityPM.Subject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
				entityPOCO.DueDate = entityPM.DueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriorityId))
            {
				entityPOCO.PriorityId = entityPM.PriorityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusId))
            {
				entityPOCO.StatusId = entityPM.StatusId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaskTypeId))
            {
				entityPOCO.TaskTypeId = entityPM.TaskTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityObjectTableId))
            {
				entityPOCO.EntityObjectTableId = entityPM.EntityObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosedByUserId))
            {
				entityPOCO.ClosedByUserId = entityPM.ClosedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosedDate))
            {
				entityPOCO.ClosedDate = entityPM.ClosedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckWithId))
            {
				entityPOCO.CheckWithId = entityPM.CheckWithId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityNumber))
            {
				entityPOCO.EntityNumber = entityPM.EntityNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAssigned))
            {
				entityPOCO.IsAssigned = entityPM.IsAssigned;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Task", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<TaskPM> { entityPM }.Cast<object>().ToList() }).Update();
		 
			new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableName = "Task", Tenant = entityPM.Tenant }).Update();
		 
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TaskPM entityPM, Task entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Subject))
            {
					entityPM.Subject = entityPOCO.Subject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DueDate))
            {
					entityPM.DueDate = entityPOCO.DueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PriorityId))
            {
					entityPM.PriorityId = entityPOCO.PriorityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusId))
            {
					entityPM.StatusId = entityPOCO.StatusId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaskTypeId))
            {
					entityPM.TaskTypeId = entityPOCO.TaskTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityObjectTableId))
            {
					entityPM.EntityObjectTableId = entityPOCO.EntityObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClosedByUserId))
            {
					entityPM.ClosedByUserId = entityPOCO.ClosedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClosedDate))
            {
					entityPM.ClosedDate = entityPOCO.ClosedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CheckWithId))
            {
					entityPM.CheckWithId = entityPOCO.CheckWithId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityNumber))
            {
					entityPM.EntityNumber = entityPOCO.EntityNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAssigned))
            {
					entityPM.IsAssigned = entityPOCO.IsAssigned;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Task", Tenant = entityPM.Tenant, Type = "PM", Entities = new List<TaskPM> { entityPM }.Cast<object>().ToList() }).Set();

		 
			new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableName = "Task", Tenant = entityPM.Tenant }).Set();
		 
		}

		public void PMToOldPM(TaskPM entityPM, TaskPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
                oldEntityPM.Subject = entityPM.Subject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DueDate))
            {
                oldEntityPM.DueDate = entityPM.DueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriorityId))
            {
                oldEntityPM.PriorityId = entityPM.PriorityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusId))
            {
                oldEntityPM.StatusId = entityPM.StatusId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaskTypeId))
            {
                oldEntityPM.TaskTypeId = entityPM.TaskTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityObjectTableId))
            {
                oldEntityPM.EntityObjectTableId = entityPM.EntityObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosedByUserId))
            {
                oldEntityPM.ClosedByUserId = entityPM.ClosedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClosedDate))
            {
                oldEntityPM.ClosedDate = entityPM.ClosedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CheckWithId))
            {
                oldEntityPM.CheckWithId = entityPM.CheckWithId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityNumber))
            {
                oldEntityPM.EntityNumber = entityPM.EntityNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAssigned))
            {
                oldEntityPM.IsAssigned = entityPM.IsAssigned;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TaskPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Subject)) //T4 find type == nText 
            {
                entityPM.Subject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Subject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.EntityNumber)) //T4 find type == nText 
            {
                entityPM.EntityNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.EntityNumber));
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
		
		private void BuildSearchFieldsGenerated(TaskPM entityPM, Task entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 