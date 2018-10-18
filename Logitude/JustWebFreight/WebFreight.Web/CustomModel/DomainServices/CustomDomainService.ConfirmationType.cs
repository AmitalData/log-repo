using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public ConfirmationTypePM GetSingleConfirmationTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            confirmationTypeQuery = new ConfirmationTypeQueryService(customContext);
            ConfirmationTypePM ConfirmationType = confirmationTypeQuery.GetSingle(id, false, false);
            return ConfirmationType;
        }

        public ConfirmationTypeList GetSingleConfirmationTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ConfirmationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConfirmationTypeListQueryService listService = new ConfirmationTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ConfirmationTypeList> GetConfirmationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.ConfirmationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConfirmationTypeListQueryService listService = new ConfirmationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConfirmationTypeList> GetConfirmationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.ConfirmationType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ConfirmationTypeListQueryService listService = new ConfirmationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConfirmationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.ConfirmationType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConfirmationTypeListQueryService queryService = new ConfirmationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}