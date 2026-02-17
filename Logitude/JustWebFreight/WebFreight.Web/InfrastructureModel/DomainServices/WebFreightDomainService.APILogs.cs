using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<APILogsPM> GetAPILogsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);

            APILogsQuery APILogsQuery = new APILogsQuery(tenant);
            return APILogsQuery.GetAPILogsPMsByTenant(tenant);
        }

        public APILogsPM GetAPILogsById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);

            APILogsQuery APILogsQuery = new APILogsQuery(tenant); 
            APILogsPM APILogs = APILogsQuery.GetSinglePM(id, tenant);
            return APILogs;

        }

        public APILogsList GetSingleAPILogsList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);
            APILogsRepository aPILogsRepository;
            aPILogsRepository = new APILogsRepository(tenant);
            APILogsQuery APILogsQuery = new APILogsQuery(aPILogsRepository);
            APILogsList APILogsList = null;
            APILogs APILogs = aPILogsRepository.GetSingleAPILogs(id, tenant);

            if (APILogs != null)
            {
                List<APILogs> singleEntityList = new List<APILogs>();
                singleEntityList.Add(APILogs);

                IQueryable<APILogs> iQueryable = singleEntityList.AsQueryable();
                IQueryable<APILogsList> iQueryableEntityList = APILogsQuery.GetIQueryableEntityList(iQueryable);
                APILogsList = iQueryableEntityList.FirstOrDefault();
            }
            return APILogsList;
        }

        public IQueryable<APILogsList> GetAPILogsList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);
            APILogsRepository aPILogsRepository;
            aPILogsRepository = new APILogsRepository(tenant);
            APILogsQuery APILogsQuery = new APILogsQuery(aPILogsRepository);
            IQueryable<APILogs> APILogss = aPILogsRepository.GetAPILogs(tenant);
            IQueryable<APILogsList> query2 = APILogsQuery.GetIQueryableEntityList(APILogss);
            return query2;
        }

        public IQueryable<APILogsList> GetAPILogsByCustomerIdList(string CustomerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);
            APILogsRepository aPILogsRepository;
            aPILogsRepository = new APILogsRepository(tenant);
            APILogsQuery APILogsQuery = new APILogsQuery(aPILogsRepository);
            IQueryable<APILogs> APILogss = aPILogsRepository.GetAPILogsByCustomerId(CustomerId, tenant);
            IQueryable<APILogsList> query2 = APILogsQuery.GetIQueryableEntityList(APILogss);
            return query2;
        }
        [Query(HasSideEffects = true)]
        public IQueryable<APILogsList> GetAPILogsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);
            APILogsRepository aPILogsRepository;
            aPILogsRepository = new APILogsRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<APILogs> APILogss = aPILogsRepository.GetAPILogs(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            APILogss = filter.GetFilteredQuery<APILogs>(nonListQueryOperation, APILogss);
            int skippedPorts = queryOperations.PageIndex;
            APILogsQuery APILogsQuery = new APILogsQuery(aPILogsRepository);
            IQueryable<APILogsList> query2 = APILogsQuery.GetIQueryableEntityList(APILogss);

            query2 = filter.GetFilteredQuery<APILogsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APILogsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> APILogsObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("APILogs", tenant).ToList();

                ObjectField objectField = (from a in APILogsObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<APILogsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<APILogsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<APILogsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<APILogsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<APILogsList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAPILogsFiltersCount(byte[] xmlFilters, int tenant)
        {
            APILogsRepository aPILogsRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);

            aPILogsRepository = new APILogsRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<APILogs> APILogss = aPILogsRepository.GetAPILogs(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            APILogss = filter.GetFilteredQuery<APILogs>(nonListQueryOperation, APILogss);
            APILogsQuery APILogsQuery = new APILogsQuery(aPILogsRepository);
            IQueryable<APILogsList> query2 = APILogsQuery.GetIQueryableEntityList(APILogss);

            query2 = filter.GetFilteredQuery<APILogsList>(listQueryOperation, query2);
            int count = query2.Take(1001).Count();
            return count;
        }

        public void InsertAPILogs(APILogsPM currentAPILogs)
        {
            SecurityUtility.AuthenticationOnTenant(currentAPILogs.Tenant);
            SecurityUtility.CheckContactFeature("APILogs", "NEW", currentAPILogs.Tenant);
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentAPILogs.Tenant);
            }

            APILogsService service = new APILogsService(objectContext, currentAPILogs.Tenant);
            service.Create(currentAPILogs);

            TableLastUpdateClass.UpdateTableHistory(currentAPILogs.Tenant, "APILogs");

        }

        public void UpdateAPILogs(APILogsPM currentAPILogs)
        {
            SecurityUtility.AuthenticationOnTenant(currentAPILogs.Tenant);
            SecurityUtility.CheckContactFeature("APILogs", "NEW", currentAPILogs.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentAPILogs.Tenant);
            }

            string entityName = "APILogs" + currentAPILogs.Id + currentAPILogs.Tenant;
            string entityPmName = "APILogsPM" + currentAPILogs.Id + currentAPILogs.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            APILogsService service = new APILogsService(objectContext, currentAPILogs.Tenant);
            service.Update(currentAPILogs);
            TableLastUpdateClass.UpdateTableHistory(currentAPILogs.Tenant, "APILogs");
        }

        public void UpdateAPILogs(APILogsList currentAPILogs)
        {

        }

        public APILogsPM GetSingleAPILogsPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }

            APILogsQuery APILogsQuery = new APILogsQuery(tenant);
            APILogsDataQuery APILogsDataQuery = new APILogsDataQuery(tenant);
            APILogsPM entityPM = APILogsQuery.GetSinglePM(id, tenant);
            var APILogsDataPM = APILogsDataQuery.GetSinglePM(id, tenant);
            entityPM.ExceptionsMessage = APILogsDataPM.ExceptionsMessage;
            entityPM.DiagnosticLog = APILogsDataPM.DiagnosticLog;
            entityPM.RequestData = APILogsDataPM.RequestData;
            entityPM.ResponseData = APILogsDataPM.ResponseData;
            return entityPM;
        }

        public APILogsDataPM GetSingleAPILogsDataPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("APILogsData", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }

            APILogsDataQuery aPILogsDataQuery = new APILogsDataQuery(tenant);
            APILogsDataPM entityPM = aPILogsDataQuery.GetSinglePM(id, tenant);

            return entityPM;
        }
    }
}