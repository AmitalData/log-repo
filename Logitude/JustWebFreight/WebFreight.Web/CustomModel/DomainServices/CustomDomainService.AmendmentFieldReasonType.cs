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

        public AmendmentFieldReasonTypePM GetSingleAmendmentFieldReasonTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            amendmentFieldReasonTypeQuery = new AmendmentFieldReasonTypeQueryService(customContext);
            AmendmentFieldReasonTypePM AmendmentFieldReasonType = amendmentFieldReasonTypeQuery.GetSingle(id, false, false);
            return AmendmentFieldReasonType;
        }

        public AmendmentFieldReasonTypeList GetSingleAmendmentFieldReasonTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.AmendmentFieldReasonType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AmendmentFieldReasonTypeListQueryService listService = new AmendmentFieldReasonTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AmendmentFieldReasonTypeList> GetAmendmentFieldReasonTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.AmendmentFieldReasonType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AmendmentFieldReasonTypeListQueryService listService = new AmendmentFieldReasonTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AmendmentFieldReasonTypeList> GetAmendmentFieldReasonTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AmendmentFieldReasonType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            AmendmentFieldReasonTypeListQueryService listService = new AmendmentFieldReasonTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAmendmentFieldReasonTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AmendmentFieldReasonType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AmendmentFieldReasonTypeListQueryService queryService = new AmendmentFieldReasonTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}