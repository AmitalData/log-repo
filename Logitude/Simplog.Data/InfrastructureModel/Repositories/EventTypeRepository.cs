using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EventTypeRepository:IRepository<EventType>
    {

        IWebFreightContext webFreightContext;

        public EventTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public EventTypeRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public EventTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<EventType> GetEventTypes()
        {
            return context.EventType;
        }



        public IQueryable<EventType> GetEventTypes(int tenant)
        {
            IQueryable<EventType> eventTypes = from a in context.EventType
                                               where  a.Tenant == tenant 
                                               select a;
            return eventTypes;
        }

        public IQueryable<EventType> GetEventTypesByCodes(int tenant, string tableId, List<string> codes)
        {
            IQueryable<EventType> eventTypes = from a in context.EventType
                                               where codes.Contains(a.Code) && a.Tenant == tenant && a.ObjectTableId == tableId
                                               select a;
            return eventTypes;
        }

        public IQueryable<EventType> GetEventTypesByTenant(int tenant)
        {
            IQueryable<EventType> eventTypes = from a in context.EventType where a.Tenant == tenant select a;
            return eventTypes;
        }

        public IQueryable<EventType> GetEventTypesByTenantAndObjectTableId(int tenant, string tableId)
        {
            IQueryable<EventType> eventTypes = from a in context.EventType where a.Tenant == tenant && a.ObjectTableId == tableId select a;
            return eventTypes;
        }

        public EventType GetSingleEventType(string id, int tenant)
        {
            EventType entity = (from a in context.EventType where a.Tenant == tenant && a.Id == id select a).Include("EntityStatus").FirstOrDefault();
            return entity;
        }


        public EventType GetSingleEventTypeByCode(string code, int tenant)
        {
            EventType entity = (from a in context.EventType where a.Tenant == tenant && a.Code == code select a).FirstOrDefault();
            return entity;
        }


        public string GetSingleEventTypeIdByCode(string code, int tenant)
        {
            string id = (from a in context.EventType where a.Tenant == tenant && a.Code == code select a.Id).FirstOrDefault();
            return id;
        }

        public string GetEventTypeNameById(string id, int tenant)
        {
            string name = (from a in context.EventType where a.Tenant == tenant && a.Id == id select a.FollowUpEnglishName).FirstOrDefault();
            return name;
        }

        public EventType GetSingleEventTypeByCodeAndObjectTableId(string code,string tableId, int tenant)
        {
            EventType entity = (from a in context.EventType where a.Tenant == tenant && a.Code == code && a.ObjectTableId == tableId select a).FirstOrDefault();
            return entity;
        }


        //public IQueryable<EventType> GetEventTypesByCodes(int tenant, string tableId,List<string> codes)
        //{
        //    IQueryable<EventType> eventTypes = from a in context.EventType 
        //                                       where codes.Contains(a.Code) && a.Tenant == tenant && a.ObjectTableId == tableId 
        //                                       select a;
        //    return eventTypes;
        //}

        public void Add(EventType entity)
        {
            context.EventType.Add(entity);
        }

        public void Remove(EventType entity)
        {
            context.EventType.Attach(entity);
            context.EventType.Remove(entity);
        }

        public void Update(EventType entity)
        {
            try
            {
                context.EventType.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<EventType> All()
        {
            return context.EventType.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<EventType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EventType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}