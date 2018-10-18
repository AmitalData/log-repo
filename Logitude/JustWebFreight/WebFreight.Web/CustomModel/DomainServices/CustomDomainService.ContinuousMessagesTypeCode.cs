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
        public ContinuousMessagesTypeCodePM GetSingleContinuousMessagesTypeCodePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            continuousMessagesTypeCodeQuery = new ContinuousMessagesTypeCodeQueryService(customContext);
            ContinuousMessagesTypeCodePM ContinuousMessagesTypeCode = continuousMessagesTypeCodeQuery.GetSingle(code, false, false);
            return ContinuousMessagesTypeCode;
        }

        public ContinuousMessagesTypeCodeList GetSingleContinuousMessagesTypeCodeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ContinuousMessagesTypeCode", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ContinuousMessagesTypeCodeListQueryService listService = new ContinuousMessagesTypeCodeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ContinuousMessagesTypeCodeList> GetContinuousMessagesTypeCodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ContinuousMessagesTypeCodeListQueryService listService = new ContinuousMessagesTypeCodeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ContinuousMessagesTypeCodeList> GetContinuousMessagesTypeCodeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ContinuousMessagesTypeCodeListQueryService listService = new ContinuousMessagesTypeCodeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetContinuousMessagesTypeCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ContinuousMessagesTypeCodeListQueryService queryService = new ContinuousMessagesTypeCodeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}