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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateRegionList(RegionList currentEntity)
        {
        }

      

        public IQueryable<RegionPM> GetRegionsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Region", "READ", tenant);

            regionQuery = new RegionQuery(tenant);
            return regionQuery.GetRegionPMs(tenant);
        }

        public RegionPM GetSingleRegionPM(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Region", "READ", tenant);

            regionQuery = new RegionQuery(tenant);
            return regionQuery.GetSingleRegionPM(Id, tenant);
        }



        public RegionList GetSingleRegionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Region", "READ", tenant);

            regionRepository = new RegionRepository(tenant);
            regionQuery = new RegionQuery(regionRepository);
            RegionList RegionList = null;
            Region Region = regionRepository.GetSingleRegion(id, tenant);

            if (Region != null)
            {
                List<Region> singleEntityList = new List<Region>();
                singleEntityList.Add(Region);

                IQueryable<Region> iQueryable = singleEntityList.AsQueryable();
                IQueryable<RegionList> iQueryableEntityList = regionQuery.GetIQueryableEntityList(iQueryable);
                RegionList = iQueryableEntityList.FirstOrDefault();
            }
            return RegionList;
        }

        public IQueryable<RegionList> GetRegionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Region", "READ", tenant);


            regionRepository = new RegionRepository(tenant);
            regionQuery = new RegionQuery(regionRepository);
            IQueryable<Region> Regions = regionRepository.GetRegions(tenant);
            IQueryable<RegionList> query2 = regionQuery.GetIQueryableEntityList(Regions).AsQueryable();
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RegionList> GetRegionFilters(byte[] xmlFilters, int tenant)
        {
            regionRepository = new RegionRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Region> iQueryable = regionRepository.GetRegions(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Region>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new RegionList()
                         {
                         
                             Id = entity.Id,
                             Name = entity.Name,
                             LocalName = entity.LocalName,
                             Tenant = entity.Tenant,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<RegionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(RegionList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> secialServicesTypetObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Region", tenant).ToList();

                ObjectField objectField = (from a in secialServicesTypetObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RegionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RegionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RegionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RegionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RegionList, bool>(queryOperations, query2);
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

        public int GetRegionCount(byte[] xmlFilters, int tenant)
        {
            regionRepository = new RegionRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Region> iQueryable = regionRepository.GetRegions(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Region>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new RegionList()
                         {
                            

                             Id = entity.Id,
                             Name = entity.Name,
                             LocalName = entity.LocalName,
                             Tenant = entity.Tenant,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<RegionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAccount(RegionPM entityPM)
        {

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Region", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            RegionService service = new RegionService(objectContext, entityPM.Tenant);
            service.Create(entityPM);



        }

        public void UpdateAccount(RegionPM currentEntityPM)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntityPM.Tenant);
            SecurityUtility.CheckContactFeature("Region", "UPDATE", currentEntityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntityPM.Tenant);
            }
            RegionService service = new RegionService(objectContext, currentEntityPM.Tenant);
            service.Update(currentEntityPM);

        }
    }
}