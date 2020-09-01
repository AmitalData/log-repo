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

    public partial class CargoTrackingCountryListQueryService
    {
	    private IQueryable<CargoTrackingCountryList> GetIqueryableList(IQueryable<CargoTrackingCountry> iQueryable)
        {
		IQueryable<CargoTrackingCountryList> query = (from a in iQueryable
                                            select new CargoTrackingCountryList()
											{
                     
					                          Id = a.Id,
					
					                          LocalName = a.LocalName,
					
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingCountry> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingCountry> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingCountry> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingCountry> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	