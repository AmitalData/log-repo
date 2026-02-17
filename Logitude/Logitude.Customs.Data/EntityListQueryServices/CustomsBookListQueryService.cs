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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsBookListQueryService
    {
	    private IQueryable<CustomsBookList> GetIqueryableList(IQueryable<CustomsBook> iQueryable)
        {
		IQueryable<CustomsBookList> query = (from a in iQueryable
                                            select new CustomsBookList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          LastUpdateDate = a.LastUpdateDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomsBook> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsBook> iQueryable, int tenant)
        {
            //throw new NotImplementedException();
            //CustomsBookFilters filters = new CustomsBookFilters();
            //iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            return iQueryable;
        }
			}


}
	