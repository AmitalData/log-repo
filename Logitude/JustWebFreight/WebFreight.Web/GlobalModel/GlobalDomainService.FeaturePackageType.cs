using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityQueries;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
	{
        FeaturePackageTypeRepository myFeaturePackageTypeRepository;

        public void UpdateFeaturePackageTypeList(FeaturePackageTypeList entityList)
        {

        }

        public FeaturePackageTypeList GetSingleFeaturePackageTypeList(string code, int tenant)
        {
            myFeaturePackageTypeRepository = new FeaturePackageTypeRepository();
            FeaturePackageType entity = myFeaturePackageTypeRepository.GetSingleFeaturePackageType(code);

            FeaturePackageTypeList entityList = null;
            FeaturePackageTypeQuery entityQuery = new FeaturePackageTypeQuery(myFeaturePackageTypeRepository);

            if (entity != null)
            {
                List<FeaturePackageType> singleEntityList = new List<FeaturePackageType>();
                singleEntityList.Add(entity);

                IQueryable<FeaturePackageType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<FeaturePackageTypeList> iQueryableEntityList = entityQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<FeaturePackageTypeList> GetFeaturePackageTypeLists(int tenant)
        {
            myFeaturePackageTypeRepository = new FeaturePackageTypeRepository();
            IQueryable<FeaturePackageType> allEtnties = myFeaturePackageTypeRepository.GetFeaturePackageTypes();

            FeaturePackageTypeQuery entityQuery = new FeaturePackageTypeQuery(myFeaturePackageTypeRepository);
            IQueryable<FeaturePackageTypeList> query2 = entityQuery.GetIQueryableEntityList(allEtnties);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public List<FeaturePackageTypeList> GetFeaturePackageTypeFilters(byte[] xmlFilters, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            myFeaturePackageTypeRepository = new FeaturePackageTypeRepository();
            IQueryable<FeaturePackageType> allRecords = myFeaturePackageTypeRepository.GetFeaturePackageTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            allRecords = filter.GetFilteredQuery<FeaturePackageType>(nonListQueryOperation, allRecords);

            int skippedRecords = queryOperations.PageIndex;

            FeaturePackageTypeQuery entityQuery = new FeaturePackageTypeQuery(myFeaturePackageTypeRepository);
            IQueryable<FeaturePackageTypeList> query2 = entityQuery.GetIQueryableEntityList(allRecords);
            query2 = filter.GetFilteredQuery<FeaturePackageTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(FeaturePackageTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("FeaturePackageType", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<FeaturePackageTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderBy(d => d.Name);
            }

            query2 = query2.Skip(skippedRecords);
            query2 = query2.Take(queryOperations.PageSize);

            return query2.ToList();
        }

        public int GetFeaturePackageTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            myFeaturePackageTypeRepository = new FeaturePackageTypeRepository();
            IQueryable<FeaturePackageType> allRecords = myFeaturePackageTypeRepository.GetFeaturePackageTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            allRecords = filter.GetFilteredQuery<FeaturePackageType>(nonListQueryOperation, allRecords);

            FeaturePackageTypeQuery entityQuery = new FeaturePackageTypeQuery(myFeaturePackageTypeRepository);
            IQueryable<FeaturePackageTypeList> query2 = entityQuery.GetIQueryableEntityList(allRecords);
            query2 = filter.GetFilteredQuery<FeaturePackageTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}