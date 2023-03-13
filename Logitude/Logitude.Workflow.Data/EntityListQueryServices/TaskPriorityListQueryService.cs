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

    public partial class TaskPriorityListQueryService
    {
	    private IQueryable<TaskPriorityList> GetIqueryableList(IQueryable<TaskPriority> iQueryable)
        {
		IQueryable<TaskPriorityList> query = (from a in iQueryable
                                            select new TaskPriorityList()
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
					
					                          DisplayOrder = a.DisplayOrder,

											  CreatedByUserName = a.CreatedByUser != null ? (a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null) : null,

											  UpdatedByUserName = a.UpdatedByUser != null ? (a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null) : null,
											  
											});
            return query;
		}

		private IQueryable<TaskPriority> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaskPriority> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<TaskPriority> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaskPriority> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	