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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class RevaluationStatusListQueryService
    {
	    private IQueryable<RevaluationStatusList> GetIqueryableList(IQueryable<RevaluationStatus> iQueryable)
        {
		IQueryable<RevaluationStatusList> query = (from a in iQueryable
                                            select new RevaluationStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<RevaluationStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<RevaluationStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<RevaluationStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<RevaluationStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	