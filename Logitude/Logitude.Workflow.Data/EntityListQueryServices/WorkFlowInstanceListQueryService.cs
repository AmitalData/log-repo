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

    public partial class WorkFlowInstanceListQueryService
    {
	    private IQueryable<WorkFlowInstanceList> GetIqueryableList(IQueryable<WorkFlowInstance> iQueryable)
        {
		IQueryable<WorkFlowInstanceList> query = (from a in iQueryable
                                            select new WorkFlowInstanceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          WorkflowId = a.WorkflowId,
					
		                    	            });
            return query;
		}

		private IQueryable<WorkFlowInstance> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstance> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WorkFlowInstance> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstance> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	