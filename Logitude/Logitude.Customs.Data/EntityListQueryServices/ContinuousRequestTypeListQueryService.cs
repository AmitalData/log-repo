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

    public partial class ContinuousRequestTypeListQueryService
    {
	    private IQueryable<ContinuousRequestTypeList> GetIqueryableList(IQueryable<ContinuousRequestType> iQueryable)
        {
		IQueryable<ContinuousRequestTypeList> query = (from a in iQueryable
                                            select new ContinuousRequestTypeList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<ContinuousRequestType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ContinuousRequestType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	