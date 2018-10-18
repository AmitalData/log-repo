using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomsDocumentStatusTypePM GetSingleCustomsDocumentStatusTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
           customsDocumentStatusTypeQuery = new CustomsDocumentStatusTypeQueryService(customContext);
           CustomsDocumentStatusTypePM CustomsDocumentStatusType = customsDocumentStatusTypeQuery.GetSingle(id, true, false);
            return CustomsDocumentStatusType;
        }

        public CustomsDocumentStatusTypeList GetSingleCustomsDocumentStatusTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomsDocumentStatusType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
           CustomsDocumentStatusTypeListQueryService listService = new CustomsDocumentStatusTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsDocumentStatusTypeList> GetCustomsDocumentStatusTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomsDocumentStatusType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
           CustomsDocumentStatusTypeListQueryService listService = new CustomsDocumentStatusTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentStatusTypeList> GetCustomsDocumentStatusTypeFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //        SecurityUtility.CheckContactFeature("Customs.CustomsDocumentStatusType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
           CustomsDocumentStatusTypeListQueryService listService = new CustomsDocumentStatusTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsDocumentStatusType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
           CustomsDocumentStatusTypeListQueryService queryService = new CustomsDocumentStatusTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

      



    }
}