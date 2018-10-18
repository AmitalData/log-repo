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
        public DangerousGoodsPackingReqPM GetSingleDangerousGoodsPackingReqPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            dangerousGoodsPackingReqQuery = new DangerousGoodsPackingReqQueryService(customContext);
            DangerousGoodsPackingReqPM DangerousGoodsPackingReq = dangerousGoodsPackingReqQuery.GetSingle(code, false, false);
            return DangerousGoodsPackingReq;
        }

        public DangerousGoodsPackingReqList GetSingleDangerousGoodsPackingReqList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DangerousGoodsPackingReq", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DangerousGoodsPackingReqListQueryService listService = new DangerousGoodsPackingReqListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<DangerousGoodsPackingReqList> GetDangerousGoodsPackingReqLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //3"Customs.DangerousGoodsPackingReq", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DangerousGoodsPackingReqListQueryService listService = new DangerousGoodsPackingReqListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DangerousGoodsPackingReqList> GetDangerousGoodsPackingReqFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DangerousGoodsPackingReq", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DangerousGoodsPackingReqListQueryService listService = new DangerousGoodsPackingReqListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDangerousGoodsPackingReqFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DangerousGoodsPackingReq", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DangerousGoodsPackingReqListQueryService queryService = new DangerousGoodsPackingReqListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}