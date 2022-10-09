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

    public partial class WorkFlowInstanceVariableListQueryService
    {
	    private IQueryable<WorkFlowInstanceVariableList> GetIqueryableList(IQueryable<WorkFlowInstanceVariable> iQueryable)
        {
		IQueryable<WorkFlowInstanceVariableList> query = (from a in iQueryable
                                            select new WorkFlowInstanceVariableList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
											  Name = a.Name,
											  Type = a.Type , 
											  Value = a.Value,
											  WorkflowInstanceId = a.WorkflowInstanceId
		                    	            });
            return query;
		}

		private IQueryable<WorkFlowInstanceVariable> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceVariable> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<WorkFlowInstanceVariable> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceVariable> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	