using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using Logitude.DashboardModule.Data.EntityLists;
using Logitude.DashboardModule.Data.Repositories;
using WebFreight.Web.Security;
using System.IO;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Reflection;
using Logitude.DashboardModule.Data;

namespace WebFreight.Web.DashboardModel.DomainServices
{
    [EnableClientAccess()]
    public class DashboardDomainService : LogitudeDomainService
    {
        [Query(HasSideEffects = true)]
        public IQueryable<MeasureTypeList> GetMeasureTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            MeasureTypeRepository MeasureTypeRepository = new MeasureTypeRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<MeasureType> iQueryable = MeasureTypeRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MeasureType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            MeasureTypeListQueryService MeasureTypeQueryService = new MeasureTypeListQueryService(context);
            IQueryable<MeasureTypeList> query2 = MeasureTypeQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<MeasureTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MeasureTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("MeasureType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MeasureTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MeasureTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MeasureTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MeasureTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MeasureTypeList, bool>(queryOperations, query2);
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
        public int GetMeasureTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            MeasureTypeRepository MeasureTypeRepository = new MeasureTypeRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<MeasureType> iQueryable = MeasureTypeRepository.GetAll();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MeasureType>(nonListQueryOperation, iQueryable);
            MeasureTypeListQueryService MeasureTypeQueryService = new MeasureTypeListQueryService(context);
            IQueryable<MeasureTypeList> query2 = MeasureTypeQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<MeasureTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<AnalyticsFactsMetaDataList> GetAnalyticsFactsMetaDataFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            AnalyticsFactsMetaDataRepository AnalyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AnalyticsFactsMetaData> iQueryable = AnalyticsFactsMetaDataRepository.GetAll(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyticsFactsMetaData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            AnalyticsFactsMetaDataListQueryService AnalyticsFactsMetaDataQueryService = new AnalyticsFactsMetaDataListQueryService(context);
            IQueryable<AnalyticsFactsMetaDataList> query2 = AnalyticsFactsMetaDataQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<AnalyticsFactsMetaDataList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AnalyticsFactsMetaDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AnalyticsFactsMetaData", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsMetaDataList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsMetaDataList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsMetaDataList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsMetaDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsMetaDataList, bool>(queryOperations, query2);
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
        public int GetAnalyticsFactsMetaDataFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            AnalyticsFactsMetaDataRepository AnalyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AnalyticsFactsMetaData> iQueryable = AnalyticsFactsMetaDataRepository.GetAll(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyticsFactsMetaData>(nonListQueryOperation, iQueryable);
            AnalyticsFactsMetaDataListQueryService AnalyticsFactsMetaDataQueryService = new AnalyticsFactsMetaDataListQueryService(context);
            IQueryable<AnalyticsFactsMetaDataList> query2 = AnalyticsFactsMetaDataQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<AnalyticsFactsMetaDataList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AnalyticsFactsFieldsMetaDataList> GetAnalyticsFactsFieldsMetaDataFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            AnalyticsFactsFieldsMetaDataRepository AnalyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AnalyticsFactsFieldsMetaData> iQueryable = AnalyticsFactsFieldsMetaDataRepository.GetAll(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaData>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;
            AnalyticsFactsFieldsMetaDataListQueryService AnalyticsFactsFieldsMetaDataQueryService = new AnalyticsFactsFieldsMetaDataListQueryService(context);
            IQueryable<AnalyticsFactsFieldsMetaDataList> query2 = AnalyticsFactsFieldsMetaDataQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaDataList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AnalyticsFactsFieldsMetaDataList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AnalyticsFactsFieldsMetaData", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AnalyticsFactsFieldsMetaDataList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.FieldCode);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.FieldCode);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }
        public int GetAnalyticsFactsFieldsMetaDataFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IDashboardContext context = DashboardContext.GetContext(tenant);
            AnalyticsFactsFieldsMetaDataRepository AnalyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(context);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AnalyticsFactsFieldsMetaData> iQueryable = AnalyticsFactsFieldsMetaDataRepository.GetAll(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaData>(nonListQueryOperation, iQueryable);
            AnalyticsFactsFieldsMetaDataListQueryService AnalyticsFactsFieldsMetaDataQueryService = new AnalyticsFactsFieldsMetaDataListQueryService(context);
            IQueryable<AnalyticsFactsFieldsMetaDataList> query2 = AnalyticsFactsFieldsMetaDataQueryService.GetIqueryableList(iQueryable);
            query2 = filter.GetFilteredQuery<AnalyticsFactsFieldsMetaDataList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}