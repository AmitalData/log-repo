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
        public ProcessingReasonPM GetSingleProcessingReasonPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            ProcessingReasonQueryService ProcessingReasonQuery = new ProcessingReasonQueryService(customContext);
            ProcessingReasonPM ProcessingReason = ProcessingReasonQuery.GetSingle(id, false, false);
            return ProcessingReason;
        }

        public ProcessingReasonList GetSingleProcessingReasonList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.ProcessingReason", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProcessingReasonListQueryService listService = new ProcessingReasonListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ProcessingReasonList> GetProcessingReasonLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.ProcessingReason", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProcessingReasonListQueryService listService = new ProcessingReasonListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ProcessingReasonList> GetProcessingReasonFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ProcessingReason", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ProcessingReasonListQueryService listService = new ProcessingReasonListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetProcessingReasonFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ProcessingReason", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProcessingReasonListQueryService queryService = new ProcessingReasonListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}