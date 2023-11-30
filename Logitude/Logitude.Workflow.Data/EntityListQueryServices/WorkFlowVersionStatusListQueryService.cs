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

    public partial class WorkFlowVersionStatusListQueryService
    {
	    private IQueryable<WorkFlowVersionStatusList> GetIqueryableList(IQueryable<WorkFlowVersionStatus> iQueryable)
        {
		IQueryable<WorkFlowVersionStatusList> query = (from a in iQueryable
                                            select new WorkFlowVersionStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<WorkFlowVersionStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowVersionStatus> iQueryable)
        {
			return iQueryable;
		}
				private IQueryable<WorkFlowVersionStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowVersionStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	