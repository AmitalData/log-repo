using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<PortTimeZoneList> GetPortTimeZoneFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            PortTimeZoneRepository PortTimeZoneRepository = new PortTimeZoneRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PortTimeZone> PortTimeZones = PortTimeZoneRepository.GetPortTimeZones();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            PortTimeZones = filter.GetFilteredQuery<PortTimeZone>(nonListQueryOperation, PortTimeZones);
            int skippedPorts = queryOperations.PageIndex;
            PortTimeZoneQuery PortTimeZoneQuery = new PortTimeZoneQuery(PortTimeZoneRepository);
            IQueryable<PortTimeZoneList> query2 = PortTimeZoneQuery.GetIQueryableEntityList(PortTimeZones);

            query2 = filter.GetFilteredQuery<PortTimeZoneList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PortTimeZoneList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> PortTimeZoneObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PortTimeZone", tenant).ToList();

                ObjectField objectField = (from a in PortTimeZoneObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PortTimeZoneList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PortTimeZoneList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PortTimeZoneList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PortTimeZoneList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PortTimeZoneList, bool>(queryOperations, query2);
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

        public int GetPortTimeZoneFiltersCount(byte[] xmlFilters, int tenant)
        {
            PortTimeZoneRepository PortTimeZoneRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);

            PortTimeZoneRepository = new PortTimeZoneRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<PortTimeZone> PortTimeZones = PortTimeZoneRepository.GetPortTimeZones();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            PortTimeZones = filter.GetFilteredQuery<PortTimeZone>(nonListQueryOperation, PortTimeZones);
            PortTimeZoneQuery PortTimeZoneQuery = new PortTimeZoneQuery(PortTimeZoneRepository);
            IQueryable<PortTimeZoneList> query2 = PortTimeZoneQuery.GetIQueryableEntityList(PortTimeZones);

            query2 = filter.GetFilteredQuery<PortTimeZoneList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}