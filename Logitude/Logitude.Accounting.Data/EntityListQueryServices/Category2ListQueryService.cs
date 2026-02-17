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

    public partial class Category2ListQueryService
    {
	    private IQueryable<Category2List> GetIqueryableList(IQueryable<Category2> iQueryable)
        {
		IQueryable<Category2List> query = (from a in iQueryable
                                            select new Category2List()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,

                                              SearchFields = a.SearchFields

					
		                    	            });
            return query;
		}

		private IQueryable<Category2> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Category2> iQueryable, int tenant)
        {
            return iQueryable;
			//throw new NotImplementedException();
		}
				private IQueryable<Category2> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Category2> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	