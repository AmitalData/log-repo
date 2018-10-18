using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateEventTypeCategoryList(EventTypeCategoryList currentEntity)
        {
        }

        public IQueryable<EventTypeCategory> GetEventTypeCategories(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            return eventTypeCategoryRepository.GetEventTypeCategories();
        }

        public IQueryable<EventTypeCategoryPM> GetEventTypeCategoriesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
            return eventTypeCategoryQuery.GetEventTypeCategoryPMs();
        }

        public EventTypeCategoryPM GetSingleEventTypeCategory(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
            return eventTypeCategoryQuery.GetSingleEventTypeCategoryPM(code);
        }

        public EventTypeCategoryList GetSingleEventTypeCategoryList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            EventTypeCategoryList eventTypeCategoryList = null;
            EventTypeCategory eventTypeCategory = eventTypeCategoryRepository.GetSingleEventTypeCategory(code);

            if (eventTypeCategory != null)
            {
                List<EventTypeCategory> singleEntityList = new List<EventTypeCategory>();
                singleEntityList.Add(eventTypeCategory);

                IQueryable<EventTypeCategory> iQueryable = singleEntityList.AsQueryable();
                eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
                IQueryable<EventTypeCategoryList> iQueryableEntityList = eventTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
                eventTypeCategoryList = iQueryableEntityList.FirstOrDefault();
            }
            return eventTypeCategoryList;
        }

        public IQueryable<EventTypeCategoryList> GetEventTypeCategoryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            IQueryable<EventTypeCategory> iQueryable = eventTypeCategoryRepository.GetEventTypeCategories();
            eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
            IQueryable<EventTypeCategoryList> query2 = eventTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<EventTypeCategoryList> GetEventTypeCategoryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EventTypeCategory> iQueryable = eventTypeCategoryRepository.GetEventTypeCategories();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EventTypeCategory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
            IQueryable<EventTypeCategoryList> query2 = eventTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<EventTypeCategoryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EventTypeCategoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EventTypeCategory", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeCategoryList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeCategoryList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeCategoryList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeCategoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EventTypeCategoryList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
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

        public int GetEventTypeCategoryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            eventTypeCategoryRepository = new EventTypeCategoryRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EventTypeCategory> iQueryable = eventTypeCategoryRepository.GetEventTypeCategories();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EventTypeCategory>(nonListQueryOperation, iQueryable);
            eventTypeCategoryQuery = new EventTypeCategoryQuery(eventTypeCategoryRepository);
            IQueryable<EventTypeCategoryList> query2 = eventTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<EventTypeCategoryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertEventTypeCategory(EventTypeCategory entity)
        {
            eventTypeCategoryRepository.Add(entity);
        }

        public void UpdateEventTypeCategory(EventTypeCategory currentEntity)
        {
            eventTypeCategoryRepository.Update(currentEntity);
        }

        public void DeleteEventTypeCategory(EventTypeCategory entity)
        {
            eventTypeCategoryRepository.Remove(entity);
        }
    }
}