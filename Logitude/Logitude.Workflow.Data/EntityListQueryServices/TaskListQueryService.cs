using Logitude.Workflow.Data.CustomFilters;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;

namespace Logitude.Workflow.Data.EntityListQueryServices
{

    public partial class TaskListQueryService
    {
		public IQueryable<TaskList> GetIqueryableList(int tenant)
        {

			IQueryable<Task> iQueryable = (from a in context.Tasks
										   where a.Tenant == tenant
										   select a);

			return GetIqueryableList(iQueryable);
		}

		private IQueryable<TaskList> GetIqueryableList(IQueryable<Task> iQueryable)
        {
		IQueryable<TaskList> query = (from a in iQueryable
                                            select new TaskList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Subject = a.Subject,
					
					                          DueDate = a.DueDate,
					
					                          OwnerId = a.OwnerId,
					
					                          PriorityId = a.PriorityId,
					
					                          StatusId = a.StatusId,
					
					                          EntityId = a.EntityId,
					
					                          TaskTypeId = a.TaskTypeId,
					
					                          EntityObjectTableId = a.EntityObjectTableId,
					
					                          ClosedByUserId = a.ClosedByUserId,
					
					                          ClosedDate = a.ClosedDate,
					
					                          IsClosed = a.IsClosed,
					
					                          IsCancelled = a.IsCancelled,
					
					                          CheckWithId = a.CheckWithId,
					
					                          EntityNumber = a.EntityNumber,

											  CreatedByUserName = a.CreatedByUser != null ? (a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null) : null,

											  UpdatedByUserName = a.UpdatedByUser != null ? (a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null) : null,

											  OwnerName = a.Owner != null ? (a.Owner.Contact != null ? a.Owner.Contact.EnglishName : null) : null,

											  PriorityName = a.Priority != null ? a.Priority.Name : null,

											  StatusName = a.Status != null ? a.Status.Name : null,

											  TaskTypeName = a.TaskType != null ? a.TaskType.Name : null,

											  EntityObjectTableName = a.EntityObjectTable != null ? (a.EntityObjectTable.FullNameTextCode != null ? a.EntityObjectTable.FullNameTextCode.DefaultText : a.EntityObjectTable.Name) : null,

											  ClosedByUserName = a.ClosedByUser != null ? (a.ClosedByUser.Contact != null ? a.ClosedByUser.Contact.EnglishName : null) : null,

											  CheckWithName = a.CheckWith != null ? a.CheckWith.EnglishName : null,

											  Fields = a.TaskExtended != null ? a.TaskExtended.Fields : null,

											  ToDoConditions = a.TaskExtended != null ? a.TaskExtended.ToDoConditions : null,

											  DoneConditions = a.TaskExtended != null ? a.TaskExtended.DoneConditions : null,

											  Description = a.TaskExtended != null ? a.TaskExtended.Description : null,
											  
											});
            return query;
		}

		private IQueryable<Task> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Task> iQueryable, int tenant)
        {
			return TaskCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
		}
				private IQueryable<Task> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Task> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	