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

    public partial class TaskExtendedListQueryService
    {
	    private IQueryable<TaskExtendedList> GetIqueryableList(IQueryable<TaskExtended> iQueryable)
        {
		IQueryable<TaskExtendedList> query = (from a in iQueryable
                                            select new TaskExtendedList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Fields = a.Fields,
					
					                          ToDoConditions = a.ToDoConditions,
					
					                          DoneConditions = a.DoneConditions,
					
					                          Description = a.Description,
					
		                    	            });
            return query;
		}

		private IQueryable<TaskExtended> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaskExtended> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<TaskExtended> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaskExtended> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	