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

    public partial class CustomsHouseTypeAdditionalListQueryService
    {
	    private IQueryable<CustomsHouseTypeAdditionalList> GetIqueryableList(IQueryable<CustomsHouseTypeAdditional> iQueryable)
        {
            IQueryable<CustomsHouseTypeAdditionalList> query = (from a in iQueryable.Include("CustomsHouseType").Include("TransportMode").Include("UnloadingSiteType")
                                                                select new CustomsHouseTypeAdditionalList()
                                                    {
                                                        Code = a.Code,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                        Name = a.CustomsHouseType != null ? a.CustomsHouseType.LocalName : null,
                                                       TransportModeId = a.TransportModeId,
                                                       TransportModeName = a.TransportMode != null? a.TransportMode.LocalName: null,
                                                       UnloadPortCode = a.UnloadPortCode,
                                                       UnloadPortName = a.UnloadingSiteType != null? a.UnloadingSiteType.LocalName: null,
                                                        SearchFields = a.SearchFields,

                                                    });
            return query;
		}

        private IQueryable<CustomsHouseTypeAdditional> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsHouseTypeAdditional> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	