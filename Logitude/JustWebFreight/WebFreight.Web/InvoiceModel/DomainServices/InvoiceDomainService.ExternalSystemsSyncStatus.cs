using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public ExternalSystemsSyncStatusPM GetSingleExternalSystemsSyncStatusPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "READ", tenant);

            externalSystemsSyncStatusQuery = new ExternalSystemsSyncStatusQuery(tenant);
            return externalSystemsSyncStatusQuery.GetSingleExternalSystemsSyncStatusPM(id, tenant);
        }

        public void UpdateExternalSystemsSyncStatusList(ExternalSystemsSyncStatusList currentEntity)
        {

        }

        public ExternalSystemsSyncStatusList GetSingleExternalSystemsSyncStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "READ", tenant);

            ExternalSystemsSyncStatusList entityList = null;
            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(tenant);
            externalSystemsSyncStatusQuery = new ExternalSystemsSyncStatusQuery(externalSystemsSyncStatusRepository);
            ExternalSystemsSyncStatus entity = externalSystemsSyncStatusRepository.GetSingleExternalSystemsSyncStatus(id, tenant);

            if (entity != null)
            {
                List<ExternalSystemsSyncStatus> SingleEntityList = new List<ExternalSystemsSyncStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ExternalSystemsSyncStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ExternalSystemsSyncStatusList> iQueryableEntityList = externalSystemsSyncStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }


        public IQueryable<ExternalSystemsSyncStatusList> GetExternalSystemsSyncStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(tenant);
            externalSystemsSyncStatusQuery = new ExternalSystemsSyncStatusQuery(externalSystemsSyncStatusRepository);
            IQueryable<ExternalSystemsSyncStatus> ExternalSystemsSyncStatuss = externalSystemsSyncStatusRepository.GetExternalSystemsSyncStatussByTenant(tenant);

            IQueryable<ExternalSystemsSyncStatusList> query2 = externalSystemsSyncStatusQuery.GetIQueryableEntityList(ExternalSystemsSyncStatuss);
            return query2;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<ExternalSystemsSyncStatusList> GetExternalSystemsSyncStatusFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "READ", tenant);

            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(tenant);
            externalSystemsSyncStatusQuery = new ExternalSystemsSyncStatusQuery(externalSystemsSyncStatusRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsSyncStatus> zones = externalSystemsSyncStatusRepository.GetExternalSystemsSyncStatuss(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsSyncStatus>(nonListQueryOperation, zones);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExternalSystemsSyncStatusList> query2 = externalSystemsSyncStatusQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsSyncStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExternalSystemsSyncStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ExternalSystemsSyncStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsSyncStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsSyncStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsSyncStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsSyncStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ExternalSystemsSyncStatusList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Subject);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Subject);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }
        public int GetExternalSystemsSyncStatusFiltersCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "READ", tenant);

            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(tenant);
            externalSystemsSyncStatusQuery = new ExternalSystemsSyncStatusQuery(externalSystemsSyncStatusRepository);

            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ExternalSystemsSyncStatus> zones = externalSystemsSyncStatusRepository.GetExternalSystemsSyncStatuss(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<ExternalSystemsSyncStatus>(nonListQueryOperation, zones);

            IQueryable<ExternalSystemsSyncStatusList> query2 = externalSystemsSyncStatusQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<ExternalSystemsSyncStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }



        public void InsertExternalSystemsSyncStatus(ExternalSystemsSyncStatusPM ExternalSystemsSyncStatus)
        {
            //  SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "NEW", ExternalSystemsSyncStatus.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(ExternalSystemsSyncStatus.Tenant);
            }
            ExternalSystemsSyncStatusService service = new ExternalSystemsSyncStatusService(objectContext, ExternalSystemsSyncStatus.Tenant);
            service.Create(ExternalSystemsSyncStatus);


            TableLastUpdateClass.UpdateTableHistory(ExternalSystemsSyncStatus.Tenant, "ExternalSystemsSyncStatus");
        }

        public void UpdateExternalSystemsSyncStatus(ExternalSystemsSyncStatusPM currentExternalSystemsSyncStatus)
        {
            SecurityUtility.CheckContactFeature("ExternalSystemsSyncStatus", "UPDATE", currentExternalSystemsSyncStatus.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(currentExternalSystemsSyncStatus.Tenant);
            }


            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(objectContext);

            string entityName = "ExternalSystemsSyncStatus" + currentExternalSystemsSyncStatus.Id + currentExternalSystemsSyncStatus.Tenant;
            string entityPmName = "ExternalSystemsSyncStatusPM" + currentExternalSystemsSyncStatus.Id + currentExternalSystemsSyncStatus.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            ExternalSystemsSyncStatusService service = new ExternalSystemsSyncStatusService(objectContext, currentExternalSystemsSyncStatus.Tenant);
            service.Update(currentExternalSystemsSyncStatus);
            TableLastUpdateClass.UpdateTableHistory(currentExternalSystemsSyncStatus.Tenant, "ExternalSystemsSyncStatus");


        }

        public void DeleteExternalSystemsSyncStatus(ExternalSystemsSyncStatusPM ExternalSystemsSyncStatus)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(ExternalSystemsSyncStatus.Tenant);
            }
            externalSystemsSyncStatusRepository = new ExternalSystemsSyncStatusRepository(objectContext);
            ExternalSystemsSyncStatus entity = externalSystemsSyncStatusRepository.GetSingleExternalSystemsSyncStatus(ExternalSystemsSyncStatus.Id, ExternalSystemsSyncStatus.Tenant);
            externalSystemsSyncStatusRepository.Remove(entity);
        }


    }
}