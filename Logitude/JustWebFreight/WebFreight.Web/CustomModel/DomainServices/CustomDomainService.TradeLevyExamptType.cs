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

        public TradeLevyExamptTypePM GetSingleTradeLevyExamptTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tradeLevyExamptTypeQuery = new TradeLevyExamptTypeQueryService(customContext);
            TradeLevyExamptTypePM TradeLevyExamptType = tradeLevyExamptTypeQuery.GetSingle(id, false, false);
            return TradeLevyExamptType;
        }

        public TradeLevyExamptTypeList GetSingleTradeLevyExamptTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.TradeLevyExamptType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TradeLevyExamptTypeListQueryService listService = new TradeLevyExamptTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<TradeLevyExamptTypeList> GetTradeLevyExamptTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.TradeLevyExamptType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TradeLevyExamptTypeListQueryService listService = new TradeLevyExamptTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TradeLevyExamptTypeList> GetTradeLevyExamptTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.TradeLevyExamptType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            TradeLevyExamptTypeListQueryService listService = new TradeLevyExamptTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTradeLevyExamptTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.TradeLevyExamptType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TradeLevyExamptTypeListQueryService queryService = new TradeLevyExamptTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}