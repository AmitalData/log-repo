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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsAirlineListQueryService
    {
	    private IQueryable<CustomsAirlineList> GetIqueryableList(IQueryable<CustomsAirline> iQueryable)
        {
		IQueryable<CustomsAirlineList> query = (from a in iQueryable
                                            select new CustomsAirlineList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          AirlineCode = a.AirlineCode,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
					                          InActive = a.InActive,
					
					                          SearchFields = a.SearchFields,
					
					                          AirlinePrefix = a.AirlinePrefix,
					
                                              ICAO = a.ICAO,
		                    	            });
            return query;
		}

		private IQueryable<CustomsAirline> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsAirline> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	