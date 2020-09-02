using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private WarehouseWeightRoundingRepository WarehouseWeightRoundingRepository;
        private WarehouseWeightRoundingQuery WarehouseWeightRoundingQuery;

        public void UpdateWarehouseWeightRoundingList(WarehouseWeightRoundingList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<WarehouseWeightRoundingList> GetWarehouseWeightRoundingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            WarehouseWeightRoundingRepository = new WarehouseWeightRoundingRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<WarehouseWeightRounding> iQueryable = WarehouseWeightRoundingRepository.GetWarehouseWeightRoundings();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseWeightRounding>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            WarehouseWeightRoundingQuery = new WarehouseWeightRoundingQuery(WarehouseWeightRoundingRepository);
            IQueryable<WarehouseWeightRoundingList> query2 = WarehouseWeightRoundingQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseWeightRoundingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WarehouseWeightRoundingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WarehouseWeightRounding", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightRoundingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightRoundingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightRoundingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightRoundingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightRoundingList, bool>(queryOperations, query2);
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

        public int GetWarehouseWeightRoundingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            WarehouseWeightRoundingRepository = new WarehouseWeightRoundingRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<WarehouseWeightRounding> iQueryable = WarehouseWeightRoundingRepository.GetWarehouseWeightRoundings();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseWeightRounding>(nonListQueryOperation, iQueryable);
            WarehouseWeightRoundingQuery = new WarehouseWeightRoundingQuery(WarehouseWeightRoundingRepository);
            IQueryable<WarehouseWeightRoundingList> query2 = WarehouseWeightRoundingQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseWeightRoundingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}