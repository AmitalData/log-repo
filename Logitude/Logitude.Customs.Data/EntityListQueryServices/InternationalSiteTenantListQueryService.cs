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

    public partial class InternationalSiteTenantListQueryService
    {
	    private IQueryable<InternationalSiteTenantList> GetIqueryableList(IQueryable<InternationalSiteTenant> iQueryable)
        {
		IQueryable<InternationalSiteTenantList> query = (from a in iQueryable
                                            select new InternationalSiteTenantList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
					                          CountryTypeCode = a.CountryTypeCode,
					
		                    	            });
            return query;
		}

		private IQueryable<InternationalSiteTenant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InternationalSiteTenant> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	