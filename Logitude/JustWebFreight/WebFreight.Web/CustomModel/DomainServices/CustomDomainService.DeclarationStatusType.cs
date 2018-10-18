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

        public DeclarationStatusTypePM GetSingleDeclarationStatusTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationStatusTypeQuery = new DeclarationStatusTypeQueryService(customContext);
            DeclarationStatusTypePM DeclarationStatusType = declarationStatusTypeQuery.GetSingle(id, false, false);
            return DeclarationStatusType;
        }

        public DeclarationStatusTypeList GetSingleDeclarationStatusTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationStatusTypeListQueryService listService = new DeclarationStatusTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeclarationStatusTypeList> GetDeclarationStatusTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationStatusTypeListQueryService listService = new DeclarationStatusTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationStatusTypeList> GetDeclarationStatusTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
    

            customContext = CustomContext.GetContext(tenant);
            DeclarationStatusTypeListQueryService listService = new DeclarationStatusTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeclarationStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 
            customContext = CustomContext.GetContext(tenant);
            DeclarationStatusTypeListQueryService queryService = new DeclarationStatusTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}