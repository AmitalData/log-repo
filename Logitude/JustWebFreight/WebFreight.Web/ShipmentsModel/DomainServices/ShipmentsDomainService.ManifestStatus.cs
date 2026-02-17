using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private ManifestStatusRepository manifestStatusRepository;

        public void UpdateManifestStatusList(ManifestStatusList entityList)
        {

        }

        public ManifestStatusList GetSingleManifestStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            manifestStatusRepository = new ManifestStatusRepository(tenant);
            ManifestStatus entityPOCO = manifestStatusRepository.GetSingleManifestStatus(code);
            ManifestStatusList entityList = new ManifestStatusList()
            {
                Code = entityPOCO.Code,
                Name = entityPOCO.Name,
                SearchFields = entityPOCO.SearchFields,
            };

            return entityList;
        }

        public IQueryable<ManifestStatusList> GetManifestStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            manifestStatusRepository = new ManifestStatusRepository(tenant);
            IQueryable<ManifestStatus> iQueryable = manifestStatusRepository.GetManifestStatus();
            var query2 = from entity in iQueryable
                         select new ManifestStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ManifestStatusList> GetManifestStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            manifestStatusRepository = new ManifestStatusRepository(tenant);
            IQueryable<ManifestStatus> iQueryable = manifestStatusRepository.GetManifestStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ManifestStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from entity in iQueryable
                         select new ManifestStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ManifestStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ManifestStatusList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> myObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ManifestStatus", tenant).ToList();

                ObjectField objectField = (from a in myObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ManifestStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ManifestStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ManifestStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ManifestStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ManifestStatusList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetManifestStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            manifestStatusRepository = new ManifestStatusRepository(tenant);
            IQueryable<ManifestStatus> iQueryable = manifestStatusRepository.GetManifestStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ManifestStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ManifestStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ManifestStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}