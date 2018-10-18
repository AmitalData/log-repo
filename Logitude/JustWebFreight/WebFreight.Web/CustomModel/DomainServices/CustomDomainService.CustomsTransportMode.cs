using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsTransportModePM GetSingleCustomsTransportModePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsTransportModeQuery = new CustomsTransportModeQueryService(customContext);
            CustomsTransportModePM CustomsTransportMode = customsTransportModeQuery.GetSingle(code, false, false);
            return CustomsTransportMode;
        }

        public CustomsTransportModeList GetSingleCustomsTransportModeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsTransportMode", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsTransportModeListQueryService listService = new CustomsTransportModeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomsTransportModeList> GetCustomsTransportModeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsTransportMode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsTransportModeListQueryService listService = new CustomsTransportModeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsTransportModeList> GetCustomsTransportModeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsTransportMode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsTransportModeListQueryService listService = new CustomsTransportModeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsTransportModeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.CustomsTransportMode", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsTransportModeListQueryService queryService = new CustomsTransportModeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}