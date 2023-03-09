	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityLists;

namespace Logitude.Workflow.Data.EntityListQueryServices
{ 

    public partial class TaskListQueryService
    {
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

											  EntityObjectTableName = a.EntityObjectTable != null ? (a.EntityObjectTable.FullNameTextCode != null ? a.EntityObjectTable.FullNameTextCode.DefaultText : null) : null,

											  ClosedByUserName = a.ClosedByUser != null ? (a.ClosedByUser.Contact != null ? a.ClosedByUser.Contact.EnglishName : null) : null,

											  CheckWithName = a.CheckWith != null ? a.CheckWith.EnglishName : null,

											  Fields = a.TaskExtended != null ? a.TaskExtended.Fields : null,

											  ToDoConditions = a.TaskExtended != null ? a.TaskExtended.ToDoConditions : null,

											  DoneConditions = a.TaskExtended != null ? a.TaskExtended.DoneConditions : null,
											  
											});
            return query;
		}

		private IQueryable<Task> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Task> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<Task> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Task> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	