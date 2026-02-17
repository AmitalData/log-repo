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

using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityLists;

namespace Logitude.TimeManagement.Data.EntityListQueryServices
{ 

    public partial class TMLocationListQueryService
    {
	    private IQueryable<TMLocationList> GetIqueryableList(IQueryable<TMLocation> iQueryable)
        {
		IQueryable<TMLocationList> query = (from a in iQueryable
                                            select new TMLocationList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<TMLocation> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMLocation> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<TMLocation> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMLocation> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	