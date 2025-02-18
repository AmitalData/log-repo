	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class CustomsHouseTypeTenantListQueryService
    {
	    private IQueryable<CustomsHouseTypeTenantList> GetIqueryableList(IQueryable<CustomsHouseTypeTenant> iQueryable)
        {
		IQueryable<CustomsHouseTypeTenantList> query = (from a in iQueryable
                                            select new CustomsHouseTypeTenantList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomsHouseTypeTenant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsHouseTypeTenant> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	