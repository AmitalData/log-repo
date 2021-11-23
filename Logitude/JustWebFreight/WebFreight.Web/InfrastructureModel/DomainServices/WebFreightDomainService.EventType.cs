using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateEventTypeList(EventTypeList currentEntity)
        {
        }

        public IQueryable<EventType> GetEventTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            return eventTypesRepository.GetEventTypesByTenant(0);
        }

        public IQueryable<EventTypePM> GetEventTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
        }

        public EventTypePM GetEventTypeByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetSinglePMByCode(code, tenant);
        }


        public List<EventTypeList> GetEventTypeListByListEventCode(List<string> codeEventList, int tenant, string objectTableId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypeIdsByListEventCode(codeEventList, tenant, objectTableId);
        }



        


        public EventTypePM GetSingleEventType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetSingleEventTypePM(id, tenant);
        }

        public IQueryable<EventTypePM> GetFirstEventTypes(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            input = input.ToUpper();
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input)).Take(50).OrderBy(p => p.EnglishName);
        }

        public IQueryable<EventTypePM> GetFirstEventTypesByTenant(string input, int tenant, bool isAgentEntry)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            input = input.ToUpper();
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(p => (p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input)) && p.Tenant == tenant).Take(50).OrderBy(p => p.EnglishName);
        }

        public IQueryable<EventTypePM> GetEventTypesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                    return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                    return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<EventTypePM> GetSingleEventTypeByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            if (byCode)
            {
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                return eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public IQueryable<EventTypePM> GetEventTypesSearch(string name, string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            string nameNew = "";
            string codeNew = "";

            if (name != null)
            {
                nameNew = name;
            }
            if (code != null)
            {
                codeNew = code;
            }
            IQueryable<EventTypePM> q2 = null;
            IQueryable<EventTypePM> q = null;
            if (codeNew != null)
            {
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                q = eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.Code.StartsWith(codeNew)).Where(d => d.Tenant == tenant);
            }
            if (nameNew != null)
            {
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                q2 = eventTypeQuery.GetEventTypePMsByTenant(tenant).Where(d => d.EnglishName.StartsWith(nameNew)).Where(d => d.Tenant == tenant);
            }

            if (codeNew == "" && nameNew == "")
            {
                return q;
            }
            else if (codeNew == "")
            {
                return q2;
            }
            else if (nameNew == "")
            {
                return q;
            }
            else
            {
                return q.Concat(q2);
            }
        }

        public EventTypeList GetSingleEventTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            EventTypeList eventTypeList = null;
            EventType eventType = eventTypesRepository.GetSingleEventType(id, tenant);

            if (eventType != null)
            {
                List<EventType> singleEntityList = new List<EventType>();
                singleEntityList.Add(eventType);

                IQueryable<EventType> iQueryable = singleEntityList.AsQueryable();
                eventTypeQuery = new EventTypeQuery(eventTypesRepository);
                IQueryable<EventTypeList> iQueryableEntityList = eventTypeQuery.GetIQueryableEntityList(iQueryable);
                eventTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return eventTypeList;
        }

        public IQueryable<EventTypeList> GetEventTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            IQueryable<EventType> eventTypes = eventTypesRepository.GetEventTypesByTenant(tenant).Where(d => d.InActive == false);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            IQueryable<EventTypeList> query2 = eventTypeQuery.GetIQueryableEntityList(eventTypes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<EventTypeList> GetEventTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EventType> eventTypes = eventTypesRepository.GetEventTypesByTenant(tenant).Where(d => d.InActive == false);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            eventTypes = filter.GetFilteredQuery<EventType>(nonListQueryOperation, eventTypes);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            IQueryable<EventTypeList> query2 = eventTypeQuery.GetIQueryableEntityList(eventTypes);
            query2 = filter.GetFilteredQuery<EventTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EventTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EventType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetEventTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EventType> eventTypes = eventTypesRepository.GetEventTypesByTenant(tenant).Where(d => d.InActive == false);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            eventTypes = filter.GetFilteredQuery<EventType>(nonListQueryOperation, eventTypes);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            IQueryable<EventTypeList> query2 = eventTypeQuery.GetIQueryableEntityList(eventTypes);
            query2 = filter.GetFilteredQuery<EventTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertEventType(EventTypePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            EventTypeService servie = new EventTypeService(objectContext , entity.Tenant);
            servie.Create(entity);

            TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "EventType");
        }

        public void UpdateEventType(EventTypePM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            string entityName = "EventType" + currentEntity.Id + currentEntity.Tenant;
            string entityPmName = "EventTypePM" + currentEntity.Id + currentEntity.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            EventTypeService service = new EventTypeService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "EventType");
        }

        public void DeleteEventType(EventTypePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            eventTypesRepository = new EventTypeRepository(objectContext);
            EventType eventType = eventTypesRepository.GetSingleEventType(entity.Id, entity.Tenant);
            eventTypesRepository.Remove(eventType);
        }

        public IQueryable<EventTypePM> GetEventTypePMsByObjectTableId(string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypesByObjectTable(objectTableId, tenant);
        }

        public IQueryable<EventTypePM> GetEventTypesByObjectTable(string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypesRepository = new EventTypeRepository(tenant);
            eventTypeQuery = new EventTypeQuery(eventTypesRepository);
            return eventTypeQuery.GetEventTypesByObjectTable(objectTableId, tenant);
        }
    }
}
