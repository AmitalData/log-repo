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
        public SplitOrMergeReasonPM GetSingleSplitOrMergeReasonPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            splitOrMergeReasonQueryService = new SplitOrMergeReasonQueryService(customContext);
            SplitOrMergeReasonPM SplitOrMergeReason = splitOrMergeReasonQueryService.GetSingle(code, false, false);
            return SplitOrMergeReason;
        }

        public SplitOrMergeReasonList GetSingleSplitOrMergeReasonList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SplitOrMergeReason", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SplitOrMergeReasonListQueryService listService = new SplitOrMergeReasonListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<SplitOrMergeReasonList> GetSplitOrMergeReasonLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            SplitOrMergeReasonListQueryService listService = new SplitOrMergeReasonListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<SplitOrMergeReasonList> GetSplitOrMergeReasonFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            SplitOrMergeReasonListQueryService listService = new SplitOrMergeReasonListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetSplitOrMergeReasonFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    SplitOrMergeReasonListQueryService queryService = new SplitOrMergeReasonListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}