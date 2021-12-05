using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CustomMapping
{
    public class CargoTrackingShipmentsMappingService
    {
        public CargoTrackingShipmentResources GetOrder(CargoTrackingShipmentQueryResult row)
        {
            var cargoTrackingShipmentContext = new CargoTrackingShipmentResources();
            var item = new CargoTrackingShipment();
            FillOrderFeilds(item, row);
            item.IsMainRecord = GetIsMainRecord(Codes.OrderType, row);
            item.ShipmentTypeCode = GetShipmentTypeCodeByTransportMode(row.OrderTransportModeId);
            SetOrderExceptionDescription(item, row);

            cargoTrackingShipmentContext.OrderPONumber = row.OrderPONumber;
            cargoTrackingShipmentContext.OrderBookingConfirmationNumber = row.OrderBookingConfirmationNumber;
            cargoTrackingShipmentContext.CargoTrackingShipment = item;
            cargoTrackingShipmentContext.ShipmentCreateDate = row.OrderCreateDate;
            return cargoTrackingShipmentContext;
        }



        public CargoTrackingShipmentResources GetForwarding(CargoTrackingShipmentQueryResult row)
        {
            var cargoTrackingShipmentContext = new CargoTrackingShipmentResources();
            var item = new CargoTrackingShipment();
            FillForwardingFeilds(item, row);
            SetForwardingWarehouseFeilds(item, row);
            SetForwardingCustomerReference(item, row);
            item.IsMainRecord = GetIsMainRecord(Codes.ForwardingType, row);
            SetForwardingExceptionDescription(item, row);
            cargoTrackingShipmentContext.CargoTrackingShipment = item;
            cargoTrackingShipmentContext.ShipmentCreateDate = row.ForwardingCreateDate;

            return cargoTrackingShipmentContext;
        }


        public CargoTrackingShipmentResources GetCustom(CargoTrackingShipmentQueryResult row)
        {
            var cargoTrackingShipmentContext = new CargoTrackingShipmentResources();
            var item = new CargoTrackingShipment();
            FillCustomFeilds(item, row);
            SetCustomWarehouseFeilds(item, row);
            SetCustomCustomerReference(item, row);
            item.IsMainRecord = GetIsMainRecord(Codes.CustomType, row);
            SetCustomExceptionDescription(item, row);
            cargoTrackingShipmentContext.CargoTrackingShipment = item;
            cargoTrackingShipmentContext.CustomsDeclarationNumber = row.CustomsDeclarationNumber;
            cargoTrackingShipmentContext.ShipmentCreateDate = row.CustomCreateDate;

            return cargoTrackingShipmentContext;
        }
        private void SetOrderExceptionDescription(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.CurrentMilestoneExceptions = null;
            var exceptions = "";
            if (row.OrderLastExceptionDate.HasValue)
                exceptions = row.OrderLastExceptionDate?.ToString();
            if (!string.IsNullOrEmpty(row.OrderLastExceptionDescription))
            {
                if (row.OrderLastExceptionDate.HasValue)
                    exceptions += ",";
                exceptions += row.OrderLastExceptionDescription;
            }
            if (!string.IsNullOrEmpty(exceptions))
            {
                item.CurrentMilestoneExceptions = exceptions;
            }
        }

        private void SetForwardingExceptionDescription(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.CurrentMilestoneExceptions = null;
            if (item.ClearanceDone.HasValue && item.ClearanceDone.Value)
            {
                return;
            }
            var exceptions = "";
            if (row.ForwardingExceptionDate.HasValue)
                exceptions = row.ForwardingExceptionDate?.ToString();
            if (!string.IsNullOrEmpty(row.ForwardingCurrentMilestoneExceptionDescription))
            {
                if (row.ForwardingExceptionDate.HasValue)
                    exceptions += ",";
                exceptions += row.ForwardingCurrentMilestoneExceptionDescription;
            }
            if (!string.IsNullOrEmpty(exceptions))
            {
                item.CurrentMilestoneExceptions = exceptions;
            }

        }
        private void SetCustomExceptionDescription(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.CurrentMilestoneExceptions = null;
            if (item.ClearanceDone.HasValue && item.ClearanceDone.Value)
            {
                return;
            }
            var exceptions = "";
            if (row.CustomExceptionDate.HasValue)
                exceptions = row.CustomExceptionDate?.ToString();
            if (!string.IsNullOrEmpty(row.CustomCurrentMilestoneExceptionDescription))
            {
                if (row.CustomExceptionDate.HasValue)
                    exceptions += ",";
                exceptions += row.CustomCurrentMilestoneExceptionDescription;
            }
            if (!string.IsNullOrEmpty(exceptions))
            {
                item.CurrentMilestoneExceptions = exceptions;
            }

        }
        private static string GetShipmentTypeCodeByTransportMode(object data)
        {
            var transportMode = (string)data;
            switch (transportMode)
            {
                case Codes.InlandTransportMode: return Codes.InlandShipmentType;
                case Codes.OceanTransportMode: return Codes.OceanShipmentType;
                case Codes.AirTransportMode: return Codes.AirShipmentType;
            }
            return null;
        }
        private bool GetIsMainRecord(string type, CargoTrackingShipmentQueryResult row)
        {
            switch (type)
            {
                case Codes.OrderType:
                    return string.IsNullOrEmpty(row.ForwardingId);
                case Codes.ForwardingType:
                    return string.IsNullOrEmpty(row.CustomId);
                case Codes.CustomType:
                    return true;
            }
            return false;
        }
        

        private void FillOrderFeilds(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.EntityId = row.OrderId;
            item.EntityType = Codes.OrderType;
            item.Tenant = row.OrderTenant;
            item.ForwardingShipmentHeaderId = row.OrderForwardingShipmentHeaderId;
            item.CustomerId = row.OrderCustomerId;
            item.TransportModeId = row.OrderTransportModeId;
            item.Master = row.OrderMaster;
            item.House = row.OrderHouse;
            item.ShipmentNumber = row.OrderShipmentNumber;
            item.FromPortId = row.OrderFromPortId;
            item.ToPortId = row.OrderToPortId;
            item.DescriptionOfGoods = row.OrderDescriptionOfGoods;
            item.SupplyDateTime = row.OrderSupplyDateTime;
            item.ShipperId = row.OrderShipperId;
            item.ConsigneeId = row.OrderConsigneeId;
            item.GrossWeight = row.OrderGrossWeight;
            item.Volume = row.OrderVolume;
            item.PickupDate = row.OrderPickupDate;
            item.CreateDate = row.OrderCreateDate;
            item.SecurityKey = row.OrderSecurityKey;
            item.ConsigneeName = row.OrderConsigneeName;
            item.ShipperName = row.OrderShipperName;
            item.CustomerReference = row.OrderCustomerReference;
            item.PickupEstimationDate = row.OrderPickupEstimationDate;
            item.DepartureDate = row.OrderDepartureDate;
            item.DepartureEstimationDate = row.OrderDepartureEstimationDate;
            item.ArrivalDate = row.OrderArrivalDate;
            item.ArrivalEstimationDate = row.OrderArrivalEstimationDate;
            item.PackagesQuantity = row.OrderPackagesQuantity;
            item.DirectionId = row.OrderDirectionId;
            item.ShipmentLevelCode = row.OrderShipmentLevelCode;
            //item.ForwardingShipmentNumber = row.OrderForwardingShipmentNumber;
            item.BookingDate = row.OrderBookingDate;
            item.BookingNotes = "Booking Conf. Num: " + row.OrderBookingConfirmationNumber;
        }
        private void FillForwardingFeilds(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.EntityType = Codes.ForwardingType;
            item.EntityId = row.ForwardingId;
            item.Tenant = row.ForwardingTenant;
            item.ForwardingShipmentHeaderId = null;
            item.CustomerId = row.ForwardingCustomerId;
            item.TransportModeId = row.ForwardingTransportModeId;
            item.Master = row.ForwardingMaster;
            item.House = row.ForwardingHouse;
            item.ShipmentNumber = row.ForwardingShipmentNumber;
            item.FromPortId = row.ForwardingFromPortId;
            item.ToPortId = row.ForwardingToPortId;
            item.ShipperId = row.ForwardingShipperId;
            item.ConsigneeId = row.ForwardingConsigneeId;
            item.GrossWeight = row.ForwardingGrossWeight;
            item.Volume = row.ForwardingVolume;
            item.PickupDate = row.ForwardingPickupDate;
            item.CreateDate = row.ForwardingCreateDate;
            item.SecurityKey = row.ForwardingSecurityKey;
            item.ConsigneeName = row.ForwardingConsigneeName;
            item.ShipperName = row.ForwardingShipperName;
            item.PickupEstimationDate = row.ForwardingPickupEstimationDate;
            item.DepartureDate = row.ForwardingDepartureDate;
            item.DepartureEstimationDate = row.ForwardingDepartureEstimationDate;
            item.ArrivalDate = row.ForwardingArrivalDate;
            item.ArrivalEstimationDate = row.ForwardingArrivalEstimationDate;
            item.PackagesQuantity = row.ForwardingPackagesQuantity;
            item.DirectionId = row.ForwardingDirectionId;
            item.ShipmentLevelCode = row.ForwardingShipmentLevelCode;
            //item.ForwardingShipmentNumber = row.ForwardingShipmentNumber;
            item.ShipmentTypeCode = row.ForwardingShipmentTypeCode;
            item.CustomsShipmentHeaderId = row.ForwardingCustomsShipmentHeaderId;
            item.FromWarehouseNotes = row.ForwardingFromWarehouseNotes;
            item.ToWarehouseEstimationDate = row.ForwardingToWarehouseEstimationDate;
            item.ToWarehouseNotes = row.ForwardingToWarehouseNotes;
            item.CustomsPaymentDate = row.ForwardingDeclarationDate;
            item.DeliveredDate = row.ForwardingDeliveredDate;
            item.DeliveredEstimationDate = row.ForwardingDeliveredEstimationDate;
            item.FirstPickupETD = row.ForwardingFirstPickupETD;
            item.WarehouseLegActualEntryDate = row.ForwardingWarehouseLegActualEntryDate;
            item.FromWarehouseEstimationDate = row.ForwardingFromWarehouseEstimationDate;
            item.WarehouseLegExpectedEntryDate = row.ForwardingWarehouseLegExpectedEntryDate;
            item.WarehouseLegRemarks = row.ForwardingWarehouseLegRemarks;
            item.DeclarationDate = row.ForwardingDeclarationDate;
            item.ContainersNumbers = row.ForwardingContainersNumbers;
            item.AssignedTruckerDate = row.ForwardingAssignedTruckerDate;
            item.AssignedTruckerEstimationDate = row.ForwardingAssignedTruckerEstimationDate;
            item.AssignedTruckerNotes = row.ForwardingAssignedTruckerNotes;
            item.AssignedCustomsAgentDate = row.ForwardingAssignedCustomsAgentDate;
            item.AssignedCustomsAgentEstDate = row.ForwardingAssignedCustomsAgentEstDate;
            item.AssignedCustomsAgentNotes = row.ForwardingAssignedCustomsAgentNotes;
            item.AssignedCustomsAgentExcReason = row.ForwardingAssignedCustomsAgentExcReason;
            item.DeliveryDate = row.ForwardingDeliveryDate;
            item.DeliveryEstimationDate = row.ForwardingDeliveryEstimationDate;
            item.DeliveryNotes = !string.IsNullOrEmpty(row.ForwardingCarrierLocalName) ? "Via: " + row.ForwardingCarrierLocalName : !string.IsNullOrEmpty(row.ForwardingCarrierEnglishName) ? "Via: " + row.ForwardingCarrierEnglishName : "";
            item.DeliveryExceptionReason = row.ForwardingDeliveryExceptionReason;
            item.GrossWeightUnitCode = row.ForwardingGrossWeightUnitCode;
            //item.ForwardingHouse = row.ForwardingHouse;
            //item.ForwardingMaster = row.ForwardingMaster;
            //item.ForwardingShipmentLevelCode = row.ForwardingShipmentLevelCode;
            item.GoodsClassificationDate = row.ForwardingGoodsClassificationDate;
            //item.GoodsClassificationEstDate    = row.ForwardingGoodsClassificationEstDate    ;
            item.GoodsClassificationNotes = row.ForwardingGoodsClassificationNotes;
            item.DocumentInspectionDate = row.ForwardingDocumentInspectionDate;
            item.DocumentInspectionEstDate = row.ForwardingDocumentInspectionEstDate;
            item.DocumentInspectionNotes = row.ForwardingDocumentInspectionNotes;
            item.GatepassArrivedEstDate = row.ForwardingGatepassArrivedEstDate;
            item.GatepassArrivedNotes = row.ForwardingGatepassArrivedNotes;
            item.ImportManifest = row.ForwardingImportManifest;
            item.PrevForwardingShipmentId = row.ForwardingPrevForwardingShipmentId;
            //item.BookingDate                   = row.ForwardingBookingDate                   ;
            //item.BookingEstimationDate         = row.ForwardingBookingEstimationDate         ;
            //item.BookingNotes                  = row.ForwardingBookingNotes                  ;
            //item.BookingExceptionReason        = row.ForwardingBookingExceptionReason        ;
            item.PaymentRequiredEstimationDate = row.ForwardingPaymentRequiredEstimationDate;
            item.PaymentRequiredDate = row.ForwardingIsPaymentRequired && row.ForwardingPaymentDateTime != null ? row.ForwardingPaymentRequiredDate : null;
            item.PaymentRequiredNotes = row.ForwardingPaymentRequiredNotes;
            item.PaymentReceivedEstomationDate = row.ForwardingPaymentReceivedEstomationDate;
            item.PaymentReceivedDate = row.ForwardingPaymentReceivedDate;
            item.PaymentReceivedNotes = row.ForwardingPaymentReceivedNotes;

        }
        private void FillCustomFeilds(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.EntityType = Codes.CustomType;
            item.PaymentReceivedDate = row.CustomPaymentReceivedDate;
            item.PaymentReceivedNotes = row.CustomPaymentReceivedNotes;
            item.EntityId = row.CustomId;
            item.Tenant = row.CustomTenant;
            item.ForwardingShipmentHeaderId = null;
            item.CustomerId = row.CustomCustomerId;
            item.TransportModeId = row.CustomTransportModeId;
            item.Master = row.CustomMaster;
            item.House = row.CustomHouse;
            item.ShipmentNumber = row.CustomShipmentNumber;
            item.FromPortId = row.CustomFromPortId;
            item.ToPortId = row.CustomToPortId;
            item.ShipperId = row.CustomShipperId;
            item.ConsigneeId = row.CustomConsigneeId;
            item.GrossWeight = row.CustomGrossWeight;
            item.Volume = row.CustomVolume;
            item.PickupDate = row.CustomPickupDate;
            item.CreateDate = row.CustomCreateDate;
            item.SecurityKey = row.CustomSecurityKey;
            item.ConsigneeName = row.CustomConsigneeName;
            item.ShipperName = row.CustomShipperName;
            item.PickupEstimationDate = row.CustomPickupEstimationDate;
            item.DepartureDate = row.CustomDepartureDate;
            item.DepartureEstimationDate = row.CustomDepartureEstimationDate;
            item.ArrivalDate = row.CustomArrivalDate;
            item.ArrivalEstimationDate = row.CustomArrivalEstimationDate;
            item.PackagesQuantity = row.CustomPackagesQuantity;
            item.DirectionId = row.CustomDirectionId;
            item.ShipmentLevelCode = row.CustomShipmentLevelCode;
            item.ForwardingShipmentNumber = row.ForwardingShipmentNumber;
            item.ShipmentTypeCode = row.CustomShipmentTypeCode;
            item.FromWarehouseNotes = row.CustomFromWarehouseNotes;
            item.ToWarehouseEstimationDate = row.CustomToWarehouseEstimationDate;
            item.ToWarehouseNotes = row.CustomToWarehouseNotes;
            item.CustomsPaymentDate = row.CustomDeclarationDate;
            item.DeliveredDate = row.CustomDeliveredDate;
            item.DeliveredEstimationDate = row.CustomDeliveredEstimationDate;
            item.FirstPickupETD = row.CustomFirstPickupETD;
            item.WarehouseLegActualEntryDate = row.CustomWarehouseLegActualEntryDate;
            item.FromWarehouseEstimationDate = row.CustomFromWarehouseEstimationDate;
            item.WarehouseLegExpectedEntryDate = row.CustomWarehouseLegExpectedEntryDate;
            item.WarehouseLegRemarks = row.CustomWarehouseLegRemarks;
            item.DeclarationDate = row.CustomDeclarationDate;
            item.CustomsClearanceDate = row.CustomsClearanceDate;
            item.ClearanceDate = row.CustomsClearanceDate;
            item.ContainersNumbers = row.CustomContainersNumbers;
            item.AssignedTruckerDate = row.CustomAssignedTruckerDate;
            item.AssignedTruckerEstimationDate = row.CustomAssignedTruckerEstimationDate;
            item.AssignedTruckerNotes = row.CustomAssignedTruckerNotes;
            item.AssignedCustomsAgentDate = row.CustomAssignedCustomsAgentDate;
            item.AssignedCustomsAgentEstDate = row.CustomAssignedCustomsAgentEstDate;
            item.AssignedCustomsAgentNotes = row.CustomAssignedCustomsAgentNotes;
            item.AssignedCustomsAgentExcReason = row.CustomAssignedCustomsAgentExcReason;
            item.DeliveryDate = row.CustomDeliveryDate;
            item.DeliveryEstimationDate = row.CustomDeliveryEstimationDate;
            item.DeliveryNotes = string.IsNullOrEmpty(row.CustomCarrierLocalName) ? row.CustomCarrierEnglishName : row.CustomCarrierLocalName;
            item.DeliveryExceptionReason = row.CustomDeliveryExceptionReason;
            item.GrossWeightUnitCode = row.CustomGrossWeightUnitCode;
            item.ForwardingHouse = row.ForwardingHouse;
            item.ForwardingMaster = row.ForwardingMaster;
            item.ForwardingShipmentLevelCode = row.ForwardingShipmentLevelCode;
            item.GoodsClassificationDate = row.CustomGoodsClassificationDate;
            //item.GoodsClassificationEstDate    = row.CustomGoodsClassificationEstDate    ;
            //item.GoodsClassificationNotes = row.CustomGoodsClassificationNotes;
            item.DocumentInspectionDate = row.CustomDocumentInspectionDate;
            item.DocumentInspectionEstDate = row.CustomDocumentInspectionEstDate;
            item.DocumentInspectionNotes = row.CustomDocumentInspectionNotes;
            item.GatepassArrivedDate = row.CustomGatepassArrivedDate;
            item.GatepassArrivedEstDate = row.CustomGatepassArrivedEstDate;
            item.GatepassArrivedNotes = row.CustomGatepassArrivedNotes;
            item.ImportManifest = row.CustomImportManifest;
            //item.BookingDate                   = row.CustomBookingDate                   ;
            //item.BookingEstimationDate         = row.CustomBookingEstimationDate         ;
            //item.BookingNotes                  = row.CustomBookingNotes                  ;
            //item.BookingExceptionReason        = row.CustomBookingExceptionReason        ;
            item.PaymentRequiredEstimationDate = row.CustomPaymentRequiredEstimationDate;
            item.PaymentRequiredDate = row.CustomIsPaymentRequired && row.CustomPaymentDateTime != null ? row.CustomPaymentReceivedDate : null;
            item.PaymentRequiredNotes = row.CustomPaymentRequiredNotes;
            item.CustomsPaymentDate = row.CustomPaymentDateTime;
            item.PaymentReceivedEstomationDate = row.CustomPaymentReceivedEstomationDate;


        }
        private void SetForwardingWarehouseFeilds(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.FromWarehouseDate = row.ForwardingDirectionId == Codes.ExportDirection ? row.ForwardingWarehouseLegActualEntryDate : null;
            item.FromWarehouseEstimationDate = row.ForwardingDirectionId == Codes.ExportDirection ? row.ForwardingFromWarehouseEstimationDate : null;
            item.FromWarehouseNotes = row.ForwardingDirectionId == Codes.ExportDirection ? row.ForwardingFromWarehouseNotes : null;

            item.ToWarehouseDate = row.ForwardingDirectionId == Codes.ImportDirection ? row.ForwardingWarehouseLegActualEntryDate : null;
            item.ToWarehouseEstimationDate = row.ForwardingDirectionId == Codes.ImportDirection ? row.ForwardingToWarehouseEstimationDate : null;
            item.ToWarehouseNotes = row.ForwardingDirectionId == Codes.ImportDirection ? row.ForwardingToWarehouseNotes : null;

        }
        private void SetCustomWarehouseFeilds(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.FromWarehouseDate = row.CustomDirectionId == Codes.ExportDirection ? row.CustomWarehouseLegActualEntryDate : null;
            item.FromWarehouseEstimationDate = row.CustomDirectionId == Codes.ExportDirection ? row.CustomWarehouseLegExpectedEntryDate : null;
            item.FromWarehouseNotes = row.CustomDirectionId == Codes.ExportDirection ? row.CustomWarehouseLegRemarks : null;

            item.ToWarehouseDate = row.CustomDirectionId == Codes.ImportDirection ? row.CustomWarehouseLegActualEntryDate : null;
            item.ToWarehouseEstimationDate = row.CustomDirectionId == Codes.ImportDirection ? row.CustomWarehouseLegExpectedEntryDate : null;
            item.ToWarehouseNotes = row.CustomDirectionId == Codes.ImportDirection ? row.CustomWarehouseLegRemarks : null;

        }

        private void SetForwardingCustomerReference(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.CustomerReference = "";
            if (!string.IsNullOrEmpty(row.ForwardingCustomerReference1))
            {
                item.CustomerReference += row.ForwardingCustomerReference1;
                if (!string.IsNullOrEmpty(row.ForwardingCustomerReference2))
                {
                    item.CustomerReference += ",";
                }
            }
            if (!string.IsNullOrEmpty(row.ForwardingCustomerReference2))
            {
                item.CustomerReference += row.ForwardingCustomerReference2;
            }
        }
        private void SetCustomCustomerReference(CargoTrackingShipment item, CargoTrackingShipmentQueryResult row)
        {
            item.CustomerReference = "";
            if (!string.IsNullOrEmpty(row.CustomCustomerReference1))
            {
                item.CustomerReference += row.CustomCustomerReference1;
                if (!string.IsNullOrEmpty(row.CustomCustomerReference2))
                {
                    item.CustomerReference += ",";
                }
            }
            if (!string.IsNullOrEmpty(row.CustomCustomerReference2))
            {
                item.CustomerReference += row.CustomCustomerReference2;
            }
        }

        
    }
}
