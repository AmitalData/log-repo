using Logitude.BL.Helpers;
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
        public TicketClassificationPM GetSingleTicketClassificationPM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            ticketClassificationQuery = new TicketClassificationQueryService(crmContext);
            TicketClassificationPM TicketClassification = ticketClassificationQuery.GetSingle(id, false, false);
            return TicketClassification;
        }

        public TicketClassificationList GetSingleTicketClassificationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            crmContext = CRMContext.GetContext(tenant);
            TicketClassificationListQueryService listService = new TicketClassificationListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public List<TicketClassificationList> GetTicketClassificationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketClassificationListQueryService listService = new TicketClassificationListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<TicketClassificationList> GetTicketClassificationsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketClassificationListQueryService listService = new TicketClassificationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetTicketClassificationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            TicketClassificationListQueryService queryService = new TicketClassificationListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertTicketClassification(TicketClassificationPM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketClassification", "NEW", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            TicketClassificationUpdateService service = new TicketClassificationUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "TicketClassification");
        }

        public void UpdateTicketClassification(TicketClassificationPM entityPm)
        {
            SecurityUtility.CheckContactFeature("TicketClassification", "UPDATE", entityPm.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            TicketClassificationUpdateService service = new TicketClassificationUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "TicketClassification");
        }
    }
}