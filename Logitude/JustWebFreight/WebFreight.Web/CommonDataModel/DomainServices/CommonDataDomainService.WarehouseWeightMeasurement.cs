using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
        private WarehouseWeightMeasurementRepository WarehouseWeightMeasurementRepository;
        private WarehouseWeightMeasurementQuery WarehouseWeightMeasurementQuery;

        public void UpdateWarehouseWeightMeasurementList(WarehouseWeightMeasurementList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<WarehouseWeightMeasurementList> GetWarehouseWeightMeasurementFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            WarehouseWeightMeasurementRepository = new WarehouseWeightMeasurementRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<WarehouseWeightMeasurement> iQueryable = WarehouseWeightMeasurementRepository.GetWarehouseWeightMeasurements();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseWeightMeasurement>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            WarehouseWeightMeasurementQuery = new WarehouseWeightMeasurementQuery(WarehouseWeightMeasurementRepository);
            IQueryable<WarehouseWeightMeasurementList> query2 = WarehouseWeightMeasurementQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseWeightMeasurementList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WarehouseWeightMeasurementList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WarehouseWeightMeasurement", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightMeasurementList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightMeasurementList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightMeasurementList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightMeasurementList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseWeightMeasurementList, bool>(queryOperations, query2);
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

        public int GetWarehouseWeightMeasurementFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            WarehouseWeightMeasurementRepository = new WarehouseWeightMeasurementRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<WarehouseWeightMeasurement> iQueryable = WarehouseWeightMeasurementRepository.GetWarehouseWeightMeasurements();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WarehouseWeightMeasurement>(nonListQueryOperation, iQueryable);
            WarehouseWeightMeasurementQuery = new WarehouseWeightMeasurementQuery(WarehouseWeightMeasurementRepository);
            IQueryable<WarehouseWeightMeasurementList> query2 = WarehouseWeightMeasurementQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseWeightMeasurementList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}