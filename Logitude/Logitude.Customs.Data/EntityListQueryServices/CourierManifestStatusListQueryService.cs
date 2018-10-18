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

    public partial class CourierManifestStatusListQueryService
    {
	    private IQueryable<CourierManifestStatusList> GetIqueryableList(IQueryable<CourierManifestStatus> iQueryable)
        {
		IQueryable<CourierManifestStatusList> query = (from a in iQueryable
                                            select new CourierManifestStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<CourierManifestStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CourierManifestStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	