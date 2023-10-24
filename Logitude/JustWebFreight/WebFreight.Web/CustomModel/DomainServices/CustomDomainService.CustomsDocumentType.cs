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
        public CustomDocumentTypePM GetSingleCustomDocumentTypePM(string id,int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customDocumentTypeQuery = new CustomDocumentTypeQueryService(customContext);
            CustomDocumentTypePM CustomDocumentType = customDocumentTypeQuery.GetSingleCustomDocumentTypeWithTenant(id, tenant);
            return CustomDocumentType;
        }

        public CustomDocumentTypeList GetSingleCustomDocumentTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomDocumentType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeListQueryService listService = new CustomDocumentTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomDocumentTypeList> GetCustomDocumentTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomDocumentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeListQueryService listService = new CustomDocumentTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomDocumentTypeList> GetCustomDocumentTypeFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //        SecurityUtility.CheckContactFeature("Customs.CustomDocumentType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeListQueryService listService = new CustomDocumentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomDocumentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomDocumentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeListQueryService queryService = new CustomDocumentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    

    }
}