using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public List<TaskSchedulerHistoryPM> GetTaskSchedulerHistorysForEntity(int tenant)
        {
            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            return TaskSchedulerHistoryQuery.GetTaskSchedulerHistoryPMs(tenant);
        }

        public void UpdateTaskSchedulerHistoryList(TaskSchedulerHistoryList currentEntity)
        {
        }

        public IQueryable<TaskSchedulerHistory> GetTaskSchedulerHistory(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            return TaskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant);
        }
         

        public TaskSchedulerHistoryPM GetSingleTaskSchedulerHistory(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            return TaskSchedulerHistoryQuery.GetSingleTaskSchedulerHistoryPMByTenant(id,tenant);
        }

        public TaskSchedulerHistoryList GetSingleTaskSchedulerHistoryList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            TaskSchedulerHistoryList TaskSchedulerHistoryList = null;
            TaskSchedulerHistory TaskSchedulerHistory = TaskSchedulerHistoryRepository.GetSingleTaskSchedulerHistory(id,tenant);

            if (TaskSchedulerHistory != null)
            {
                List<TaskSchedulerHistory> singleEntityList = new List<TaskSchedulerHistory>();
                singleEntityList.Add(TaskSchedulerHistory);

                IQueryable<TaskSchedulerHistory> iQueryable = singleEntityList.AsQueryable();
                TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
                IQueryable<TaskSchedulerHistoryList> iQueryableEntityList = TaskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);
                TaskSchedulerHistoryList = iQueryableEntityList.FirstOrDefault();
            }
            return TaskSchedulerHistoryList;
        }

        public IQueryable<TaskSchedulerHistoryList> GetTaskSchedulerHistoryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            IQueryable<TaskSchedulerHistory> iQueryable = TaskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            IQueryable<TaskSchedulerHistoryList> query2 = TaskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }
        public IQueryable<TaskSchedulerHistoryList> GetTaskSchedulerHistoryListsByTaskId(int tenant,string TaskId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            IQueryable<TaskSchedulerHistory> iQueryable = TaskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant,TaskId);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            IQueryable<TaskSchedulerHistoryList> query2 = TaskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }
        [Query(HasSideEffects = true)]
        public IQueryable<TaskSchedulerHistoryList> GetTaskSchedulerHistoryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TaskSchedulerHistory> iQueryable = TaskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TaskSchedulerHistory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            IQueryable<TaskSchedulerHistoryList> query2 = TaskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TaskSchedulerHistoryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TaskSchedulerHistoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TaskSchedulerHistory", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TaskSchedulerHistoryList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TaskSchedulerHistoryList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TaskSchedulerHistoryList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TaskSchedulerHistoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TaskSchedulerHistoryList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.EndDateTime);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.EndDateTime);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTaskSchedulerHistoryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TaskSchedulerHistory> iQueryable = TaskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TaskSchedulerHistory>(nonListQueryOperation, iQueryable);
            TaskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(TaskSchedulerHistoryRepository);
            IQueryable<TaskSchedulerHistoryList> query2 = TaskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TaskSchedulerHistoryList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
         

        public void InsertTaskSchedulerHistory(TaskSchedulerHistoryPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TaskSchedulerHistoryService service = new TaskSchedulerHistoryService(objectContext , entity.Tenant);
            service.Create(entity);
               
        }

        public void UpdateTaskSchedulerHistory(TaskSchedulerHistoryPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            TaskSchedulerHistoryService service = new TaskSchedulerHistoryService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
             
        }

        public void DeleteTaskSchedulerHistory(TaskSchedulerHistoryPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TaskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(objectContext);
            TaskSchedulerHistory TaskSchedulerHistory = TaskSchedulerHistoryRepository.GetSingleTaskSchedulerHistory(entity.Id, entity.Tenant);
            TaskSchedulerHistoryRepository.Remove(TaskSchedulerHistory);
        }
    }
}