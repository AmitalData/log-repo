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
        public List<SharedLogisticsUpdatePM> GetSharedLogisticsUpdatesForEntity(string entityId, int tenant)
        {
            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            return sharedLogisticsUpdateQuery.GetSharedLogisticsUpdatePMsForEntity(entityId, tenant);
        }

        public void UpdateSharedLogisticsUpdateList(SharedLogisticsUpdateList currentEntity)
        {
        }

        public IQueryable<SharedLogisticsUpdate> GetSharedLogisticsUpdate(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            return sharedLogisticsUpdateRepository.GetSharedLogisticsUpdates();
        }

        public List<SharedLogisticsUpdatePM> GetSharedLogisticsUpdateByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            return sharedLogisticsUpdateQuery.GetSharedLogisticsUpdatePMs();
        }

        public SharedLogisticsUpdatePM GetSingleSharedLogisticsUpdate(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            return sharedLogisticsUpdateQuery.GetSingleSharedLogisticUpdatePM(id);
        }

        public SharedLogisticsUpdateList GetSingleSharedLogisticsUpdateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            SharedLogisticsUpdateList sharedLogisticsUpdateList = null;
            SharedLogisticsUpdate sharedLogisticsUpdate = sharedLogisticsUpdateRepository.GetSingleSharedLogisticUpdate(id);

            if (sharedLogisticsUpdate != null)
            {
                List<SharedLogisticsUpdate> singleEntityList = new List<SharedLogisticsUpdate>();
                singleEntityList.Add(sharedLogisticsUpdate);

                IQueryable<SharedLogisticsUpdate> iQueryable = singleEntityList.AsQueryable();
                sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
                IQueryable<SharedLogisticsUpdateList> iQueryableEntityList = sharedLogisticsUpdateQuery.GetIQueryableEntityList(iQueryable);
                sharedLogisticsUpdateList = iQueryableEntityList.FirstOrDefault();
            }
            return sharedLogisticsUpdateList;
        }

        public IQueryable<SharedLogisticsUpdateList> GetSharedLogisticsUpdateLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            IQueryable<SharedLogisticsUpdate> iQueryable = sharedLogisticsUpdateRepository.GetSharedLogisticsUpdates();
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            IQueryable<SharedLogisticsUpdateList> query2 = sharedLogisticsUpdateQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<SharedLogisticsUpdateList> GetSharedLogisticsUpdateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<SharedLogisticsUpdate> iQueryable = sharedLogisticsUpdateRepository.GetSharedLogisticsUpdates();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SharedLogisticsUpdate>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            IQueryable<SharedLogisticsUpdateList> query2 = sharedLogisticsUpdateQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<SharedLogisticsUpdateList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SharedLogisticsUpdateList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SharedLogisticsUpdate", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsUpdateList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsUpdateList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsUpdateList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsUpdateList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SharedLogisticsUpdateList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Subject);
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

        public int GetSharedLogisticsUpdateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<SharedLogisticsUpdate> iQueryable = sharedLogisticsUpdateRepository.GetSharedLogisticsUpdates();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SharedLogisticsUpdate>(nonListQueryOperation, iQueryable);
            sharedLogisticsUpdateQuery = new SharedLogisticsUpdateQuery(sharedLogisticsUpdateRepository);
            IQueryable<SharedLogisticsUpdateList> query2 = sharedLogisticsUpdateQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<SharedLogisticsUpdateList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapSharedLogisticsUpdatePMSharedLogisticsUpdate(SharedLogisticsUpdatePM sharedLogisticsUpdatePM, SharedLogisticsUpdate sharedLogisticsUpdate)
        //{
        //    sharedLogisticsUpdate.EntityId = sharedLogisticsUpdatePM.EntityId;
        //    sharedLogisticsUpdate.DocumentId = sharedLogisticsUpdatePM.DocumentId;
        //    sharedLogisticsUpdate.HandledByUserId = sharedLogisticsUpdatePM.HandledByUserId;
        //    sharedLogisticsUpdate.HandledDate = sharedLogisticsUpdatePM.HandledDate;
        //    sharedLogisticsUpdate.ObjectTableId = sharedLogisticsUpdatePM.ObjectTableId;
        //    sharedLogisticsUpdate.Read = sharedLogisticsUpdatePM.Read;
        //    sharedLogisticsUpdate.ReceivedDate = sharedLogisticsUpdatePM.ReceivedDate;
        //    sharedLogisticsUpdate.ReceivedFrom = sharedLogisticsUpdatePM.ReceivedFrom;
        //    sharedLogisticsUpdate.Status = sharedLogisticsUpdatePM.Status;
        //    sharedLogisticsUpdate.Subject = sharedLogisticsUpdatePM.Subject;
        //}

        public void InsertSharedLogisticsUpdate(SharedLogisticsUpdatePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            SharedLogisticsUpdateService service = new SharedLogisticsUpdateService(objectContext , entity.Tenant);
            service.Create(entity);
              
            //sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(objectContext);
            //SharedLogisticsUpdate newSharedLogisticsUpdate = new SharedLogisticsUpdate();
            //entity.Id = IdCounter.GetNumber("SharedLogisticsUpdate", entity.Tenant).ToString();
            //newSharedLogisticsUpdate.Id = entity.Id;
            //MapSharedLogisticsUpdatePMSharedLogisticsUpdate(entity, newSharedLogisticsUpdate);
            //sharedLogisticsUpdateRepository.Add(newSharedLogisticsUpdate);
        }

        public void UpdateSharedLogisticsUpdate(SharedLogisticsUpdatePM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            SharedLogisticsUpdateService service = new SharedLogisticsUpdateService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(objectContext);

            //currentEntity.HandledDate = TenantServerConfigration.GetCurrentDateTime(currentEntity.Tenant);
            //SharedLogisticsUpdate sharedLogisticsUpdate = sharedLogisticsUpdateRepository.GetSingleSharedLogisticUpdate(currentEntity.Id);
            //MapSharedLogisticsUpdatePMSharedLogisticsUpdate(currentEntity, sharedLogisticsUpdate);
            //sharedLogisticsUpdateRepository.Update(sharedLogisticsUpdate);
        }

        public void DeleteSharedLogisticsUpdate(SharedLogisticsUpdatePM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            sharedLogisticsUpdateRepository = new SharedLogisticsUpdateRepository(objectContext);
            SharedLogisticsUpdate sharedLogisticsUpdate = sharedLogisticsUpdateRepository.GetSingleSharedLogisticUpdate(entity.Id);
            sharedLogisticsUpdateRepository.Remove(sharedLogisticsUpdate);
        }
    }
}