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

    public partial class DepositListQueryService
    {
	    private IQueryable<DepositList> GetIqueryableList(IQueryable<Deposit> iQueryable)
        {
		IQueryable<DepositList> query = (from a in iQueryable
                                            select new DepositList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<Deposit> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Deposit> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<Deposit> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Deposit> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	