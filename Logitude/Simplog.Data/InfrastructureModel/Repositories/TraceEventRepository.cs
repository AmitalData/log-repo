using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TraceEventRepository:IRepository<TraceEvent>
    {
        IWebFreightContext webFreightContext;

        public TraceEventRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public TraceEventRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TraceEvent GetSingleTraceEvent(string id)
        {
            return (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public TraceEvent GetSingleTraceEventByExternalId(string externalId, int tenant)
        {
            return (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                    where a.ExternalId == externalId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public TraceEvent GetSingleTraceEventByEntityId(string entityId, string eventTypeId, int tenant)
        {
            TraceEvent traceevent = (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                     where a.EntityId == entityId && a.Tenant == tenant && a.EventTypeId == eventTypeId && !a.Deleted
                                     select a).FirstOrDefault();
            return traceevent;
        }

        public TraceEvent GetSingleTraceEventByExternalIdandEventTypeCode(string externalId, string eventTypeCode, int tenant)
        {
            return (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                    where a.ExternalId == externalId && a.EventType.Code.ToLower() == eventTypeCode.ToLower() && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TraceEvent> GetTraceEvents(int tenant, string entityId, string objectTableId)
        {
            IQueryable<TraceEvent> iQueryable = from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                                 where a.Tenant == tenant 
                                                 && a.EntityId == entityId
                                                 && a.ObjectTableId == objectTableId
                                                 select a;

            return iQueryable;
        }

        public IQueryable<TraceEvent> GetEntityStatusTraceEvents(int tenant, string entityId, string objectTableId)
        {
            return (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                                 where a.Tenant == tenant
                                                 && a.EntityId == entityId
                                                 && a.ObjectTableId == objectTableId
                                                 && a.EventType.EntityStatusId != null
                                                 && a.Deleted == false
                                                 select a).OrderBy(a => a.EventDateTime);
        }

        public TraceEvent GetLatestEntityStatusTraceEvent(int tenant, string entityId, string objectTableId)
        {
            return (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                    where a.Tenant == tenant
                    && a.EntityId == entityId
                    && a.ObjectTableId == objectTableId
                    && a.EventType.EntityStatusId != null
                    && a.Deleted == false
                    select a).OrderByDescending(a => a.EventDateTime).FirstOrDefault();
        }

        public IQueryable<TraceEvent> GetAllTraceEventsByEventType(string entityId, string eventTypeId, int tenant)
        {
            IQueryable<TraceEvent> iQueryable = from a in context.TraceEvent
                                                where a.Tenant == tenant
                                                && a.EntityId == entityId
                                                && a.EventTypeId == eventTypeId
                                                select a;

            return iQueryable;
        }

        public void Add(TraceEvent entity)
        {
            context.TraceEvent.Add(entity);
        }

        public void Remove(TraceEvent entity)
        {
            context.TraceEvent.Attach(entity);
            context.TraceEvent.Remove(entity);
        }

        public void Update(TraceEvent entity)
        {
            context.TraceEvent.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TraceEvent> All()
        {
            return context.TraceEvent.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<TraceEvent> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TraceEvent GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TraceEvent GetLastExceptionTraceEventByShipmentId(string entityId , int tenant)
        {
            TraceEvent traceevent = (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                     where a.EntityId == entityId && a.Tenant == tenant && a.EventType.Code == "EXCE" 
                                     select a).OrderByDescending(d=>d.LogDateTime).FirstOrDefault();
            return traceevent;
        }

        public TraceEvent GetLastExceptionTraceEventByContainerId(string entityId, string eventCode, int tenant)
        {
            TraceEvent traceevent = (from a in context.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                     where a.EntityId == entityId && a.Tenant == tenant && a.EventType.Code == eventCode
                                     select a).OrderByDescending(d => d.LogDateTime).FirstOrDefault();
            return traceevent;
        }
    }
}