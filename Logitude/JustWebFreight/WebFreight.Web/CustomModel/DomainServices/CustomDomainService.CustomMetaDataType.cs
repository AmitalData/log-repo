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
        public CustomMetaDataTypePM GetSingleCustomMetaDataTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customMetaDataTypeQuery = new CustomMetaDataTypeQueryService(customContext);
            CustomMetaDataTypePM CustomMetaDataType = customMetaDataTypeQuery.GetSingle(id, true, false);
            return CustomMetaDataType;
        }

        public CustomMetaDataTypeList GetSingleCustomMetaDataTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomMetaDataType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomMetaDataTypeListQueryService listService = new CustomMetaDataTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomMetaDataTypeList> GetCustomMetaDataTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomMetaDataType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomMetaDataTypeListQueryService listService = new CustomMetaDataTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomMetaDataTypeList> GetCustomMetaDataTypeFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //        SecurityUtility.CheckContactFeature("Customs.CustomMetaDataType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomMetaDataTypeListQueryService listService = new CustomMetaDataTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomMetaDataTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomMetaDataType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomMetaDataTypeListQueryService queryService = new CustomMetaDataTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

  


    }
}