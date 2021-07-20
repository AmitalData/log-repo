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

    public partial class PoaAuthorizationTypeLookupListQueryService
    {
	    private IQueryable<PoaAuthorizationTypeLookupList> GetIqueryableList(IQueryable<PoaAuthorizationTypeLookup> iQueryable)
        {
		IQueryable<PoaAuthorizationTypeLookupList> query = (from a in iQueryable
                                            select new PoaAuthorizationTypeLookupList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
											  EnglishName=a.EnglishName,
											  Inactive=a.Inactive,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<PoaAuthorizationTypeLookup> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PoaAuthorizationTypeLookup> iQueryable)
        {
			return iQueryable;

		}
			}


}
	