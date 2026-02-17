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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private ShipmentCustomerTypeRepository shipmentCustomerTypeRepository;
        private ShipmentCustomerTypeQuery shipmentCustomerTypeQuery;

        public IQueryable<ShipmentCustomerType> GetShipmentCustomerTypesByTenant(int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentCustomerTypeRepository.GetShipmentCustomerTypes();
        }

        public ShipmentCustomerTypePM GetSingleShipmentCustomerTypePM(string code, int tenant)
        {
            shipmentCustomerTypeQuery = new ShipmentCustomerTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentCustomerTypeQuery.GetSingleShipmentCustomerTypePM(code);
        }

        public ShipmentCustomerType GetSingleShipmentCustomerType(string code, int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentCustomerTypeRepository.GetSingleShipmentCustomerType(code);
        }

        public ShipmentCustomerTypeList GetSingleShipmentCustomerTypeList(string code, int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentCustomerType entityPM = shipmentCustomerTypeRepository.GetSingleShipmentCustomerType(code);
            ShipmentCustomerTypeList entityList = new ShipmentCustomerTypeList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
                ShowInLOV = entityPM.ShowInLOV,
            };
            return entityList;
        }

        public IQueryable<ShipmentCustomerTypeList> GetShipmentCustomerTypeLists(int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentCustomerType> iQueryable = shipmentCustomerTypeRepository.GetShipmentCustomerTypes();
            var query2 = from entity in iQueryable
                         select new ShipmentCustomerTypeList()
                         {
                             ShowInLOV = entity.ShowInLOV,
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentCustomerTypeList> GetShipmentCustomerTypeFilters(byte[] xmlFilters, int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentCustomerType> iQueryable = shipmentCustomerTypeRepository.GetShipmentCustomerTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomerType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentCustomerTypeList()
                         {
                             ShowInLOV = entity.ShowInLOV,
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<ShipmentCustomerTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentCustomerTypeList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<ShipmentCustomerTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomerTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomerTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomerTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomerTypeList, bool>(queryOperations, query2);
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

        public int GetShipmentCustomerTypeCount(byte[] xmlFilters, int tenant)
        {
            shipmentCustomerTypeRepository = new ShipmentCustomerTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentCustomerType> iQueryable = shipmentCustomerTypeRepository.GetShipmentCustomerTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomerType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentCustomerTypeList()
                         {
                             ShowInLOV = entity.ShowInLOV,
                             Name = entity.Name,
                             Code = entity.Code,
                             SearchFields = entity.SearchFields
                         };

            query2 = filter.GetFilteredQuery<ShipmentCustomerTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertShipmentCustomerType(ShipmentCustomerType entity)
        {
            shipmentCustomerTypeRepository.Add(entity);
        }

        public void UpdateShipmentCustomerType(ShipmentCustomerType currentEntity)
        {
            shipmentCustomerTypeRepository.Update(currentEntity);
        }

        public void DeleteShipmentCustomerType(ShipmentCustomerType entity)
        {
            shipmentCustomerTypeRepository.Remove(entity);
        }

    }
}