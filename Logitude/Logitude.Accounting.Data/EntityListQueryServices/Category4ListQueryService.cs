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

    public partial class Category4ListQueryService
    {
	    private IQueryable<Category4List> GetIqueryableList(IQueryable<Category4> iQueryable)
        {
		IQueryable<Category4List> query = (from a in iQueryable
                                            select new Category4List()
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

		private IQueryable<Category4> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Category4> iQueryable, int tenant)
        {
            return iQueryable;
            //throw new NotImplementedException();
        }
				private IQueryable<Category4> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Category4> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	