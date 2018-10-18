using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public DepositEssenceTypePM GetSingleDepositEssenceTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            depositEssenceTypeQuery = new DepositEssenceTypeQueryService(customContext);
            DepositEssenceTypePM DepositEssenceType = depositEssenceTypeQuery.GetSingle(code, false, false);
            return DepositEssenceType;
        }

        public DepositEssenceTypeList GetSingleDepositEssenceTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DepositEssenceTypeListQueryService listService = new DepositEssenceTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<DepositEssenceTypeList> GetDepositEssenceTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         
            customContext = CustomContext.GetContext(tenant);
            DepositEssenceTypeListQueryService listService = new DepositEssenceTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DepositEssenceTypeList> GetDepositEssenceTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         
            customContext = CustomContext.GetContext(tenant);
            DepositEssenceTypeListQueryService listService = new DepositEssenceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDepositEssenceTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          
            customContext = CustomContext.GetContext(tenant);
            DepositEssenceTypeListQueryService queryService = new DepositEssenceTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}