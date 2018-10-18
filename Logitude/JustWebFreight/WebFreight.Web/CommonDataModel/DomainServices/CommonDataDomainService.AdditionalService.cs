using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateAdditionalServiceList(AdditionalServiceList currentEntity)
        {
        }

        public IQueryable<AdditionalService> GetAdditionalServices(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceRepository = new AdditionalServiceRepository(tenant);
            return additionalServiceRepository.GetAdditionalServices(0);
        }

        public IQueryable<AdditionalServicePM> GetAdditionalServicesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceQuery = new AdditionalServiceQuery(tenant);
            return additionalServiceQuery.GetAdditionalServicePMsByTenant(tenant);
        }

        public AdditionalServicePM GetSingleAdditionalService(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceQuery = new AdditionalServiceQuery(tenant);
            return additionalServiceQuery.GetSinglePM(id, tenant);
        }
        
        public AdditionalServiceList GetSingleAdditionalServiceList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceRepository = new AdditionalServiceRepository(tenant);
            additionalServiceQuery = new AdditionalServiceQuery(additionalServiceRepository);
            AdditionalServiceList AdditionalServiceList = null;
            AdditionalService AdditionalService = additionalServiceRepository.GetSingleAdditionalService(id, tenant);

            if (AdditionalService != null)
            {
                List<AdditionalService> singleEntityList = new List<AdditionalService>();
                singleEntityList.Add(AdditionalService);

                IQueryable<AdditionalService> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AdditionalServiceList> iQueryableEntityList = additionalServiceQuery.GetIQueryableEntityList(iQueryable);
                AdditionalServiceList = iQueryableEntityList.FirstOrDefault();
            }
            return AdditionalServiceList;
        }

        public IQueryable<AdditionalServiceList> GetAdditionalServiceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceRepository = new AdditionalServiceRepository(tenant);
            additionalServiceQuery = new AdditionalServiceQuery(additionalServiceRepository);
            IQueryable<AdditionalService> AdditionalServicees = additionalServiceRepository.GetAdditionalServices(tenant);
            IQueryable<AdditionalServiceList> query2 = additionalServiceQuery.GetIQueryableEntityList(AdditionalServicees);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AdditionalServiceList> GetAdditionalServiceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceRepository = new AdditionalServiceRepository(tenant);
            additionalServiceQuery = new AdditionalServiceQuery(additionalServiceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AdditionalService> AdditionalServicees = additionalServiceRepository.GetAdditionalServices(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AdditionalServicees = filter.GetFilteredQuery<AdditionalService>(nonListQueryOperation, AdditionalServicees);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<AdditionalServiceList> query2 = additionalServiceQuery.GetIQueryableEntityList(AdditionalServicees);

            query2 = filter.GetFilteredQuery<AdditionalServiceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AdditionalServiceList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AdditionalService", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalServiceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalServiceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalServiceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalServiceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AdditionalServiceList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAdditionalServiceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("AdditionalService", "READ", tenant);

            additionalServiceRepository = new AdditionalServiceRepository(tenant);
            additionalServiceQuery = new AdditionalServiceQuery(additionalServiceRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AdditionalService> AdditionalServicees = additionalServiceRepository.GetAdditionalServices(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            AdditionalServicees = filter.GetFilteredQuery<AdditionalService>(nonListQueryOperation, AdditionalServicees);

            IQueryable<AdditionalServiceList> query2 = additionalServiceQuery.GetIQueryableEntityList(AdditionalServicees);

            query2 = filter.GetFilteredQuery<AdditionalServiceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAdditionalService(AdditionalServicePM AdditionalService)
        {
            SecurityUtility.CheckContactFeature("AdditionalService", "NEW", AdditionalService.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AdditionalService.Tenant);
            }
            AdditionalServiceService service = new AdditionalServiceService(objectContext, AdditionalService.Tenant);
            service.Create(AdditionalService);

            TableLastUpdateClass.UpdateTableHistory(AdditionalService.Tenant, "AdditionalService");
        }

        public void UpdateAdditionalService(AdditionalServicePM currentAdditionalService)
        {
            SecurityUtility.CheckContactFeature("AdditionalService", "UPDATE", currentAdditionalService.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentAdditionalService.Tenant);
            }

            additionalServiceRepository = new AdditionalServiceRepository(objectContext);

            string entityName = "AdditionalService" + currentAdditionalService.Id + currentAdditionalService.Tenant;
            string entityPmName = "AdditionalServicePM" + currentAdditionalService.Id + currentAdditionalService.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            AdditionalServiceService service = new AdditionalServiceService(objectContext, currentAdditionalService.Tenant);
            service.Update(currentAdditionalService);
        }

        public void DeleteAdditionalService(AdditionalServicePM AdditionalService)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(AdditionalService.Tenant);
            }
            additionalServiceRepository = new AdditionalServiceRepository(objectContext);
            AdditionalService entity = additionalServiceRepository.GetSingleAdditionalService(AdditionalService.Id, AdditionalService.Tenant);
            additionalServiceRepository.Remove(entity);
        }
    }
}