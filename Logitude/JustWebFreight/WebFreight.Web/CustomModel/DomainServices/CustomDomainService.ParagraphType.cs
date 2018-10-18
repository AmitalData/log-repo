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
        public ParagraphTypePM GetSingleParagraphTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paragraphTypeQuery = new ParagraphTypeQueryService(customContext);
            ParagraphTypePM ParagraphType = paragraphTypeQuery.GetSingle(id, false, false);
            return ParagraphType;
        }

        public ParagraphTypeList GetSingleParagraphTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ParagraphTypeListQueryService listService = new ParagraphTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ParagraphTypeList> GetParagraphTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);        
            customContext = CustomContext.GetContext(tenant);
            ParagraphTypeListQueryService listService = new ParagraphTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ParagraphTypeList> GetParagraphTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      

            customContext = CustomContext.GetContext(tenant);
            ParagraphTypeListQueryService listService = new ParagraphTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetParagraphTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        
            customContext = CustomContext.GetContext(tenant);
            ParagraphTypeListQueryService queryService = new ParagraphTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}