using Logitude.CargoTracking.BL.CoreBL;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.Enums;
using Logitude.CargoTracking.Def.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public List<CustomCargoTrackingShipmentList> GetCargoTrackingShipmentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            CargoTrackingUsersShipmentService usersShipmentService = new CargoTrackingUsersShipmentService();
            CargoTrackingShipmentSearchInput filter = buildCargoTrackingShipmentFilters(queryOperations);
            return usersShipmentService.GetUserShipments(filter).Select(x => MapCargoTrackingShipmentList(x)).ToList();
        }

        public int GetCargoTrackingShipmentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            CargoTrackingUsersShipmentService usersShipmentService = new CargoTrackingUsersShipmentService();
            CargoTrackingShipmentSearchInput filter = buildCargoTrackingShipmentFilters(queryOperations);
            return usersShipmentService.GetAllShipmentsCountForFirstPageOnly(filter);
        }

        private CustomCargoTrackingShipmentList MapCargoTrackingShipmentList(CargoTrackingShipmentList cargoTrackingShipmentList)
        {
            CustomCargoTrackingShipmentList customCargoTrackingShipmentList = new CustomCargoTrackingShipmentList();
            customCargoTrackingShipmentList.TransportModeId = cargoTrackingShipmentList.TransportModeId;
            customCargoTrackingShipmentList.CustomerReference = cargoTrackingShipmentList.CustomerReference;
            customCargoTrackingShipmentList.DirectionId = cargoTrackingShipmentList.DirectionId;
            customCargoTrackingShipmentList.House = cargoTrackingShipmentList.House;
            customCargoTrackingShipmentList.Master = cargoTrackingShipmentList.Master;
            customCargoTrackingShipmentList.ConsigneeName = cargoTrackingShipmentList.ConsigneeName;
            customCargoTrackingShipmentList.CurrentMilestoneDate = cargoTrackingShipmentList.CurrentMilestoneDate;
            customCargoTrackingShipmentList.CurrentMilestoneName = cargoTrackingShipmentList.CurrentMilestoneName;
            customCargoTrackingShipmentList.FromPortName = cargoTrackingShipmentList.FromPortName;
            customCargoTrackingShipmentList.ToPortName = cargoTrackingShipmentList.ToPortName;
            customCargoTrackingShipmentList.ArrivalDate = cargoTrackingShipmentList.ArrivalDate;
            customCargoTrackingShipmentList.ArrivalEstimationDate = cargoTrackingShipmentList.ArrivalEstimationDate;
            customCargoTrackingShipmentList.DepartureDate = cargoTrackingShipmentList.DepartureDate;
            customCargoTrackingShipmentList.DepartureEstimationDate = cargoTrackingShipmentList.DepartureEstimationDate;
            customCargoTrackingShipmentList.GrossWeight = cargoTrackingShipmentList.GrossWeight;
            customCargoTrackingShipmentList.CurrentMilestoneExceptions = cargoTrackingShipmentList.CurrentMilestoneExceptions;
            customCargoTrackingShipmentList.IsOrder = cargoTrackingShipmentList.EntityType == "O";
            customCargoTrackingShipmentList.HasException = cargoTrackingShipmentList.CurrentMilestoneExceptions != null;
            customCargoTrackingShipmentList.Client = GetClientName(cargoTrackingShipmentList);
            return customCargoTrackingShipmentList;
        }

        private string GetClientName(CargoTrackingShipmentList shipment) {
            string client = "";
            const string CustomsEntityType = "C";
            switch (shipment.DirectionId)
            {
                case ShipmentDirections.Import:
                    {
                        client = shipment.ShipperName;
                        break;
                    }

                case ShipmentDirections.Export:
                    {
                        client = shipment.ConsigneeName;
                        break;
                    }
            }

            if (shipment.EntityType == CustomsEntityType)
            {
                client = shipment.ShipperName;
            }
            return client;
        }

        private CargoTrackingShipmentSearchInput buildCargoTrackingShipmentFilters(QueryOperations queryOperations) {
            CargoTrackingShipmentSearchInput filter = new CargoTrackingShipmentSearchInput();
            var hasException = queryOperations.QueryFilterItems.Where(x => x.FieldName == "HasException").FirstOrDefault();
            var ordersOnly = queryOperations.QueryFilterItems.Where(x => x.FieldName == "OrdersOnly").FirstOrDefault();
            var estimatedArrivalOnly = queryOperations.QueryFilterItems.Where(x => x.FieldName == "EstimatedArrivalOnly").FirstOrDefault();
            var operationalOpenedOnly = queryOperations.QueryFilterItems.Where(x => x.FieldName == "OperationalOpenedOnly").FirstOrDefault();
            var tenant = queryOperations.QueryFilterItems.Where(x => x.FieldName == "Tenant").FirstOrDefault();
            var searchText = queryOperations.QueryFilterItems.Where(x => x.FieldName == "SearchText").FirstOrDefault();
            var customersIds = queryOperations.QueryFilterItems.Where(x => x.FieldName == "CustomersIds").FirstOrDefault();
            var milestonesCodes = queryOperations.QueryFilterItems.Where(x => x.FieldName == "MilestonesCodes").FirstOrDefault();
            var transportModeCodes = queryOperations.QueryFilterItems.Where(x => x.FieldName == "TransportModeCodes").FirstOrDefault();
            var directionCodes = queryOperations.QueryFilterItems.Where(x => x.FieldName == "DirectionCodes").FirstOrDefault();
            filter.HasException = hasException != null ? (bool)hasException.FieldValue : false;
            filter.OrdersOnly = ordersOnly != null ? (bool)ordersOnly.FieldValue : false;
            filter.EstimatedArrivalOnly = estimatedArrivalOnly != null ? (bool)estimatedArrivalOnly.FieldValue : false;
            filter.OperationalOpenedOnly = operationalOpenedOnly != null ? (bool)operationalOpenedOnly.FieldValue : false;
            filter.SearchText = searchText != null ? (string)searchText.FieldValue : null;
            filter.Tenant = tenant != null ? Convert.ToInt32(tenant.FieldValue) : -1;
            
            filter.CustomersIds = new List<string>();
            if (customersIds != null && customersIds.FieldValue != null)
            {
                var array = customersIds.FieldValue.ToString().Split('_');
                filter.CustomersIds = new List<string>(array);
            }
            filter.MilestonesCodes = new List<string>();
            if (milestonesCodes != null && milestonesCodes.FieldValue != null)
            {
                var array = milestonesCodes.FieldValue.ToString().Split('_');
                filter.MilestonesCodes = new List<string>(array);
            }
            filter.TransportModeCodes = new List<string>();
            if (transportModeCodes != null && transportModeCodes.FieldValue != null)
            {
                var array = transportModeCodes.FieldValue.ToString().Split('_');
                filter.TransportModeCodes = new List<string>(array);
            }
            filter.DirectionCodes = new List<string>();
            if (directionCodes != null && directionCodes.FieldValue != null)
            {
                var array = directionCodes.FieldValue.ToString().Split('_');
                filter.DirectionCodes = new List<string>(array);
            }

            //filter.Tenant = queryOperations.te;
            filter.PageIndex = queryOperations.PageIndex;
            filter.PageSize = queryOperations.PageSize;
            return filter;
        }
    }

    public class CustomCargoTrackingShipmentList : CargoTrackingShipmentList {
        [DataMember]
        public bool IsOrder { get; set; }
        [DataMember]
        public bool HasException { get; set; }
        [DataMember]
        public string Client { get; set; }
    }
}
