using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private ShipmentLevelRepository shipmentLevelRepository;
        private ShipmentLevelQuery shipmentLevelQuery;

        public IQueryable<ShipmentLevel> GetShipmentLevelsByTenant(int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentLevelRepository.GetShipmentLevels(tenant);
        }

        public ShipmentLevelPM GetSingleShipmentLevelPM(string code, int tenant)
        {
            shipmentLevelQuery = new ShipmentLevelQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentLevelQuery.GetSingleShipmentLevelPM(code);
        }

        public ShipmentLevel GetSingleShipmentLevel(string code, int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentLevelRepository.GetSingleShipmentLevel(code);
        }

        public ShipmentLevelList GetSingleShipmentLevelList(string code, int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentLevel entityPM = shipmentLevelRepository.GetSingleShipmentLevel(code);
            ShipmentLevelList entityList = new ShipmentLevelList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields
            };
            return entityList;
        }

        public IQueryable<ShipmentLevelList> GetShipmentLevelLists(int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentLevel> iQueryable = shipmentLevelRepository.GetShipmentLevels(tenant);
            var query2 = from entity in iQueryable
                         select new ShipmentLevelList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentLevelList> GetShipmentLevelFilters(byte[] xmlFilters, int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentLevel> iQueryable = shipmentLevelRepository.GetShipmentLevels(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentLevel>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentLevelList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<ShipmentLevelList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentLevelList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentLevelList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentLevelList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentLevelList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentLevelList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentLevelList, bool>(queryOperations, query2);
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

        public int GetShipmentLevelCount(byte[] xmlFilters, int tenant)
        {
            shipmentLevelRepository = new ShipmentLevelRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentLevel> iQueryable = shipmentLevelRepository.GetShipmentLevels(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentLevel>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentLevelList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<ShipmentLevelList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertShipmentLevel(ShipmentLevel entity)
        {
            shipmentLevelRepository.Add(entity);
        }

        public void UpdateShipmentLevel(ShipmentLevel currentEntity)
        {
            shipmentLevelRepository.Update(currentEntity);
        }

        public void DeleteShipmentLevel(ShipmentLevel entity)
        {
            shipmentLevelRepository.Remove(entity);
        }

    }
}