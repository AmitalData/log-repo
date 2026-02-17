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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateWeightUnitList(WeightUnitList currentEntity)
        {
        }

        public IQueryable<WeightUnit> GetWeightUnits(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitRepository = new WeightUnitRepository(tenant);
            return weightUnitRepository.GetWeightUnits();
        }

        public IQueryable<WeightUnitPM> WeightUnitsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitQuery = new WeightUnitQuery(tenant);
            return weightUnitQuery.GetWeightUnitPMs();
        }

        public WeightUnitPM GetSingleWeightUnit(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitQuery = new WeightUnitQuery(tenant);
            return weightUnitQuery.GetSingleWeightUnitPM(code);
        }

        public WeightUnitList GetSingleWeightUnitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitRepository = new WeightUnitRepository(tenant);
            WeightUnitList weightUnitList = null;
            WeightUnit weightUnit = weightUnitRepository.GetSingleWeightUnit(code);

            if (weightUnit != null)
            {
                List<WeightUnit> singleEntityList = new List<WeightUnit>();
                singleEntityList.Add(weightUnit);

                weightUnitQuery = new WeightUnitQuery(weightUnitRepository);
                IQueryable<WeightUnit> iQueryable = singleEntityList.AsQueryable();
                IQueryable<WeightUnitList> iQueryableEntityList = weightUnitQuery.GetIQueryableEntityList(iQueryable);
                weightUnitList = iQueryableEntityList.FirstOrDefault();
            }
            return weightUnitList;
        }

        public IQueryable<WeightUnitList> GetWeightUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitRepository = new WeightUnitRepository(tenant);
            weightUnitQuery = new WeightUnitQuery(weightUnitRepository);

            IQueryable<WeightUnit> iQueryable = weightUnitRepository.GetWeightUnits();
            IQueryable<WeightUnitList> query2 = weightUnitQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<WeightUnitList> GetWeightUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitRepository = new WeightUnitRepository(tenant);
            weightUnitQuery = new WeightUnitQuery(weightUnitRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WeightUnit> iQueryable = weightUnitRepository.GetWeightUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WeightUnit>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<WeightUnitList> query2 = weightUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WeightUnitList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WeightUnitList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("WeightUnit", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WeightUnitList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WeightUnitList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WeightUnitList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WeightUnitList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WeightUnitList, bool>(queryOperations, query2);
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

        public int GetWeightUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            weightUnitRepository = new WeightUnitRepository(tenant);
            weightUnitQuery = new WeightUnitQuery(weightUnitRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<WeightUnit> iQueryable = weightUnitRepository.GetWeightUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<WeightUnit>(nonListQueryOperation, iQueryable);

            IQueryable<WeightUnitList> query2 = weightUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WeightUnitList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertWeightUnit(WeightUnit entity)
        {
            weightUnitRepository.Add(entity);
        }

        public void UpdateWeightUnit(WeightUnit currentEntity)
        {
            weightUnitRepository.Update(currentEntity);
        }

        public void DeleteWeightUnit(WeightUnit entity)
        {
            weightUnitRepository.Remove(entity);
        }
    }
}