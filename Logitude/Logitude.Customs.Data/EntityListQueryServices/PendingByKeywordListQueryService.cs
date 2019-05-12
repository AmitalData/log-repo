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

    public partial class PendingByKeywordListQueryService
    {
	    private IQueryable<PendingByKeywordList> GetIqueryableList(IQueryable<PendingByKeyword> iQueryable)
        {
		IQueryable<PendingByKeywordList> query = (from a in iQueryable
                                            select new PendingByKeywordList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<PendingByKeyword> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PendingByKeyword> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	