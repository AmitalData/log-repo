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
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public TapagPM GetSingleTapagPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tapagQuery = new TapagQueryService(customContext);
            TapagPM Tapag = tapagQuery.GetSingle(id, true, false);
            return Tapag;
        }

        public TapagList GetSingleTapagList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TapagListQueryService listService = new TapagListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public TapagList GetSingleTapagByLeadingFileNumber(string leadingFileNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TapagQueryService queryService = new TapagQueryService(customContext);
            return queryService.GetSingleTapagListByLeadingFileNumber(leadingFileNumber, tenant);
        }

        public List<TapagList> GetTapagLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagListQueryService listService = new TapagListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TapagList> GetDeclarationTapagsLists(string declarationId,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
         
            customContext = CustomContext.GetContext(tenant);

            tapagQuery = new TapagQueryService(customContext);
            List<TapagList> tapags = tapagQuery.GetDeclarationTapags(declarationId, tenant);
            return tapags;
        }



        public List<TapagList> GetTapagFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            TapagListQueryService listService = new TapagListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTapagFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            TapagListQueryService queryService = new TapagListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertTapag(TapagPM entityPm)
        {
            SecurityUtility.CheckContactFeature("General" , "CUSTOMS", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            TapagUpdateService service = new TapagUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);

        }

        public void UpdateTapag(TapagPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("General" , "CUSTOMS", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            TapagUpdateService service = new TapagUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }

        public void UpdateTapagList(TapagList list)
        {

        }
    }
}