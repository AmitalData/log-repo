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
        public ReturnConditionPM GetSingleReturnConditionPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            returnConditionQuery = new ReturnConditionQueryService(customContext);
            ReturnConditionPM ReturnCondition = returnConditionQuery.GetSingle(code, false, false);
            return ReturnCondition;
        }

        public ReturnConditionList GetSingleReturnConditionList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ReturnCondition", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ReturnConditionListQueryService listService = new ReturnConditionListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ReturnConditionList> GetReturnConditionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ReturnCondition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ReturnConditionListQueryService listService = new ReturnConditionListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ReturnConditionList> GetReturnConditionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ReturnCondition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ReturnConditionListQueryService listService = new ReturnConditionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetReturnConditionFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ReturnCondition", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ReturnConditionListQueryService queryService = new ReturnConditionListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }




    }
}