using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public PendingByKeywordPM GetSinglePendingByKeywordPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            pendingByKeywordQuery = new PendingByKeywordQueryService(customContext);
            PendingByKeywordPM PendingByKeyword = pendingByKeywordQuery.GetSingle(id, true, false);
            return PendingByKeyword;
        }

        public PendingByKeywordList GetSinglePendingByKeywordList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.PendingByKeyword", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PendingByKeywordListQueryService listService = new PendingByKeywordListQueryService(customContext);
            return listService.GetSingle(id);
        }


        public List<PendingByKeywordList> GetPendingByKeywordLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.PendingByKeyword", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PendingByKeywordListQueryService listService = new PendingByKeywordListQueryService(customContext);
            return listService.GetList(tenant);
         
        }

        
        [Query(HasSideEffects = true)] 
        public List<PendingByKeywordList> GetPendingByKeywordFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.PendingByKeyword", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            PendingByKeywordListQueryService listService = new PendingByKeywordListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }


        public int GetPendingByKeywordFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.PendingByKeyword", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PendingByKeywordListQueryService queryService = new PendingByKeywordListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }
        

        public void UpdatePendingByKeyword(PendingByKeywordPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.PendingByKeyword", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            PendingByKeywordUpdateService service = new PendingByKeywordUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }
        
    }
}