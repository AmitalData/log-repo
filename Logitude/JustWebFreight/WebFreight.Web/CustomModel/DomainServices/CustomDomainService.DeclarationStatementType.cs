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

        public DeclarationStatementTypePM GetSingleDeclarationStatementTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationStatementTypeQuery = new DeclarationStatementTypeQueryService(customContext);
            DeclarationStatementTypePM DeclarationStatementType = declarationStatementTypeQuery.GetSingle(id, false, false);
            return DeclarationStatementType;
        }

        public DeclarationStatementTypeList GetSingleDeclarationStatementTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.DeclarationStatementType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationStatementTypeListQueryService listService = new DeclarationStatementTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeclarationStatementTypeList> GetDeclarationStatementTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.DeclarationStatementType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationStatementTypeListQueryService listService = new DeclarationStatementTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationStatementTypeList> GetDeclarationStatementTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.DeclarationStatementType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationStatementTypeListQueryService listService = new DeclarationStatementTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeclarationStatementTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.DeclarationStatementType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationStatementTypeListQueryService queryService = new DeclarationStatementTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}