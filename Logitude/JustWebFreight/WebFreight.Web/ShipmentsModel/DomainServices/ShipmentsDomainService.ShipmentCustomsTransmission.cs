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
        private ShipmentCustomsTransmissionRepository ShipmentCustomsTransmissionRepository;
        private ShipmentCustomsTransmissionQuery ShipmentCustomsTransmissionQuery;

        [Query(HasSideEffects = true)]
        public IQueryable<ShipmentCustomsTransmissionList> GetShipmentCustomsTransmissionFilters(byte[] xmlFilters, int tenant)
        {
            ShipmentCustomsTransmissionRepository = new ShipmentCustomsTransmissionRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentCustomsTransmission> iQueryable = ShipmentCustomsTransmissionRepository.GetShipmentCustomsTransmissions(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomsTransmission>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new ShipmentCustomsTransmissionList()
                         {
                             Id = entity.Id,
                             Tenant = entity.Tenant,
                             CommunicationLogId = entity.CommunicationLogId,
                             Status = entity.Status,
                             Error = entity.Error,
                             ShipmentId = entity.ShipmentId,
                             MessageCode = entity.MessageCode,
                             SentByUserId = entity.SentByUserId,
                             LastSendDate = entity.LastSendDate, 
                         };

            query2 = filter.GetFilteredQuery<ShipmentCustomsTransmissionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShipmentCustomsTransmissionList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsTransmissionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsTransmissionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsTransmissionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsTransmissionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShipmentCustomsTransmissionList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
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

        public int GetShipmentCustomsTransmissionCount(byte[] xmlFilters, int tenant)
        {
            ShipmentCustomsTransmissionRepository = new ShipmentCustomsTransmissionRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ShipmentCustomsTransmission> iQueryable = ShipmentCustomsTransmissionRepository.GetShipmentCustomsTransmissions(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ShipmentCustomsTransmission>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new ShipmentCustomsTransmissionList()
                         {
                             Id = entity.Id,
                             Tenant = entity.Tenant,
                             CommunicationLogId = entity.CommunicationLogId,
                             Status = entity.Status,
                             Error = entity.Error,
                             ShipmentId = entity.ShipmentId,
                             MessageCode = entity.MessageCode,
                             SentByUserId = entity.SentByUserId,
                             LastSendDate = entity.LastSendDate,
                         };

            query2 = filter.GetFilteredQuery<ShipmentCustomsTransmissionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

    }
}