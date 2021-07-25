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

    public partial class BuyerRoleTypeListQueryService
    {
	    private IQueryable<BuyerRoleTypeList> GetIqueryableList(IQueryable<BuyerRoleType> iQueryable)
        {
		IQueryable<BuyerRoleTypeList> query = (from a in iQueryable
                                            select new BuyerRoleTypeList()
											{
                     
					                          SearchFields = a.SearchFields,
					                          Inactive = a.Inactive,
											  EnglishName=a.EnglishName,
											  LocalName=a.LocalName,
											  Code=a.Code
											  
					
		                    	            });
            return query;
		}

		private IQueryable<BuyerRoleType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BuyerRoleType> iQueryable)
        {
			return iQueryable;
		}
	}


}
	