using Logitude.Workflow.Data.CustomFilters;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
			string objcetTableId = new ObjectTableRepository(0).GetObjectTableIdByName("Task");

			IQueryable<TaskList> query = (from a in iQueryable
										  join customFieldsMainObject in context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
										  from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
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

											  IsAssigned = a.IsAssigned,

											  StartDate = a.StartDate,

											  Field1 = customFieldsMainObject != null ? customFieldsMainObject.Field1 : null,
											  Field2 = customFieldsMainObject != null ? customFieldsMainObject.Field2 : null,
											  Field3 = customFieldsMainObject != null ? customFieldsMainObject.Field3 : null,
											  Field4 = customFieldsMainObject != null ? customFieldsMainObject.Field4 : null,
											  Field5 = customFieldsMainObject != null ? customFieldsMainObject.Field5 : null,
											  Field6 = customFieldsMainObject != null ? customFieldsMainObject.Field6 : null,
											  Field7 = customFieldsMainObject != null ? customFieldsMainObject.Field7 : null,
											  Field8 = customFieldsMainObject != null ? customFieldsMainObject.Field8 : null,
											  Field9 = customFieldsMainObject != null ? customFieldsMainObject.Field9 : null,
											  Field10 = customFieldsMainObject != null ? customFieldsMainObject.Field10 : null,
											  Field11 = customFieldsMainObject != null ? customFieldsMainObject.Field11 : null,
											  Field12 = customFieldsMainObject != null ? customFieldsMainObject.Field12 : null,
											  Field13 = customFieldsMainObject != null ? customFieldsMainObject.Field13 : null,
											  Field14 = customFieldsMainObject != null ? customFieldsMainObject.Field14 : null,
											  Field15 = customFieldsMainObject != null ? customFieldsMainObject.Field15 : null,
											  Field16 = customFieldsMainObject != null ? customFieldsMainObject.Field16 : null,
											  Field17 = customFieldsMainObject != null ? customFieldsMainObject.Field17 : null,
											  Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
											  Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
											  Field20 = customFieldsMainObject != null ? customFieldsMainObject.Field20 : null,
											  Field21 = customFieldsMainObject != null ? customFieldsMainObject.Field21 : null,
											  Field22 = customFieldsMainObject != null ? customFieldsMainObject.Field22 : null,
											  Field23 = customFieldsMainObject != null ? customFieldsMainObject.Field23 : null,
											  Field24 = customFieldsMainObject != null ? customFieldsMainObject.Field24 : null,
											  Field25 = customFieldsMainObject != null ? customFieldsMainObject.Field25 : null,
											  Field26 = customFieldsMainObject != null ? customFieldsMainObject.Field26 : null,
											  Field27 = customFieldsMainObject != null ? customFieldsMainObject.Field27 : null,
											  Field28 = customFieldsMainObject != null ? customFieldsMainObject.Field28 : null,
											  Field29 = customFieldsMainObject != null ? customFieldsMainObject.Field29 : null,
											  Field30 = customFieldsMainObject != null ? customFieldsMainObject.Field30 : null,
											  Field31 = customFieldsMainObject != null ? customFieldsMainObject.Field31 : null,
											  Field32 = customFieldsMainObject != null ? customFieldsMainObject.Field32 : null,
											  Field33 = customFieldsMainObject != null ? customFieldsMainObject.Field33 : null,
											  Field34 = customFieldsMainObject != null ? customFieldsMainObject.Field34 : null,
											  Field35 = customFieldsMainObject != null ? customFieldsMainObject.Field35 : null,
											  Field36 = customFieldsMainObject != null ? customFieldsMainObject.Field36 : null,
											  Field37 = customFieldsMainObject != null ? customFieldsMainObject.Field37 : null,
											  Field38 = customFieldsMainObject != null ? customFieldsMainObject.Field38 : null,
											  Field39 = customFieldsMainObject != null ? customFieldsMainObject.Field39 : null,
											  Field40 = customFieldsMainObject != null ? customFieldsMainObject.Field40 : null,
											  Field41 = customFieldsMainObject != null ? customFieldsMainObject.Field41 : null,
											  Field42 = customFieldsMainObject != null ? customFieldsMainObject.Field42 : null,
											  Field43 = customFieldsMainObject != null ? customFieldsMainObject.Field43 : null,
											  Field44 = customFieldsMainObject != null ? customFieldsMainObject.Field44 : null,
											  Field45 = customFieldsMainObject != null ? customFieldsMainObject.Field45 : null,
											  Field46 = customFieldsMainObject != null ? customFieldsMainObject.Field46 : null,
											  Field47 = customFieldsMainObject != null ? customFieldsMainObject.Field47 : null,
											  Field48 = customFieldsMainObject != null ? customFieldsMainObject.Field48 : null,
											  Field49 = customFieldsMainObject != null ? customFieldsMainObject.Field49 : null,
											  Field50 = customFieldsMainObject != null ? customFieldsMainObject.Field50 : null,
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
	