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
using System.Data.Entity;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingIncrementalStatListQueryService
    {
	    private IQueryable<CargoTrackingIncrementalStatList> GetIqueryableList(IQueryable<CargoTrackingIncrementalStat> iQueryable)
        {
		IQueryable<CargoTrackingIncrementalStatList> query = (from a in iQueryable
                                            select new CargoTrackingIncrementalStatList()
											{
                     
					                          Id = a.Id,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          Shipments = a.Shipments,
					
					                          Cards = a.Cards,
					
					                          Ports = a.Ports,
					
					                          Countries = a.Countries,
					
					                          TransportModes = a.TransportModes,

											  ErrorLog = a.ErrorLog
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingIncrementalStat> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingIncrementalStat> iQueryable)
        {
            List<QueryFilterItem> queryFilters = queryOperations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "Start_End_Date" && item.FieldValue!=null && item.FieldValue2!=null)
                    {
                        DateTime FromDate = Convert.ToDateTime(item.FieldValue);
                        DateTime ToDate = Convert.ToDateTime(item.FieldValue2);
 
                        if (FromDate != null && ToDate!=null)
                        {
                            iQueryable = iQueryable.Where(d => DbFunctions.TruncateTime(d.StartDate) >= DbFunctions.TruncateTime(FromDate) && DbFunctions.TruncateTime(d.EndDate) <= DbFunctions.TruncateTime(ToDate));
                        }
                    }
                }
            }

            return iQueryable;
        }
				private IQueryable<CargoTrackingIncrementalStat> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingIncrementalStat> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	