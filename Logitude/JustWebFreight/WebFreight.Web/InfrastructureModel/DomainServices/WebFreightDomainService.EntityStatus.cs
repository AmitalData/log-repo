using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
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

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateEntityStatusList(EntityStatusList currentEntity)
        {
        }

        public IQueryable<EntityStatus> GetEntityStatus(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            return entityStatusRepository.GetEntityStatusByTenant(0);
        }

        public IQueryable<EntityStatusPM> GetEntityStatusByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            return entityStatusQuery.GetEntityStatusPMsByTenant(tenant);
        }

        public EntityStatusPM GetSingleEntityStatus(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            return entityStatusQuery.GetSingleEntityStatuPMs(id, tenant);
        }

        public EntityStatusList GetSingleEntityStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            EntityStatusList entityStatusList = null;
            EntityStatus entityStatus = entityStatusRepository.GetSingleEntityStatus(id, tenant);

            if (entityStatus != null)
            {
                List<EntityStatus> singleEntityList = new List<EntityStatus>();
                singleEntityList.Add(entityStatus);

                IQueryable<EntityStatus> iQueryable = singleEntityList.AsQueryable();
                entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
                IQueryable<EntityStatusList> iQueryableEntityList = entityStatusQuery.GetIQueryableEntityList(iQueryable);
                entityStatusList = iQueryableEntityList.FirstOrDefault();
            }
            return entityStatusList;
        }

        public IQueryable<EntityStatusList> GetEntityStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            
            IQueryable<EntityStatus> iQueryable = entityStatusRepository.GetEntityStatusByTenant(tenant).Where(d => d.InActive == false);
            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            IQueryable<EntityStatusList> query2 = entityStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<EntityStatusList> GetEntityStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EntityStatus> iQueryable = entityStatusRepository.GetEntityStatusByTenant(tenant).Where(d => d.InActive == false);
            
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EntityStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            IQueryable<EntityStatusList> query2 = entityStatusQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<EntityStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(EntityStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("EntityStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<EntityStatusList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetEntityStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityStatusRepository = new EntityStatusRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<EntityStatus> iQueryable = entityStatusRepository.GetEntityStatusByTenant(tenant).Where(d => d.InActive == false);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<EntityStatus>(nonListQueryOperation, iQueryable);

            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            IQueryable<EntityStatusList> query2 = entityStatusQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<EntityStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapEntityStatusPMEntityStatus(EntityStatusPM entityStatusPM,EntityStatus entityStatus)
        //{
        //    objectTabelRepository = new ObjectTabelRepository(entityStatusPM.Tenant);
        //    entityStatus.Name = entityStatusPM.Name;
        //    ObjectTable table = null;
        //    if (entityStatusPM.ObjectTableId != null)
        //    {
        //        table = objectTabelRepository.GetSingleObjectTable(entityStatusPM.ObjectTableId, entityStatus.Tenant, true);
        //    }
        //    entityStatus.ObjectTableId = entityStatusPM.ObjectTableId;
        //    entityStatus.StatusWeight = entityStatusPM.StatusWeight;
        //    entityStatus.Tenant = entityStatusPM.Tenant;
        //    entityStatus.Code = entityStatusPM.Code;
        //    entityStatus.InActive = entityStatusPM.InActive;
        //    entityStatus.SearchFields = entityStatusPM.Code + "," + entityStatusPM.Name + "," + (table != null ? table.Name : "");           
        //}

        public void InsertEntityStatus(EntityStatusPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            EntityStatusService service = new EntityStatusService(objectContext , entity.Tenant);
            service.Create(entity);

            //entityStatusRepository = new EntityStatusRepository(objectContext);
            //EntityStatus newEntityStatus = new EntityStatus();
            //entity.Id = IdCounter.GetNumber("EntityStatus", entity.Tenant).ToString();
            //newEntityStatus.Id = entity.Id;
            //MapEntityStatusPMEntityStatus(entity, newEntityStatus);
            //entityStatusRepository.Add(newEntityStatus);
        }

        public void UpdateEntityStatus(EntityStatusPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            EntityStatusService service = new EntityStatusService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //entityStatusRepository = new EntityStatusRepository(objectContext);
            //string entityName = "EntityStatus" + currentEntity.Id + currentEntity.Tenant;
            //string entityPMName = "EntityStatusPM" + currentEntity.Id + currentEntity.Tenant;
            //if (CacheManager.CacheWrapper.Get(entityName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityName);
            //}
            //if (CacheManager.CacheWrapper.Get(entityPMName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityPMName);
            //}

            //EntityStatus entityStatus = EntityStatusRepository.GetSingleEntityStatus(currentEntity.Id, currentEntity.Tenant, false);
            //MapEntityStatusPMEntityStatus(currentEntity, entityStatus);
            //entityStatusRepository.Update(entityStatus);
        }

        public void DeleteEntityStatus(EntityStatusPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            entityStatusRepository = new EntityStatusRepository(objectContext);
            EntityStatus entityStatus = EntityStatusRepository.GetSingleEntityStatus(entity.Id, entity.Tenant, false);
            entityStatusRepository.Remove(entityStatus);
        }
    }
}