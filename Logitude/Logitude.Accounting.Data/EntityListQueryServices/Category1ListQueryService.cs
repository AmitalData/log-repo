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

    public partial class Category1ListQueryService
    {
	    private IQueryable<Category1List> GetIqueryableList(IQueryable<Category1> iQueryable)
        {
		IQueryable<Category1List> query = (from a in iQueryable
                                            select new Category1List()
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

		private IQueryable<Category1> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Category1> iQueryable, int tenant)
        {
			//throw new NotImplementedException();
            return iQueryable;
		}
				private IQueryable<Category1> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Category1> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
    }


}
	