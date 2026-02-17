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
        public void UpdateCustomPickList(CustomPickListList currentEntity)
        {
        }

        public IQueryable<CustomPickList> GetCustomPickLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            return customPickListsRepository.GetCustomPickLists(tenant);
        }


        public IQueryable<CustomPickList> GetCustomPickListIS(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            return customPickListsRepository.GetCustomPickLists(tenant);
        }


        public IQueryable<CustomPickListPM> GetCustomPickListsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);

            return customPickListQuery.GetCustomPickListPMsByTenant(tenant);
        }

        public IQueryable<CustomPickListPM> GetCustomPickListsByCodeTenant(int tenant, string code)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            return customPickListQuery.GetCustomPickListPMsByCode(tenant, code);
        }

        public CustomPickListPM GetSingleCustomPickListPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            return customPickListQuery.GetSinglePM(id, tenant);
        }

        public CustomPickListList GetSingleCustomPickListList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);
            CustomPickListList customPickListList = null;
            CustomPickList customPickList = customPickListsRepository.GetSingleCustomPickList(id, tenant);

            if (customPickList != null)
            {
                List<CustomPickList> singleEntityList = new List<CustomPickList>();
                singleEntityList.Add(customPickList);

                IQueryable<CustomPickList> iQueryable = singleEntityList.AsQueryable();
                customPickListQuery = new CustomPickListQuery(customPickListsRepository);
                IQueryable<CustomPickListList> iQueryableEntityList = customPickListQuery.GetIQueryableEntityList(iQueryable);
                customPickListList = iQueryableEntityList.FirstOrDefault();
            }
            return customPickListList;
        }

        public IQueryable<CustomPickListList> GetCustomPickListLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);

            IQueryable<CustomPickList> iQueryable = customPickListsRepository.GetCustomPickLists(tenant);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            IQueryable<CustomPickListList> query2 = customPickListQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }




        public List<CustomPickListList> GetCustomPickListListsIsMultipleChoice(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);

      
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            List<CustomPickListList> query2 = customPickListQuery.GetCustomPickListListIsMultipleChoice();
            return query2;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<CustomPickListList> GetCustomPickListFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomPickList> iQueryable = customPickListsRepository.GetCustomPickLists(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomPickList>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            IQueryable<CustomPickListList> query2 = customPickListQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomPickListList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomPickListList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomPickList", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomPickListList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomPickListList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomPickListList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomPickListList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomPickListList, bool>(queryOperations, query2);
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

        public int GetCustomPickListFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
            }
            customPickListsRepository = new CustomPickListRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomPickList> iQueryable = customPickListsRepository.GetCustomPickLists(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomPickList>(nonListQueryOperation, iQueryable);
            customPickListQuery = new CustomPickListQuery(customPickListsRepository);
            IQueryable<CustomPickListList> query2 = customPickListQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomPickListList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //void MapCustomPickListCustomPickListPM(CustomPickListPM picklistpm, CustomPickList picklist)
        //{
        //    picklist.Code = picklistpm.Code;
        //    picklist.Tenant = picklistpm.Tenant;
        //    picklist.Value = picklistpm.Value;
        //}

        public void InsertCustomPickList(CustomPickListPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            CustomPickListService service = new CustomPickListService(objectContext, entity.Tenant);
            service.Create(entity);

            //customPickListsRepository = new CustomPickListRepository(objectContext);

            //CustomPickList newCustomPickList = new CustomPickList();
            //newCustomPickList.Id = entity.Id = IdCounter.GetNumber("CustomPickList", entity.Tenant);
            //MapCustomPickListCustomPickListPM(entity, newCustomPickList);
            //customPickListsRepository.Add(newCustomPickList);
        }

        public void UpdateCustomPickList(CustomPickListPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            CustomPickListService service = new CustomPickListService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //string entityName = "CustomPickList" + currentEntity.Id + currentEntity.Tenant;
            //if (CacheManager.CacheWrapper.Get(entityName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityName);
            //}

            //customPickListsRepository = new CustomPickListRepository(objectContext);
            //CustomPickList entity = customPickListsRepository.GetSingleCustomPickList(currentEntity.Id, currentEntity.Tenant);
            //MapCustomPickListCustomPickListPM(currentEntity, entity);
            //customPickListsRepository.Update(entity);
        }

        public void DeleteCustomPickList(CustomPickListPM entity)
        {
            string entityName = "CustomPickList" + entity.Id + entity.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            customPickListsRepository = new CustomPickListRepository(objectContext);
            var poco = customPickListsRepository.GetSingleCustomPickList(entity.Id, entity.Tenant);
            customPickListsRepository.Remove(poco);
        }
    }
}