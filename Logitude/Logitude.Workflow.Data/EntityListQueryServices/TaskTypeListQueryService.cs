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
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityLists;

namespace Logitude.Workflow.Data.EntityListQueryServices
{ 

    public partial class TaskTypeListQueryService
    {
	    private IQueryable<TaskTypeList> GetIqueryableList(IQueryable<TaskType> iQueryable)
        {
		IQueryable<TaskTypeList> query = (from a in iQueryable
                                            select new TaskTypeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          Description = a.Description,
					
					                          EntityObjectTableId = a.EntityObjectTableId,

											  EntityObjectTableName = a.EntityObjectTable != null ? (a.EntityObjectTable.FullNameTextCode != null ? a.EntityObjectTable.FullNameTextCode.DefaultText : a.EntityObjectTable.Name) : null,

											  CreatedByUserName = a.CreatedByUser != null ? (a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null) : null,

											  UpdatedByUserName = a.UpdatedByUser != null ? (a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null) : null,
											  
											});
            return query;
		}

		private IQueryable<TaskType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaskType> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<TaskType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaskType> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	