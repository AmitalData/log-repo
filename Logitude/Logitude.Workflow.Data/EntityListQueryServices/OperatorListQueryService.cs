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

    public partial class OperatorListQueryService
    {
	    private IQueryable<OperatorList> GetIqueryableList(IQueryable<Operator> iQueryable)
        {
		IQueryable<OperatorList> query = (from a in iQueryable
                                            select new OperatorList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          Sign = a.Sign,
					
					                          CategoryCode = a.CategoryCode,
					
		                    	            });
            return query;
		}

		private IQueryable<Operator> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Operator> iQueryable)
        {
			return iQueryable;
		}
				private IQueryable<Operator> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Operator> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	