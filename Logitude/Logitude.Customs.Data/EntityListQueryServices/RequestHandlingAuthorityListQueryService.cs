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

    public partial class RequestHandlingAuthorityListQueryService
    {
	    private IQueryable<RequestHandlingAuthorityList> GetIqueryableList(IQueryable<RequestHandlingAuthority> iQueryable)
        {
		IQueryable<RequestHandlingAuthorityList> query = (from a in iQueryable
                                            select new RequestHandlingAuthorityList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<RequestHandlingAuthority> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<RequestHandlingAuthority> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	