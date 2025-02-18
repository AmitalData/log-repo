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

    public partial class GovernmentProcTypeTenantListQueryService
    {
	    private IQueryable<GovernmentProcTypeTenantList> GetIqueryableList(IQueryable<GovernmentProcTypeTenant> iQueryable)
        {
		IQueryable<GovernmentProcTypeTenantList> query = (from a in iQueryable
                                            select new GovernmentProcTypeTenantList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          IsImport = a.IsImport,
					
					                          IndexOrder = a.IndexOrder,
					
					                          IsExport = a.IsExport,
					
		                    	            });
            return query;
		}

		private IQueryable<GovernmentProcTypeTenant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GovernmentProcTypeTenant> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	