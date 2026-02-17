using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateMeasurementList(MeasurementList currentEntity)
        {
        }

        public IQueryable<MeasurementPM> GetMeasurementsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementQuery = new MeasurementQuery(tenant);
            return measurementQuery.GetUnitOfMeasurementPMs(tenant);
        }

        public MeasurementPM GetSingleMeasurement(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementQuery = new MeasurementQuery(tenant);
            return measurementQuery.GetSingleMeasurementPM(id);
        }

        public MeasurementList GetSingleMeasurementList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            MeasurementList measurementList = null;
            Measurement measurement = measurementRepository.GetSingleMeasurement(id, tenant);

            if (measurement != null)
            {
                List<Measurement> singleEntityList = new List<Measurement>();
                singleEntityList.Add(measurement);

                measurementQuery = new MeasurementQuery(measurementRepository);
                IQueryable<Measurement> iQueryable = singleEntityList.AsQueryable();
                IQueryable<MeasurementList> iQueryableEntityList = measurementQuery.GetIQueryableEntityList(iQueryable);
                measurementList = iQueryableEntityList.FirstOrDefault();
            }
            return measurementList;
        }

        public IQueryable<MeasurementList> GetMeasurementLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            measurementQuery = new MeasurementQuery(measurementRepository);

            IQueryable<Measurement> iQueryable = measurementRepository.GetMeasurementsByTenant(tenant);
            IQueryable<MeasurementList> query2 = measurementQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<MeasurementList> GetMeasurementFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            measurementQuery = new MeasurementQuery(measurementRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Measurement> iQueryable = measurementRepository.GetUnitOfMeasurements(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Measurement>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<MeasurementList> query2 = measurementQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MeasurementList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(MeasurementList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Measurement", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MeasurementList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MeasurementList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MeasurementList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MeasurementList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MeasurementList, bool>(queryOperations, query2);
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

        public int GetMeasurementFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            measurementQuery = new MeasurementQuery(measurementRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Measurement> iQueryable = measurementRepository.GetUnitOfMeasurements(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Measurement>(nonListQueryOperation, iQueryable);

            IQueryable<MeasurementList> query2 = measurementQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<MeasurementList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<Measurement> GetFirstMeasurements(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            input = input.ToUpper();
            return measurementRepository.GetUnitOfMeasurements(tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public IQueryable<Measurement> GetMeasurementsByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return measurementRepository.GetUnitOfMeasurements(tenant).Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return measurementRepository.GetUnitOfMeasurements(tenant).Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return measurementRepository.GetUnitOfMeasurements(tenant);
            }
        }

        public IQueryable<Measurement> GetSingleMeasurementByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            measurementRepository = new MeasurementRepository(tenant);
            if (byCode)
            {
                return measurementRepository.GetUnitOfMeasurements(tenant).Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return measurementRepository.GetUnitOfMeasurements(tenant).Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertMeasurement(MeasurementPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }
            MeasurementService service = new MeasurementService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

        }

        public void UpdateMeasurement(MeasurementPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            string entityName = "Measurement" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "MeasurementPM" + entityPM.Id + entityPM.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            MeasurementService service = new MeasurementService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteMeasurement(MeasurementPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            MeasurementRepository measurementRepository = new MeasurementRepository(entity.Tenant);
            Measurement measurement = measurementRepository.GetSingleMeasurement(entity.Id, entity.Tenant);
            measurementRepository.Remove(measurement);
        }
    }
}