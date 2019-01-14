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

    public partial class IntegrityCheckStatusListQueryService
    {
	    private IQueryable<IntegrityCheckStatusList> GetIqueryableList(IQueryable<IntegrityCheckStatus> iQueryable)
        {
		IQueryable<IntegrityCheckStatusList> query = (from a in iQueryable
                                            select new IntegrityCheckStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<IntegrityCheckStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<IntegrityCheckStatus> iQueryable)
        {
			return iQueryable;
            //throw new NotImplementedException();
        }
        private IQueryable<IntegrityCheckStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<IntegrityCheckStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	