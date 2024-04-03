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

    public partial class TaskStatusListQueryService
    {
	    private IQueryable<TaskStatusList> GetIqueryableList(IQueryable<TaskStatus> iQueryable)
        {
		IQueryable<TaskStatusList> query = (from a in iQueryable
                                            select new TaskStatusList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          Closed = a.Closed,

											  CreatedByUserName = a.CreatedByUser != null ? (a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null) : null,

											  UpdatedByUserName = a.UpdatedByUser != null ? (a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null) : null,
											  
											});
            return query;
		}

		private IQueryable<TaskStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaskStatus> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<TaskStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaskStatus> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	