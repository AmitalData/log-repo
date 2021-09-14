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

    public partial class ContainerizationHatataStatusListQueryService
    {
	    private IQueryable<ContainerizationHatataStatusList> GetIqueryableList(IQueryable<ContainerizationHatataStatus> iQueryable)
        {
		IQueryable<ContainerizationHatataStatusList> query = (from a in iQueryable
                                            select new ContainerizationHatataStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ContainerizationHatataStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ContainerizationHatataStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	