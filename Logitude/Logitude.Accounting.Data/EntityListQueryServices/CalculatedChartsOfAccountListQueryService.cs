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

    public partial class CalculatedChartsOfAccountListQueryService
    {
	    private IQueryable<CalculatedChartsOfAccountList> GetIqueryableList(IQueryable<CalculatedChartsOfAccount> iQueryable)
        {
		IQueryable<CalculatedChartsOfAccountList> query = (from a in iQueryable
                                            select new CalculatedChartsOfAccountList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDateTime = a.CreateDateTime,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedDateTime = a.UpdatedDateTime,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
		                    	            });
            return query;
		}

		private IQueryable<CalculatedChartsOfAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccount> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CalculatedChartsOfAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccount> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	