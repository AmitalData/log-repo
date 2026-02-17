using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
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
        private ShipmentCustomsMessageTypeRepository shipmentCustomsMessageTypeRepository;
        private ShipmentCustomsMessageTypeQuery shipmentCustomsMessageTypeQuery;

        public void UpdateShipmentCustomsMessageTypeList(ShipmentCustomsMessageTypeList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentCustomsMessageTypeList> GetShipmentCustomsMessageTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentCustomsMessageTypeRepository = new ShipmentCustomsMessageTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShipmentCustomsMessageType> iQueryable = shipmentCustomsMessageTypeRepository.GetShipmentCustomsMessageTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomsMessageType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            shipmentCustomsMessageTypeQuery = new ShipmentCustomsMessageTypeQuery(shipmentCustomsMessageTypeRepository);
            IQueryable<ShipmentCustomsMessageTypeList> query2 = shipmentCustomsMessageTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ShipmentCustomsMessageTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentCustomsMessageTypeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShipmentCustomsMessageType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsMessageTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsMessageTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsMessageTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsMessageTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsMessageTypeList, bool>(queryOperations, query2);
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

        public int GetShipmentCustomsMessageTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shipmentCustomsMessageTypeRepository = new ShipmentCustomsMessageTypeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShipmentCustomsMessageType> iQueryable = shipmentCustomsMessageTypeRepository.GetShipmentCustomsMessageTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomsMessageType>(nonListQueryOperation, iQueryable);
            shipmentCustomsMessageTypeQuery = new ShipmentCustomsMessageTypeQuery(shipmentCustomsMessageTypeRepository);
            IQueryable<ShipmentCustomsMessageTypeList> query2 = shipmentCustomsMessageTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ShipmentCustomsMessageTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}