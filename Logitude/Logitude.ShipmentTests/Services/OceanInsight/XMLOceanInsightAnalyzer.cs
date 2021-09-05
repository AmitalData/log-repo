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
            if (emptyPickupLocationElement != null)
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
            if (tsp3_locElement != null)
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
            if (tsp4_locElement != null)
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
            if (dlv_locElement != null)
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
            if (lif_locElement != null)
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
            if (empty_return_locElement != null)
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
       
        
    }
    public class ContainerEventElement
    {
        public string CreatedDate { get; set; }
        public string EventCode { get; set; }
        public string Details { get; set; }
    }
    public class ContenerShipmentElement
    {
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
        public string pod_discharge_planned_initial;
        public string pod_departure_planned_initial;
        public string pod_discharge_planned_last;
        public string pod_departure_planned_last;
        public string pod_departure_actual;
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

}
