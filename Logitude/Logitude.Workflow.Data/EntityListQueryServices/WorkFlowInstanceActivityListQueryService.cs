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

    public partial class WorkFlowInstanceActivityListQueryService
    {
	    private IQueryable<WorkFlowInstanceActivityList> GetIqueryableList(IQueryable<WorkFlowInstanceActivity> iQueryable)
        {
		IQueryable<WorkFlowInstanceActivityList> query = (from a in iQueryable
                                            select new WorkFlowInstanceActivityList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Sequence = a.Sequence,
					
					                          ActionName = a.ActionName,
					
					                          StartTime = a.StartTime,
					
					                          Duration = a.Duration,
					
					                          StatusCode = a.StatusCode,

											  StatusName = a.Status != null ? a.Status.Name : null,

											  ErrorMessage = a.ErrorMessage,

											  Result = a.Result
											});
            return query;
		}

		private IQueryable<WorkFlowInstanceActivity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceActivity> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<WorkFlowInstanceActivity> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceActivity> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	