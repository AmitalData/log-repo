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

    public partial class CurrencyTypeTenantListQueryService
    {
	    private IQueryable<CurrencyTypeTenantList> GetIqueryableList(IQueryable<CurrencyTypeTenant> iQueryable)
        {
		IQueryable<CurrencyTypeTenantList> query = (from a in iQueryable
                                            select new CurrencyTypeTenantList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
		                    	            });
            return query;
		}

		private IQueryable<CurrencyTypeTenant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CurrencyTypeTenant> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	