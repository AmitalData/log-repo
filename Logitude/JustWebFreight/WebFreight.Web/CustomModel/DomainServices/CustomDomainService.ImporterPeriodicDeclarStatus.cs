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
        public ImporterPeriodicDeclarStatusPM GetSingleImporterPeriodicDeclarationStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            importerPeriodicDeclarationStatusQuery = new ImporterPeriodicDeclarStatusQueryService(customContext);
            ImporterPeriodicDeclarStatusPM ImporterPeriodicDeclarationStatus = importerPeriodicDeclarationStatusQuery.GetSingle(code, false, false);
            return ImporterPeriodicDeclarationStatus;
        }

        public ImporterPeriodicDeclarStatusList GetSingleImporterPeriodicDeclarationStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ImporterPeriodicDeclarStatusListQueryService listService = new ImporterPeriodicDeclarStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ImporterPeriodicDeclarStatusList> GetImporterPeriodicDeclarationStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterPeriodicDeclarationStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterPeriodicDeclarStatusListQueryService listService = new ImporterPeriodicDeclarStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ImporterPeriodicDeclarStatusList> GetImporterPeriodicDeclarationStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterPeriodicDeclarationStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterPeriodicDeclarStatusListQueryService listService = new ImporterPeriodicDeclarStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetImporterPeriodicDeclarationStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ImporterPeriodicDeclarationStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterPeriodicDeclarStatusListQueryService queryService = new ImporterPeriodicDeclarStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}