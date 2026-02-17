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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class SharedLogisticsSettingListQueryService
    {
	    private IQueryable<SharedLogisticsSettingList> GetIqueryableList(IQueryable<SharedLogisticsSetting> iQueryable)
        {
		IQueryable<SharedLogisticsSettingList> query = (from a in iQueryable
                                            select new SharedLogisticsSettingList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<SharedLogisticsSetting> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SharedLogisticsSetting> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<SharedLogisticsSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<SharedLogisticsSetting> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	