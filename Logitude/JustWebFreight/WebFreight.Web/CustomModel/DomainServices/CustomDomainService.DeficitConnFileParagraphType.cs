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

        public DeficitConnFileParagraphTypePM GetSingleDeficitConnectedFileParagraphTypePM(string deficitId, string declarationId, string paragraphTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            deficitConnectedFileParagraphTypeQuery = new DeficitConnFileParagraphTypeQueryService(customContext);
            DeficitConnFileParagraphTypePM DeficitConnectedFileParagraphType = deficitConnectedFileParagraphTypeQuery.GetSingle(deficitId, declarationId, paragraphTypeCode, false, false);
            return DeficitConnectedFileParagraphType;
        }

        public DeficitConnFileParagraphTypeList GetSingleDeficitConnectedFileParagraphTypeList(string deficitId, string declarationId, string paragraphTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeficitConnFileParagraphTypeListQueryService listService = new DeficitConnFileParagraphTypeListQueryService(customContext);
            return listService.GetSingle(deficitId, declarationId, paragraphTypeCode);
        }

        public List<DeficitConnFileParagraphTypeList> GetDeficitConnectedFileParagraphTypeList( string declarationId, string deficitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            deficitConnectedFileParagraphTypeQuery = new DeficitConnFileParagraphTypeQueryService(customContext);
            return deficitConnectedFileParagraphTypeQuery.GetDeficitConnectedFileParagraphTypesByDeclarationId(declarationId,deficitId, tenant);
        }



        public List<DeficitConnFileParagraphTypeList> GetDeficitConnectedFileParagraphTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeficitConnFileParagraphTypeListQueryService listService = new DeficitConnFileParagraphTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeficitConnFileParagraphTypeList> GetDeficitConnectedFileParagraphTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            DeficitConnFileParagraphTypeListQueryService listService = new DeficitConnFileParagraphTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeficitConnFileParagraphTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeficitConnFileParagraphTypeListQueryService queryService = new DeficitConnFileParagraphTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
    }
}