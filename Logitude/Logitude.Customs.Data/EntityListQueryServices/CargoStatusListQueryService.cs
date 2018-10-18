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

    public partial class CargoStatusListQueryService
    {
	    private IQueryable<CargoStatusList> GetIqueryableList(IQueryable<CargoStatus> iQueryable)
        {
		IQueryable<CargoStatusList> query = (from a in iQueryable
                                            select new CargoStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoStatus> iQueryable)
        {
            return iQueryable;

        }
			}


}
	