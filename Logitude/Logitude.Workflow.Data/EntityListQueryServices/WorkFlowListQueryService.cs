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

    public partial class WorkFlowListQueryService
    {
	    private IQueryable<WorkFlowList> GetIqueryableList(IQueryable<WorkFlow> iQueryable)
        {
		IQueryable<WorkFlowList> query = (from a in iQueryable
                                            select new WorkFlowList()
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

											  StatusCode = a.StatusCode,

											  StatusName = a.Status != null ? a.Status.Name : null,

											  CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null : null,

											  UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null : null,

											  RetriesDelay = a.RetriesDelay,
											  RetriesNumber = a.RetriesNumber,
											  Entity = a.Entity,
											  Trigger = a.Trigger,
											  WorkFlowTriggerTypeCode = a.WorkFlowTriggerTypeCode,
											  WorkFlowTriggerTypeName = a.WorkFlowTriggerType.Name,
											  WorkFlowNumber = a.WorkFlowNumber

											});
            return query;
		}

		private IQueryable<WorkFlow> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlow> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<WorkFlow> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlow> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	