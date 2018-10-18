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
        private ShipmentReceivableStatusRepository shipmentReceivableStatusRepository;
        private ShipmentReceivableStatusQuery shipmentReceivableStatusQuery;

        public IQueryable<ShipmentReceivableStatus> GetShipmentReceivableStatus(int tenant)
        {
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            //this.ChangeConnectionString(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentReceivableStatusRepository.GetStatusTypes();
        }

        public IQueryable<ShipmentReceivableStatus> GetShipmentReceivableStatusByTenant(int tenant)
        {
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            //this.ChangeConnectionString(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentReceivableStatusRepository.GetStatusTypes();
        }

        public IQueryable<ShipmentReceivableStatus> GetFirstShipmentReceivableStatus(string input, int tenant)
        {
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            //this.ChangeConnectionString(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            input = input.ToUpper();
            return shipmentReceivableStatusRepository.GetStatusTypes().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        #region Filters
        public ShipmentReceivableStatusPM GetSingleShipmentReceivableStatus(string code, int tenant)
        {
            shipmentReceivableStatusQuery = new ShipmentReceivableStatusQuery(tenant);
            //this.ChangeConnectionString(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentReceivableStatusQuery.GetSingleShipmentReceivableStatusPM(code);
        }

        public ShipmentReceivableStatusList GetSingleShipmentReceivableStatusList(string code, int tenant)
        {
            shipmentReceivableStatusQuery = new ShipmentReceivableStatusQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ShipmentReceivableStatusPM entityPM = shipmentReceivableStatusQuery.GetSingleShipmentReceivableStatusPM(code);
            ShipmentReceivableStatusList entityList = new ShipmentReceivableStatusList()
            {
                Code = entityPM.Code,
                Name = entityPM.Name,
                SearchFields = entityPM.SearchFields,

            };
            return entityList;
        }

        public IQueryable<ShipmentReceivableStatusList> GetShipmentReceivableStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            IQueryable<ShipmentReceivableStatus> iQueryable = shipmentReceivableStatusRepository.GetStatusTypes();
            var query2 = from entity in iQueryable
                         select new ShipmentReceivableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentReceivableStatusList> GetShipmentReceivableStatusFilters(byte[] xmlFilters, int tenant)
        {
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentReceivableStatus> iQueryable = shipmentReceivableStatusRepository.GetStatusTypes();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentReceivableStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentReceivableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentReceivableStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentReceivableStatusList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<ShipmentReceivableStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentReceivableStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentReceivableStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentReceivableStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentReceivableStatusList, bool>(queryOperations, query2);
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

        public int GetShipmentReceivableStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentReceivableStatus> iQueryable = shipmentReceivableStatusRepository.GetStatusTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentReceivableStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentReceivableStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<ShipmentReceivableStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
        #endregion

        public void InsertShipmentReceivableStatus(ShipmentReceivableStatus entity)
        {
            shipmentReceivableStatusRepository.Add(entity);
        }

        public void UpdateShipmentReceivableStatus(ShipmentReceivableStatus currentEntity)
        {
            shipmentReceivableStatusRepository.Update(currentEntity);
        }

        public void DeleteShipmentReceivableStatus(ShipmentReceivableStatus entity)
        {
            shipmentReceivableStatusRepository.Remove(entity);
        }
    }
}