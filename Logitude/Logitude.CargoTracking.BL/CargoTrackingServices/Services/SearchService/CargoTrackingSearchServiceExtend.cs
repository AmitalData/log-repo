using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.SearchService
{
    public static partial class CargoTrackingSearchService
    {
        public static List<CargoTrackingShipmentSearch> GetMasterReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.Master))
            {
                return list;
            }
            var isPublic = SetIsPublicForMasterColumn(shipment);
            var cargoTrackingShipmentSearchFull = new CargoTrackingShipmentSearch()
            {
                IsPublic = isPublic,
                ReferenceType = "Master",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.Master
            };
            list.Add(cargoTrackingShipmentSearchFull);

            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetHouseReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.House))
            {
                return list;
            }
            var cargoTrackingShipmentSearchFull = new CargoTrackingShipmentSearch()
            {
                IsPublic = true,
                ReferenceType = "House",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.House
            };
            list.Add(cargoTrackingShipmentSearchFull);
            var splits = shipment.House.Split('-');

            if (splits.Length <= 1)
                return list;

            foreach (var item in splits)
            {
                if (string.IsNullOrEmpty(item))
                {
                    break;
                }
                var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
                {
                    IsPublic = true,
                    ReferenceType = "House",
                    ShipmentDate = shipment.CreateDate,
                    ShipmentId = entityId,
                    Tenant = shipment.Tenant,
                    SearchFields = item
                };
                list.Add(cargoTrackingShipmentSearch);
            }
            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetContainerNumbersReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.ContainersNumbers))
            {
                return list;
            }
            var splits = shipment.ContainersNumbers.Split(',');
            var isPublic = CheckContainerShipmentSearchPublicity(shipment);
            foreach (var item in splits)
            {
                if (string.IsNullOrEmpty(item))
                {
                    break;
                }
                var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
                {
                    IsPublic = isPublic,
                    ReferenceType = "Container Numbers",
                    ShipmentDate = shipment.CreateDate,
                    ShipmentId = entityId,
                    Tenant = shipment.Tenant,
                    SearchFields = item
                };
                list.Add(cargoTrackingShipmentSearch);
            }
            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetCustomerReferenceReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.CustomerReference))
            {
                return list;
            }
            var splits = shipment.CustomerReference.Split(',');
            foreach (var item in splits)
            {
                if (string.IsNullOrEmpty(item))
                {
                    break;
                }
                var forwardingCustomerReference = item;
                if (forwardingCustomerReference.Length > 30)
                {
                    forwardingCustomerReference = forwardingCustomerReference.Substring(29);
                }
                var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
                {
                    IsPublic = true,
                    ReferenceType = "Customer Reference",
                    ShipmentDate = shipment.CreateDate,
                    ShipmentId = entityId,
                    Tenant = shipment.Tenant,
                    SearchFields = forwardingCustomerReference
                };
                list.Add(cargoTrackingShipmentSearch);
            }
            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetConsigneeNameReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.ConsigneeName))
            {
                return list;
            }
            if (shipment.ConsigneeName.Length > 30)
            {
                shipment.ConsigneeName = shipment.ConsigneeName.Substring(29);
            }
            var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
            {
                IsPublic = false,
                ReferenceType = "Consignee Name",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.ConsigneeName
            };
            list.Add(cargoTrackingShipmentSearch);
            return list;
        }
        public static List<CargoTrackingShipmentSearch> GetShipperNameReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.ShipperName))
            {
                return list;
            }
            if (shipment.ShipperName.Length > 30)
            {
                shipment.ShipperName = shipment.ShipperName.Substring(29);
            }
            var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
            {
                IsPublic = false,
                ReferenceType = "Shipper Name",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.ShipperName
            };
            list.Add(cargoTrackingShipmentSearch);


            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetForwarderShipmentNumberReferences(CargoTrackingShipment shipment, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.ForwardingShipmentNumber))
            {
                return list;
            }
            var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
            {
                IsPublic = false,
                ReferenceType = "Forwarding Shipment Number",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.ForwardingShipmentNumber
            };
            list.Add(cargoTrackingShipmentSearch);
            return list;
        }


        public static List<CargoTrackingShipmentSearch> GetCustomsDeclarationNumberReferences(CargoTrackingShipmentResources shipmentContext, string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipmentContext.CustomsDeclarationNumber))
            {
                return list;
            }
            var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
            {
                IsPublic = false,
                ReferenceType = "Customs Declaration Number",
                ShipmentDate = shipmentContext.CargoTrackingShipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipmentContext.CargoTrackingShipment.Tenant,
                SearchFields = shipmentContext.CustomsDeclarationNumber
            };
            list.Add(cargoTrackingShipmentSearch);


            return list;
        }

        public static List<CargoTrackingShipmentSearch> GetShipmentNumberReferences(CargoTrackingShipment shipment,string entityId)
        {
            var list = new List<CargoTrackingShipmentSearch>();
            if (string.IsNullOrEmpty(shipment.ShipmentNumber))
            {
                return list;
            }
            var splits = shipment.ShipmentNumber.Split('/');
            var cargoTrackingShipmentSearch = new CargoTrackingShipmentSearch()
            {
                IsPublic = true,
                ReferenceType = "Shipment Number",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = shipment.ShipmentNumber
            };
            list.Add(cargoTrackingShipmentSearch);

            if (splits.Length <= 1)
                return list;

            var cargoTrackingShipmentSearch2 = new CargoTrackingShipmentSearch()
            {
                IsPublic = true,
                ReferenceType = "Shipment Number",
                ShipmentDate = shipment.CreateDate,
                ShipmentId = entityId,
                Tenant = shipment.Tenant,
                SearchFields = splits[1]
            };
            list.Add(cargoTrackingShipmentSearch2);
            return list;
        }
        public static bool SetIsPublicForMasterColumn(CargoTrackingShipment shipment)
        {
            if (shipment.ShipmentLevelCode.Equals("H"))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(shipment.ForwardingShipmentLevelCode) && shipment.ForwardingShipmentLevelCode.Equals("H"))
            {
                return false;
            }

            return true;
        }
        public static bool CheckContainerShipmentSearchPublicity(CargoTrackingShipment shipment)
        {
            if (string.IsNullOrEmpty(shipment.ShipmentTypeCode))
                return false;
            List<string> PrivateShipmentTypes = new List<string> { "LCLD", "MYGO", "MYGI" };
            return !PrivateShipmentTypes.Contains(shipment.ShipmentTypeCode?.ToUpper());
        }
        public static bool IsShipmentValidToCreateRefrences(CargoTrackingShipmentResources shipment)
        {
            var deference = DateTime.Now - shipment.ShipmentCreateDate;
            return deference.TotalDays / 30 <= 6;
        }
    }
}
