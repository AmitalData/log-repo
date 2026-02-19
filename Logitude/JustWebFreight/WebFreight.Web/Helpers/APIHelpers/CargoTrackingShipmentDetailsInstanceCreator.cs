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
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.BL.CoreBL;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Security.Policy;
using System.Windows.Forms;
using Simplog.Server.Infrastructure;
using NPOI.SS.Formula.Functions;
using Logitude.CargoTracking.BL.Enums;


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
                PaymentLink = shipment.IsPaymentRequired? BuildUrl(shipment.SecurityKey,"PREQ") :null,
                ChargesAmountInNIS = GetTotalChargesInNIS(shipment.PaymentRequestXML),
                IsCustomerIDNumberRequired = shipment.IsUserIDNumberRequired,
                CustomerIDNumberLink = shipment.IsUserIDNumberRequired ? BuildUrl(shipment.SecurityKey, "UID") : null,
                LastMileDetails = GetCardConnectedToShipment(shipment),
                ShipmentMilestones = GetShipmentMilestones(cargoTrackingShipment),
                StatusDetails= GetStatusDetails(shipment),
                ShipmentExceptions = cargoTrackingShipment.CurrentMilestoneExceptions != null ?  GetShipmentExceptions(cargoTrackingShipment) : null,
            };
        }
        private string BuildUrl( string securityKey,string menu)
        {
            return string.Concat(LogitudeSettings.LogitudeURL, "/LinksGateway.aspx?Menu=", menu, "&SecurityKey=", securityKey, "&Tenant=", tenant);
        }
        
        private ShipmentExceptions GetShipmentExceptions(CargoTrackingShipmentList cargoTrackingShipment)
        {
            return new ShipmentExceptions { Exception = cargoTrackingShipment.CurrentMilestoneExceptions };
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
            List<Milestone> allMilestones = GetAllShipmentMilestones(cargoTrackingShipment);
            if (allMilestones == null) { return null; }
            allMilestones.OrderBy(m => m.Weight);
            return FillMilestoneDatasList(cargoTrackingShipment, allMilestones);
        }

        private List<StatusDetails> GetStatusDetails(ShipmentPM shipment)
        {
            List<TraceEventPM> allEvent = GetAllShipmentEvents(shipment);
            if (allEvent == null) { return null; }
          
            return FillStatusDetailsList(allEvent);
        }

        private List<Milestone> GetAllShipmentMilestones(CargoTrackingShipmentList cargoTrackingShipment)
        {
            CargoTrackingShipmentQueryService cargoTrackingShipmentQueryService = new CargoTrackingShipmentQueryService(MyContext);
            var milestone = cargoTrackingShipmentQueryService.GetMilestonesDictionaryByCode();
            var cargoTrackingMilestoneBuilder = new CargoTrackingMilestoneBuilder();
            return cargoTrackingMilestoneBuilder.BuildShipmentMilstones(cargoTrackingShipment, milestone, true);
        }

        private List<TraceEventPM> GetAllShipmentEvents(ShipmentPM shipment)
        {
            string shipmentObjectTableId = ObjectTableRepository.GetObjectTableByName("Shipment");
            TraceEventQuery traceEventQuery = new TraceEventQuery(shipment.Tenant);
            EventTypeQuery eventTypeQuery=new EventTypeQuery(shipment.Tenant);
            List<TraceEventPM> list = traceEventQuery.GetTraceEventPMsByTenantByEntityIdByWeight(shipment.Tenant, shipment.Id, shipmentObjectTableId)?.ToList();
            return list;
        }
        private List<MilestoneData> FillMilestoneDatasList(CargoTrackingShipmentList cargoTrackingShipment, List<Milestone> allMilestones)
        {
            AddNewMilestoneDataToMilestoneDatas(cargoTrackingShipment);
            List<CargoTrackingMilestoneList> CargoTrackingMilestoneList = GetCargoTrackingMilestoneList();
            foreach (Milestone milestone in allMilestones.OrderBy(m => m.Weight))
            {
                CreateMappedMilestoneDataInstance(CargoTrackingMilestoneList, milestone);
            }

            return milestoneDatas;
        }


        private List<StatusDetails> FillStatusDetailsList(List<TraceEventPM> allEvents)
        {
            List < StatusDetails > statusDetailsList=new List<StatusDetails> ();
            foreach (TraceEventPM ev in allEvents.OrderBy(m => m.EventDateTime))
            {
                StatusDetails statusDetails = CreateStatusDetailsInstance(ev);
                statusDetailsList.Add(statusDetails);
            }

            return statusDetailsList;
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
                Date = cargoTrackingShipment.CreateDate?.ToString("dd/MM/yyyy"),
                Time = cargoTrackingShipment.CreateDate?.ToString("HH:mm"),
            };
        }

        private static StatusDetails CreateStatusDetailsInstance(TraceEventPM traceEventPM)
        {
            return new StatusDetails()
            {
                StatusCode = traceEventPM.EventTypeCode,
                StatusName = traceEventPM.EventTypeEnglishName,
               StatusDateTime=traceEventPM.EventDateTime,
               StatusRemarks=traceEventPM.Notes
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
            CargoTrackingMilestoneList cargoTrackingMilestone = cargoTrackingMilestoneList.Find(m => m.Code == milestone.Code);
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
                Code = cargoTrackingMilestone.ExternalCode,
                Date = milestone.Date?.ToString("dd/MM/yyyy"),
                Time = milestone.Date?.ToString("HH:mm"),
                EstimationDate = milestone.Date == null ? milestone.EstimationDate?.ToString("dd/MM/yyyy") : null,
                EstimationTime = milestone.Date == null ? milestone.EstimationDate?.ToString("HH:mm") : null,
                Remarks = milestone.Notes,
            };
        }

    }
}