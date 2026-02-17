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

    public partial class MamanStatusListQueryService
    {
	    private IQueryable<MamanStatusList> GetIqueryableList(IQueryable<MamanStatus> iQueryable)
        {
		IQueryable<MamanStatusList> query = (from a in iQueryable
                                            select new MamanStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<MamanStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<MamanStatus> iQueryable)
        {
            return iQueryable;
		}
	}


}
	