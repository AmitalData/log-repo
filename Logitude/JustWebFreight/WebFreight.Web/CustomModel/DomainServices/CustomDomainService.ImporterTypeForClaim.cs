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
        public ImporterTypeForClaimPM GetSingleImporterTypeForClaimPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            ImporterTypeForClaimQueryService importerTypeForClaimQuery = new ImporterTypeForClaimQueryService(customContext);
            ImporterTypeForClaimPM ImporterTypeForClaim = importerTypeForClaimQuery.GetSingle(id, false, false);
            return ImporterTypeForClaim;
        }

        public ImporterTypeForClaimList GetSingleImporterTypeForClaimList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.ImporterTypeForClaim", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ImporterTypeForClaimListQueryService listService = new ImporterTypeForClaimListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ImporterTypeForClaimList> GetImporterTypeForClaimLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.ImporterTypeForClaim", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterTypeForClaimListQueryService listService = new ImporterTypeForClaimListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ImporterTypeForClaimList> GetImporterTypeForClaimFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ImporterTypeForClaim", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ImporterTypeForClaimListQueryService listService = new ImporterTypeForClaimListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetImporterTypeForClaimFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.ImporterTypeForClaim", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ImporterTypeForClaimListQueryService queryService = new ImporterTypeForClaimListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}