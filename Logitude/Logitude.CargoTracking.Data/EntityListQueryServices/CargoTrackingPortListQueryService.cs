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

    public partial class CargoTrackingPortListQueryService
    {
	    private IQueryable<CargoTrackingPortList> GetIqueryableList(IQueryable<CargoTrackingPort> iQueryable)
        {
		IQueryable<CargoTrackingPortList> query = (from a in iQueryable
                                            select new CargoTrackingPortList()
											{
                     
					                          Id = a.Id,
					
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          CountryId = a.CountryId,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingPort> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingPort> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingPort> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingPort> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	