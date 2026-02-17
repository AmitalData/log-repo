using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateDimensionsUnitList(DimensionsUnitList currentEntity)
        {
        }

        public IQueryable<DimensionsUnit> GetDimensionsUnits(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            return dimensionsUnitRepository.GetDimensionsUnits();
        }

        public IQueryable<DimensionsUnit> DimensionsUnitsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            return dimensionsUnitRepository.GetDimensionsUnits();
        }

        public DimensionsUnitPM GetSingleDimensionsUnit(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitQuery = new DimensionsUnitQuery(tenant);
            return dimensionsUnitQuery.GetSinglePM(code, tenant);
        }

        public DimensionsUnitList GetSingleDimensionsUnitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            DimensionsUnitList dimensionsUnitList = null;
            DimensionsUnit dimensionsUnit = dimensionsUnitRepository.GetSingleDimensionsUnit(code);

            if (dimensionsUnit != null)
            {
                List<DimensionsUnit> singleEntityList = new List<DimensionsUnit>();
                singleEntityList.Add(dimensionsUnit);

                dimensionsUnitQuery = new DimensionsUnitQuery(dimensionsUnitRepository);
                IQueryable<DimensionsUnit> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DimensionsUnitList> iQueryableEntityList = dimensionsUnitQuery.GetIQueryableEntityList(iQueryable);
                dimensionsUnitList = iQueryableEntityList.FirstOrDefault();
            }
            return dimensionsUnitList;
        }

        public IQueryable<DimensionsUnitList> GetDimensionsUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            dimensionsUnitQuery = new DimensionsUnitQuery(dimensionsUnitRepository);

            IQueryable<DimensionsUnit> iQueryable = dimensionsUnitRepository.GetDimensionsUnits();
            IQueryable<DimensionsUnitList> query2 = dimensionsUnitQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        public IQueryable<DimensionsUnitList> GetDimensionsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            dimensionsUnitQuery = new DimensionsUnitQuery(dimensionsUnitRepository);

            IQueryable<DimensionsUnit> iQueryable = dimensionsUnitRepository.GetDimensionsUnits();
            IQueryable<DimensionsUnitList> query2 = dimensionsUnitQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DimensionsUnitList> GetDimensionsUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            dimensionsUnitQuery = new DimensionsUnitQuery(dimensionsUnitRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DimensionsUnit> iQueryable = dimensionsUnitRepository.GetDimensionsUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DimensionsUnit>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DimensionsUnitList> query2 = dimensionsUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DimensionsUnitList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DimensionsUnitList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DimensionsUnit", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DimensionsUnitList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DimensionsUnitList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DimensionsUnitList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DimensionsUnitList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DimensionsUnitList, bool>(queryOperations, query2);
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

        public int GetDimensionsUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            dimensionsUnitRepository = new DimensionsUnitRepository(tenant);
            dimensionsUnitQuery = new DimensionsUnitQuery(dimensionsUnitRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DimensionsUnit> iQueryable = dimensionsUnitRepository.GetDimensionsUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DimensionsUnit>(nonListQueryOperation, iQueryable);

            IQueryable<DimensionsUnitList> query2 = dimensionsUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DimensionsUnitList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertDimensionsUnit(DimensionsUnit entity)
        {
            dimensionsUnitRepository.Add(entity);
        }

        public void UpdateDimensionsUnit(DimensionsUnit currentEntity)
        {
            dimensionsUnitRepository.Update(currentEntity);
        }

        public void DeleteDimensionsUnit(DimensionsUnit entity)
        {
            dimensionsUnitRepository.Remove(entity);
        }
    }
}