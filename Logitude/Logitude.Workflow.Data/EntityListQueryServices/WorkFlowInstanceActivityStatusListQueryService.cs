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

    public partial class WorkFlowInstanceActivityStatusListQueryService
    {
	    private IQueryable<WorkFlowInstanceActivityStatusList> GetIqueryableList(IQueryable<WorkFlowInstanceActivityStatus> iQueryable)
        {
		IQueryable<WorkFlowInstanceActivityStatusList> query = (from a in iQueryable
                                            select new WorkFlowInstanceActivityStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<WorkFlowInstanceActivityStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceActivityStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WorkFlowInstanceActivityStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowInstanceActivityStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	