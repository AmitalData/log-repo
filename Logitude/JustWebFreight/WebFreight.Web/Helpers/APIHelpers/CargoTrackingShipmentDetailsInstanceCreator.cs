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
using Logitude.Server.Tools;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class CargoTrackingShipmentDetailsInstanceCreator
    {
        private int tenant;
        ShipmentQuery query;
        List<MilestoneData> milestoneDatas;
        ICargoTrackingContext MyContext;


        public CargoTrackingShipmentDetailsInstanceCreator(int tenant)
        {
            this.tenant = tenant;
            query = new ShipmentQuery(tenant);
            milestoneDatas = new List<MilestoneData>();
            MyContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentDetails CreateCargoTrackingShipmentDetailsInstance(string houseNumber)
        {
            string shipmentId = query.GetShipmentIdIfOneShipmentHaveHouseNumber(houseNumber, tenant);
            if(shipmentId != null)
            {
               return GetCargoTrackingShipmentDetails(shipmentId);
            }
            else
            {
                return null;
            }
        }

        private CargoTrackingShipmentDetails GetCargoTrackingShipmentDetails(string shipmentId)
        {
            ShipmentPM shipment = query.GetSinglePM(shipmentId, tenant);
            CargoTrackingShipmentList cargoTrackingShipment = GetCargoTrackingShipmentByShipmentId(shipmentId);
            CargoTrackingShipmentDetails cargoTrackingShipmentDetails = new CargoTrackingShipmentDetails()
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

            return cargoTrackingShipmentDetails;
        }

        private CargoTrackingShipmentList GetCargoTrackingShipmentByShipmentId(string shipmentId)
        {
            List<string> shipmentIds = new List<string>();
            shipmentIds.Add(shipmentId);
            CargoTrackingShipmentListQueryService cargoTrackingService = new CargoTrackingShipmentListQueryService(MyContext);
            List<CargoTrackingShipmentList> cargoTrackingShipments = cargoTrackingService.GetShipmentsByIds(shipmentIds, tenant);
            return cargoTrackingShipments!=null? cargoTrackingShipments.FirstOrDefault(): null;
        }

        private LastMileDetails GetCardConnectedToShipment(ShipmentPM shipment)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            CardPM cardPM = cardQuery.GetSinglePM(shipment.TruckerId, tenant);
            if (cardPM != null)
            {
                LastMileDetails card = new LastMileDetails()
                {
                    Code = cardPM.Code,
                    EnglishName = cardPM.EnglishName,
                    LocalName = cardPM.LocalName,
                };
                return card;
            }
            return null;
        }


        private string GetTotalChargesInNIS(string paymentRequestXML)
        {
            if (!string.IsNullOrEmpty(paymentRequestXML))
            {
                var paymentRequest = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(paymentRequestXML);
                if (paymentRequest != null)
                {
                    return paymentRequest.TotalChargesInNIS;
                }

            }
            return " ";
            
        }

        private string GetCarrierPrefixAWB(ShipmentPM shipment, CargoTrackingShipmentList cargoTrackingShipment)
        {
            string CarrierPrefixAWB = "";
            if (!string.IsNullOrEmpty(shipment.AirlinePrefix)) {
                string perfix = shipment.AirlinePrefix;
                CarrierPrefixAWB = perfix + " - " + cargoTrackingShipment.Master;
            }
            else
            {
                CarrierPrefixAWB = cargoTrackingShipment.Master;
            }

            return CarrierPrefixAWB;
        }

        private List<MilestoneData> GetShipmentMilestones(CargoTrackingShipmentList cargoTrackingShipment)
        {
            List<Milestone> allMilestones = GetAllShipmentMilestones(cargoTrackingShipment);
            if(allMilestones != null)
            {
                return FillMilestoneDatasList(cargoTrackingShipment, allMilestones);
            }
            return null;
        }

        private List<MilestoneData> FillMilestoneDatasList(CargoTrackingShipmentList cargoTrackingShipment, List<Milestone> allMilestones)
        {
            SetCreatedMilestone(cargoTrackingShipment);
            CargoTrackingMilestoneListQueryService queryService = new CargoTrackingMilestoneListQueryService(MyContext);
            foreach (Milestone milestone in allMilestones)
            {
                CargoTrackingMilestoneList cargoTrackingMilestone = queryService.GetSingle(milestone.Id.ToString());
                bool isMilestoneHaveDateOREstimationDate = milestone.EstimationDate != null || milestone.Date != null;
                if (cargoTrackingMilestone != null && isMilestoneHaveDateOREstimationDate)
                {
                    MilestoneData milestoneData = new MilestoneData()
                    {
                        EnglishName = cargoTrackingMilestone.EnglishName,
                        LocalName = cargoTrackingMilestone.LocalName,
                        Code = getMilestoneCode(milestone.Id.ToString()),
                        Date = milestone.Date?.ToString("dd/MM/yyyy"),
                        Time = milestone.Date?.ToString("HH:mm"),
                        EstimationDate = milestone.EstimationDate?.ToString("dd/MM/yyyy"),
                        EstimationTime = milestone.EstimationDate?.ToString("HH:mm"),
                        Remarks = milestone.Notes,
                    };
                    milestoneDatas.Add(milestoneData);
                }
            }

            return milestoneDatas;
        }

        private void SetCreatedMilestone(CargoTrackingShipmentList cargoTrackingShipment)
        {
            MilestoneData milestoneData = new MilestoneData()
            {
                EnglishName = "Created",
                LocalName = "נוצר",
                Code = "OPN",
                Date = cargoTrackingShipment.CreateDate.ToString("dd/MM/yyyy"),
                Time = cargoTrackingShipment.CreateDate.ToString("HH:mm"),
            };
            milestoneDatas.Add(milestoneData);
        }

        private string getMilestoneCode(string milestoneId)
        {
            string code = "";
            switch (milestoneId)
            {
                case CargoTrackingMilestoneValues.Booking:
                    code = "BKN";
                    break;

                case CargoTrackingMilestoneValues.Pickup:
                    code = "PIC";
                    break;

                case CargoTrackingMilestoneValues.FromWarehouse:
                    code = "FWH";
                    break;

                case CargoTrackingMilestoneValues.Departure:
                    code = "DPT";
                    break;

                case CargoTrackingMilestoneValues.Arrival:
                    code = "ATA";
                    break;

                case CargoTrackingMilestoneValues.ToWarehouse:
                    code = "TWH";
                    break;

                case CargoTrackingMilestoneValues.AssignedToCustomsAgent:
                    code = "ASG";
                    break;

                case CargoTrackingMilestoneValues.CustomsProcess:
                    code = "CSP";
                    break;

                case CargoTrackingMilestoneValues.CustomsPayment:
                    code = "RSH";
                    break;

                case CargoTrackingMilestoneValues.Clearance:
                    code = "RSG";
                    break;

                case CargoTrackingMilestoneValues.AssignedToTrucker:
                    code = "TRG";
                    break;

                case CargoTrackingMilestoneValues.DeliveryOut:
                    code = "DTC";
                    break;

                case CargoTrackingMilestoneValues.Delivered:
                    code = "POD";
                    break;
            }
            return code;
        }

        private List<Milestone> GetAllShipmentMilestones(CargoTrackingShipmentList cargoTrackingShipment)
        {
            CargoTrackingShipmentListQueryService cargoTrackingMilestoneQuery = new CargoTrackingShipmentListQueryService(MyContext);
            return cargoTrackingMilestoneQuery.BuildShipmentMilstones(cargoTrackingShipment);
        }

    }
}