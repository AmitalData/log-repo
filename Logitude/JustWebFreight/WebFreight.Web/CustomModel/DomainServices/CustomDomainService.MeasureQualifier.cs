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
        public MeasureQualifierPM GetSingleMeasureQualifierPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            measureQualifierQuery = new MeasureQualifierQueryService(customContext);
            MeasureQualifierPM MeasureQualifier = measureQualifierQuery.GetSingle(id, false, false);
            return MeasureQualifier;
        }

        public MeasureQualifierList GetSingleMeasureQualifierList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.MeasureQualifier", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            MeasureQualifierListQueryService listService = new MeasureQualifierListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<MeasureQualifierList> GetMeasureQualifierLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //    SecurityUtility.CheckContactFeature("Customs.MeasureQualifier", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MeasureQualifierListQueryService listService = new MeasureQualifierListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<MeasureQualifierList> GetMeasureQualifierFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.MeasureQualifier", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            MeasureQualifierListQueryService listService = new MeasureQualifierListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetMeasureQualifierFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //      SecurityUtility.CheckContactFeature("Customs.MeasureQualifier", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MeasureQualifierListQueryService queryService = new MeasureQualifierListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}