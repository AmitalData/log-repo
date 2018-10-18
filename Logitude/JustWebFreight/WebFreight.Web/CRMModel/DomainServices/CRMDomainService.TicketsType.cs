using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public TicketTypePM GetSingleTicketTypePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            ticketTypeQuery = new TicketTypeQueryService(crmContext);
            TicketTypePM TicketType = ticketTypeQuery.GetSingle(id, false, false);
            return TicketType;
        }

        public TicketTypeList GetSingleTicketTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            crmContext = CRMContext.GetContext(tenant);
            TicketTypeListQueryService listService = new TicketTypeListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<TicketTypeList> GetTicketTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketTypeListQueryService listService = new TicketTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketTypeList> GetTicketTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketTypeListQueryService listService = new TicketTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetTicketTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketTypeListQueryService queryService = new TicketTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertTicketType(TicketTypePM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketType", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            TicketTypeUpdateService service = new TicketTypeUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateTicketType(TicketTypePM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketType", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            TicketTypeUpdateService service = new TicketTypeUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }
    }
}