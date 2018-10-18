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
        public TicketSeverityPM GetSingleTicketSeverityPM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            ticketSeverityQuery = new TicketSeverityQueryService(crmContext);
            TicketSeverityPM TicketSeverity = ticketSeverityQuery.GetSingle(id, false, false);
            return TicketSeverity;
        }

        public TicketSeverityList GetSingleTicketSeverityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            crmContext = CRMContext.GetContext(tenant);
            TicketSeverityListQueryService listService = new TicketSeverityListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<TicketSeverityList> GetTicketSeverityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketSeverityListQueryService listService = new TicketSeverityListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketSeverityList> GetTicketSeverityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketSeverityListQueryService listService = new TicketSeverityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetTicketSeverityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketSeverityListQueryService queryService = new TicketSeverityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertTicketSeverity(TicketSeverityPM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketSeverity", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            TicketSeverityUpdateService service = new TicketSeverityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateTicketSeverity(TicketSeverityPM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketSeverity", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            TicketSeverityUpdateService service = new TicketSeverityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }
    }
}