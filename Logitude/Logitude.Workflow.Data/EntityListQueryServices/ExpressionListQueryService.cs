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

    public partial class ExpressionListQueryService
    {
	    private IQueryable<ExpressionList> GetIqueryableList(IQueryable<Expression> iQueryable)
        {
		IQueryable<ExpressionList> query = (from a in iQueryable
                                            select new ExpressionList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          Body = a.Body,
					
					                          Description = a.Description,
					
					                          CategoryCode = a.CategoryCode,
											  Title = a.Title
					
		                    	            });
            return query;
		}

		private IQueryable<Expression> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Expression> iQueryable)
        {
			return iQueryable;
		}
				private IQueryable<Expression> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Expression> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	