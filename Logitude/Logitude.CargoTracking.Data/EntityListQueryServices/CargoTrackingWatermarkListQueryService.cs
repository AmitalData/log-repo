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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingWatermarkListQueryService
    {
	    private IQueryable<CargoTrackingWatermarkList> GetIqueryableList(IQueryable<CargoTrackingWatermark> iQueryable)
        {
		IQueryable<CargoTrackingWatermarkList> query = (from a in iQueryable
                                            select new CargoTrackingWatermarkList()
											{
                     
					                          TableName = a.TableName,
					
					                          LastUpdateDate = a.LastUpdateDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingWatermark> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingWatermark> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingWatermark> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingWatermark> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	