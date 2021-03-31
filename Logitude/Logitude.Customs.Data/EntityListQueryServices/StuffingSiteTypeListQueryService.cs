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

    public partial class StuffingSiteTypeListQueryService
    {
	    private IQueryable<StuffingSiteTypeList> GetIqueryableList(IQueryable<StuffingSiteType> iQueryable)
        {
		IQueryable<StuffingSiteTypeList> query = (from a in iQueryable
                                            select new StuffingSiteTypeList()
											{
												Code=a.Code,
												LocalName=a.LocalName,
												EnglishName=a.EnglishName,
					                          SearchFields = a.SearchFields,
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<StuffingSiteType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<StuffingSiteType> iQueryable)
        {
			return iQueryable;
		}
			
	}


}
	