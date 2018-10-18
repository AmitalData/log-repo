using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using System;
using System.Collections.Generic;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateMoveTypeList(MoveTypeList list)
        {

        }

        public IQueryable<MoveType> GetMoveTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeRepository = new MoveTypeRepository(tenant);
            return moveTypeRepository.GetMoveTypesByTenant(0);
        }

        public IQueryable<MoveTypePM> GetMoveTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeQuery = new MoveTypeQuery(tenant);
            return moveTypeQuery.GetMoveTypePMs(tenant).Where(d => d.Tenant == tenant);
        }

        public bool DoesMoveTypeExist(string code, int tenant)
        {
            moveTypeRepository = new MoveTypeRepository(tenant);
            return (moveTypeRepository.GetMoveTypesByTenant(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<MoveTypeList> GetMoveTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeRepository = new MoveTypeRepository(tenant);
            moveTypeQuery = new MoveTypeQuery(moveTypeRepository);

            IQueryable<MoveType> iQueryable = moveTypeRepository.GetMoveTypesByTenant(tenant);
            IQueryable<MoveTypeList> query2 = moveTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<MoveTypeList> GetMoveTypeFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeRepository = new MoveTypeRepository(tenant);
            moveTypeQuery = new MoveTypeQuery(moveTypeRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MoveType> iQueryable = moveTypeRepository.GetMoveTypesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MoveType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<MoveTypeList> query2 = moveTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<MoveTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MoveTypeList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<MoveTypeList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<MoveTypeList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<MoveTypeList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<MoveTypeList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Code);
                            break;
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

        public int GetMoveTypeCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeRepository = new MoveTypeRepository(tenant);
            moveTypeQuery = new MoveTypeQuery(moveTypeRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MoveType> iQueryable = moveTypeRepository.GetMoveTypesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MoveType>(nonListQueryOperation, iQueryable);

            IQueryable<MoveTypeList> query2 = moveTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<MoveTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public MoveTypeList GetSingleMoveTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            MoveTypeList entityList = null;
            moveTypeRepository = new MoveTypeRepository(tenant);
            moveTypeQuery = new MoveTypeQuery(moveTypeRepository);
            MoveType entity = moveTypeRepository.GetSingleMoveType(id, tenant);

            if (entity != null)
            {
                List<MoveType> singleEntityList = new List<MoveType>();
                singleEntityList.Add(entity);

                IQueryable<MoveType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<MoveTypeList> iQueryableEntityList = moveTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public MoveTypePM GetSingleMoveType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("MoveType", "READ", tenant);

            moveTypeQuery = new MoveTypeQuery(tenant);
            return moveTypeQuery.GetSingleMoveTypePM(id, tenant);
        }

        public void InsertMoveType(MoveTypePM entityPM)
        {
            SecurityUtility.CheckContactFeature("MoveType", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            }

            MoveTypeService servie = new MoveTypeService(objectContext, entityPM.Tenant);
            servie.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "MoveType");
        }

        public void UpdateMoveType(MoveTypePM entityPM)
        {
            SecurityUtility.CheckContactFeature("MoveType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            }

            string entityName = "MoveType" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "MoveTypePM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Remove(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Remove(entityPmName);
            }

            MoveTypeService service = new MoveTypeService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "MoveType");
        }

        public void DeleteMoveType(MoveTypePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            }

            moveTypeRepository = new MoveTypeRepository(objectContext);

            MoveType entity = moveTypeRepository.GetSingleMoveType(entityPM.Id, entityPM.Tenant);
            moveTypeRepository.Remove(entity);
        }
    }
}