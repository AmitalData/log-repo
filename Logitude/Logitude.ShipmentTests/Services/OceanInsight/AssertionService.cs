using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Services.OceanInsight
{
    public class AssertionService: OceanInsightService
    {
        public void AssertShipmentContainerSimulator(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            shipmentContainerSimulator.Errors.Should().BeNullOrEmpty(string.Join(", ", shipmentContainerSimulator.Errors));
        }
        public void AssertAddCommunicationLogs()
        {
            var tryEvreySecound = 4;
            var tineLifeInSecound = 60 * 3;
            var containerObjectTableID = GetObjectTableId("Container");
            var containerEntityId = ShipmentData.ShipmentPM.ShipmentPackages.First().ContainerEntityId;
            var isDone = Waiter.RunAndWait(tryEvreySecound, tineLifeInSecound, () => HaveCommunicationLogsBeenAdded(containerObjectTableID, containerEntityId));
            isDone.Should().BeTrue();
        }
        public bool HaveCommunicationLogsBeenAdded(string objectTableId, string EntityId)
        {
            var filters = CreateCommunicationLogFilter(objectTableId, EntityId);
            var communicationLogs = APICaller.CallGetByFilters<List<CommunicationLogList>>(Urls.CommunicationLogViewsGetByFilters, UserTenant.Token, filters)?.Data;
            if (communicationLogs == null || communicationLogs.Count <= 0)
                return false;

            var lastCommunicationLog = communicationLogs.First(e => e.InOut == "In");
            if (lastCommunicationLog.CommunicationStatusTypeCode != "D")
                return false;

            if (lastCommunicationLog.From != "Amital")
                return false;

            return true;
        }
        public void AssertChangeDetails()
        {
            var shipment = GetShipment();
            var container = GetContainer(shipment.ShipmentPackages.First().ContainerEntityId);
            XMLOceanInsightAnalyzer xMLOceanInsightAnalyzer = new XMLOceanInsightAnalyzer(ShipmentData.XMLData);
            AssertStatusCode(xMLOceanInsightAnalyzer.ContenerShipment);
            AssertChangeContainerDetails(container, xMLOceanInsightAnalyzer.containerUpdatedFields);
            AssertChangeShipmentDetails(shipment, xMLOceanInsightAnalyzer);
        }
        public void AssertStatusCode(ContenerShipmentElement contenerShipment)
        {
            contenerShipment.container_status.Should().NotBe("20");
        }
        public void AssertChangeContainerDetails(ContainerPM containerPM, ContainerUpdatedFields containerUpdatedFields)
        {
            containerPM.EmptyPickupLocation.Should().Be(containerUpdatedFields.EmptyPickupLocation);
            containerPM.ActualDelivery.Should().Be(containerUpdatedFields.ActualDelivery);
            containerPM.ActualEmptyPickupDate.Should().Be(containerUpdatedFields.ActualEmptyPickupDate?.Date);
            containerPM.ActualEmptyReturn.Should().Be(containerUpdatedFields.ActualEmptyReturn);
            containerPM.ActualLIFArrival.Should().Be(containerUpdatedFields.ActualLIFArrival);
            containerPM.ActualLIFDeparture.Should().Be(containerUpdatedFields.ActualLIFDeparture);
            containerPM.ActualOriginPickup.Should().Be(containerUpdatedFields.ActualOriginPickup);
            containerPM.ActualPODDeparture.Should().Be(containerUpdatedFields.ActualPODDeparture);
            containerPM.ActualPODDischarge.Should().Be(containerUpdatedFields.ActualPODDischarge);
            containerPM.ActualPODVesselArrival.Should().Be(containerUpdatedFields.ActualPODVesselArrival);
            containerPM.ActualPOLArrival.Should().Be(containerUpdatedFields.ActualPOLArrival);
            containerPM.ActualPOLLoaded.Should().Be(containerUpdatedFields.ActualPOLLoaded);
            containerPM.ActualPOLVesselDeparture.Should().Be(containerUpdatedFields.ActualPOLVesselDeparture);
            containerPM.ActualTrans1VesselDeparture.Should().Be(containerUpdatedFields.ActualTrans1VesselDeparture);
            containerPM.ActualTrans2VesselDeparture.Should().Be(containerUpdatedFields.ActualTrans2VesselDeparture);
            containerPM.ActualTrans3VesselDeparture.Should().Be(containerUpdatedFields.ActualTrans3VesselDeparture);
            containerPM.ActualTrans4VesselDeparture.Should().Be(containerUpdatedFields.ActualTrans4VesselDeparture);
            containerPM.ActualTransshipment1Discharge.Should().Be(containerUpdatedFields.ActualTransshipment1Discharge);
            containerPM.ActualTransshipment1Loaded.Should().Be(containerUpdatedFields.ActualTransshipment1Loaded);
            containerPM.ActualTransshipment1VesselArrival.Should().Be(containerUpdatedFields.ActualTransshipment1VesselArrival);
            containerPM.ActualTransshipment2Discharge.Should().Be(containerUpdatedFields.ActualTransshipment2Discharge);
            containerPM.ActualTransshipment2Loaded.Should().Be(containerUpdatedFields.ActualTransshipment2Loaded);
            containerPM.ActualTransshipment2VesselArrival.Should().Be(containerUpdatedFields.ActualTransshipment2VesselArrival);
            containerPM.ActualTransshipment3Discharge.Should().Be(containerUpdatedFields.ActualTransshipment3Discharge);
            containerPM.ActualTransshipment3Loaded.Should().Be(containerUpdatedFields.ActualTransshipment3Loaded);
            containerPM.ActualTransshipment3VesselArrival.Should().Be(containerUpdatedFields.ActualTransshipment3VesselArrival);
            containerPM.ActualTransshipment4Discharge.Should().Be(containerUpdatedFields.ActualTransshipment4Discharge);
            containerPM.ActualTransshipment4Loaded.Should().Be(containerUpdatedFields.ActualTransshipment4Loaded);
            containerPM.ActualTransshipment4VesselArrival.Should().Be(containerUpdatedFields.ActualTransshipment4VesselArrival);
            containerPM.AvailablityDate.Should().Be(containerUpdatedFields.AvailablityDate);
            containerPM.CarrierReleaseDate.Should().Be(containerUpdatedFields.CarrierReleaseDate);
            containerPM.CarrierReleaseState.Should().Be(containerUpdatedFields.CarrierReleaseState);
            containerPM.CurrentLocation.Should().Be(containerUpdatedFields.CurrentLocation);
            //containerPM.CurrentStatus.Should().Be(containerUpdatedFields.CurrentStatus);
            containerPM.CurrentStatusDate.Should().Be(containerUpdatedFields.CurrentStatusDate?.Date);
            containerPM.CustomsReleaseDate.Should().Be(containerUpdatedFields.CustomsReleaseDate);
            containerPM.CustomsReleaseState.Should().Be(containerUpdatedFields.CustomsReleaseState);
            containerPM.DeliveryLocation.Should().Be(containerUpdatedFields.DeliveryLocation);
            containerPM.DepartureLocation.Should().Be(containerUpdatedFields.DepartureLocation);
            containerPM.DestinationLocation.Should().Be(containerUpdatedFields.DestinationLocation);
            containerPM.EmptyPickupLocation.Should().Be(containerUpdatedFields.EmptyPickupLocation);
            containerPM.EmptyReturnLocation.Should().Be(containerUpdatedFields.EmptyReturnLocation);
            containerPM.EstimatedDelivery.Should().Be(containerUpdatedFields.EstimatedDelivery);
            containerPM.EstimatedEmptyPickupDate.Should().Be(containerUpdatedFields.EstimatedEmptyPickupDate);
            containerPM.EstimatedEmptyReturn.Should().Be(containerUpdatedFields.EstimatedEmptyReturn);
            containerPM.EstimatedLIFArrival.Should().Be(containerUpdatedFields.EstimatedLIFArrival);
            containerPM.EstimatedLIFDeparture.Should().Be(containerUpdatedFields.EstimatedLIFDeparture);
            containerPM.EstimatedOriginPickup.Should().Be(containerUpdatedFields.EstimatedOriginPickup);
            containerPM.EstimatedPODDeparture.Should().Be(containerUpdatedFields.EstimatedPODDeparture);
            containerPM.EstimatedPODDischarge.Should().Be(containerUpdatedFields.EstimatedPODDischarge);
            containerPM.EstimatedPODVesselArrival.Should().Be(containerUpdatedFields.EstimatedPODVesselArrival);
            containerPM.EstimatedPOLArrival.Should().Be(containerUpdatedFields.EstimatedPOLArrival);
            containerPM.EstimatedPOLLoaded.Should().Be(containerUpdatedFields.EstimatedPOLLoaded);
            containerPM.EstimatedPOLVesselDeparture.Should().Be(containerUpdatedFields.EstimatedPOLVesselDeparture);
            containerPM.EstimatedTrans1VesselArrival.Should().Be(containerUpdatedFields.EstimatedTrans1VesselArrival);
            containerPM.EstimatedTrans1VesselDeparture.Should().Be(containerUpdatedFields.EstimatedTrans1VesselDeparture);
            containerPM.EstimatedTrans2VesselArrival.Should().Be(containerUpdatedFields.EstimatedTrans2VesselArrival);
            containerPM.EstimatedTrans2VesselDeparture.Should().Be(containerUpdatedFields.EstimatedTrans2VesselDeparture);
            containerPM.EstimatedTrans3VesselArrival.Should().Be(containerUpdatedFields.EstimatedTrans3VesselArrival);
            containerPM.EstimatedTrans3VesselDeparture.Should().Be(containerUpdatedFields.EstimatedTrans3VesselDeparture);
            containerPM.EstimatedTrans4VesselArrival.Should().Be(containerUpdatedFields.EstimatedTrans4VesselArrival);
            containerPM.EstimatedTrans4VesselDeparture.Should().Be(containerUpdatedFields.EstimatedTrans4VesselDeparture);
            containerPM.EstimatedTransshipment1Discharge.Should().Be(containerUpdatedFields.EstimatedTransshipment1Discharge);
            containerPM.EstimatedTransshipment1Loaded.Should().Be(containerUpdatedFields.EstimatedTransshipment1Loaded);
            containerPM.EstimatedTransshipment2Discharge.Should().Be(containerUpdatedFields.EstimatedTransshipment2Discharge);
            containerPM.EstimatedTransshipment2Loaded.Should().Be(containerUpdatedFields.EstimatedTransshipment2Loaded);
            containerPM.EstimatedTransshipment3Discharge.Should().Be(containerUpdatedFields.EstimatedTransshipment3Discharge);
            containerPM.EstimatedTransshipment3Loaded.Should().Be(containerUpdatedFields.EstimatedTransshipment3Loaded);
            containerPM.EstimatedTransshipment4Discharge.Should().Be(containerUpdatedFields.EstimatedTransshipment4Discharge);
            containerPM.EstimatedTransshipment4Loaded.Should().Be(containerUpdatedFields.EstimatedTransshipment4Loaded);
            containerPM.GateIn.Should().Be(containerUpdatedFields.GateIn);
            containerPM.GateOut.Should().Be(containerUpdatedFields.GateOut);
            containerPM.HasContainerException.Should().Be(containerUpdatedFields.HasContainerException);
            containerPM.Leg1Vessel.Should().Be(containerUpdatedFields.Leg1Vessel);
            containerPM.Leg1Voyage.Should().Be(containerUpdatedFields.Leg1Voyage);
            containerPM.Leg2Vessel.Should().Be(containerUpdatedFields.Leg2Vessel);
            containerPM.Leg2Voyage.Should().Be(containerUpdatedFields.Leg2Voyage);
            containerPM.Leg3Vessel.Should().Be(containerUpdatedFields.Leg3Vessel);
            containerPM.Leg3Voyage.Should().Be(containerUpdatedFields.Leg3Voyage);
            containerPM.Leg4Vessel.Should().Be(containerUpdatedFields.Leg4Vessel);
            containerPM.Leg4Voyage.Should().Be(containerUpdatedFields.Leg4Voyage);
            containerPM.Leg5Vessel.Should().Be(containerUpdatedFields.Leg5Vessel);
            containerPM.Leg5Voyage.Should().Be(containerUpdatedFields.Leg5Voyage);
            containerPM.LIFLocation.Should().Be(containerUpdatedFields.LIFLocation);
            containerPM.MainCarriageATA.Should().Be(containerUpdatedFields.MainCarriageATA);
            containerPM.MainCarriageATD.Should().Be(containerUpdatedFields.MainCarriageATD);
            containerPM.MainCarriageETA.Should().Be(containerUpdatedFields.MainCarriageETA);
            containerPM.MainCarriageETD.Should().Be(containerUpdatedFields.MainCarriageETD);
            containerPM.OriginLocation.Should().Be(containerUpdatedFields.OriginLocation);
            containerPM.PODLocation.Should().Be(containerUpdatedFields.PODLocation);
            containerPM.POLLocation.Should().Be(containerUpdatedFields.POLLocation);
            containerPM.Transshipment1Location.Should().Be(containerUpdatedFields.Transshipment1Location);
            containerPM.Transshipment2Location.Should().Be(containerUpdatedFields.Transshipment2Location);
            containerPM.Transshipment3Location.Should().Be(containerUpdatedFields.Transshipment3Location);
            containerPM.Transshipment4Location.Should().Be(containerUpdatedFields.Transshipment4Location);
            containerPM.TransshipmentCount.Should().Be(containerUpdatedFields.TransshipmentCount);


        }
        public void AssertChangeShipmentDetails(ShipmentPM shipment, XMLOceanInsightAnalyzer xMLOceanInsightAnalyzer)
        {
            AssertChangePackageStatus(shipment.ShipmentPackages.First(), xMLOceanInsightAnalyzer.ContenerShipment);
            AssertChangeMainCarriage(shipment, xMLOceanInsightAnalyzer.containerUpdatedFields);
        }

        private void AssertChangePackageStatus(PackagePM packagePM, ContenerShipmentElement contenerShipment)
        {
            packagePM.LastStatusCode.Should().Be(contenerShipment.container_status);
        }

        public void AssertChangeMainCarriage(ShipmentPM shipment, ContainerUpdatedFields containerUpdatedFields)
        {
            shipment.MainCarriageETD.Should().Be(containerUpdatedFields.MainCarriageETD);
            shipment.MainCarriageETA.Should().Be(containerUpdatedFields.MainCarriageETA);
            shipment.MainCarriageATD.Should().Be(containerUpdatedFields.MainCarriageATD);
            shipment.MainCarriageATA.Should().Be(containerUpdatedFields.MainCarriageATA);
        }

        

        
        

       
        
        

        
    }
}
