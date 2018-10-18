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
        public ActionCodePM GetSingleActionCodePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            actionCodeQueryService = new ActionCodeQueryService(customContext);
            ActionCodePM ActionCode = actionCodeQueryService.GetSingle(code, false, false);
            return ActionCode;
        }

        public ActionCodeList GetSingleActionCodeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ActionCode", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ActionCodeListQueryService listService = new ActionCodeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ActionCodeList> GetActionCodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ActionCodeListQueryService listService = new ActionCodeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ActionCodeList> GetActionCodeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ActionCodeListQueryService listService = new ActionCodeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetActionCodeFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    ActionCodeListQueryService queryService = new ActionCodeListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}