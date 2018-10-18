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

        public StorageStatusPM GetSingleStorageStatusPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            StorageStatusQueryService storageStatusQuery = new StorageStatusQueryService(customContext);
            StorageStatusPM StorageStatus = storageStatusQuery.GetSingle(id, false, false);
            return StorageStatus;
        }

        public StorageStatusList GetSingleStorageStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            StorageStatusListQueryService listService = new StorageStatusListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<StorageStatusList> GetStorageStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            StorageStatusListQueryService listService = new StorageStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<StorageStatusList> GetStorageStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
    

            customContext = CustomContext.GetContext(tenant);
            StorageStatusListQueryService listService = new StorageStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetStorageStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 
            customContext = CustomContext.GetContext(tenant);
            StorageStatusListQueryService queryService = new StorageStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}