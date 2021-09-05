using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logitude.ShipmentTests.Services.OceanInsight
{
    public class XMLOceanInsightAnalyzer
    {
        public ContainerEventElement ContainerEvent { get; set; }
        public ContenerShipmentElement ContenerShipment { get; set; }
        public ContainerUpdatedFields containerUpdatedFields { get; set; }
        public XMLOceanInsightAnalyzer(XmlDocument xmlDocument)
        {
            ContainerEvent = new ContainerEventElement();
            ContenerShipment = new ContenerShipmentElement();

            XmlNodeList xnList = xmlDocument.SelectNodes("//container");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    this.GetEventSectionFields(item);
                    this.GetShipmentSectionFields(item);
                }
            }
            this.BuildContainerUpdatedFields();
        }
        private void GetEventSectionFields(XmlNode node)
        {
            if (node.ChildNodes != null && node.Name == "event")
            {
                ContainerEvent.CreatedDate = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "created").FirstOrDefault()?.InnerText;
                ContainerEvent.EventCode = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "code").FirstOrDefault()?.InnerText;

                XmlElement detailsElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "details").FirstOrDefault();
                if (detailsElement != null)
                {
                    ContainerEvent.Details = detailsElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "message").FirstOrDefault()?.InnerText;
                }
            }
        }
        private void GetShipmentSectionFields(XmlNode node)
        {
            if (node.ChildNodes != null && node.Name == "shipment")
            {
                this.GetDirectFieldsOfShipment(node);
                this.GetEmptyPickupLocationElement(node);
                this.GetDepartureLocationElement(node);
                this.GetDestinationLocationElement(node);
                this.GetOriginLocationElement(node);
                this.GetTransshipment1Leg(node);
                this.GetTransshipment2Leg(node);
                this.GetTransshipment3Leg(node);
                this.GetTransshipment4Leg(node);
                this.GetLeg1Element(node);
                this.GetLeg2Element(node);
                this.GetLeg3Element(node);
                this.GetLeg4Element(node);
                this.GetLeg5Element(node);
                this.GetDeliveryLocationElement(node);
                this.GetLifLocationElement(node);
                this.GetEmptyReturnElement(node);
            }
        }
        private void GetDirectFieldsOfShipment(XmlNode node)
        {
            ContenerShipment.oceanInsightsId = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "shipmentsubscription_id").FirstOrDefault()?.InnerText;
            ContenerShipment.container_number = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
            ContenerShipment.container_number_FromXML = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
            ContenerShipment.carrier_scac = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_scac").FirstOrDefault()?.InnerText;
            ContenerShipment.container_status = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "status").FirstOrDefault()?.InnerText;
            ContenerShipment.weight = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "weight").FirstOrDefault()?.InnerText;
            ContenerShipment.ETD_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.ETD_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.ATD_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.ATD_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_detected").FirstOrDefault()?.InnerText;
            ContenerShipment.ETA_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.ETA_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.ETA_predection = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_predection").FirstOrDefault()?.InnerText;
            ContenerShipment.ATA_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.ATA_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_detected").FirstOrDefault()?.InnerText;
            ContenerShipment.emptyPickup_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.emptyPickup_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.emptyPickup_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.gateInDate_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.gateInDate_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.gateInDate_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.origin_pickup_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.origin_pickup_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.origin_pickup_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.pol_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.pol_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.pol_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.ts_count = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "ts_count").FirstOrDefault()?.InnerText;
            ContenerShipment.pod_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_discharge_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.pod_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_discharge_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.pod_departure_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.pod_departure_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.pod_departure_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_actual").FirstOrDefault()?.InnerText;
        }
        private void GetEmptyPickupLocationElement(XmlNode node)
        {
            XmlElement emptyPickupLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_loc").FirstOrDefault();
            if (ContenerShipment.emptyPickupLocationElement != null)
            {
                ContenerShipment.emptyPickupLocation = emptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
        }
        private void GetDepartureLocationElement(XmlNode node)
        {
            XmlElement departureLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loc").FirstOrDefault();
            if (departureLocationElement != null)
            {
                ContenerShipment.departureLocation = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                ContenerShipment.pol_loc_locode = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
        }
        private void GetDestinationLocationElement(XmlNode node)
        {
            XmlElement destinationLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_loc").FirstOrDefault();
            if (destinationLocationElement != null)
            {
                ContenerShipment.destinationLocation = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
        }
        private void GetOriginLocationElement(XmlNode node)
        {
            XmlElement origin_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_loc").FirstOrDefault();
            if (origin_locElement != null)
            {
                ContenerShipment.origin_loc_locode = origin_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
        }
        private void GetTransshipment1Leg(XmlNode node)
        {
            XmlElement tsp1_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loc").FirstOrDefault();
            if (tsp1_locElement != null)
            {
                ContenerShipment.tsp1_loc_locode = tsp1_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.tsp1_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp1_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_actual").FirstOrDefault()?.InnerText;

        }
        private void GetTransshipment2Leg(XmlNode node)
        {
            XmlElement tsp2_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loc").FirstOrDefault();
            if (tsp2_locElement != null)
            {
                ContenerShipment.tsp2_loc_locode = tsp2_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.tsp2_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp2_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_actual").FirstOrDefault()?.InnerText;
        }
        private void GetTransshipment3Leg(XmlNode node)
        {
            XmlElement tsp3_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loc").FirstOrDefault();
            if (ContenerShipment.tsp3_locElement != null)
            {
                ContenerShipment.tsp3_loc_locode = tsp3_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.tsp3_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp3_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_actual").FirstOrDefault()?.InnerText;
        }
        private void GetTransshipment4Leg(XmlNode node)
        {
            XmlElement tsp4_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loc").FirstOrDefault();
            if (ContenerShipment.tsp4_locElement != null)
            {
                ContenerShipment.tsp4_loc_locode = tsp4_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.tsp4_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.tsp4_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_actual").FirstOrDefault()?.InnerText;
        }
        private void GetLeg1Element(XmlNode node)
        {
            XmlElement leg1_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg1_vessel").FirstOrDefault();
            if (leg1_vessel_Element != null)
            {
                ContenerShipment.leg1_vessel_name = leg1_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.leg1_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg1_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg2Element(XmlNode node)
        {
            XmlElement leg2_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg2_vessel").FirstOrDefault();
            if (leg2_vessel_Element != null)
            {
                ContenerShipment.leg2_vessel_name = leg2_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.leg2_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg2_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg3Element(XmlNode node)
        {
            XmlElement leg3_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg3_vessel").FirstOrDefault();
            if (leg3_vessel_Element != null)
            {
                ContenerShipment.leg3_vessel_name = leg3_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.leg3_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg3_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg4Element(XmlNode node)
        {
            XmlElement leg4_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg4_vessel").FirstOrDefault();
            if (leg4_vessel_Element != null)
            {
                ContenerShipment.leg4_vessel_name = leg4_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.leg4_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg4_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg5Element(XmlNode node)
        {
            XmlElement leg5_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg5_vessel").FirstOrDefault();
            if (leg5_vessel_Element != null)
            {
                ContenerShipment.leg5_vessel_name = leg5_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.leg5_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg5_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetDeliveryLocationElement(XmlNode node)
        {
            XmlElement dlv_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_loc").FirstOrDefault();
            if (ContenerShipment.dlv_locElement != null)
            {
                ContenerShipment.dlv_loc_locode = dlv_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.dlv_delivery_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.dlv_delivery_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.dlv_delivery_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_actual").FirstOrDefault()?.InnerText;
        }
        private void GetLifLocationElement(XmlNode node)
        {
            XmlElement lif_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_loc").FirstOrDefault();
            if (ContenerShipment.lif_locElement != null)
            {
                ContenerShipment.lif_loc_locode = lif_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.lif_arrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.lif_arrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.lif_arrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.lif_departure_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.lif_departure_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.lif_departure_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_actual").FirstOrDefault()?.InnerText;
        }
        private void GetEmptyReturnElement(XmlNode node)
        {
            XmlElement empty_return_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_loc").FirstOrDefault();
            if (ContenerShipment.empty_return_locElement != null)
            {
                ContenerShipment.empty_return_loc_locode = empty_return_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
            ContenerShipment.empty_return_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_planned_initial").FirstOrDefault()?.InnerText;
            ContenerShipment.empty_return_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_planned_last").FirstOrDefault()?.InnerText;
            ContenerShipment.empty_return_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_actual").FirstOrDefault()?.InnerText;
            ContenerShipment.customs_release_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "customs_release_date").FirstOrDefault()?.InnerText;
            ContenerShipment.customs_release_state = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "customs_release_state").FirstOrDefault()?.InnerText;
            ContenerShipment.carrier_release_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_release_date").FirstOrDefault()?.InnerText;
            ContenerShipment.carrier_release_state = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_release_state").FirstOrDefault()?.InnerText;
            ContenerShipment.availability_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "availability_date").FirstOrDefault()?.InnerText;
        }

        private DateTime? ComputeMainCarriageETD()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ETD_last))
            {
                return ConvertStringToDateTime(ContenerShipment.ETD_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ETD_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageETA()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ETA_last))
            {
                return ConvertStringToDateTime(ContenerShipment.ETA_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ETA_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ETA_predection))
            {
                return ConvertStringToDateTime(ContenerShipment.ETA_predection);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATD()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ATD_detected))
            {
                return ConvertStringToDateTime(ContenerShipment.ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ATD_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.ATD_actual);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATA()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ATA_detected))
            {
                return ConvertStringToDateTime(ContenerShipment.ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ATA_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.ATA_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.emptyPickup_last))
            {
                return ConvertStringToDateTime(ContenerShipment.emptyPickup_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.emptyPickup_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.emptyPickup_initial);
            }

            return null;
        }
        private DateTime? ComputeActualEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.emptyPickup_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.emptyPickup_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedGateInDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.gateInDate_last))
            {
                return ConvertStringToDateTime(ContenerShipment.gateInDate_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.gateInDate_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.gateInDate_initial);
            }

            return null;
        }
        private DateTime? ComputeActualGateInDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.gateInDate_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.gateInDate_actual);
            }

            return null;
        }
        
        private string ComputeCurrentStatusLocation()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.destinationLocation))
            {
                return ContenerShipment.destinationLocation;
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.departureLocation))
            {
                return ContenerShipment.departureLocation;
            }

            else
            {
                return ContenerShipment.emptyPickupLocation;
            }
        }
        private DateTime? ComputeEstimatedOriginPickup()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.origin_pickup_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.origin_pickup_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.origin_pickup_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.origin_pickup_planned_initial);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPOLLoaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pol_loaded_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.pol_loaded_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.pol_loaded_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.pol_loaded_planned_initial);
            }

            return null;
        }
        private DateTime? ComputeActualOriginPickup()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.origin_pickup_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.origin_pickup_actual);
            }
            return null;
        }
        private DateTime? ComputeActualPOLLoaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pol_loaded_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.pol_loaded_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ETD_last))
            {
                return ConvertStringToDateTime(ContenerShipment.ETD_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ETD_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeActualPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ATD_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.ATD_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedTrans1VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vslarrival_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vslarrival_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_discharge_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp1_discharge_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_discharge_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_loaded_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp1_loaded_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_loaded_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp1_vsldeparture_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp1_vsldeparture_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans2VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vslarrival_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vslarrival_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_discharge_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp2_discharge_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_discharge_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_loaded_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp2_loaded_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_loaded_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp2_vsldeparture_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp2_vsldeparture_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans3VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vslarrival_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vslarrival_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_discharge_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp3_discharge_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_discharge_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_loaded_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp3_loaded_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_loaded_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp3_vsldeparture_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp3_vsldeparture_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans4VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vslarrival_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vslarrival_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_discharge_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp4_discharge_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_discharge_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_loaded_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp4_loaded_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_loaded_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeActualPODDischarge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pod_discharge_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_discharge_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.tsp4_vsldeparture_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.tsp4_vsldeparture_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ETA_last))
            {
                return ConvertStringToDateTime(ContenerShipment.ETA_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.ETA_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.ETA_initial);
            }

            return null;
        }
        private DateTime? ComputeActualPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.ATA_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.ATA_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPODDischarge()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pod_discharge_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_discharge_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.pod_discharge_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_discharge_planned_initial);
            }
            return null;
        }
        //private DateTime? ComputeActualPODDischarge()
        //{
        //    if (!string.IsNullOrEmpty(ContenerShipment.pod_discharge_actual))
        //    {
        //        return ConvertStringToDateTime(ContenerShipment.pod_discharge_actual);
        //    }
        //    return null;
        //}
        private DateTime? ComputeEstimatedPODDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pod_departure_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_departure_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.pod_departure_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_departure_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualPODDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.pod_departure_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.pod_departure_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedDelivery()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.dlv_delivery_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.dlv_delivery_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.dlv_delivery_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.dlv_delivery_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualDelivery()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.dlv_delivery_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.dlv_delivery_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedLIFArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.lif_arrival_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_arrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.lif_arrival_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_arrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualLIFArrival()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.lif_arrival_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_arrival_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedLIFDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.lif_departure_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_departure_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.lif_departure_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_departure_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualLIFDeparture()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.lif_departure_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.lif_departure_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedEmptyReturn()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.empty_return_planned_last))
            {
                return ConvertStringToDateTime(ContenerShipment.empty_return_planned_last);
            }

            else if (!string.IsNullOrEmpty(ContenerShipment.empty_return_planned_initial))
            {
                return ConvertStringToDateTime(ContenerShipment.empty_return_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualEmptyReturn()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.empty_return_actual))
            {
                return ConvertStringToDateTime(ContenerShipment.empty_return_actual);
            }

            return null;
        }
        private DateTime? ComputeCustomsReleaseDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.customs_release_date))
            {
                return ConvertStringToDateTime(ContenerShipment.customs_release_date);
            }

            return null;
        }
        private DateTime? ComputeCarrierReleaseDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.carrier_release_date))
            {
                return ConvertStringToDateTime(ContenerShipment.carrier_release_date);
            }

            return null;
        }
        private DateTime? ComputeAvailablityDate()
        {
            if (!string.IsNullOrEmpty(ContenerShipment.availability_date))
            {
                return ConvertStringToDateTime(ContenerShipment.availability_date);
            }

            return null;
        }
        public DateTime? ConvertStringToDateTime(string XMLValue)
        {
            string dateTimeString = this.GetCorrectDateTimeString(XMLValue);

            if (!string.IsNullOrEmpty(dateTimeString))
            {
                return Convert.ToDateTime(dateTimeString);
            }

            else
            {
                return null;
            }
        }
        private string GetCorrectDateTimeString(string XMLValue)
        {
            string dateTimeString = "";

            if (!string.IsNullOrEmpty(XMLValue))
            {
                if (XMLValue.Length > 16)
                {
                    dateTimeString = XMLValue.Substring(0, 16);
                }

                else
                {
                    dateTimeString = XMLValue;
                }
            }

            return dateTimeString;
        }
        private DateTime? GetEventDate()
        {
            if (!string.IsNullOrEmpty(ContainerEvent.CreatedDate))
            {
                return ConvertStringToDateTime(ContainerEvent.CreatedDate);
            }

            return null;
        }
        private void BuildContainerUpdatedFields()
        {
            ContainerUpdatedFields containerUpdatedFields = new ContainerUpdatedFields();
            containerUpdatedFields.MainCarriageETD = this.ComputeMainCarriageETD();
            containerUpdatedFields.MainCarriageETA = this.ComputeMainCarriageETA();
            containerUpdatedFields.MainCarriageATD = this.ComputeMainCarriageATD();
            containerUpdatedFields.MainCarriageATA = this.ComputeMainCarriageATA();
            containerUpdatedFields.EstimatedEmptyPickupDate = this.ComputeEstimatedEmptyPickupDate();
            containerUpdatedFields.ActualEmptyPickupDate = this.ComputeActualEmptyPickupDate();
            containerUpdatedFields.EstimatedPOLArrival = this.ComputeEstimatedGateInDate();
            containerUpdatedFields.ActualPOLArrival = this.ComputeActualGateInDate();
            containerUpdatedFields.EmptyPickupLocation = ContenerShipment.emptyPickupLocation;
            containerUpdatedFields.DepartureLocation = ContenerShipment.departureLocation;
            containerUpdatedFields.DestinationLocation = ContenerShipment.destinationLocation;
            containerUpdatedFields.CurrentStatusDate = this.GetEventDate();
            containerUpdatedFields.CurrentLocation = this.ComputeCurrentStatusLocation();
            containerUpdatedFields.OriginLocation = ContenerShipment.origin_loc_locode;
            containerUpdatedFields.EstimatedOriginPickup = this.ComputeEstimatedOriginPickup();
            containerUpdatedFields.ActualOriginPickup = this.ComputeActualOriginPickup();
            containerUpdatedFields.POLLocation = ContenerShipment.pol_loc_locode;
            containerUpdatedFields.EstimatedPOLLoaded = this.ComputeEstimatedPOLLoaded();
            containerUpdatedFields.ActualPOLLoaded = this.ComputeActualPOLLoaded();
            containerUpdatedFields.EstimatedPOLVesselDeparture = this.ComputeEstimatedPOLVesselDeparture();
            containerUpdatedFields.ActualPOLVesselDeparture = ComputeActualPOLVesselDeparture();
            containerUpdatedFields.TransshipmentCount = ContenerShipment.ts_count;
            containerUpdatedFields.Transshipment1Location = ContenerShipment.tsp1_loc_locode;
            containerUpdatedFields.EstimatedTrans1VesselArrival = this.ComputeEstimatedTrans1VesselArrival();
            containerUpdatedFields.ActualTransshipment1VesselArrival = this.ComputeActualTransshipment1VesselArrival();
            containerUpdatedFields.EstimatedTransshipment1Discharge = this.ComputeEstimatedTransshipment1Discharge();
            containerUpdatedFields.ActualTransshipment1Discharge = ComputeActualTransshipment1Discharge();
            containerUpdatedFields.EstimatedTransshipment1Loaded = this.ComputeEstimatedTransshipment1Loaded();
            containerUpdatedFields.ActualTransshipment1Loaded = ComputeActualTransshipment1Loaded();
            containerUpdatedFields.EstimatedTrans1VesselDeparture = this.ComputeEstimatedTransshipment1VesselDeparture();
            containerUpdatedFields.ActualTrans1VesselDeparture = ComputeActualTransshipment1VesselDeparture();
            containerUpdatedFields.Transshipment2Location = ContenerShipment.tsp2_loc_locode;
            containerUpdatedFields.EstimatedTrans2VesselArrival = this.ComputeEstimatedTrans2VesselArrival();
            containerUpdatedFields.ActualTransshipment2VesselArrival = this.ComputeActualTransshipment2VesselArrival();
            containerUpdatedFields.EstimatedTransshipment2Discharge = this.ComputeEstimatedTransshipment2Discharge();
            containerUpdatedFields.ActualTransshipment2Discharge = ComputeActualTransshipment2Discharge();
            containerUpdatedFields.EstimatedTransshipment2Loaded = this.ComputeEstimatedTransshipment2Loaded();
            containerUpdatedFields.ActualTransshipment2Loaded = ComputeActualTransshipment2Loaded();
            containerUpdatedFields.EstimatedTrans2VesselDeparture = this.ComputeEstimatedTransshipment2VesselDeparture();
            containerUpdatedFields.ActualTrans2VesselDeparture = ComputeActualTransshipment2VesselDeparture();
            containerUpdatedFields.Transshipment3Location = ContenerShipment.tsp3_loc_locode;
            containerUpdatedFields.EstimatedTrans3VesselArrival = this.ComputeEstimatedTrans3VesselArrival();
            containerUpdatedFields.ActualTransshipment3VesselArrival = this.ComputeActualTransshipment3VesselArrival();
            containerUpdatedFields.EstimatedTransshipment3Discharge = this.ComputeEstimatedTransshipment3Discharge();
            containerUpdatedFields.ActualTransshipment3Discharge = ComputeActualTransshipment3Discharge();
            containerUpdatedFields.EstimatedTransshipment3Loaded = this.ComputeEstimatedTransshipment3Loaded();
            containerUpdatedFields.ActualTransshipment3Loaded = ComputeActualTransshipment3Loaded();
            containerUpdatedFields.EstimatedTrans3VesselDeparture = this.ComputeEstimatedTransshipment3VesselDeparture();
            containerUpdatedFields.ActualTrans3VesselDeparture = ComputeActualTransshipment3VesselDeparture();
            containerUpdatedFields.Transshipment4Location = ContenerShipment.tsp4_loc_locode;
            containerUpdatedFields.EstimatedTrans4VesselArrival = this.ComputeEstimatedTrans4VesselArrival();
            containerUpdatedFields.ActualTransshipment4VesselArrival = this.ComputeActualTransshipment4VesselArrival();
            containerUpdatedFields.EstimatedTransshipment4Discharge = this.ComputeEstimatedTransshipment4Discharge();
            containerUpdatedFields.ActualTransshipment4Discharge = ComputeActualTransshipment4Discharge();
            containerUpdatedFields.EstimatedTransshipment4Loaded = this.ComputeEstimatedTransshipment4Loaded();
            containerUpdatedFields.ActualTransshipment4Loaded = ComputeActualTransshipment4Loaded();
            containerUpdatedFields.EstimatedTrans4VesselDeparture = this.ComputeEstimatedTransshipment4VesselDeparture();
            containerUpdatedFields.ActualTrans4VesselDeparture = ComputeActualTransshipment4VesselDeparture();
            containerUpdatedFields.Leg1Vessel = ContenerShipment.leg1_vessel_name;
            containerUpdatedFields.Leg1Voyage = ContenerShipment.leg1_voyage;
            containerUpdatedFields.Leg2Vessel = ContenerShipment.leg2_vessel_name;
            containerUpdatedFields.Leg2Voyage = ContenerShipment.leg2_voyage;
            containerUpdatedFields.Leg3Vessel = ContenerShipment.leg3_vessel_name;
            containerUpdatedFields.Leg3Voyage = ContenerShipment.leg3_voyage;
            containerUpdatedFields.Leg4Vessel = ContenerShipment.leg4_vessel_name;
            containerUpdatedFields.Leg4Voyage = ContenerShipment.leg4_voyage;
            containerUpdatedFields.Leg5Vessel = ContenerShipment.leg5_vessel_name;
            containerUpdatedFields.Leg5Voyage = ContenerShipment.leg5_voyage;
            containerUpdatedFields.PODLocation = ContenerShipment.destinationLocation;
            containerUpdatedFields.EstimatedPODVesselArrival = ComputeEstimatedPODVesselArrival();
            containerUpdatedFields.ActualPODVesselArrival = ComputeActualPODVesselArrival();
            containerUpdatedFields.EstimatedPODDischarge = ComputeEstimatedPODDischarge();
            containerUpdatedFields.ActualPODDischarge = ComputeActualPODDischarge();
            containerUpdatedFields.EstimatedPODDeparture = ComputeEstimatedPODDeparture();
            containerUpdatedFields.ActualPODDeparture = ComputeActualPODDeparture();
            containerUpdatedFields.DeliveryLocation = ContenerShipment.dlv_loc_locode;
            containerUpdatedFields.EstimatedDelivery = ComputeEstimatedDelivery();
            containerUpdatedFields.ActualDelivery = ComputeActualDelivery();
            containerUpdatedFields.LIFLocation = ContenerShipment.lif_loc_locode;
            containerUpdatedFields.EstimatedLIFArrival = ComputeEstimatedLIFArrival();
            containerUpdatedFields.ActualLIFArrival = ComputeActualLIFArrival();
            containerUpdatedFields.EstimatedLIFDeparture = ComputeEstimatedLIFDeparture();
            containerUpdatedFields.ActualLIFDeparture = this.ComputeActualLIFDeparture();
            containerUpdatedFields.EmptyReturnLocation = ContenerShipment.empty_return_loc_locode;
            containerUpdatedFields.EstimatedEmptyReturn = this.ComputeEstimatedEmptyReturn();
            containerUpdatedFields.ActualEmptyReturn = this.ComputeActualEmptyReturn();
            containerUpdatedFields.CustomsReleaseState = ContenerShipment.customs_release_state;
            containerUpdatedFields.CustomsReleaseDate = this.ComputeCustomsReleaseDate();
            containerUpdatedFields.CarrierReleaseState = ContenerShipment.carrier_release_state;
            containerUpdatedFields.CarrierReleaseDate = this.ComputeCarrierReleaseDate();
            containerUpdatedFields.AvailablityDate = this.ComputeAvailablityDate();
            return containerUpdatedFields;
        }
    }
    public class ContainerEventElement
    {
        public string CreatedDate { get; set; }
        public string EventCode { get; set; }
        public string Details { get; set; }
    }
    public class ContenerShipmentElement
    {
        public string pod_discharge_planned_initial = null;
        public string pod_discharge_planned_last = null;
        public string pod_discharge_actual = null;
        public string pod_departure_planned_initial = null;
        public string pod_departure_planned_last = null;
        public string pod_departure_actual = null;
        public string pol_loc_locode = null;
        public string oceanInsightsId;
        public string container_number;
        public string shipmentPackagesId;
        public string container_number_FromXML;
        public string carrier_scac;
        public string container_status;
        public string details;
        public string weight;
        public string createdDate;
        public string eventCode;
        public string ETD_initial;
        public string ETD_last;
        public string ATD_actual;
        public string ATD_detected;
        public string ETA_initial;
        public string ETA_last;
        public string ETA_predection;
        public string ATA_actual;
        public string ATA_detected;
        public string emptyPickup_last;
        public string emptyPickup_initial;
        public string emptyPickup_actual;
        public string emptyPickupLocation;
        public string gateInDate_last;
        public string gateInDate_initial;
        public string gateInDate_actual;
        public string departureLocation;
        public string destinationLocation;
        public string origin_loc_locode;
        public string origin_pickup_planned_initial = null;
        public string origin_pickup_planned_last = null;
        public string origin_pickup_actual = null;
        public string pol_loaded_planned_initial = null;
        public string pol_loaded_planned_last = null;
        public string pol_loaded_actual = null;
        public string ts_count;
        public string tsp1_loc_locode = null;
        public string tsp1_vslarrival_planned_initial = null;
        public string tsp1_vslarrival_planned_last = null;
        public string tsp1_vslarrival_actual = null;
        public string tsp1_discharge_planned_last = null;
        public string tsp1_discharge_actual = null;
        public string tsp1_loaded_planned_initial = null;
        public string tsp1_loaded_planned_last = null;
        public string tsp1_loaded_actual = null;
        public string tsp1_vsldeparture_planned_initial = null;
        public string tsp1_vsldeparture_planned_last = null;
        public string tsp1_vsldeparture_actual = null;
        public string tsp1_discharge_planned_initial = null;
        public string tsp2_loc_locode = null;
        public string tsp2_vslarrival_planned_initial = null;
        public string tsp2_vslarrival_planned_last = null;
        public string tsp2_vslarrival_actual = null;
        public string tsp2_discharge_planned_initial = null;
        public string tsp2_discharge_planned_last = null;
        public string tsp2_discharge_actual = null;
        public string tsp2_loaded_planned_initial = null;
        public string tsp2_loaded_planned_last = null;
        public string tsp2_loaded_actual = null;
        public string tsp2_vsldeparture_planned_initial = null;
        public string tsp2_vsldeparture_planned_last = null;
        public string tsp2_vsldeparture_actual = null;
        public string tsp3_loc_locode = null;
        public string tsp3_vslarrival_planned_initial = null;
        public string tsp3_vslarrival_planned_last = null;
        public string tsp3_vslarrival_actual = null;
        public string tsp3_discharge_planned_initial = null;
        public string tsp3_discharge_planned_last = null;
        public string tsp3_discharge_actual = null;
        public string tsp3_loaded_planned_initial = null;
        public string tsp3_loaded_planned_last = null;
        public string tsp3_loaded_actual = null;
        public string tsp3_vsldeparture_planned_initial = null;
        public string tsp3_vsldeparture_planned_last = null;
        public string tsp3_vsldeparture_actual = null;
        public string tsp4_loc_locode = null;
        public string tsp4_vslarrival_planned_initial = null;
        public string tsp4_vslarrival_planned_last = null;
        public string tsp4_vslarrival_actual = null;
        public string tsp4_discharge_planned_initial = null;
        public string tsp4_discharge_planned_last = null;
        public string tsp4_discharge_actual = null;
        public string tsp4_loaded_planned_initial = null;
        public string tsp4_loaded_planned_last = null;
        public string tsp4_loaded_actual = null;
        public string tsp4_vsldeparture_planned_initial = null;
        public string tsp4_vsldeparture_planned_last = null;
        public string tsp4_vsldeparture_actual = null;
        public string leg1_vessel_name = null;
        public string leg1_voyage = null;
        public string leg2_vessel_name = null;
        public string leg2_voyage = null;
        public string leg3_vessel_name = null;
        public string leg3_voyage = null;
        public string leg4_vessel_name = null;
        public string leg4_voyage = null;
        public string leg5_vessel_name = null;
        public string leg5_voyage = null;
        public string dlv_loc_locode = null;
        public string dlv_delivery_planned_initial = null;
        public string dlv_delivery_planned_last = null;
        public string dlv_delivery_actual = null;
        public string lif_loc_locode = null;
        public string lif_arrival_planned_initial = null;
        public string lif_arrival_planned_last = null;
        public string lif_arrival_actual = null;
        public string lif_departure_planned_initial = null;
        public string lif_departure_planned_last = null;
        public string lif_departure_actual = null;
        public string empty_return_loc_locode = null;
        public string empty_return_planned_initial = null;
        public string empty_return_planned_last = null;
        public string empty_return_actual = null;
        public string customs_release_date = null;
        public string carrier_release_date = null;
        public string customs_release_state = null;
        public string carrier_release_state = null;
        public string availability_date = null;
    }
    public class ContainerUpdatedFields
    {
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? EstimatedEmptyPickupDate { get; set; }
        public DateTime? ActualEmptyPickupDate { get; set; }
        public DateTime? EstimatedPOLArrival { get; set; }
        public DateTime? ActualPOLArrival { get; set; }
        public string EmptyPickupLocation { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public bool HasContainerException { get; set; }
        public DateTime? ActualEmptyPickup { get; set; }
        public DateTime? EstimatedEmptyPickup { get; set; }
        public string OriginLocation { get; set; }
        public DateTime? EstimatedOriginPickup { get; set; }
        public DateTime? ActualOriginPickup { get; set; }
        public string POLLocation { get; set; }
        public DateTime? EstimatedPOLLoaded { get; set; }
        public DateTime? ActualPOLLoaded { get; set; }
        public DateTime? EstimatedPOLVesselDeparture { get; set; }
        public DateTime? ActualPOLVesselDeparture { get; set; }
        public string TransshipmentCount { get; set; }
        public string Transshipment1Location { get; set; }
        public DateTime? EstimatedTrans1VesselArrival { get; set; }
        public DateTime? ActualTransshipment1VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment1Discharge { get; set; }
        public DateTime? ActualTransshipment1Discharge { get; set; }
        public DateTime? EstimatedTransshipment1Loaded { get; set; }
        public DateTime? ActualTransshipment1Loaded { get; set; }
        public DateTime? EstimatedTrans1VesselDeparture { get; set; }
        public DateTime? ActualTrans1VesselDeparture { get; set; }
        public string Transshipment2Location { get; set; }
        public DateTime? EstimatedTrans2VesselArrival { get; set; }
        public DateTime? ActualTransshipment2VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment2Discharge { get; set; }
        public DateTime? ActualTransshipment2Discharge { get; set; }
        public DateTime? EstimatedTransshipment2Loaded { get; set; }
        public DateTime? ActualTransshipment2Loaded { get; set; }
        public DateTime? EstimatedTrans2VesselDeparture { get; set; }
        public DateTime? ActualTrans2VesselDeparture { get; set; }
        public string Transshipment3Location { get; set; }
        public DateTime? EstimatedTrans3VesselArrival { get; set; }
        public DateTime? ActualTransshipment3VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment3Discharge { get; set; }
        public DateTime? ActualTransshipment3Discharge { get; set; }
        public DateTime? EstimatedTransshipment3Loaded { get; set; }
        public DateTime? ActualTransshipment3Loaded { get; set; }
        public DateTime? EstimatedTrans3VesselDeparture { get; set; }
        public DateTime? ActualTrans3VesselDeparture { get; set; }
        public string Transshipment4Location { get; set; }
        public DateTime? EstimatedTrans4VesselArrival { get; set; }
        public DateTime? ActualTransshipment4VesselArrival { get; set; }
        public DateTime? EstimatedTransshipment4Discharge { get; set; }
        public DateTime? ActualTransshipment4Discharge { get; set; }
        public DateTime? EstimatedTransshipment4Loaded { get; set; }
        public DateTime? ActualTransshipment4Loaded { get; set; }
        public DateTime? EstimatedTrans4VesselDeparture { get; set; }
        public DateTime? ActualTrans4VesselDeparture { get; set; }
        public string Leg1Vessel { get; set; }
        public string Leg1Voyage { get; set; }
        public string Leg2Vessel { get; set; }
        public string Leg2Voyage { get; set; }
        public string Leg3Vessel { get; set; }
        public string Leg3Voyage { get; set; }
        public string Leg4Vessel { get; set; }
        public string Leg4Voyage { get; set; }
        public string Leg5Vessel { get; set; }
        public string Leg5Voyage { get; set; }
        public string PODLocation { get; set; }
        public DateTime? EstimatedPODVesselArrival { get; set; }
        public DateTime? ActualPODVesselArrival { get; set; }
        public DateTime? EstimatedPODDischarge { get; set; }
        public DateTime? ActualPODDischarge { get; set; }
        public DateTime? EstimatedPODDeparture { get; set; }
        public DateTime? ActualPODDeparture { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public string LIFLocation { get; set; }
        public DateTime? EstimatedLIFArrival { get; set; }
        public DateTime? ActualLIFArrival { get; set; }
        public DateTime? EstimatedLIFDeparture { get; set; }
        public DateTime? ActualLIFDeparture { get; set; }
        public string GateIn { get; set; }
        public string GateOut { get; set; }
        public string EmptyReturnLocation { get; set; }
        public DateTime? EstimatedEmptyReturn { get; set; }
        public DateTime? ActualEmptyReturn { get; set; }
        public string CustomsReleaseState { get; set; }
        public DateTime? CustomsReleaseDate { get; set; }
        public string CarrierReleaseState { get; set; }
        public DateTime? CarrierReleaseDate { get; set; }
        public DateTime? AvailablityDate { get; set; }
    }

}
