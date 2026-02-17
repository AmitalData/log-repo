using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
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
        private ShipmentTypeRepository shipmentTypeRepository;
        private ShipmentTypeQuery shipmentTypeQuery;

        public IQueryable<ShipmentType> GetShipmentTypes(int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentTypeRepository.GetShipmentTypes();
        }

        public IQueryable<ShipmentType> GetShipmentTypesByTenant(int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentTypeRepository.GetShipmentTypes();
        }

        public ShipmentTypePM GetSingleShipmentType(string id, int tenant)
        {
            shipmentTypeQuery = new ShipmentTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentTypeQuery.GetSingleShipmentTypePM(id);
        }

        public ShipmentTypeList GetSingleShipmentTypeList(string id, int tenant)
        {
            shipmentTypeQuery = new ShipmentTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentTypePM entityPM = shipmentTypeQuery.GetSingleShipmentTypePM(id);
            ShipmentTypeList entityList = new ShipmentTypeList()
            {
                Id = entityPM.Id,
                Name = entityPM.Name,
                TransportModeName = entityPM.TransportModeId,
                TransportModeId = entityPM.TransportModeId,
                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<ShipmentTypeList> GetShipmentTypeLists(int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentType> iQueryable = shipmentTypeRepository.GetShipmentTypes();
            var query2 = from entity in iQueryable
                         select new ShipmentTypeList()
                         {
                             Id = entity.Id,
                             Name = entity.Name,
                             TransportModeName = entity.TransportModeId,
                             TransportModeId = entity.TransportModeId,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentTypeList> GetShipmentTypeFilters(byte[] xmlFilters, int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentType> iQueryable = shipmentTypeRepository.GetShipmentTypes();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentTypeList()
                         {
                             Id = entity.Id,
                             Name = entity.Name,
                             TransportModeName = entity.TransportModeId,
                             TransportModeId = entity.TransportModeId,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentTypeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShipmentType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentTypeList, bool>(queryOperations, query2);
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
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetShipmentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentType> iQueryable = shipmentTypeRepository.GetShipmentTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentTypeList()
                         {
                             Id = entity.Id,
                             Name = entity.Name,
                             TransportModeName = entity.TransportModeId,
                             TransportModeId = entity.TransportModeId,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<ShipmentType> GetShipmentTypesByTransportMode(string mode, int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentType> q = shipmentTypeRepository.GetShipmentTypes().Where(d => d.TransportModeId == mode);
            return q;
        }

        public IQueryable<ShipmentType> GetShipmentTypesByTransportModeId(string mode, int tenant)
        {
            shipmentTypeRepository = new ShipmentTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (String.IsNullOrEmpty(mode))
            {
                return shipmentTypeRepository.GetShipmentTypes();
            }
            else
            {
                return shipmentTypeRepository.GetShipmentTypes().Where(d => d.TransportModeId == mode);
            }
        }

        public void InsertShipmentType(ShipmentType entity)
        {
            shipmentTypeRepository.Add(entity);
        }

        public void UpdateShipmentType(ShipmentType currentEntity)
        {
            shipmentTypeRepository.Update(currentEntity);
        }

        public void DeleteShipmentType(ShipmentType entity)
        {
            shipmentTypeRepository.Remove(entity);
        }
    }
}