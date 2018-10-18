using System;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<TraceEventPM> GetTraceEventsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            traceEventQuery = new TraceEventQuery(traceEventsRepository);
            return traceEventQuery.GetTraceEventPMsByTenant(tenant).Where(d => d.Tenant == tenant);
        }

        public IQueryable<TraceEventPM> GetTraceEventsForEntity(string tableId, string recordId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            traceEventsRepository = new TraceEventRepository(tenant);
            traceEventQuery = new TraceEventQuery(traceEventsRepository);
            IQueryable<TraceEventPM> traceEvents = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, recordId, tableId).OrderByDescending(s => s.LogDateTime);/*.Where(d => d.ObjectTableId == TableID && d.EntityId == RecordID && d.Tenant == tenant)*/
            return traceEvents;
        }

        public void InsertTraceEvent(TraceEventPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            TraceEventService service = new TraceEventService(objectContext, entity.Tenant);
            service.Create(entity);
        }       

        public void UpdateTraceEvent(TraceEventPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            TraceEventService service = new TraceEventService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
        }

        public void DeleteTraceEvent(TraceEventPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            traceEventsRepository = new TraceEventRepository(entity.Tenant);
            TraceEvent traceEvent = traceEventsRepository.GetSingleTraceEvent(entity.Id);
            traceEvent.Deleted = true;
            traceEventsRepository.Update(traceEvent);
        }    
    }
}