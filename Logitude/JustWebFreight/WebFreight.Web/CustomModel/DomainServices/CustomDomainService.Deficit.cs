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
        public DeficitPM GetSingleDeficitPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            deficitQuery = new DeficitQueryService(customContext);
            DeficitPM Deficit = deficitQuery.GetSingle(id, false, false);
            return Deficit;
        }


        public DeficitPM GetDeficitPMByPaymentOrderNumberOrTapagId(string paymentNumber,string tapagId,  int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            deficitQuery = new DeficitQueryService(customContext);
            DeficitPM Deficit = deficitQuery.GetDeficitByPaymentOrderNumberOrTapagId(paymentNumber,tapagId, tenant);
            return Deficit;
        }


        public DeficitList GetSingleDeficitList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeficitListQueryService listService = new DeficitListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeficitList> GetDeficitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeficitListQueryService listService = new DeficitListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeficitList> GetDeficitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            DeficitListQueryService listService = new DeficitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeficitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeficitListQueryService queryService = new DeficitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations,tenant);

        }

        public void InsertDeficit(DeficitPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Deficit", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            DeficitUpdateService service = new DeficitUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            service.Update(entityPm, true);

        }

        public void UpdateDeficit(DeficitPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Deficit", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            DeficitUpdateService service = new DeficitUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            service.Update(currententityPm, true);

        }

        public void UpdateDeficitList(DeficitList list)
        {

        }
    }
}