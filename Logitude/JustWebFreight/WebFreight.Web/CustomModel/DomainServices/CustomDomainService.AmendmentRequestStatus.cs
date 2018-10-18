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

        public AmendmentRequestStatusPM GetSingleAmendmentRequestStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            amendmentRequestStatusQuery = new AmendmentRequestStatusQueryService(customContext);
            AmendmentRequestStatusPM AmendmentRequestStatus = amendmentRequestStatusQuery.GetSingle(id, false, false);
            return AmendmentRequestStatus;
        }

        public AmendmentRequestStatusList GetSingleAmendmentRequestStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.AmendmentRequestStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AmendmentRequestStatusListQueryService listService = new AmendmentRequestStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AmendmentRequestStatusList> GetAmendmentRequestStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.AmendmentRequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AmendmentRequestStatusListQueryService listService = new AmendmentRequestStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AmendmentRequestStatusList> GetAmendmentRequestStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AmendmentRequestStatus", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            AmendmentRequestStatusListQueryService listService = new AmendmentRequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAmendmentRequestStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AmendmentRequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AmendmentRequestStatusListQueryService queryService = new AmendmentRequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}