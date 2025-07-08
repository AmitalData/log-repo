using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Text;

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
            string key = $"GetSingleEventTypeByCode({code}, {tenant})";
            return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<EventType>(key, () =>
            {
                return GetSingleEventTypeByCodeReal(code, tenant);
            });
        }
        EventType GetSingleEventTypeByCodeReal(string code, int tenant)
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
            entity.LocalName = TryConvertFromBase64(entity.LocalName);
            entity.FollowUpLocalName = TryConvertFromBase64(entity.FollowUpLocalName);

            context.EventType.Add(entity);
        }

        public void Remove(EventType entity)
        {
            context.EventType.Attach(entity);
            context.EventType.Remove(entity);
        }

        public void Update(EventType entity)
        {
            entity.LocalName = TryConvertFromBase64(entity.LocalName);
            entity.FollowUpLocalName = TryConvertFromBase64(entity.FollowUpLocalName);

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

        public string GetCustomFieldByEventTypeId(string id, int tenant)
        {
            return (from a in context.EventType
                    where a.Id == id && a.Tenant == tenant 
                    select a.CustomField).FirstOrDefault();
        }



        public List<EventType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EventType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
        public static string TryConvertFromBase64(string input)
        {
            try
            {
                if (input == null)
                {
                    return null;
                }
                if (input.StartsWith("BS64:") || input.StartsWith("\"BS64:"))
                {

                    return ConvertFromBase64(input);


                }
                return input;

            }
            catch (FormatException)
            {
                return input;
            }
        }

        private static string ConvertFromBase64(string input)
        {
            string substringToRemove = "\"";
            string backUp = input;
            try
            {
                input = input.Trim('\"');
                input = input.Substring(5);//REMOVE BS64:
                byte[] data = Convert.FromBase64String(input);
                string decodedString = Encoding.UTF8.GetString(data);
                decodedString = decodedString.Trim('\"');
                decodedString = decodedString.Replace("\\\"", "\"").Replace("\\\\", "\\");

                return decodedString;

            }
            catch (FormatException)
            {
                return backUp;
            }

        }
    }
}