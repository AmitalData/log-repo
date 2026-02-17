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

    public partial class CollateralRequestStatusListQueryService
    {
	    private IQueryable<CollateralRequestStatusList> GetIqueryableList(IQueryable<CollateralRequestStatus> iQueryable)
        {
            IQueryable<CollateralRequestStatusList> query = (from a in iQueryable
                                                             select new CollateralRequestStatusList()
                                                    {
                                                        Code = a.Code,
                                                        EnglishName = a.EnglishName,
                                                        LocalName = a.LocalName,
                                                        SearchFields = a.SearchFields,
                                                        Inactive = a.Inactive

                                                    });
            return query;
		}

		private IQueryable<CollateralRequestStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CollateralRequestStatus> iQueryable)
        {
            return iQueryable;
		}
	}


}
	