using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public DepositFileTypePM GetSingleDepositFileTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            depositFileTypeQuery = new DepositFileTypeQueryService(customContext);
            DepositFileTypePM DepositFileType = depositFileTypeQuery.GetSingle(id, false, false);
            return DepositFileType;
        }

        public DepositFileTypeList GetSingleDepositFileTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DepositFileTypeListQueryService listService = new DepositFileTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DepositFileTypeList> GetDepositFileTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        
            customContext = CustomContext.GetContext(tenant);
            DepositFileTypeListQueryService listService = new DepositFileTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DepositFileTypeList> GetDepositFileTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        

            customContext = CustomContext.GetContext(tenant);
            DepositFileTypeListQueryService listService = new DepositFileTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDepositFileTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        
            customContext = CustomContext.GetContext(tenant);
            DepositFileTypeListQueryService queryService = new DepositFileTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}