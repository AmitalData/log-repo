using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private CommodityRepository commodityRepository;
        private CommodityQuery commodityQuery;

        public void UpdateCommodityList(CommodityList currentEntity)
        {
        }

        public CommodityPM GetSingleCommodityPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "READ", tenant);

            commodityQuery = new CommodityQuery(tenant);
            return commodityQuery.GetSinglePM(id, tenant);
        }

        public CommodityList GetSingleCommodityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "READ", tenant);

            commodityRepository = new CommodityRepository(tenant);
            CommodityList entityList = null;

            Commodity entityPoco = commodityRepository.GetSingleCommodity(id, tenant);

            if (entityPoco != null)
            {
                List<Commodity> singleEntityList = new List<Commodity>();
                singleEntityList.Add(entityPoco);

                commodityQuery = new CommodityQuery(commodityRepository);
                IQueryable<Commodity> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CommodityList> iQueryableEntityList = commodityQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<CommodityList> GetCommodityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "READ", tenant);

            commodityRepository = new CommodityRepository(tenant);
            commodityQuery = new CommodityQuery(commodityRepository);

            IQueryable<Commodity> iQueryable = commodityRepository.GetCommodities(tenant);
            IQueryable<CommodityList> query2 = commodityQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CommodityList> GetCommodityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "READ", tenant);

            commodityRepository = new CommodityRepository(tenant);
            commodityQuery = new CommodityQuery(commodityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Commodity> iQueryable = commodityRepository.GetCommodities(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Commodity>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CommodityList> query2 = commodityQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CommodityList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PackageList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Commodity", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CommodityList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CommodityList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CommodityList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CommodityList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CommodityList, bool>(queryOperations, query2);
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

        public int GetCommodityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "READ", tenant);

            commodityRepository = new CommodityRepository(tenant);
            commodityQuery = new CommodityQuery(commodityRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Commodity> iQueryable = commodityRepository.GetCommodities(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Commodity>(nonListQueryOperation, iQueryable);

            IQueryable<CommodityList> query2 = commodityQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CommodityList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCommodity(CommodityPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Commodity", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommodityService service = new CommodityService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateCommodity(CommodityPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Commodity", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CommodityService service = new CommodityService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteCommodity(CommodityPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            commodityRepository = new CommodityRepository(objectContext);

            Commodity entity = commodityRepository.GetSingleCommodity(entityPM.Id, entityPM.Tenant);
            if (entity != null)
            {
                commodityRepository.Remove(entity);
            }
        }

        [Invoke]
        public void CopyCommodityToTenant(string myTenantZeroCommodityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Commodity", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            commodityRepository = new CommodityRepository(0);
            Commodity myTenantZeroCommodity = commodityRepository.GetSingleCommodity(myTenantZeroCommodityId, 0);
            if (myTenantZeroCommodity != null)
            {
                bool isCommodityExists = commodityRepository.IsCommodityExists(myTenantZeroCommodity.Code, tenant);

                if (!isCommodityExists)
                {
                    CommodityPM entityPM = new CommodityPM()
                    {
                        Tenant = tenant,
                        Code = myTenantZeroCommodity.Code,
                        Name = myTenantZeroCommodity.Name,
                        AirlineId = myTenantZeroCommodity.AirlineId,
                        InActive = myTenantZeroCommodity.InActive,
                        SearchFields = myTenantZeroCommodity.SearchFields,
                    };

                    CommodityService service = new CommodityService(objectContext, tenant);
                    service.Create(entityPM);

                    objectContext.SaveChanges();
                }
            }
        }
    }
}