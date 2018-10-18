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
        public ImporterDeclarationTypePM GetSingleImporterDeclarationTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            importerDeclarationTypeQuery = new ImporterDeclarationTypeQueryService(customContext);
            ImporterDeclarationTypePM ImporterDeclarationType = importerDeclarationTypeQuery.GetSingle(code, false, false);
            return ImporterDeclarationType;
        }

        public ImporterDeclarationTypeList GetSingleImporterDeclarationTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterDeclarationType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ImporterDeclarationTypeListQueryService listService = new ImporterDeclarationTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ImporterDeclarationTypeList> GetImporterDeclarationTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterDeclarationTypeListQueryService listService = new ImporterDeclarationTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ImporterDeclarationTypeList> GetImporterDeclarationTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterDeclarationTypeListQueryService listService = new ImporterDeclarationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetImporterDeclarationTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterDeclarationTypeListQueryService queryService = new ImporterDeclarationTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}