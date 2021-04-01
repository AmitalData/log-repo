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

    public partial class ContainerTypeListQueryService
    {
	    private IQueryable<ContainerTypeList> GetIqueryableList(IQueryable<ContainerType> iQueryable)
        {
		IQueryable<ContainerTypeList> query = (from a in iQueryable
                                            select new ContainerTypeList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<ContainerType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ContainerType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	