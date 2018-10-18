using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CourtInstancePM GetSingleCourtInstancePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            courtInstanceQuery = new CourtInstanceQueryService(customContext);
            CourtInstancePM CourtInstance = courtInstanceQuery.GetSingle(code, false, false);
            return CourtInstance;
        }

        public CourtInstanceList GetSingleCourtInstanceList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CourtInstance", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CourtInstanceList> GetCourtInstanceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CourtInstanceList> GetCourtInstanceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CourtInstanceListQueryService listService = new CourtInstanceListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCourtInstanceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CourtInstanceListQueryService queryService = new CourtInstanceListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}