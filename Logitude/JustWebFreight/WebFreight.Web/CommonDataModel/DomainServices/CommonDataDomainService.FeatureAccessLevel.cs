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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private FeatureAccessLevelRepository featureAccessLevelRepository;

        public void UpdateFeatureAccessLevel(FeatureAccessLevelList entityList)
        {

        }

        public FeatureAccessLevelList GetSingleFeatureAccessLevelList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureAccessLevelRepository = new FeatureAccessLevelRepository(tenant);
            FeatureAccessLevelQuery featureAccessLevelQuery = new FeatureAccessLevelQuery(featureAccessLevelRepository);

            FeatureAccessLevelList entityList = null;
            FeatureAccessLevel entityPOCO = featureAccessLevelRepository.GetSingleFeatureAccessLevel(code);

            if (entityPOCO != null)
            {
                List<FeatureAccessLevel> singleEntityList = new List<FeatureAccessLevel>();
                singleEntityList.Add(entityPOCO);

                IQueryable<FeatureAccessLevel> iQueryable = singleEntityList.AsQueryable();
                IQueryable<FeatureAccessLevelList> iQueryableEntityList = featureAccessLevelQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<FeatureAccessLevelList> GetFeatureAccessLevelLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureAccessLevelRepository = new FeatureAccessLevelRepository(tenant);
            FeatureAccessLevelQuery featureAccessLevelQuery = new FeatureAccessLevelQuery(featureAccessLevelRepository);
            IQueryable<FeatureAccessLevel> entities = featureAccessLevelRepository.GetFeatureAccessLevels();
            IQueryable<FeatureAccessLevelList> query2 = featureAccessLevelQuery.GetIQueryableEntityList(entities);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<FeatureAccessLevelList> GetFeatureAccessLevelFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            featureAccessLevelRepository = new FeatureAccessLevelRepository(tenant);
            FeatureAccessLevelQuery featureAccessLevelQuery = new FeatureAccessLevelQuery(featureAccessLevelRepository);
            IQueryable<FeatureAccessLevel> entities = featureAccessLevelRepository.GetFeatureAccessLevels();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            entities = filter.GetFilteredQuery<FeatureAccessLevel>(nonListQueryOperation, entities);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<FeatureAccessLevelList> query2 = featureAccessLevelQuery.GetIQueryableEntityList(entities);

            query2 = filter.GetFilteredQuery<FeatureAccessLevelList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(FeatureAccessLevelList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("FeatureAccessLevel", tenant).ToList();

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<FeatureAccessLevelList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<FeatureAccessLevelList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<FeatureAccessLevelList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<FeatureAccessLevelList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<FeatureAccessLevelList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetFeatureAccessLevelFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            featureAccessLevelRepository = new FeatureAccessLevelRepository(tenant);
            FeatureAccessLevelQuery featureAccessLevelQuery = new FeatureAccessLevelQuery(featureAccessLevelRepository);
            IQueryable<FeatureAccessLevel> entities = featureAccessLevelRepository.GetFeatureAccessLevels();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            entities = filter.GetFilteredQuery<FeatureAccessLevel>(nonListQueryOperation, entities);

            IQueryable<FeatureAccessLevelList> query2 = featureAccessLevelQuery.GetIQueryableEntityList(entities);

            query2 = filter.GetFilteredQuery<FeatureAccessLevelList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}