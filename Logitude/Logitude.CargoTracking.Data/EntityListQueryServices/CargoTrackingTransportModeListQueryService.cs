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
using Logitude.CargoTracking.Data.Repositories;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingTransportModeListQueryService
    {
	    private IQueryable<CargoTrackingTransportModeList> GetIqueryableList(IQueryable<CargoTrackingTransportMode> iQueryable)
        {
		IQueryable<CargoTrackingTransportModeList> query = (from a in iQueryable
                                            select new CargoTrackingTransportModeList()
											{
                     
					                          Id = a.Id,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingTransportMode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingTransportMode> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingTransportMode> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingTransportMode> iQueryable)
        {
			return iQueryable;
		}

        public List<CargoTrackingTransportMode> GetAllFromCache()
        {
			return CacheManager.GetOrInsertNewObject(
                "AllCargoTrackingTransportModes",
				() => new CargoTrackingTransportModeRepository(context).GetAll().ToList()
			);
        }
    }
}
	