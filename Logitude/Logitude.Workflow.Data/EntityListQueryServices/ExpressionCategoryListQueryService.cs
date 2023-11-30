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

    public partial class ExpressionCategoryListQueryService
    {
	    private IQueryable<ExpressionCategoryList> GetIqueryableList(IQueryable<ExpressionCategory> iQueryable)
        {
		IQueryable<ExpressionCategoryList> query = (from a in iQueryable
                                            select new ExpressionCategoryList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ExpressionCategory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExpressionCategory> iQueryable)
        {
			return iQueryable;
		}
				private IQueryable<ExpressionCategory> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ExpressionCategory> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	