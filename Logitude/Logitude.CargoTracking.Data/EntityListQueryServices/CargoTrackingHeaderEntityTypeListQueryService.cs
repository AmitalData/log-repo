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

    public partial class CargoTrackingHeaderEntityTypeListQueryService
    {
	    private IQueryable<CargoTrackingHeaderEntityTypeList> GetIqueryableList(IQueryable<CargoTrackingHeaderEntityType> iQueryable)
        {
		IQueryable<CargoTrackingHeaderEntityTypeList> query = (from a in iQueryable
                                            select new CargoTrackingHeaderEntityTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingHeaderEntityType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingHeaderEntityType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingHeaderEntityType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingHeaderEntityType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	