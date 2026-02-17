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
        private ShipmentPayableStatusRepository shipmentPayableStatusRepository;
        private ShipmentPayableStatusQuery shipmentPayableStatusQuery;

        public IQueryable<ShipmentPayableStatus> GetShipmentPayableStatus(int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableStatusRepository.GetStatusTypes();
        }

        public IQueryable<ShipmentPayableStatus> GetShipmentPayableStatusByTenant(int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableStatusRepository.GetStatusTypes();
        }

        public IQueryable<ShipmentPayableStatus> GetFirstShipmentPayableStatus(string input, int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return shipmentPayableStatusRepository.GetStatusTypes().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public ShipmentPayableStatusPM GetSingleShipmentPayableStatus(string code, int tenant)
        {
            shipmentPayableStatusQuery = new ShipmentPayableStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentPayableStatusQuery.GetSingleShipmentPayableStatusPM(code);
        }

        public ShipmentPayableStatusList GetSingleShipmentPayableStatusList(string code, int tenant)
        {
            shipmentPayableStatusQuery = new ShipmentPayableStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentPayableStatusPM entityPM = shipmentPayableStatusQuery.GetSingleShipmentPayableStatusPM(code);
            ShipmentPayableStatusList entityList = new ShipmentPayableStatusList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,
            };
            return entityList;
        }

        public IQueryable<ShipmentPayableStatusList> GetShipmentPayableStatusLists(int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            IQueryable<ShipmentPayableStatus> iQueryable = shipmentPayableStatusRepository.GetStatusTypes();
            var query2 = from entity in iQueryable
                         select new ShipmentPayableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentPayableStatusList> GetShipmentPayableStatusFilters(byte[] xmlFilters, int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPayableStatus> iQueryable = shipmentPayableStatusRepository.GetStatusTypes();

            //PortCustomFilter customfilters = new PortCustomFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentPayableStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentPayableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentPayableStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentPayableStatusList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<ShipmentPayableStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentPayableStatusList, bool>(queryOperations, query2);
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

        public int GetShipmentPayableStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            shipmentPayableStatusRepository = new ShipmentPayableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentPayableStatus> iQueryable = shipmentPayableStatusRepository.GetStatusTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentPayableStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentPayableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentPayableStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        public void InsertShipmentPayableStatus(ShipmentPayableStatus entity)
        {
            shipmentPayableStatusRepository.Add(entity);
        }

        public void UpdateShipmentPayableStatus(ShipmentPayableStatus currentEntity)
        {
            shipmentPayableStatusRepository.Update(currentEntity);
        }

        public void DeleteShipmentPayableStatus(ShipmentPayableStatus entity)
        {
            shipmentPayableStatusRepository.Remove(entity);
        }

    }
}