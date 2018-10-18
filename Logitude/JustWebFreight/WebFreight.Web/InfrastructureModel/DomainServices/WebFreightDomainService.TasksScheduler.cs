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
        public List<TasksSchedulerPM> GetTasksSchedulersForEntity(int tenant)
        {
            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            return TasksSchedulerQuery.GetTasksSchedulerPMs(tenant);
        }

        public void UpdateTasksSchedulerList(TasksSchedulerList currentEntity)
        {
        }

        public IQueryable<TasksScheduler> GetTasksScheduler(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            return TasksSchedulerRepository.GetTasksScheduler(tenant);
        }
         

        public TasksSchedulerPM GetSingleTasksScheduler(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            return TasksSchedulerQuery.GetSingleTasksSchedulerPMByTenant(id,tenant);
        }

        public TasksSchedulerList GetSingleTasksSchedulerList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            TasksSchedulerList TasksSchedulerList = null;
            TasksScheduler TasksScheduler = TasksSchedulerRepository.GetSingleTasksScheduler(id,tenant);

            if (TasksScheduler != null)
            {
                List<TasksScheduler> singleEntityList = new List<TasksScheduler>();
                singleEntityList.Add(TasksScheduler);

                IQueryable<TasksScheduler> iQueryable = singleEntityList.AsQueryable();
                TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
                IQueryable<TasksSchedulerList> iQueryableEntityList = TasksSchedulerQuery.GetIQueryableEntityList(iQueryable);
                TasksSchedulerList = iQueryableEntityList.FirstOrDefault();
            }
            return TasksSchedulerList;
        }

        public IQueryable<TasksSchedulerList> GetTasksSchedulerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            IQueryable<TasksScheduler> iQueryable = TasksSchedulerRepository.GetTasksScheduler(tenant);
            TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            IQueryable<TasksSchedulerList> query2 = TasksSchedulerQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TasksSchedulerList> GetTasksSchedulerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TasksScheduler> iQueryable = TasksSchedulerRepository.GetTasksScheduler(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TasksScheduler>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            IQueryable<TasksSchedulerList> query2 = TasksSchedulerQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TasksSchedulerList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TasksSchedulerList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TasksScheduler", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TasksSchedulerList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TasksSchedulerList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TasksSchedulerList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TasksSchedulerList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TasksSchedulerList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTasksSchedulerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            TasksSchedulerRepository = new TasksSchedulerRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TasksScheduler> iQueryable = TasksSchedulerRepository.GetTasksScheduler(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TasksScheduler>(nonListQueryOperation, iQueryable);
            TasksSchedulerQuery = new TasksSchedulerQuery(TasksSchedulerRepository);
            IQueryable<TasksSchedulerList> query2 = TasksSchedulerQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TasksSchedulerList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
         

        public void InsertTasksScheduler(TasksSchedulerPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TasksSchedulerService service = new TasksSchedulerService(objectContext , entity.Tenant);
            service.Create(entity);
               
        }

        public void UpdateTasksScheduler(TasksSchedulerPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            TasksSchedulerService service = new TasksSchedulerService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
             
        }

        public void DeleteTasksScheduler(TasksSchedulerPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TasksSchedulerRepository = new TasksSchedulerRepository(objectContext);
            TasksScheduler TasksScheduler = TasksSchedulerRepository.GetSingleTasksScheduler(entity.Id, entity.Tenant);
            TasksSchedulerRepository.Remove(TasksScheduler);
        }
    }
}