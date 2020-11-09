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

    public partial class LoadingSiteTypeListQueryService
    {
	    private IQueryable<LoadingSiteTypeList> GetIqueryableList(IQueryable<LoadingSiteType> iQueryable)
        {
		IQueryable<LoadingSiteTypeList> query = (from a in iQueryable
                                            select new LoadingSiteTypeList()
											{
                     
					                          Code = a.Code,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<LoadingSiteType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LoadingSiteType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	