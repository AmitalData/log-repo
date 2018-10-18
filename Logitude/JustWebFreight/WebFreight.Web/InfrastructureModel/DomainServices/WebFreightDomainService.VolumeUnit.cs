using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateVolumeUnitList(VolumeUnitList currentEntity)
        {
        }

        public IQueryable<VolumeUnit> GetVolumeUnits(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            return volumeUnitsRepository.GetVolumeUnits();
        }

        public IQueryable<VolumeUnitPM> GetVolumeUnitsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
            return volumeUnitQuery.GetVolumeUnitPMs();
        }

        public VolumeUnitPM GetSingleVolumeUnit(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
            return volumeUnitQuery.GetSingleVolumeUnitPM(code);
        }

        public VolumeUnitList GetSingleVolumeUnitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            VolumeUnitList volumeUnitList = null;
            VolumeUnit volumeUnit = volumeUnitsRepository.GetSingleVolumeUnit(code);

            if (volumeUnit != null)
            {
                List<VolumeUnit> singleEntityList = new List<VolumeUnit>();
                singleEntityList.Add(volumeUnit);

                IQueryable<VolumeUnit> iQueryable = singleEntityList.AsQueryable();
                volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
                IQueryable<VolumeUnitList> iQueryableEntityList = volumeUnitQuery.GetIQueryableEntityList(iQueryable);
                volumeUnitList = iQueryableEntityList.FirstOrDefault();
            }
            return volumeUnitList;
        }

        public IQueryable<VolumeUnitList> GetVolumeUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            IQueryable<VolumeUnit> iQueryable = volumeUnitsRepository.GetVolumeUnits();
            volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
            IQueryable<VolumeUnitList> query2 = volumeUnitQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<VolumeUnitList> GetVolumeUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<VolumeUnit> iQueryable = volumeUnitsRepository.GetVolumeUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VolumeUnit>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
            IQueryable<VolumeUnitList> query2 = volumeUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<VolumeUnitList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VolumeUnitList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("VolumeUnit", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VolumeUnitList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VolumeUnitList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VolumeUnitList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VolumeUnitList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VolumeUnitList, bool>(queryOperations, query2);
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

        public int GetVolumeUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<VolumeUnit> iQueryable = volumeUnitsRepository.GetVolumeUnits();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<VolumeUnit>(nonListQueryOperation, iQueryable);
            volumeUnitQuery = new VolumeUnitQuery(volumeUnitsRepository);
            IQueryable<VolumeUnitList> query2 = volumeUnitQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<VolumeUnitList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<VolumeUnit> GetFirstVolumeUnits(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            input = input.ToUpper();
            return volumeUnitsRepository.GetVolumeUnits().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public IQueryable<VolumeUnit> GetVolumeUnitsByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    return volumeUnitsRepository.GetVolumeUnits().Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return volumeUnitsRepository.GetVolumeUnits().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return volumeUnitsRepository.GetVolumeUnits();
            }
        }

        public IQueryable<VolumeUnit> GetSingleVolumeUnitByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            volumeUnitsRepository = new VolumeUnitRepository(tenant);
            if (byCode)
            {
                return volumeUnitsRepository.GetVolumeUnits().Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return volumeUnitsRepository.GetVolumeUnits().Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertVolumeUnit(VolumeUnit entity)
        {
            volumeUnitsRepository.Add(entity);
        }

        public void UpdateVolumeUnit(VolumeUnit currentEntity)
        {
            volumeUnitsRepository.Update(currentEntity);
        }

        public void DeleteVolumeUnit(VolumeUnit entity)
        {
            volumeUnitsRepository.Remove(entity);
        }
    }
}