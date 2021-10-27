using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.BL.APIDataContract;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.Server.Tools;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class CargoTrackingShipmentDetailsInstanceCreator
    {
        private int tenant;
        ShipmentQuery query;
        List<MilestoneData> milestoneDatas;
        ICargoTrackingContext MyContext;
        Dictionary<string, string> mileStoneCodes;


        public CargoTrackingShipmentDetailsInstanceCreator(int tenant)
        {
            this.tenant = tenant;
            query = new ShipmentQuery(tenant);
            milestoneDatas = new List<MilestoneData>();
            MyContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentDetailsResult CreateCargoTrackingShipmentDetailsResultInstance(string houseNumber)
        {
            CargoTrackingShipmentDetailsResult cargoTrackingShipmentDetailsResult = new CargoTrackingShipmentDetailsResult();
            CargoTrackingShipmentDetails cargoTrackingShipmentDetails = CreateCargoTrackingShipmentDetailsInstance(houseNumber);
            if (cargoTrackingShipmentDetails == null) { cargoTrackingShipmentDetailsResult.HasMoreThanOneShipmentWithSameHouse = true; }
            cargoTrackingShipmentDetailsResult.CargoTrackingShipmentDetails = cargoTrackingShipmentDetails;
            return cargoTrackingShipmentDetailsResult;
        }
        private CargoTrackingShipmentDetails CreateCargoTrackingShipmentDetailsInstance(string houseNumber)
        {
            string shipmentId = query.GetShipmentIdIfOneShipmentHaveHouseNumber(houseNumber, tenant);
            if (string.IsNullOrEmpty(shipmentId)) { return null; }

            return CreateMappedCargoTrackingShipmentDetailsInstance(shipmentId);
        }

        private CargoTrackingShipmentDetails CreateMappedCargoTrackingShipmentDetailsInstance(string shipmentId)
        {
            ShipmentPM shipment = query.GetSinglePM(shipmentId, tenant);
            CargoTrackingShipmentList cargoTrackingShipment = GetCargoTrackingShipmentByShipmentId(shipmentId);
            return CreateCargoTrackingShipmentDetailsInstance(shipment, cargoTrackingShipment);
        }

        private CargoTrackingShipmentDetails CreateCargoTrackingShipmentDetailsInstance(ShipmentPM shipment, CargoTrackingShipmentList cargoTrackingShipment)
        {
            return new CargoTrackingShipmentDetails()
            {
                CourierBLNumber = shipment.House,
                ShipmentNumber = shipment.ShipmentNumber,
                CarrierPrefixAWB = GetCarrierPrefixAWB(shipment, cargoTrackingShipment),
                EstimatedArrivalDateTime = cargoTrackingShipment.ArrivalEstimationDate,
                ActualArrivalDateTime = cargoTrackingShipment.ArrivalDate,
                PaymentDateTime = cargoTrackingShipment.CustomsPaymentDate,
                CustomsClearanceDateTime = cargoTrackingShipment.ClearanceDate,
                IsPaymentRequired = shipment.IsPaymentRequired,
                ChargesAmountInNIS = GetTotalChargesInNIS(shipment.PaymentRequestXML),
                IsCustomerIDNumberRequired = shipment.IsUserIDNumberRequired,
                LastMileDetails = GetCardConnectedToShipment(shipment),
                ShipmentMilestones = GetShipmentMilestones(cargoTrackingShipment),
            };
        }

        private CargoTrackingShipmentList GetCargoTrackingShipmentByShipmentId(string shipmentId)
        {
            CargoTrackingShipmentListQueryService cargoTrackingService = new CargoTrackingShipmentListQueryService(MyContext);
            CargoTrackingShipmentList cargoTrackingShipments = cargoTrackingService.GetCargoTrackingShipmentByEntityId(shipmentId, tenant);
            return cargoTrackingShipments;
        }
        private string GetCarrierPrefixAWB(ShipmentPM shipment, CargoTrackingShipmentList cargoTrackingShipment)
        {
            if (string.IsNullOrEmpty(shipment.AirlinePrefix)) { return cargoTrackingShipment.Master; }
            return String.Concat(shipment.AirlinePrefix, "-", cargoTrackingShipment.Master);
        }
        private string GetTotalChargesInNIS(string paymentRequestXML)
        {
            if (string.IsNullOrEmpty(paymentRequestXML)) { return null; }

            var paymentRequest = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(paymentRequestXML);
            if (paymentRequest == null) { return null; }
            return paymentRequest.TotalChargesInNIS;
        }
        private LastMileDetails GetCardConnectedToShipment(ShipmentPM shipment)
        {
            CardPM cardPM = GetCardConnectedToTrucker(shipment);
            if (cardPM == null) { return null; }
            return CreateLastMilestoneDetailsInstance(cardPM);
        }

        private CardPM GetCardConnectedToTrucker(ShipmentPM shipment)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            CardPM cardPM = cardQuery.GetSinglePM(shipment.TruckerId, tenant);
            return cardPM;
        }

        private static LastMileDetails CreateLastMilestoneDetailsInstance(CardPM cardPM)
        {
            return new LastMileDetails()
            {
                Code = cardPM.Code,
                EnglishName = cardPM.EnglishName,
                LocalName = cardPM.LocalName,
            };
        }

        private List<MilestoneData> GetShipmentMilestones(CargoTrackingShipmentList cargoTrackingShipment)
        {
            BuildMileStoneCodesDictionary();
            List<Milestone> allMilestones = GetAllShipmentMilestones(cargoTrackingShipment);
            if (allMilestones == null) { return null; }
            allMilestones.OrderBy(m => m.Id);
            return FillMilestoneDatasList(cargoTrackingShipment, allMilestones);
        }

        private void BuildMileStoneCodesDictionary()
        {
            mileStoneCodes = new Dictionary<string, string>(){
                {CargoTrackingMilestoneValues.Booking, "BKN"},
                {CargoTrackingMilestoneValues.Pickup, "PIC"},
                {CargoTrackingMilestoneValues.FromWarehouse, "FWH"},
                {CargoTrackingMilestoneValues.Departure, "DPT"},
                {CargoTrackingMilestoneValues.Arrival, "ATA"},
                {CargoTrackingMilestoneValues.ToWarehouse, "TWH"},
                {CargoTrackingMilestoneValues.AssignedToCustomsBroker, "ASG"},
                {CargoTrackingMilestoneValues.CustomsProcess, "CSP"},
                {CargoTrackingMilestoneValues.CustomsPayment, "RSH"},
                {CargoTrackingMilestoneValues.Clearance, "RSG"},
                {CargoTrackingMilestoneValues.AssignedToTrucker, "TRG"},
                {CargoTrackingMilestoneValues.DeliveryOut, "DTC"},
                {CargoTrackingMilestoneValues.Delivered, "POD"},
            };
        }

        private List<Milestone> GetAllShipmentMilestones(CargoTrackingShipmentList cargoTrackingShipment)
        {
            return BuildShipmentMilstones(cargoTrackingShipment);
        }

        public List<Milestone> BuildShipmentMilstones(CargoTrackingShipmentList cargoShipmentPM)
        {
            var milestones  = new List<Milestone>
            {
                new Milestone()
                {
                    Id = 1,
                    Code = "Created",
                    Name = "Created",
                    Date = cargoShipmentPM.CreateDate,
                    EstimationDate = null,
                    Done = true,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = false
                },


                new Milestone()
                {
                    Id = 2,
                    Code = "Booking",
                    Name = "Booking",
                    Date = cargoShipmentPM.BookingDate,
                    EstimationDate = cargoShipmentPM.BookingEstimationDate,
                    Done = cargoShipmentPM.BookingDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.BookingDate == null && cargoShipmentPM.BookingEstimationDate != null
                },
                new Milestone()
                {
                    Id = 3,
                    Code = "Pickup",
                    Name = "Pickup",
                    Date = cargoShipmentPM.PickupDate,
                    EstimationDate = cargoShipmentPM.PickupEstimationDate,
                    Done = cargoShipmentPM.PickupDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PickupDate == null && cargoShipmentPM.PickupEstimationDate != null
                },
                new Milestone()
                {
                    Id = 4,
                    Code = "OriginWarehouse",
                    Name = "Origin Warehouse",
                    Date = cargoShipmentPM.FromWarehouseDate,
                    EstimationDate = cargoShipmentPM.FromWarehouseEstimationDate,
                    Done = cargoShipmentPM.FromWarehouseDone,
                    Notes = cargoShipmentPM.FromWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.FromWarehouseDate == null && cargoShipmentPM.FromWarehouseEstimationDate != null
                },
                new Milestone()
                {
                    Id = 5,
                    Code = "Departure",
                    Name = "Departure",
                    Date = cargoShipmentPM.DepartureDate,
                    EstimationDate = cargoShipmentPM.DepartureEstimationDate,
                    Done = cargoShipmentPM.DepartureDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DepartureDate == null && cargoShipmentPM.DepartureEstimationDate != null
                },
                new Milestone()
                {
                    Id = 6,
                    Code = "Arrival",
                    Name = "Arrival",
                    Date = cargoShipmentPM.ArrivalDate,
                    EstimationDate = cargoShipmentPM.ArrivalEstimationDate,
                    Done = cargoShipmentPM.ArrivalDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ArrivalDate == null && cargoShipmentPM.ArrivalEstimationDate != null
                },
                new Milestone()
                {
                    Id = 7,
                    Code = "DestinationWarehouse",
                    Name = "Destination Warehouse",
                    Date = cargoShipmentPM.ToWarehouseDate,
                    EstimationDate = cargoShipmentPM.ToWarehouseEstimationDate,
                    Done = cargoShipmentPM.ToWarehouseDone,
                    Notes = cargoShipmentPM.ToWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ToWarehouseDate == null && cargoShipmentPM.ToWarehouseEstimationDate != null
                },
                new Milestone()
                {
                    Id = 8,
                    Code = "AssignedToCustomsBroker",
                    Name = "Assigned To Customs Broker",
                    Date = cargoShipmentPM.AssignedCustomsAgentDate,
                    EstimationDate = cargoShipmentPM.AssignedCustomsAgentEstDate,
                    Done = cargoShipmentPM.AssignedCustomsAgentDone,
                    Notes = cargoShipmentPM.AssignedCustomsAgentNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.AssignedCustomsAgentDate == null && cargoShipmentPM.AssignedCustomsAgentEstDate != null
                },
                new Milestone()
                {
                    Id = 9,
                    Code = "CustomsProcess",
                    Name = "Customs Process",
                    //Date = cargoShipmentPM.process,
                    EstimationDate = null,
                    //Done = cargoShipmentPM.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    //IsEstimation = !cargoShipmentPM.CustomsPaymentDone
                },
                new Milestone()
                {
                    Id = 10,
                    Code = "GoodsClassification",
                    Name = "Goods Classification",
                    Date = cargoShipmentPM.GoodsClassificationDate,
                    EstimationDate = cargoShipmentPM.GoodsClassificationEstDate,
                    Done = cargoShipmentPM.GoodsClassificationDone,
                    Notes = cargoShipmentPM.GoodsClassificationNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.GoodsClassificationDate == null && cargoShipmentPM.GoodsClassificationEstDate != null
                },
                new Milestone()
                {
                    Id = 11,
                    Code = "DocumentInspection",
                    Name = "Document Inspection",
                    Date = cargoShipmentPM.DocumentInspectionDate,
                    EstimationDate = cargoShipmentPM.DocumentInspectionEstDate,
                    Done = cargoShipmentPM.DocumentInspectionDone,
                    Notes = cargoShipmentPM.DocumentInspectionNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DocumentInspectionDate == null && cargoShipmentPM.DocumentInspectionEstDate != null
                },
                new Milestone()
                {
                    Id = 12,
                    Code = "PaymentRequested",
                    Name = "Payment Requested",
                    Date = cargoShipmentPM.PaymentRequiredDate,
                    EstimationDate = cargoShipmentPM.PaymentRequiredEstimationDate,
                    Done = cargoShipmentPM.PaymentRequiredDone,
                    Notes = cargoShipmentPM.PaymentRequiredNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PaymentRequiredDate == null && cargoShipmentPM.PaymentRequiredEstimationDate != null
                },
                new Milestone()
                {
                    Id = 13,
                    Code = "PaymentReceived",
                    Name = "Payment Received",
                    Date = cargoShipmentPM.PaymentReceivedDate,
                    EstimationDate = cargoShipmentPM.PaymentReceivedEstomationDate,
                    Done = cargoShipmentPM.PaymentReceivedDone,
                    Notes = cargoShipmentPM.PaymentReceivedNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.PaymentReceivedDate == null && cargoShipmentPM.PaymentReceivedEstomationDate != null
                },
                new Milestone()
                {
                    Id = 14,
                    Code = "CustomsPayment",
                    Name = "Customs Payment",
                    Date = cargoShipmentPM.CustomsPaymentDate,
                    EstimationDate = null,
                    Done = cargoShipmentPM.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.CustomsPaymentDone != true
                },
                new Milestone()
                {
                    Id = 15,
                    Code = "Clearance",
                    Name = "Clearance",
                    Date = cargoShipmentPM.ClearanceDate,
                    EstimationDate = null,
                    Done = cargoShipmentPM.ClearanceDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.ClearanceDone != true
                },
                new Milestone()
                {
                    Id = 16,
                    Code = "GatepassArrived",
                    Name = "Gatepass Arrived",
                    Date = cargoShipmentPM.GatepassArrivedDate,
                    EstimationDate = cargoShipmentPM.GatepassArrivedEstDate,
                    Done = cargoShipmentPM.GatepassArrivedDone,
                    Notes = cargoShipmentPM.GatepassArrivedNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.GatepassArrivedDate == null && cargoShipmentPM.GatepassArrivedEstDate != null
                },
                new Milestone()
                {
                    Id = 17,
                    Code = "AssignedToTrucker",
                    Name = "Assigned To Trucker",
                    Date = cargoShipmentPM.AssignedTruckerDate,
                    EstimationDate = cargoShipmentPM.AssignedTruckerEstimationDate,
                    Done = cargoShipmentPM.AssignedTruckerDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.AssignedTruckerDate == null && cargoShipmentPM.AssignedTruckerEstimationDate != null
                },
                new Milestone()
                {
                    Id = 18,
                    Code = "DeliveryOnTheWay",
                    Name = "Delivery on the way",
                    Date = cargoShipmentPM.DeliveryDate,
                    EstimationDate = cargoShipmentPM.DeliveryEstimationDate,
                    Done = cargoShipmentPM.DeliveryDone,
                    Notes = cargoShipmentPM.DeliveryNotes,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DeliveryDate == null && cargoShipmentPM.DeliveryEstimationDate != null
                },
                new Milestone()
                {
                    Id = 19,
                    Code = "Delivered",
                    Name = "Delivered",
                    Date = cargoShipmentPM.DeliveredDate,
                    EstimationDate = cargoShipmentPM.DeliveredEstimationDate,
                    Done = cargoShipmentPM.DeliveredDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = cargoShipmentPM.DeliveredDate == null && cargoShipmentPM.DeliveredEstimationDate != null
                },
                new Milestone()
                {
                    Id = 20,
                    Code = "Invoiced",
                    Name = "Invoiced",
                    //Date = cargoShipmentPM.invoi,
                    //EstimationDate = cargoShipmentPM.DeliveredEstimationDate,
                    //Done = cargoShipmentPM.DeliveredDone,
                    Notes = null,
                    IsCurrent = false,
                    //IsEstimation = !cargoShipmentPM.DeliveredDone
                }
            };

            return milestones.OrderByDescending(d => d.Id).ToList();
        }
        private List<MilestoneData> FillMilestoneDatasList(CargoTrackingShipmentList cargoTrackingShipment, List<Milestone> allMilestones)
        {
            AddNewMilestoneDataToMilestoneDatas(cargoTrackingShipment);
            List<CargoTrackingMilestoneList> CargoTrackingMilestoneList = GetCargoTrackingMilestoneList();
            foreach (Milestone milestone in allMilestones.OrderBy(m => m.Id))
            {
                CreateMappedMilestoneDataInstance(CargoTrackingMilestoneList, milestone);
            }

            return milestoneDatas;
        }
        private void AddNewMilestoneDataToMilestoneDatas(CargoTrackingShipmentList cargoTrackingShipment)
        {
            MilestoneData milestoneData = CreateMilestoneDataInstance(cargoTrackingShipment);
            milestoneDatas.Add(milestoneData);
        }
        private static MilestoneData CreateMilestoneDataInstance(CargoTrackingShipmentList cargoTrackingShipment)
        {
            return new MilestoneData()
            {
                EnglishName = "Created",
                LocalName = "נוצר",
                Code = "OPN",
                Date = cargoTrackingShipment.CreateDate.ToString("dd/MM/yyyy"),
                Time = cargoTrackingShipment.CreateDate.ToString("HH:mm"),
            };
        }
        private List<CargoTrackingMilestoneList> GetCargoTrackingMilestoneList()
        {
            CargoTrackingMilestoneListQueryService queryService = new CargoTrackingMilestoneListQueryService(MyContext);
            List<CargoTrackingMilestoneList> CargoTrackingMilestoneList = queryService.GetList(tenant);
            return CargoTrackingMilestoneList;
        }

        private void CreateMappedMilestoneDataInstance(List<CargoTrackingMilestoneList> cargoTrackingMilestoneList, Milestone milestone)
        {
            CargoTrackingMilestoneList cargoTrackingMilestone = cargoTrackingMilestoneList.Find(m => m.Code == milestone.Id.ToString());
            bool isMilestoneHaveDateOREstimationDate = milestone.EstimationDate != null || milestone.Date != null;
            if (cargoTrackingMilestone != null && isMilestoneHaveDateOREstimationDate)
            {
                MilestoneData milestoneData = CreateMilestoneDataInstance(milestone, cargoTrackingMilestone);
                milestoneDatas.Add(milestoneData);
            }
        }

        private MilestoneData CreateMilestoneDataInstance(Milestone milestone, CargoTrackingMilestoneList cargoTrackingMilestone)
        {
            return new MilestoneData()
            {
                EnglishName = cargoTrackingMilestone.EnglishName,
                LocalName = cargoTrackingMilestone.LocalName,
                Code = getMilestoneCode(milestone.Id.ToString()),
                Date = milestone.Date?.ToString("dd/MM/yyyy"),
                Time = milestone.Date?.ToString("HH:mm"),
                EstimationDate = milestone.Date == null ? milestone.EstimationDate?.ToString("dd/MM/yyyy") : null,
                EstimationTime = milestone.Date == null ? milestone.EstimationDate?.ToString("HH:mm") : null,
                Remarks = milestone.Notes,
            };
        }
        private string getMilestoneCode(string milestoneId)
        {
            return mileStoneCodes[milestoneId];
        }


    }
}