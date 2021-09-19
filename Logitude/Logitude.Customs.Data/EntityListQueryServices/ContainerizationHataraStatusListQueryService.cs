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

    public partial class ContainerizationHataraStatusListQueryService
    {
	    private IQueryable<ContainerizationHataraStatusList> GetIqueryableList(IQueryable<ContainerizationHataraStatus> iQueryable)
        {
		IQueryable<ContainerizationHataraStatusList> query = (from a in iQueryable
                                            select new ContainerizationHataraStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ContainerizationHataraStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ContainerizationHataraStatus> iQueryable)
        {
			return iQueryable;
		}
			}


}
	