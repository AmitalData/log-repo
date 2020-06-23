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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingPort2ListQueryService
    {
	    private IQueryable<CargoTrackingPort2List> GetIqueryableList(IQueryable<CargoTrackingPort2> iQueryable)
        {
		IQueryable<CargoTrackingPort2List> query = (from a in iQueryable
                                            select new CargoTrackingPort2List()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingPort2> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingPort2> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingPort2> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingPort2> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	