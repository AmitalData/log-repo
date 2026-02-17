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
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsHouseTypeListQueryService
    {
       public int Tenant;
	    private IQueryable<CustomsHouseTypeList> GetIqueryableList(IQueryable<CustomsHouseType> iQueryable)
        {
            //CustomsHouseTypeAdditionalRepository additionalRepository = new CustomsHouseTypeAdditionalRepository(context);
            //IQueryable<CustomsHouseTypeAdditional> additionals = additionalRepository.GetAll(Tenant);
            //IQueryable<CustomsHouseTypeList> query = (from a in iQueryable

            //                                          join d in additionals.Include("CustomsTransportMode").Include("UnloadingSiteType")
            //                                          on a.Code equals d.Code into xy
            //                                          from s in xy.DefaultIfEmpty()
            //                                          select new CustomsHouseTypeList()
            //                                          {
            //                                              Code = a.Code,
            //                                              EnglishName = a.EnglishName,
            //                                              LocalName = a.LocalName,
            //                                              SearchFields = a.Code + "," + a.LocalName,
            //                                              Inactive = a.Inactive,
            //                                              UnloadPortCode = s.UnloadPortCode,
            //                                              UnloadPortName = s.UnloadingSiteType != null ? s.UnloadingSiteType.LocalName : null,
            //                                              TransportModeId = s.TransportModeId,
            //                                              TransportModeName = s.CustomsTransportMode != null? s.CustomsTransportMode.LocalName : null,

            //                                          });

            IQueryable<CustomsHouseTypeList> query = (from a in iQueryable

                                                      join d in context.CustomsHouseTypeAdditionals.Include("CustomsTransportMode").Include("UnloadingSiteType")
                                                      on a.Code equals d.Code 
                                                      select new CustomsHouseTypeList()
                                                      {
                                                          Code = a.Code,
                                                          EnglishName = a.EnglishName,
                                                          LocalName = a.LocalName,
                                                          SearchFields = a.Code + "," + a.LocalName,
                                                          Inactive = a.Inactive,
                                                          UnloadPortCode = d.UnloadPortCode,
                                                          UnloadPortName = d.UnloadingSiteType != null ? d.UnloadingSiteType.LocalName : null,
                                                          TransportModeId = d.TransportModeId,
                                                          TransportModeName = d.CustomsTransportMode != null ? d.CustomsTransportMode.LocalName : null,
                                                      });
            return query;
		}

		private IQueryable<CustomsHouseType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsHouseType> iQueryable)
        {
            return iQueryable;
		}
	}


}
	