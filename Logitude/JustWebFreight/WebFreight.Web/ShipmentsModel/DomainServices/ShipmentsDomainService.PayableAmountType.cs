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
        private ShipmentPayableAmountTypeRepository shipmentPayableAmountTypeRepository;
        private ShipmentPayableAmountTypeQuery shipmentPayableAmountTypeQuery;

        public IQueryable<ShipmentPayableAmountType> GetShipmentPayableAmountTypesByTenant(int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableAmountTypeRepository.GetShipmentPayableAmountTypes();
        }

        public ShipmentPayableAmountTypePM GetSingleShipmentPayableAmountTypePM(string code, int tenant)
        {
            shipmentPayableAmountTypeQuery = new ShipmentPayableAmountTypeQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableAmountTypeQuery.GetSingleShipmentPayableAmountTypePM(code);
        }

        public ShipmentPayableAmountType GetSingleShipmentPayableAmountType(string code, int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableAmountTypeRepository.GetSingleShipmentPayableAmountType(code);
        }

        public ShipmentPayableAmountTypeList GetSingleShipmentPayableAmountTypeList(string code, int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentPayableAmountType entityPM = shipmentPayableAmountTypeRepository.GetSingleShipmentPayableAmountType(code);
            ShipmentPayableAmountTypeList entityList = new ShipmentPayableAmountTypeList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
            };
            return entityList;
        }

        public IQueryable<ShipmentPayableAmountTypeList> GetShipmentPayableAmountTypeLists(int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentPayableAmountType> iQueryable = shipmentPayableAmountTypeRepository.GetShipmentPayableAmountTypes();
            var query2 = from entity in iQueryable
                         select new ShipmentPayableAmountTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentPayableAmountTypeList> GetShipmentPayableAmountTypeFilters(byte[] xmlFilters, int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPayableAmountType> iQueryable = shipmentPayableAmountTypeRepository.GetShipmentPayableAmountTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentPayableAmountType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentPayableAmountTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<ShipmentPayableAmountTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentPayableAmountTypeList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<ShipmentPayableAmountTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableAmountTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableAmountTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableAmountTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableAmountTypeList, bool>(queryOperations, query2);
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

        public int GetShipmentPayableAmountTypeCount(byte[] xmlFilters, int tenant)
        {
            shipmentPayableAmountTypeRepository = new ShipmentPayableAmountTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPayableAmountType> iQueryable = shipmentPayableAmountTypeRepository.GetShipmentPayableAmountTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentPayableAmountType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentPayableAmountTypeList()
                         {
                             Name = entity.Name,
                             Code = entity.Code,
                         };

            query2 = filter.GetFilteredQuery<ShipmentPayableAmountTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertShipmentPayableAmountType(ShipmentPayableAmountType entity)
        {
            shipmentPayableAmountTypeRepository.Add(entity);
        }

        public void UpdateShipmentPayableAmountType(ShipmentPayableAmountType currentEntity)
        {
            shipmentPayableAmountTypeRepository.Update(currentEntity);
        }

        public void DeleteShipmentPayableAmountType(ShipmentPayableAmountType entity)
        {
            shipmentPayableAmountTypeRepository.Remove(entity);
        }

    }
}