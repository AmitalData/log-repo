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
        public TicketStagePM GetSingleTicketStagePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            ticketStageQuery = new TicketStageQueryService(crmContext);
            TicketStagePM TicketStage = ticketStageQuery.GetSingle(id, false, false);
            return TicketStage;
        }

        public TicketStageList GetSingleTicketStageList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            crmContext = CRMContext.GetContext(tenant);
            TicketStageListQueryService listService = new TicketStageListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<TicketStageList> GetTicketStageLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketStageListQueryService listService = new TicketStageListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketStageList> GetTicketStageFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketStageListQueryService listService = new TicketStageListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetTicketStageFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketStageListQueryService queryService = new TicketStageListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertTicketStage(TicketStagePM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketStage", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            TicketStageUpdateService service = new TicketStageUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateTicketStage(TicketStagePM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketStage", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            TicketStageUpdateService service = new TicketStageUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }
    }
}