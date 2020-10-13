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

    public partial class CalculatedChartsOfAccountsLineListQueryService
    {
	    private IQueryable<CalculatedChartsOfAccountsLineList> GetIqueryableList(IQueryable<CalculatedChartsOfAccountsLine> iQueryable)
        {
		IQueryable<CalculatedChartsOfAccountsLineList> query = (from a in iQueryable
                                            select new CalculatedChartsOfAccountsLineList()
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

		private IQueryable<CalculatedChartsOfAccountsLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccountsLine> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CalculatedChartsOfAccountsLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccountsLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	