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

    public partial class GLAccountRecocileDataListQueryService
    {
	    private IQueryable<GLAccountRecocileDataList> GetIqueryableList(IQueryable<GLAccountRecocileData> iQueryable)
        {
		IQueryable<GLAccountRecocileDataList> query = (from a in iQueryable
                                            select new GLAccountRecocileDataList()
											{
                     
					                          AccountId = a.AccountId,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountRecocileData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountRecocileData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<GLAccountRecocileData> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountRecocileData> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	