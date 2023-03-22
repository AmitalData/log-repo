using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Xml;

namespace WebFreight.Web.ContainerTracking
{
    public class OceanInsightAnalyzer
    {
        private ArrayOfQueueTask externalTasksQueues;
        private string oceanInsightsEnvelopeParameters;
        private List<LogitudeOceanInsightsRequest> oceanInsights;
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private int? logitudeTenant = null;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string objectTableName = "Container";
        private string containerId;
        private string objectTableId;
        private string loggedContactId;

        private ShipmentPM shipmentPM;
        private ContainerPM container;

        private IShipmentsContext shipmentContext;
        private ICommonDataContext commonContext;
        private ShipmentContainerStatusRepository shipmentContainerStatusRepository;
        private ContainerRepository containerRepository;        
        private ContainerStatusRepository containerStatusRepository;
        private ContainersExternalDataRepository containersExternalDataRepository;
        private ShipmentRepository shipmentRepository;
        private ShipmentQuery shipmentQuery;
        private ContainerQuery containerQuery;
        private PortRepository portRepository;
        private VesselRepository vesselRepository;
        private PortTimeZoneRepository portTimeZoneRepository;
        private CommunicationLogRepository communicationLogRepository;
        private ComputingPartnerTranslationHelper computingPartnerTranslator;

        private ContainersExternalData containersExternalData_DB;
        private ContainersExternalData containersExternalData_New;
        private ContainersExternal containersExternal;

        #region XML Properties
        private string oceanInsightsId;
        private string container_number;
        private string shipmentPackagesId;
        private string container_number_FromXML;
        private string carrier_scac;
        private string container_status;
        private string details;
        private string weight;
        private string createdDate;
        private string eventCode;
        private string ETD_initial;
        private string ETD_last;
        private string ATD_actual;
        private string ATD_detected;
        private string ETA_initial;
        private string ETA_last;
        private string ETA_predection;
        private string ATA_actual;
        private string ATA_detected;
        private string emptyPickup_last;
        private string emptyPickup_initial;
        private string emptyPickup_actual;
        private string emptyPickupLocation;
        private string emptyPickupTimeZone;
        private string gateInDate_last;
        private string gateInDate_initial;
        private string gateInDate_actual;
        private string departureLocation;
        private string destinationLocation;
        private string origin_loc_locode;
        private string origin_loc_timezone;
        private string origin_pickup_planned_initial = null;
        private string origin_pickup_planned_last = null;
        private string origin_pickup_actual = null;
        private string pol_loc_locode = null;
        private string pol_loc_timezone = null;
        private string pol_loaded_planned_initial = null;
        private string pol_loaded_planned_last = null;
        private string pol_loaded_actual = null;
        private string ts_count = null;
        private string tsp1_loc_locode = null;
        private string tsp1_loc_timezone = null;
        private string tsp1_vslarrival_planned_initial = null;
        private string tsp1_vslarrival_planned_last = null;
        private string tsp1_vslarrival_actual = null;
        private string tsp1_vslarrival_detected = null;
        private string tsp1_discharge_planned_last = null;
        private string tsp1_discharge_actual = null;
        private string tsp1_loaded_planned_initial = null;
        private string tsp1_loaded_planned_last = null;
        private string tsp1_loaded_actual = null;
        private string tsp1_vsldeparture_planned_initial = null;
        private string tsp1_vsldeparture_planned_last = null;
        private string tsp1_vsldeparture_actual = null;
        private string tsp1_vsldeparture_detected = null;
        private string tsp1_discharge_planned_initial = null;
        private string tsp2_loc_locode = null;
        private string tsp2_loc_timezone = null;
        private string tsp2_vslarrival_planned_initial = null;
        private string tsp2_vslarrival_planned_last = null;
        private string tsp2_vslarrival_actual = null;
        private string tsp2_vslarrival_detected = null;
        private string tsp2_discharge_planned_initial = null;
        private string tsp2_discharge_planned_last = null;
        private string tsp2_discharge_actual = null;
        private string tsp2_loaded_planned_initial = null;
        private string tsp2_loaded_planned_last = null;
        private string tsp2_loaded_actual = null;
        private string tsp2_vsldeparture_planned_initial = null;
        private string tsp2_vsldeparture_planned_last = null;
        private string tsp2_vsldeparture_actual = null;
        private string tsp2_vsldeparture_detected = null;
        private string tsp3_loc_locode = null;
        private string tsp3_loc_timezone = null;
        private string tsp3_vslarrival_planned_initial = null;
        private string tsp3_vslarrival_planned_last = null;
        private string tsp3_vslarrival_actual = null;
        private string tsp3_vslarrival_detected = null;
        private string tsp3_discharge_planned_initial = null;
        private string tsp3_discharge_planned_last = null;
        private string tsp3_discharge_actual = null;
        private string tsp3_loaded_planned_initial = null;
        private string tsp3_loaded_planned_last = null;
        private string tsp3_loaded_actual = null;
        private string tsp3_vsldeparture_planned_initial = null;
        private string tsp3_vsldeparture_planned_last = null;
        private string tsp3_vsldeparture_actual = null;
        private string tsp3_vsldeparture_detected = null;
        private string tsp4_loc_locode = null;
        private string tsp4_loc_timezone = null;
        private string tsp4_vslarrival_planned_initial = null;
        private string tsp4_vslarrival_planned_last = null;
        private string tsp4_vslarrival_actual = null;
        private string tsp4_vslarrival_detected = null;
        private string tsp4_discharge_planned_initial = null;
        private string tsp4_discharge_planned_last = null;
        private string tsp4_discharge_actual = null;
        private string tsp4_loaded_planned_initial = null;
        private string tsp4_loaded_planned_last = null;
        private string tsp4_loaded_actual = null;
        private string tsp4_vsldeparture_planned_initial = null;
        private string tsp4_vsldeparture_planned_last = null;
        private string tsp4_vsldeparture_actual = null;
        private string tsp4_vsldeparture_detected = null;
        private string leg1_vessel_name = null;
        private string leg1_voyage = null;
        private string leg2_vessel_name = null;
        private string leg2_voyage = null;
        private string leg3_vessel_name = null;
        private string leg3_voyage = null;
        private string leg4_vessel_name = null;
        private string leg4_voyage = null;
        private string leg5_vessel_name = null;
        private string leg5_voyage = null;
        private string pod_loc_locode = null;
        private string pod_loc_timezone = null;
        private string pod_discharge_planned_initial = null;
        private string pod_discharge_planned_last = null;
        private string pod_discharge_actual = null;
        private string pod_departure_planned_initial = null;
        private string pod_departure_planned_last = null;
        private string pod_departure_actual = null;
        private string dlv_loc_locode = null;
        private string dlv_loc_timezone = null;
        private string dlv_delivery_planned_initial = null;
        private string dlv_delivery_planned_last = null;
        private string dlv_delivery_actual = null;
        private string lif_loc_locode = null;
        private string lif_loc_timezone = null;
        private string lif_arrival_planned_initial = null;
        private string lif_arrival_planned_last = null;
        private string lif_arrival_actual = null;
        private string lif_departure_planned_initial = null;
        private string lif_departure_planned_last = null;
        private string lif_departure_actual = null;
        private string empty_return_loc_locode = null;
        private string empty_return_loc_timezone = null;
        private string empty_return_planned_initial = null;
        private string empty_return_planned_last = null;
        private string empty_return_actual = null;
        private string customs_release_date = null;
        private string carrier_release_date = null;
        private string customs_release_state = null;
        private string carrier_release_state = null;
        private string availability_date = null;
        private string availability_locode = null;
        private string availability_timezone = null;
        private string POLShipmentUpdateIndicator = null;
        private string PODShipmentUpdateIndicator = null;
        private string computingPartnerCode;
        private string xmlId = null;
        private ContainerUpdatedFields containerUpdatedFields;
        #endregion

        public OceanInsightAnalyzer(ArrayOfQueueTask externalTasksQueues)
        {
            this.externalTasksQueues = externalTasksQueues;
            this.computingPartnerCode = "G-OCI";
            this.logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(0);
        }

        public ContainerUpdatedFields Run()
        {
            this.AnalyzeOceanInsightsParametersXML();
            this.GetLogitudeOceanInsights();
            this.ProcessLogitudeTenant();
            return containerUpdatedFields;
        }

        private void AnalyzeOceanInsightsParametersXML()
        {
            var oceanInsightsQueueTask = externalTasksQueues.QueueTask.Where(a => a.Action == "OceanInsights.PushUpdate").FirstOrDefault();
            if (oceanInsightsQueueTask != null)
            {
                var oceanInsightsParameters = oceanInsightsQueueTask.Parameters.FirstOrDefault();
                if (oceanInsightsParameters != null)
                {
                    oceanInsightsEnvelopeParameters = oceanInsightsParameters.Value;
                    this.GetOceanInsightsParametersXMLFields();
                }
            }
        }
        private void GetOceanInsightsParametersXMLFields()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(oceanInsightsEnvelopeParameters);
            XmlNodeList xnList = xmlDoc.SelectNodes("//container");
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
                xmlId = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "id").FirstOrDefault()?.InnerText;
                createdDate = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "created").FirstOrDefault()?.InnerText;
                eventCode = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "code").FirstOrDefault()?.InnerText;

                XmlElement detailsElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "details").FirstOrDefault();
                if (detailsElement != null)
                {
                    details = detailsElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "message").FirstOrDefault()?.InnerText;
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
                this.GetAvailabilityLocationElement(node);
            }
        }
        private void GetDirectFieldsOfShipment(XmlNode node)
        {
            oceanInsightsId = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "shipmentsubscription_id").FirstOrDefault()?.InnerText;
            container_number = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
            container_number_FromXML = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
            carrier_scac = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_scac").FirstOrDefault()?.InnerText;
            container_status = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "status").FirstOrDefault()?.InnerText;
            weight = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "weight").FirstOrDefault()?.InnerText;
            ETD_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            ETD_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            ATD_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_actual").FirstOrDefault()?.InnerText;
            ATD_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_vsldeparture_detected").FirstOrDefault()?.InnerText;
            ETA_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            ETA_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            ETA_predection = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_predection").FirstOrDefault()?.InnerText;
            ATA_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_actual").FirstOrDefault()?.InnerText;
            ATA_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_vslarrival_detected").FirstOrDefault()?.InnerText;
            emptyPickup_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_planned_last").FirstOrDefault()?.InnerText;
            emptyPickup_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_planned_initial").FirstOrDefault()?.InnerText;
            emptyPickup_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_actual").FirstOrDefault()?.InnerText;
            gateInDate_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_planned_last").FirstOrDefault()?.InnerText;
            gateInDate_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_planned_initial").FirstOrDefault()?.InnerText;
            gateInDate_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_arrival_actual").FirstOrDefault()?.InnerText;
            origin_pickup_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_planned_initial").FirstOrDefault()?.InnerText;
            origin_pickup_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_planned_last").FirstOrDefault()?.InnerText;
            origin_pickup_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_pickup_actual").FirstOrDefault()?.InnerText;
            pol_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_planned_initial").FirstOrDefault()?.InnerText;
            pol_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_planned_last").FirstOrDefault()?.InnerText;
            pol_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loaded_actual").FirstOrDefault()?.InnerText;
            ts_count = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "ts_count").FirstOrDefault()?.InnerText;
            pod_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_discharge_planned_last").FirstOrDefault()?.InnerText;
            pod_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_discharge_planned_initial").FirstOrDefault()?.InnerText;
            pod_departure_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_planned_initial").FirstOrDefault()?.InnerText;
            pod_departure_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_planned_last").FirstOrDefault()?.InnerText;
            pod_departure_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_departure_actual").FirstOrDefault()?.InnerText;
            pod_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_discharge_actual").FirstOrDefault()?.InnerText;
        }
        private void GetEmptyPickupLocationElement(XmlNode node)
        {
            XmlElement emptyPickupLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_loc").FirstOrDefault();
            if (emptyPickupLocationElement != null)
            {
                emptyPickupLocation = emptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                emptyPickupTimeZone = emptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }
        }
        private void GetDepartureLocationElement(XmlNode node)
        {
            XmlElement departureLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loc").FirstOrDefault();
            if (departureLocationElement != null)
            {
                departureLocation = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                pol_loc_locode = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                pol_loc_timezone = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }
        }
        private void GetDestinationLocationElement(XmlNode node)
        {
            XmlElement destinationLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_loc").FirstOrDefault();
            if (destinationLocationElement != null)
            {
                destinationLocation = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                pod_loc_locode = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                pod_loc_timezone = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;

            }
        }
        private void GetOriginLocationElement(XmlNode node)
        {
            XmlElement origin_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_loc").FirstOrDefault();
            if (origin_locElement != null)
            {
                origin_loc_locode = origin_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                origin_loc_timezone = origin_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }
        }
        private void GetTransshipment1Leg(XmlNode node)
        {
            XmlElement tsp1_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loc").FirstOrDefault();
            if (tsp1_locElement != null)
            {
                tsp1_loc_locode = tsp1_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                tsp1_loc_timezone = tsp1_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            tsp1_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            tsp1_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            tsp1_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_actual").FirstOrDefault()?.InnerText;
            tsp1_vslarrival_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vslarrival_detected").FirstOrDefault()?.InnerText;
            tsp1_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_planned_initial").FirstOrDefault()?.InnerText;
            tsp1_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_planned_last").FirstOrDefault()?.InnerText;
            tsp1_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_discharge_actual").FirstOrDefault()?.InnerText;
            tsp1_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_planned_initial").FirstOrDefault()?.InnerText;
            tsp1_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_planned_last").FirstOrDefault()?.InnerText;
            tsp1_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loaded_actual").FirstOrDefault()?.InnerText;
            tsp1_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            tsp1_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            tsp1_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_actual").FirstOrDefault()?.InnerText;
            tsp1_vsldeparture_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_vsldeparture_detected").FirstOrDefault()?.InnerText;
        }
        private void GetTransshipment2Leg(XmlNode node)
        {
            XmlElement tsp2_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loc").FirstOrDefault();
            if (tsp2_locElement != null)
            {
                tsp2_loc_locode = tsp2_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                tsp2_loc_timezone = tsp2_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            tsp2_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            tsp2_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            tsp2_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_actual").FirstOrDefault()?.InnerText;
            tsp2_vslarrival_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vslarrival_detected").FirstOrDefault()?.InnerText;
            tsp2_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_planned_initial").FirstOrDefault()?.InnerText;
            tsp2_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_planned_last").FirstOrDefault()?.InnerText;
            tsp2_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_discharge_actual").FirstOrDefault()?.InnerText;
            tsp2_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_planned_initial").FirstOrDefault()?.InnerText;
            tsp2_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_planned_last").FirstOrDefault()?.InnerText;
            tsp2_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_loaded_actual").FirstOrDefault()?.InnerText;
            tsp2_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            tsp2_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            tsp2_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_actual").FirstOrDefault()?.InnerText;
            tsp2_vsldeparture_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp2_vsldeparture_detected").FirstOrDefault()?.InnerText;
        }
        private void GetTransshipment3Leg(XmlNode node)
        {
            XmlElement tsp3_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loc").FirstOrDefault();
            if (tsp3_locElement != null)
            {
                tsp3_loc_locode = tsp3_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                tsp3_loc_timezone = tsp3_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            tsp3_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            tsp3_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            tsp3_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_actual").FirstOrDefault()?.InnerText;
            tsp3_vslarrival_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vslarrival_detected").FirstOrDefault()?.InnerText;
            tsp3_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_planned_initial").FirstOrDefault()?.InnerText;
            tsp3_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_planned_last").FirstOrDefault()?.InnerText;
            tsp3_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_discharge_actual").FirstOrDefault()?.InnerText;
            tsp3_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_planned_initial").FirstOrDefault()?.InnerText;
            tsp3_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_planned_last").FirstOrDefault()?.InnerText;
            tsp3_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_loaded_actual").FirstOrDefault()?.InnerText;
            tsp3_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            tsp3_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            tsp3_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_actual").FirstOrDefault()?.InnerText;
            tsp3_vsldeparture_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp3_vsldeparture_detected").FirstOrDefault()?.InnerText;
        }
        private void GetTransshipment4Leg(XmlNode node)
        {
            XmlElement tsp4_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loc").FirstOrDefault();
            if (tsp4_locElement != null)
            {
                tsp4_loc_locode = tsp4_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                tsp4_loc_timezone = tsp4_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            tsp4_vslarrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_planned_initial").FirstOrDefault()?.InnerText;
            tsp4_vslarrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_planned_last").FirstOrDefault()?.InnerText;
            tsp4_vslarrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_actual").FirstOrDefault()?.InnerText;
            tsp4_vslarrival_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vslarrival_detected").FirstOrDefault()?.InnerText;
            tsp4_discharge_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_planned_initial").FirstOrDefault()?.InnerText;
            tsp4_discharge_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_planned_last").FirstOrDefault()?.InnerText;
            tsp4_discharge_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_discharge_actual").FirstOrDefault()?.InnerText;
            tsp4_loaded_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_planned_initial").FirstOrDefault()?.InnerText;
            tsp4_loaded_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_planned_last").FirstOrDefault()?.InnerText;
            tsp4_loaded_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_loaded_actual").FirstOrDefault()?.InnerText;
            tsp4_vsldeparture_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
            tsp4_vsldeparture_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
            tsp4_vsldeparture_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_actual").FirstOrDefault()?.InnerText;
            tsp4_vsldeparture_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp4_vsldeparture_detected").FirstOrDefault()?.InnerText;
        }
        private void GetLeg1Element(XmlNode node)
        {
            XmlElement leg1_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg1_vessel").FirstOrDefault();
            if (leg1_vessel_Element != null)
            {
                leg1_vessel_name = leg1_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            leg1_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg1_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg2Element(XmlNode node)
        {
            XmlElement leg2_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg2_vessel").FirstOrDefault();
            if (leg2_vessel_Element != null)
            {
                leg2_vessel_name = leg2_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            leg2_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg2_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg3Element(XmlNode node)
        {
            XmlElement leg3_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg3_vessel").FirstOrDefault();
            if (leg3_vessel_Element != null)
            {
                leg3_vessel_name = leg3_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            leg3_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg3_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg4Element(XmlNode node)
        {
            XmlElement leg4_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg4_vessel").FirstOrDefault();
            if (leg4_vessel_Element != null)
            {
                leg4_vessel_name = leg4_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            leg4_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg4_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetLeg5Element(XmlNode node)
        {
            XmlElement leg5_vessel_Element = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg5_vessel").FirstOrDefault();
            if (leg5_vessel_Element != null)
            {
                leg5_vessel_name = leg5_vessel_Element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
            }
            leg5_voyage = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "leg5_voyage").FirstOrDefault()?.InnerText;
        }
        private void GetDeliveryLocationElement(XmlNode node)
        {
            XmlElement dlv_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_loc").FirstOrDefault();
            if (dlv_locElement != null)
            {
                dlv_loc_locode = dlv_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                dlv_loc_timezone = dlv_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            dlv_delivery_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_planned_initial").FirstOrDefault()?.InnerText;
            dlv_delivery_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_planned_last").FirstOrDefault()?.InnerText;
            dlv_delivery_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "dlv_delivery_actual").FirstOrDefault()?.InnerText;
        }
        private void GetLifLocationElement(XmlNode node)
        {
            XmlElement lif_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_loc").FirstOrDefault();
            if (lif_locElement != null)
            {
                lif_loc_locode = lif_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                lif_loc_timezone = lif_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            lif_arrival_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_planned_last").FirstOrDefault()?.InnerText;
            lif_arrival_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_planned_initial").FirstOrDefault()?.InnerText;
            lif_arrival_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_arrival_actual").FirstOrDefault()?.InnerText;
            lif_departure_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_planned_initial").FirstOrDefault()?.InnerText;
            lif_departure_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_planned_last").FirstOrDefault()?.InnerText;
            lif_departure_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "lif_departure_actual").FirstOrDefault()?.InnerText;
        }
        private void GetEmptyReturnElement(XmlNode node)
        {
            XmlElement empty_return_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_loc").FirstOrDefault();
            if (empty_return_locElement != null)
            {
                empty_return_loc_locode = empty_return_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                empty_return_loc_timezone = empty_return_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }

            empty_return_planned_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_planned_initial").FirstOrDefault()?.InnerText;
            empty_return_planned_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_planned_last").FirstOrDefault()?.InnerText;
            empty_return_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_return_actual").FirstOrDefault()?.InnerText;
            customs_release_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "customs_release_date").FirstOrDefault()?.InnerText;
            customs_release_state = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "customs_release_state").FirstOrDefault()?.InnerText;
            carrier_release_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_release_date").FirstOrDefault()?.InnerText;
            carrier_release_state = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_release_state").FirstOrDefault()?.InnerText;
            availability_date = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "availability_date").FirstOrDefault()?.InnerText;
        }
        private void GetAvailabilityLocationElement(XmlNode node)
        {
            XmlElement availabilityemptyPickupLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "availability_loc").FirstOrDefault();
            if (availabilityemptyPickupLocationElement != null)
            {
                availability_locode = availabilityemptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
                availability_timezone = availabilityemptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "timezone").FirstOrDefault()?.InnerText;
            }
        }

        private void GetLogitudeOceanInsights()
        {
            if (!string.IsNullOrEmpty(this.oceanInsightsId))
            {
                this.GetLogitudeOceanInsightsByOceanInsightsId();
            }

            if (string.IsNullOrEmpty(this.oceanInsightsId) || this.oceanInsights == null)
            {
                this.GetLogitudeOceanInsightsByOceanInsightsContainerNumberAndScac();
            }
        }
        private void GetLogitudeOceanInsightsByOceanInsightsId()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                oceanInsights = this.logitudeOceanInsightsRequestRepository.GetLogitudeOceanInsightsRequestByOceanInsigntId(this.oceanInsightsId);
                scope.Complete();
            }
        }
        private void GetLogitudeOceanInsightsByOceanInsightsContainerNumberAndScac()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                oceanInsights = this.logitudeOceanInsightsRequestRepository.GetLogitudeOceanInsightsRequestByContainerNumberAndScac(container_number_FromXML, this.carrier_scac);
                scope.Complete();
            }
        }

        private void ProcessLogitudeTenant()
        {
            if (oceanInsights != null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    foreach (var item in oceanInsights)
                    {
                        this.logitudeTenant = item.Tenant;
                        if (this.logitudeTenant != null)
                        {
                            this.Initialize();
                            this.GetShipmentById(item);
                            this.GetContainerDataByContainerNumber(item);

                            if (IsCommunicationLogsExsit())
                            {
                                continue;
                            }

                            this.AddContainerStatusCommunicationLog(item);
                            this.CreateLogitudeOceanInsightsResponse();

                            if (IsUpdatingShipmentAndContainer())
                            {
                                this.CreateShipmentContainerStatus(item);
                                this.GetContainersExternalData();
                                this.BuildContainerUpdatedFields();                                
                            }
                        }
                    }
                    scope.Complete();
                }
            }
        }

        private void Initialize()
        {
            this.shipmentContext = ShipmentsContext.GetContext(logitudeTenant.Value);
            this.shipmentContainerStatusRepository = new ShipmentContainerStatusRepository(shipmentContext);
            this.containerRepository = new ContainerRepository(shipmentContext);
            this.containerQuery = new ContainerQuery(containerRepository);
            this.containerStatusRepository = new ContainerStatusRepository(shipmentContext);
            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.shipmentQuery = new ShipmentQuery(shipmentRepository);
            this.portRepository = new PortRepository(logitudeTenant.Value);
            this.vesselRepository = new VesselRepository(logitudeTenant.Value);
            this.portTimeZoneRepository = new PortTimeZoneRepository(logitudeTenant.Value);
            this.computingPartnerTranslator = new ComputingPartnerTranslationHelper(logitudeTenant.Value);
            this.containersExternalDataRepository = new ContainersExternalDataRepository(shipmentContext);
        }
        private void GetShipmentById(LogitudeOceanInsightsRequest oceanInsight)
        {
            shipmentPM = shipmentQuery.GetSinglePM(oceanInsight?.ShipmentId, logitudeTenant.Value, null, true);
        }
        private void GetContainerDataByContainerNumber(LogitudeOceanInsightsRequest oceanInsight)
        {
            container_number = this.GetContainerNumber(oceanInsight);
            container = containerQuery.GetContainerByNumberAndShipmentIdAndTenant(container_number, oceanInsight.ShipmentId, logitudeTenant.Value);
            containerId = container?.Id;
            shipmentPackagesId = this.GetShipmentPackagesId(container);
        }
        private string GetContainerNumber(LogitudeOceanInsightsRequest oceanInsight)
        {
            string containerNumber = "";

            if (!string.IsNullOrEmpty(oceanInsight.ContainerNumber))
            {
                containerNumber = oceanInsight.ContainerNumber;
            }
            else
            {
                containerNumber = this.GetContainerNumberFromShipmentContainers();
            }

            return containerNumber;
        }
        private string GetContainerNumberFromShipmentContainers()
        {
            string containerNumber = "";
            var package = shipmentPM?.ShipmentPackages?.Where(a => a.ContainerNumber == container_number_FromXML).FirstOrDefault();
            if (package != null)
            {
                containerNumber = package.ContainerNumber;
            }
            else
            {
                this.objectTableName = "Shipment";
            }
            return containerNumber;
        }
        private string GetShipmentPackagesId(ContainerPM container)
        {
            if (container != null)
            {
                return container.ShipmentPackagesId;
            }
            else
            {
                return this.GetShipmentPackagesIdFromShipmentContainers();
            }
        }
        private string GetShipmentPackagesIdFromShipmentContainers()
        {
            string shipmentPackagesId = "";
            var package = shipmentPM?.ShipmentPackages?.Where(a => a.ContainerNumber == container_number_FromXML).FirstOrDefault();
            if (package != null)
            {
                shipmentPackagesId = package.Id;
                this.containerId = package.ContainerEntityId;
            }

            return shipmentPackagesId;
        }

        private bool IsCommunicationLogsExsit()
        {
            int tenant = this.logitudeTenant.Value;
            this.communicationLogRepository = new CommunicationLogRepository(tenant);
            var isCommunicationLogsExsit = this.communicationLogRepository.IsCommunicationLogExsit(xmlId, tenant);
            return isCommunicationLogsExsit;
        }

        private void AddContainerStatusCommunicationLog(LogitudeOceanInsightsRequest oceanInsight)
        {
            this.commonContext = CommonDataContext.GetContext(this.logitudeTenant.Value);
            this.communicationLogRepository = new CommunicationLogRepository(this.logitudeTenant.Value);
            this.GetCommuniactionLogObjectTableId();
            this.GetLoggedContactId();
            this.BuildCommunicationLog(oceanInsight);
        }
        private void GetCommuniactionLogObjectTableId()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(logitudeTenant.Value);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                objectTableId = objectTable.Id;
            }
        }
        private void GetLoggedContactId()
        {
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }
            else
            {
                email = "system@tenant" + logitudeTenant.Value + ".com";
            }
            ContactRepository contactRepository = new ContactRepository(this.commonContext);
            var loggedContact = contactRepository.GetSingleContactByEmail(email, logitudeTenant.Value);
            this.loggedContactId = loggedContact.Id;
        }
        private void BuildCommunicationLog(LogitudeOceanInsightsRequest oceanInsight)
        {
            CommunicationsParams logParams = new CommunicationsParams()
            {
                Tenant = logitudeTenant.Value,
                From = "Amital",
                To = "Logitude",
                CommunicationLogTypeCode = "A",
                Priority = 1,
                InOut = "I",
                Status = "D",
                LoggingUserId = this.loggedContactId,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = string.IsNullOrEmpty(container_number) ? oceanInsight?.ShipmentId : containerId,
                LoggingEntityReference = string.IsNullOrEmpty(container_number) ? oceanInsight?.BLNumber : container_number,
                Subject = communicationLogSubject,
                FolderName = communicationLogTo.ToLower(),
                ByteData = GetXMLByteDataFromText(),
                UniqueNumber = xmlId
            };

            logParams.WasAnalyzed = IsWasAnalyzed();
            Communications.AddCommunicationLog(logParams);
        }
        private byte[] GetXMLByteDataFromText()
        {
            var doc = new XmlDocument();
            doc.LoadXml(oceanInsightsEnvelopeParameters);
            var memoryStream = new MemoryStream();
            var xmlWriter = XmlWriter.Create(memoryStream,
                        new XmlWriterSettings
                        {
                            OmitXmlDeclaration = false,
                            ConformanceLevel = ConformanceLevel.Document,
                            Encoding = UTF8Encoding.UTF8
                        });
            doc.Save(xmlWriter);
            byte[] documentXML = memoryStream.ToArray();
            return documentXML;
        }
        private bool? IsWasAnalyzed()
        {
            if (IsUpdatingShipmentAndContainer())
            {
                return true;
            }
            return false;
        }

        private void CreateLogitudeOceanInsightsResponse()
        {
            if (container != null)
            {
                LogitudeOceanInsightsResponseRepository logitudeOceanInsightsResponseRepository = new LogitudeOceanInsightsResponseRepository(logitudeTenant.Value);
                LogitudeOceanInsightsResponse logitudeOceanInsightsResponse = logitudeOceanInsightsResponseRepository.GetLogitudeOceanInsightsResponseByContainerNumberAndScac(container_number, carrier_scac, logitudeTenant.Value);
                if (logitudeOceanInsightsResponse == null)
                {
                    logitudeOceanInsightsResponse = new LogitudeOceanInsightsResponse()
                    {
                        Id = IdCounter.GetNumber("LogitudeOceanInsightsResponse", logitudeTenant.Value),
                        FirstResponseDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value),
                        LastResponseDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value),
                        ContainerNumber = container_number,
                        SCACCode = carrier_scac,
                        Tenant = logitudeTenant != null ? logitudeTenant.Value : 0,
                        CarrierName = GetCarrierName()
                    };
                    logitudeOceanInsightsResponseRepository.Add(logitudeOceanInsightsResponse);
                }
                else
                {
                    logitudeOceanInsightsResponse.LastResponseDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
                    logitudeOceanInsightsResponseRepository.Update(logitudeOceanInsightsResponse);
                }

                logitudeOceanInsightsResponseRepository.SubmitChanges();
            }
        }
        private string GetCarrierName()
        {
            string carrierName = "";
            CardRepository cardRepository = new CardRepository(logitudeTenant.Value);
            Card shippingLine = cardRepository.GetSingleCard(container.MainCarriageCarrierId, logitudeTenant.Value);
            carrierName = shippingLine?.EnglishName;
            return carrierName;
        }

        private void CreateShipmentContainerStatus(LogitudeOceanInsightsRequest oceanInsight)
        {
            string iHash = this.GetHashedData(oceanInsight.ShipmentId);
            DateTime logDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
            DateTime? eventDate = this.GetEventDate();
            double containerWeight = this.GetContainerWeight();
            DateTime? departureDate = this.ComputeDepartureDate();
            DateTime? arrivalDate = this.ComputeArrivalDate();
            string departureDateInfo = this.ComputeDepartureDateInfo();
            string arrivalDateInfo = this.ComputeArrivalDateInfo();
            string statusDetails = this.GetStatusDetails();

            ShipmentContainerStatus containerStatus = new ShipmentContainerStatus()
            {
                Id = IdCounter.GetNumber("ShipmentContainerStatus", 0),
                Tenant = this.logitudeTenant.Value,
                ShipmentId = oceanInsight.ShipmentId,
                StatusSource = "OIN",
                ContainerStatusCode = this.container_status,
                Details = statusDetails,
                RecordHash = iHash,
                Weight = containerWeight,
                ReceivingDate = logDate,
                EventDate = eventDate,
                ContainerId = this.shipmentPackagesId,
                ContainerNumber = this.container_number,
                DepartureDate = departureDate,
                ArrivalDate = arrivalDate,
                TimeOfDepartureInfo = departureDateInfo,
                TimeOfArrivalInfo = arrivalDateInfo,
            };

            shipmentContainerStatusRepository.Add(containerStatus);
            shipmentContainerStatusRepository.SubmitChanges();
        }

        private string GetHashedData(string shipmentId)
        {
            string information = shipmentId + "0" + this.container_number;
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;
        }
        private DateTime? GetEventDate()
        {
            if (!string.IsNullOrEmpty(createdDate))
            {
                return AnalyzeEventDateValue(createdDate);
            }
            return null;
        }
        private DateTime? AnalyzeEventDateValue(string XMLValue)
        {
            XMLDateParts xMLDateParts = this.GetXMLDateParts(XMLValue);
            XMLDateParts currentTenantTimeZoneDateParts = this.GetCurrentTenantTimeZoneDateParts();
            return ComputeDateTimeRqgardingTimeZone(xMLDateParts, currentTenantTimeZoneDateParts);
        }
        private XMLDateParts GetXMLDateParts(string XMLValue)
        {
            XMLDateParts xMLDateParts = new XMLDateParts();

            if (string.IsNullOrEmpty(XMLValue))
            {
                return null;
            }

            if (XMLValue.Length > 16)
            {
                xMLDateParts.DateTimeValue = ConvertStringToDateTime(XMLValue.Substring(0, 16));
                xMLDateParts.TimeZoneSign = XMLValue.Substring(16, 1);
                xMLDateParts.TimeZoneHoursValue = ConvertStringToInteger(XMLValue.Substring(17, 2));
                xMLDateParts.TimeZoneMinutsValue = ConvertStringToInteger(XMLValue.Substring(19, 2));
                xMLDateParts.TimeZone = new TimeSpan(xMLDateParts.TimeZoneHoursValue, xMLDateParts.TimeZoneMinutsValue, 0);
            }

            else
            {
                xMLDateParts.DateTimeValue = ConvertStringToDateTime(XMLValue);
            }

            return xMLDateParts;
        }
        private XMLDateParts GetCurrentTenantTimeZoneDateParts()
        {
            Tenant tenant = TenantRepository.GetSingleTenant(logitudeTenant.Value, true);
            double? currentTenantTimeZone = tenant.TimeZoneOffset;
            XMLDateParts xMLDateParts = new XMLDateParts();
            xMLDateParts.TimeZoneSign = currentTenantTimeZone > 0 ? "" : "-";
            xMLDateParts.TimeZoneHoursValue = ConvertStringToInteger(currentTenantTimeZone?.ToString());
            xMLDateParts.TimeZoneMinutsValue = 0;
            xMLDateParts.TimeZone = new TimeSpan(xMLDateParts.TimeZoneHoursValue, xMLDateParts.TimeZoneMinutsValue, 0);
            return xMLDateParts;
        }
        private DateTime? ConvertStringToDateTime(string dateValue)
        {
            if (!string.IsNullOrEmpty(dateValue))
            {
                return Convert.ToDateTime(dateValue);
            }

            else
            {
                return null;
            }
        }
        private int ConvertStringToInteger(string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                return Convert.ToInt32(stringValue);
            }

            else
            {
                return 0;
            }
        }
        private DateTime? ComputeDateTimeRqgardingTimeZone(XMLDateParts xMLDateParts, XMLDateParts portTimeZoneDateParts)
        {
            if (xMLDateParts == null && portTimeZoneDateParts == null)
            {
                return null;
            }

            DateTime computesDateTime = xMLDateParts.DateTimeValue.Value;
            computesDateTime = this.ProcessPortTimeZoneCalculations(computesDateTime, portTimeZoneDateParts);
            computesDateTime = this.ProcessXMLTimeZoneCalculations(computesDateTime, xMLDateParts);
            return computesDateTime;
        }
        private DateTime ProcessPortTimeZoneCalculations(DateTime computesDateTime, XMLDateParts portTimeZoneDateParts)
        {
            DateTime calculatedDate = computesDateTime;
            if (portTimeZoneDateParts.TimeZoneSign == "-")
            {
                calculatedDate = computesDateTime.Subtract(portTimeZoneDateParts.TimeZone);
            }

            else
            {
                calculatedDate = computesDateTime.Add(portTimeZoneDateParts.TimeZone);
            }

            return calculatedDate;
        }
        private DateTime ProcessXMLTimeZoneCalculations(DateTime computesDateTime, XMLDateParts xMLDateParts)
        {
            DateTime calculatedDate = computesDateTime;
            if (xMLDateParts.TimeZoneSign == "-")
            {
                calculatedDate = computesDateTime.Add(xMLDateParts.TimeZone);
            }

            else
            {
                calculatedDate = computesDateTime.Subtract(xMLDateParts.TimeZone);
            }

            return calculatedDate;
        }
        private double GetContainerWeight()
        {
            double containerWeight = 0;
            if (!string.IsNullOrEmpty(weight))
            {
                Double.TryParse(weight, out containerWeight);
            }

            return containerWeight;
        }
        private DateTime? ComputeDepartureDate()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return ConvertStringToDateTime_old(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return ConvertStringToDateTime_old(ATD_actual);
            }

            else if (!string.IsNullOrEmpty(ETD_last))
            {
                return ConvertStringToDateTime_old(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return ConvertStringToDateTime_old(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeArrivalDate()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return ConvertStringToDateTime_old(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return ConvertStringToDateTime_old(ATA_actual);
            }

            else if (!string.IsNullOrEmpty(ETA_last))
            {
                return ConvertStringToDateTime_old(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return ConvertStringToDateTime_old(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return ConvertStringToDateTime_old(ETA_predection);
            }

            return null;
        }
        private string ComputeDepartureDateInfo()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return "A";
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return "A";
            }

            else if (!string.IsNullOrEmpty(ETD_last))
            {
                return "E";
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return "E";
            }

            return "";
        }
        private string ComputeArrivalDateInfo()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return "A";
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return "A";
            }

            else if (!string.IsNullOrEmpty(ETA_last))
            {
                return "E";
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return "E";
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return "E";
            }

            return "";
        }
        public DateTime? ConvertStringToDateTime_old(string XMLValue)
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
        private string GetStatusDetails()
        {
            string statusDetails = null;

            if (!string.IsNullOrEmpty(this.details))
            {
                if (this.details.Length > 250)
                {
                    statusDetails = this.details.Substring(0, 250);
                }

                else
                {
                    statusDetails = this.details;
                }
            }

            return statusDetails;
        }
        private bool IsUpdatingShipmentAndContainer()
        {
            if (shipmentPM.IsOperationalClosed)
            {
                return false;
            }
            if (!IsTheSamePOLLocation())
            {
                return false;
            }
            if (!IsTheSamePODLocation())
            {
                return false;
            }
            if (this.eventCode != null && this.eventCode != "20" &&
                                (Int32.Parse(this.eventCode) >= 0 && Int32.Parse(this.eventCode) <= 31)
                                && !string.IsNullOrEmpty(this.container_number))
            {
                return true;
            }

            return false;
        }
        private bool IsTheSamePOLLocation()
        {
            var pOLLocation = this.GetTranslatedPortCode(pol_loc_locode);
            var polPortId = this.GetPortId(pOLLocation);
            if (shipmentPM.MainCarriageFromPortId == polPortId)
            {
                return true;
            }

            return false;
        }
        private bool IsTheSamePODLocation()
        {
            var pODLocation = this.GetTranslatedPortCode(pod_loc_locode);
            var podPortId = this.GetPortId(pODLocation);
            if (shipmentPM.MainCarriageFinalDestinationPortId == podPortId)
            {
                return true;
            }

            return false;
        }
        private string GetTranslatedPortCode(string XMLportCode)
        {
            string portCode = XMLportCode;

            string translatedPortCode = this.computingPartnerTranslator.GetLogitudeCodeTranslation(XMLportCode, computingPartnerCode, "Port");
            if (!string.IsNullOrEmpty(translatedPortCode))
            {
                portCode = translatedPortCode;
            }

            return portCode;
        }
        private string GetPortId(string portCode)
        {
            Port port = portRepository.GetOceanPortByCombinedCode(portCode, this.logitudeTenant.Value);
            string portId = null;
            if (port != null)
            {
                portId = port.Id;
            }
            else
            {
                portId = this.CopyPortCopyToCurrentTenant(portCode);
            }
            return portId;
        }
        private string CopyPortCopyToCurrentTenant(string portCode)
        {
            string portId = null;
            PortQuery portQuery = new PortQuery(portRepository);
            Port portZero = portRepository.GetOceanPortByCombinedCode(portCode, 0);
            if (portZero != null)
            {
                var newPort = portQuery.GetPortCopyToCurrentTenant(portZero.Id, this.logitudeTenant.Value);
                portId = newPort.Id;
            }

            return portId;
        }

        private void GetContainersExternalData()
        {
            if (container != null)
            {
                this.containersExternal = new ContainersExternal();
                this.containersExternalData_DB = containersExternalDataRepository.GetSingleContainersExternalData(container.Id, container.Tenant);
                if (containersExternalData_DB == null)
                {
                    this.containersExternalData_DB = new ContainersExternalData() { Id = container.Id, Tenant = container.Tenant };
                    containersExternal.IsNew = true;
                }
                containersExternal.ContainersExternalData_DB = this.containersExternalData_DB;
                containersExternal.IsFromOceanInsights = true;
                this.SetContainersExternalData(containersExternal);
            }
        }
        private void SetContainersExternalData(ContainersExternal containersExternal)
        {
            this.containersExternalData_New = new ContainersExternalData() { Id = container.Id, Tenant = container.Tenant };
            this.containersExternalData_New.GateIn = this.ComputePOLGateIn();
            this.containersExternalData_New.GateOut = this.ComputPODGateOut();
            containersExternal.ContainersExternalData_New = this.containersExternalData_New;
        }

        private void BuildContainerUpdatedFields()
        {
            containerUpdatedFields = new ContainerUpdatedFields();
            containerUpdatedFields.Tenant = logitudeTenant.Value;
            containerUpdatedFields.CurrentStatusDate = this.GetEventDate();
            containerUpdatedFields.CurrentStatus = this.GetContainerStatusName();
            containerUpdatedFields.CurrentLocation = this.ComputeCurrentStatusLocation();
            containerUpdatedFields.EmptyPickupLocation = this.GetTranslatedPortCode(emptyPickupLocation);
            containerUpdatedFields.DepartureLocation = this.departureLocation;
            containerUpdatedFields.DestinationLocation = this.destinationLocation;
            containerUpdatedFields.OriginLocation = this.GetTranslatedPortCode(origin_loc_locode);
            containerUpdatedFields.POLLocation = this.GetTranslatedPortCode(pol_loc_locode);
            containerUpdatedFields.Transshipment1Location = this.GetTranslatedPortCode(tsp1_loc_locode);
            containerUpdatedFields.Transshipment2Location = this.GetTranslatedPortCode(tsp2_loc_locode);
            containerUpdatedFields.Transshipment3Location = this.GetTranslatedPortCode(tsp3_loc_locode);
            containerUpdatedFields.Transshipment4Location = this.GetTranslatedPortCode(tsp4_loc_locode);
            containerUpdatedFields.PODLocation = this.GetTranslatedPortCode(pod_loc_locode);
            containerUpdatedFields.DeliveryLocation = this.GetTranslatedPortCode(dlv_loc_locode);
            containerUpdatedFields.LIFLocation = this.GetTranslatedPortCode(lif_loc_locode);
            containerUpdatedFields.EmptyReturnLocation = this.GetTranslatedPortCode(empty_return_loc_locode);
            containerUpdatedFields.AvailabilityLocation = this.GetTranslatedPortCode(availability_locode);
            this.HandleVesselLegs();
            containerUpdatedFields.Leg1Voyage = leg1_voyage;
            containerUpdatedFields.Leg2Voyage = leg2_voyage;
            containerUpdatedFields.Leg3Voyage = leg3_voyage;
            containerUpdatedFields.Leg4Voyage = leg4_voyage;
            containerUpdatedFields.Leg5Voyage = leg5_voyage;
            containerUpdatedFields.CustomsReleaseState = customs_release_state;
            containerUpdatedFields.CarrierReleaseState = carrier_release_state;
            //containerUpdatedFields.TransshipmentCount = ts_count;
            containerUpdatedFields.MainCarriageETD = this.ComputeMainCarriageETD();
            containerUpdatedFields.MainCarriageETA = this.ComputeMainCarriageETA();
            containerUpdatedFields.MainCarriageATD = this.ComputeMainCarriageATD();
            containerUpdatedFields.MainCarriageATA = this.ComputeMainCarriageATA();
            containerUpdatedFields.EstimatedEmptyPickupDate = this.ComputeEstimatedEmptyPickupDate();
            containerUpdatedFields.ActualEmptyPickupDate = this.ComputeActualEmptyPickupDate();
            containerUpdatedFields.EstimatedPOLArrival = this.ComputeEstimatedGateInDate();
            containerUpdatedFields.ActualPOLArrival = this.ComputeActualGateInDate();
            containerUpdatedFields.EstimatedOriginPickup = this.ComputeEstimatedOriginPickup();
            containerUpdatedFields.ActualOriginPickup = this.ComputeActualOriginPickup();
            containerUpdatedFields.EstimatedPOLLoaded = this.ComputeEstimatedPOLLoaded();
            containerUpdatedFields.ActualPOLLoaded = this.ComputeActualPOLLoaded();
            containerUpdatedFields.EstimatedPOLVesselDeparture = this.ComputeEstimatedPOLVesselDeparture();
            containerUpdatedFields.ActualPOLVesselDeparture = ComputeActualPOLVesselDeparture();
            containerUpdatedFields.EstimatedTrans1VesselArrival = this.ComputeEstimatedTrans1VesselArrival();
            containerUpdatedFields.ActualTransshipment1VesselArrival = this.ComputeActualTransshipment1VesselArrival();
            containerUpdatedFields.EstimatedTransshipment1Discharge = this.ComputeEstimatedTransshipment1Discharge();
            containerUpdatedFields.ActualTransshipment1Discharge = ComputeActualTransshipment1Discharge();
            containerUpdatedFields.EstimatedTransshipment1Loaded = this.ComputeEstimatedTransshipment1Loaded();
            containerUpdatedFields.ActualTransshipment1Loaded = ComputeActualTransshipment1Loaded();
            containerUpdatedFields.EstimatedTrans1VesselDeparture = this.ComputeEstimatedTransshipment1VesselDeparture();
            containerUpdatedFields.ActualTrans1VesselDeparture = ComputeActualTransshipment1VesselDeparture();
            containerUpdatedFields.EstimatedTrans2VesselArrival = this.ComputeEstimatedTrans2VesselArrival();
            containerUpdatedFields.ActualTransshipment2VesselArrival = this.ComputeActualTransshipment2VesselArrival();
            containerUpdatedFields.EstimatedTransshipment2Discharge = this.ComputeEstimatedTransshipment2Discharge();
            containerUpdatedFields.ActualTransshipment2Discharge = ComputeActualTransshipment2Discharge();
            containerUpdatedFields.EstimatedTransshipment2Loaded = this.ComputeEstimatedTransshipment2Loaded();
            containerUpdatedFields.ActualTransshipment2Loaded = ComputeActualTransshipment2Loaded();
            containerUpdatedFields.EstimatedTrans2VesselDeparture = this.ComputeEstimatedTransshipment2VesselDeparture();
            containerUpdatedFields.ActualTrans2VesselDeparture = ComputeActualTransshipment2VesselDeparture();
            containerUpdatedFields.EstimatedTrans3VesselArrival = this.ComputeEstimatedTrans3VesselArrival();
            containerUpdatedFields.ActualTransshipment3VesselArrival = this.ComputeActualTransshipment3VesselArrival();
            containerUpdatedFields.EstimatedTransshipment3Discharge = this.ComputeEstimatedTransshipment3Discharge();
            containerUpdatedFields.ActualTransshipment3Discharge = ComputeActualTransshipment3Discharge();
            containerUpdatedFields.EstimatedTransshipment3Loaded = this.ComputeEstimatedTransshipment3Loaded();
            containerUpdatedFields.ActualTransshipment3Loaded = ComputeActualTransshipment3Loaded();
            containerUpdatedFields.EstimatedTrans3VesselDeparture = this.ComputeEstimatedTransshipment3VesselDeparture();
            containerUpdatedFields.ActualTrans3VesselDeparture = ComputeActualTransshipment3VesselDeparture();
            containerUpdatedFields.EstimatedTrans4VesselArrival = this.ComputeEstimatedTrans4VesselArrival();
            containerUpdatedFields.ActualTransshipment4VesselArrival = this.ComputeActualTransshipment4VesselArrival();
            containerUpdatedFields.EstimatedTransshipment4Discharge = this.ComputeEstimatedTransshipment4Discharge();
            containerUpdatedFields.ActualTransshipment4Discharge = ComputeActualTransshipment4Discharge();
            containerUpdatedFields.EstimatedTransshipment4Loaded = this.ComputeEstimatedTransshipment4Loaded();
            containerUpdatedFields.ActualTransshipment4Loaded = ComputeActualTransshipment4Loaded();
            containerUpdatedFields.EstimatedTrans4VesselDeparture = this.ComputeEstimatedTransshipment4VesselDeparture();
            containerUpdatedFields.ActualTrans4VesselDeparture = ComputeActualTransshipment4VesselDeparture();
            containerUpdatedFields.EstimatedPODVesselArrival = ComputeEstimatedPODVesselArrival();
            containerUpdatedFields.ActualPODVesselArrival = ComputeActualPODVesselArrival();
            containerUpdatedFields.EstimatedPODDischarge = ComputeEstimatedPODDischarge();
            containerUpdatedFields.ActualPODDischarge = ComputeActualPODDischarge();
            containerUpdatedFields.EstimatedPODDeparture = ComputeEstimatedPODDeparture();
            containerUpdatedFields.ActualPODDeparture = ComputeActualPODDeparture();
            containerUpdatedFields.EstimatedDelivery = ComputeEstimatedDelivery();
            containerUpdatedFields.ActualDelivery = ComputeActualDelivery();
            containerUpdatedFields.EstimatedLIFArrival = ComputeEstimatedLIFArrival();
            containerUpdatedFields.ActualLIFArrival = ComputeActualLIFArrival();
            containerUpdatedFields.EstimatedOnCarriageDeparture = ComputeEstimatedOnCarriageDeparture();
            containerUpdatedFields.ActualOnCarriageDeparture = this.ComputeActualOnCarriageDeparture();
            containerUpdatedFields.EmptyReturnLocation = this.GetTranslatedPortCode(empty_return_loc_locode);
            containerUpdatedFields.EstimatedEmptyReturn = this.ComputeEstimatedEmptyReturn();
            containerUpdatedFields.ActualEmptyReturn = this.ComputeActualEmptyReturn();
            containerUpdatedFields.CustomsReleaseDate = this.ComputeCustomsReleaseDate();
            containerUpdatedFields.CarrierReleaseDate = this.ComputeCarrierReleaseDate();
            containerUpdatedFields.AvailablityDate = this.ComputeAvailablityDate();
            containerUpdatedFields.ShipmentContext = shipmentContext;
            containerUpdatedFields.ContainerRepository = containerRepository;
            containerUpdatedFields.ShipmentPM = shipmentPM;
            containerUpdatedFields.ContainerPM = container;
            containerUpdatedFields.ContainersExternal = containersExternal;
            containerUpdatedFields.ContainerStatus = container_status;
            containerUpdatedFields.ShipmentPackageId = shipmentPackagesId;
            containerUpdatedFields.EventDate = this.GetEventDate();
        }

        private DateTime? ComputeMainCarriageETD()
        {
            if (!string.IsNullOrEmpty(ETD_last))
            {
                return AnalyzeXMLDateValue(ETD_last, pol_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return AnalyzeXMLDateValue(ETD_initial, pol_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageETA()
        {
            if (!string.IsNullOrEmpty(ETA_last))
            {
                return ConvertStringToDateTime_old(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return ConvertStringToDateTime_old(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return ConvertStringToDateTime_old(ETA_predection);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATD()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return ConvertStringToDateTime_old(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return ConvertStringToDateTime_old(ATD_actual);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATA()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return ConvertStringToDateTime_old(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return ConvertStringToDateTime_old(ATA_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_last))
            {
                return AnalyzeXMLDateValue(emptyPickup_last, emptyPickupTimeZone);
            }

            else if (!string.IsNullOrEmpty(emptyPickup_initial))
            {
                return AnalyzeXMLDateValue(emptyPickup_initial, emptyPickupTimeZone);
            }

            return null;
        }
        private DateTime? ComputeActualEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_actual))
            {
                return AnalyzeXMLDateValue(emptyPickup_actual, emptyPickupTimeZone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_last))
            {
                return AnalyzeXMLDateValue(gateInDate_last, pol_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(gateInDate_initial))
            {
                return AnalyzeXMLDateValue(gateInDate_initial, pol_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeActualGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_actual))
            {
                return AnalyzeXMLDateValue(gateInDate_actual, pol_loc_timezone);
            }

            return null;
        }
        private string GetContainerStatusName()
        {
            return containerStatusRepository.GetSingleContainerStatus(this.container_status)?.Name;
        }
        private string ComputeCurrentStatusLocation()
        {
            if (!string.IsNullOrEmpty(this.destinationLocation))
            {
                return this.destinationLocation;
            }

            else if (!string.IsNullOrEmpty(this.departureLocation))
            {
                return this.departureLocation;
            }

            else
            {
                return this.emptyPickupLocation;
            }
        }
        private DateTime? ComputeEstimatedOriginPickup()
        {
            if (!string.IsNullOrEmpty(origin_pickup_planned_last))
            {
                return AnalyzeXMLDateValue(origin_pickup_planned_last, origin_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(origin_pickup_planned_initial))
            {
                return AnalyzeXMLDateValue(origin_pickup_planned_initial, origin_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPOLLoaded()
        {
            if (!string.IsNullOrEmpty(pol_loaded_planned_last))
            {
                return AnalyzeXMLDateValue(pol_loaded_planned_last, pol_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(pol_loaded_planned_initial))
            {
                return AnalyzeXMLDateValue(pol_loaded_planned_initial, pol_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeActualOriginPickup()
        {
            if (!string.IsNullOrEmpty(origin_pickup_actual))
            {
                return AnalyzeXMLDateValue(origin_pickup_actual, origin_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualPOLLoaded()
        {
            if (!string.IsNullOrEmpty(pol_loaded_actual))
            {
                return AnalyzeXMLDateValue(pol_loaded_actual, pol_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ETD_last))
            {
                return AnalyzeXMLDateValue(ETD_last, pol_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return AnalyzeXMLDateValue(ETD_initial, pol_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeActualPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ATD_actual))
            {
                return AnalyzeXMLDateValue(ATD_actual, pol_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(ATD_detected))
            //{
            //    return AnalyzeXMLDateValue(ATD_detected, pol_loc_timezone);

            //}
            return null;
        }
        private DateTime? ComputeEstimatedTrans1VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp1_vslarrival_planned_last))
            {
                return AnalyzeXMLDateValue(tsp1_vslarrival_planned_last, tsp1_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(tsp1_vslarrival_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp1_vslarrival_planned_initial, tsp1_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp1_vslarrival_actual))
            {
                return AnalyzeXMLDateValue(tsp1_vslarrival_actual, tsp1_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp1_vslarrival_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp1_vslarrival_detected, tsp1_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(tsp1_discharge_planned_last))
            {
                return AnalyzeXMLDateValue(tsp1_discharge_planned_last, tsp1_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp1_discharge_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp1_discharge_planned_initial, tsp1_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(tsp1_discharge_actual))
            {
                return AnalyzeXMLDateValue(tsp1_discharge_actual, tsp1_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(tsp1_loaded_planned_last))
            {
                return AnalyzeXMLDateValue(tsp1_loaded_planned_last, tsp1_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp1_loaded_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp1_loaded_planned_initial, tsp1_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(tsp1_loaded_actual))
            {
                return AnalyzeXMLDateValue(tsp1_loaded_actual, tsp1_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp1_vsldeparture_planned_last))
            {
                return AnalyzeXMLDateValue(tsp1_vsldeparture_planned_last, tsp1_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp1_vsldeparture_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp1_vsldeparture_planned_initial, tsp1_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp1_vsldeparture_actual))
            {
                return AnalyzeXMLDateValue(tsp1_vsldeparture_actual, tsp1_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp1_vsldeparture_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp1_vsldeparture_detected, tsp1_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTrans2VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp2_vslarrival_planned_last))
            {
                return AnalyzeXMLDateValue(tsp2_vslarrival_planned_last, tsp2_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(tsp2_vslarrival_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp2_vslarrival_planned_initial, tsp2_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp2_vslarrival_actual))
            {
                return AnalyzeXMLDateValue(tsp2_vslarrival_actual, tsp2_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp2_vslarrival_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp2_vslarrival_detected, tsp2_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(tsp2_discharge_planned_last))
            {
                return AnalyzeXMLDateValue(tsp2_discharge_planned_last, tsp2_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp2_discharge_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp2_discharge_planned_initial, tsp2_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(tsp2_discharge_actual))
            {
                return AnalyzeXMLDateValue(tsp2_discharge_actual, tsp2_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(tsp2_loaded_planned_last))
            {
                return AnalyzeXMLDateValue(tsp2_loaded_planned_last, tsp2_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp2_loaded_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp2_loaded_planned_initial, tsp2_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(tsp2_loaded_actual))
            {
                return AnalyzeXMLDateValue(tsp2_loaded_actual, tsp2_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp2_vsldeparture_planned_last))
            {
                return AnalyzeXMLDateValue(tsp2_vsldeparture_planned_last, tsp2_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp2_vsldeparture_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp2_vsldeparture_planned_initial, tsp2_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp2_vsldeparture_actual))
            {
                return AnalyzeXMLDateValue(tsp2_vsldeparture_actual, tsp2_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp2_vsldeparture_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp2_vsldeparture_detected, tsp2_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTrans3VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp3_vslarrival_planned_last))
            {
                return AnalyzeXMLDateValue(tsp3_vslarrival_planned_last, tsp3_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(tsp3_vslarrival_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp3_vslarrival_planned_initial, tsp3_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp3_vslarrival_actual))
            {
                return AnalyzeXMLDateValue(tsp3_vslarrival_actual, tsp3_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp3_vslarrival_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp3_vslarrival_detected, tsp3_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(tsp3_discharge_planned_last))
            {
                return AnalyzeXMLDateValue(tsp3_discharge_planned_last, tsp3_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp3_discharge_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp3_discharge_planned_initial, tsp3_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(tsp3_discharge_actual))
            {
                return AnalyzeXMLDateValue(tsp3_discharge_actual, tsp3_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(tsp3_loaded_planned_last))
            {
                return AnalyzeXMLDateValue(tsp3_loaded_planned_last, tsp3_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp3_loaded_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp3_loaded_planned_initial, tsp3_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(tsp3_loaded_actual))
            {
                return AnalyzeXMLDateValue(tsp3_loaded_actual, tsp3_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp3_vsldeparture_planned_last))
            {
                return AnalyzeXMLDateValue(tsp3_vsldeparture_planned_last, tsp3_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp3_vsldeparture_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp3_vsldeparture_planned_initial, tsp3_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp3_vsldeparture_actual))
            {
                return AnalyzeXMLDateValue(tsp3_vsldeparture_actual, tsp3_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp3_vsldeparture_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp3_vsldeparture_detected, tsp3_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTrans4VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp4_vslarrival_planned_last))
            {
                return AnalyzeXMLDateValue(tsp4_vslarrival_planned_last, tsp4_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(tsp4_vslarrival_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp4_vslarrival_planned_initial, tsp4_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp4_vslarrival_actual))
            {
                return AnalyzeXMLDateValue(tsp4_vslarrival_actual, tsp4_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp4_vslarrival_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp4_vslarrival_detected, tsp4_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(tsp4_discharge_planned_last))
            {
                return AnalyzeXMLDateValue(tsp4_discharge_planned_last, tsp4_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp4_discharge_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp4_discharge_planned_initial, tsp4_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(tsp4_discharge_actual))
            {
                return AnalyzeXMLDateValue(tsp4_discharge_actual, tsp4_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(tsp4_loaded_planned_last))
            {
                return AnalyzeXMLDateValue(tsp4_loaded_planned_last, tsp4_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp4_loaded_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp4_loaded_planned_initial, tsp4_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(tsp4_loaded_actual))
            {
                return AnalyzeXMLDateValue(tsp4_loaded_actual, tsp4_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp4_vsldeparture_planned_last))
            {
                return AnalyzeXMLDateValue(tsp4_vsldeparture_planned_last, tsp4_loc_timezone);
            }
            else if (!string.IsNullOrEmpty(tsp4_vsldeparture_planned_initial))
            {
                return AnalyzeXMLDateValue(tsp4_vsldeparture_planned_initial, tsp4_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp4_vsldeparture_actual))
            {
                return AnalyzeXMLDateValue(tsp4_vsldeparture_actual, tsp4_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(tsp4_vsldeparture_detected))
            //{
            //    return AnalyzeXMLDateValue(tsp4_vsldeparture_detected, tsp4_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ETA_last))
            {
                return AnalyzeXMLDateValue(ETA_last, pod_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return AnalyzeXMLDateValue(ETA_initial, pod_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeActualPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ATA_actual))
            {
                return AnalyzeXMLDateValue(ATA_actual, pod_loc_timezone);
            }
            //else if (!string.IsNullOrEmpty(ATA_detected))
            //{
            //    return AnalyzeXMLDateValue(ATA_detected, pod_loc_timezone);
            //}

            return null;
        }
        private DateTime? ComputeEstimatedPODDischarge()
        {
            if (!string.IsNullOrEmpty(pod_discharge_planned_last))
            {
                return AnalyzeXMLDateValue(pod_discharge_planned_last, pod_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(pod_discharge_planned_initial))
            {
                return AnalyzeXMLDateValue(pod_discharge_planned_initial, pod_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualPODDischarge()
        {
            if (!string.IsNullOrEmpty(pod_discharge_actual))
            {
                return AnalyzeXMLDateValue(pod_discharge_actual, pod_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedPODDeparture()
        {
            if (!string.IsNullOrEmpty(pod_departure_planned_last))
            {
                return AnalyzeXMLDateValue(pod_departure_planned_last, pod_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(pod_departure_planned_initial))
            {
                return AnalyzeXMLDateValue(pod_departure_planned_initial, pod_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualPODDeparture()
        {
            if (!string.IsNullOrEmpty(pod_departure_actual))
            {
                return AnalyzeXMLDateValue(pod_departure_actual, pod_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedDelivery()
        {
            if (!string.IsNullOrEmpty(dlv_delivery_planned_last))
            {
                return AnalyzeXMLDateValue(dlv_delivery_planned_last, dlv_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(dlv_delivery_planned_initial))
            {
                return AnalyzeXMLDateValue(dlv_delivery_planned_initial, dlv_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualDelivery()
        {
            if (!string.IsNullOrEmpty(dlv_delivery_actual))
            {
                return AnalyzeXMLDateValue(dlv_delivery_actual, dlv_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedLIFArrival()
        {
            if (!string.IsNullOrEmpty(lif_arrival_planned_last))
            {
                return AnalyzeXMLDateValue(lif_arrival_planned_last, lif_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(lif_arrival_planned_initial))
            {
                return AnalyzeXMLDateValue(lif_arrival_planned_initial, lif_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualLIFArrival()
        {
            if (!string.IsNullOrEmpty(lif_arrival_actual))
            {
                return AnalyzeXMLDateValue(lif_arrival_actual, lif_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedOnCarriageDeparture()
        {
            if (!string.IsNullOrEmpty(lif_departure_planned_last))
            {
                return AnalyzeXMLDateValue(lif_departure_planned_last, lif_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(lif_departure_planned_initial))
            {
                return AnalyzeXMLDateValue(lif_departure_planned_initial, lif_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualOnCarriageDeparture()
        {
            if (!string.IsNullOrEmpty(lif_departure_actual))
            {
                return AnalyzeXMLDateValue(lif_departure_actual, lif_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeEstimatedEmptyReturn()
        {
            if (!string.IsNullOrEmpty(empty_return_planned_last))
            {
                return AnalyzeXMLDateValue(empty_return_planned_last, empty_return_loc_timezone);
            }

            else if (!string.IsNullOrEmpty(empty_return_planned_initial))
            {
                return AnalyzeXMLDateValue(empty_return_planned_initial, empty_return_loc_timezone);
            }
            return null;
        }
        private DateTime? ComputeActualEmptyReturn()
        {
            if (!string.IsNullOrEmpty(empty_return_actual))
            {
                return AnalyzeXMLDateValue(empty_return_actual, empty_return_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputeCustomsReleaseDate()
        {
            if (!string.IsNullOrEmpty(customs_release_date))
            {
                return AnalyzeXMLDateValue(customs_release_date, availability_timezone);
            }

            return null;
        }
        private DateTime? ComputeCarrierReleaseDate()
        {
            if (!string.IsNullOrEmpty(carrier_release_date))
            {
                return AnalyzeXMLDateValue(carrier_release_date, availability_timezone);
            }

            return null;
        }
        private DateTime? ComputeAvailablityDate()
        {
            if (!string.IsNullOrEmpty(availability_date))
            {
                return AnalyzeXMLDateValue(availability_date, availability_timezone);
            }

            return null;
        }
        private DateTime? ComputePOLGateIn()
        {
            if (!string.IsNullOrEmpty(gateInDate_actual))
            {
                return AnalyzeXMLDateValue(gateInDate_actual, pol_loc_timezone);
            }

            return null;
        }
        private DateTime? ComputPODGateOut()
        {
            if (!string.IsNullOrEmpty(pod_departure_actual))
            {
                return AnalyzeXMLDateValue(pod_departure_actual, pod_loc_timezone);
            }

            return null;
        }

        private void HandleVesselLegs()
        {
            var vesselLeg1 = GetVessel(leg1_vessel_name);
            var vesselLeg2 = GetVessel(leg2_vessel_name);
            var vesselLeg3 = GetVessel(leg3_vessel_name);
            var vesselLeg4 = GetVessel(leg4_vessel_name);
            var vesselLeg5 = GetVessel(leg5_vessel_name);
            containerUpdatedFields.Leg1Vessel = vesselLeg1 == null ? leg1_vessel_name : vesselLeg1.EnglishName;
            containerUpdatedFields.Leg1VesselId = vesselLeg1?.Id;
            containerUpdatedFields.Leg2Vessel = vesselLeg2 == null ? leg2_vessel_name : vesselLeg2.EnglishName;
            containerUpdatedFields.Leg2VesselId = vesselLeg2?.Id;
            containerUpdatedFields.Leg3Vessel = vesselLeg3 == null ? leg3_vessel_name : vesselLeg3.EnglishName;
            containerUpdatedFields.Leg3VesselId = vesselLeg3?.Id;
            containerUpdatedFields.Leg4Vessel = vesselLeg4 == null ? leg4_vessel_name : vesselLeg4.EnglishName;
            containerUpdatedFields.Leg4VesselId = vesselLeg4?.Id;
            containerUpdatedFields.Leg5Vessel = vesselLeg5 == null ? leg5_vessel_name : vesselLeg5.EnglishName;
            containerUpdatedFields.Leg5VesselId = vesselLeg5?.Id;
        }
        private Vessel GetVessel(string vesselName)
        {
            Vessel vessel = vesselRepository.GetSingleVesselByName(vesselName, this.logitudeTenant.Value);
            return vessel;
        }


        private DateTime? AnalyzeXMLDateValue(string XMLValue, string timeZone)
        {
            XMLDateParts xMLDateParts = this.GetXMLDateParts(XMLValue);
            XMLDateParts portTimeZoneDateParts = this.GetPortTimeZoneDateParts(timeZone);
            return ComputeDateTimeRqgardingTimeZone(xMLDateParts, portTimeZoneDateParts);
        }
        private XMLDateParts GetPortTimeZoneDateParts(string timeZone)
        {
            XMLDateParts xMLDateParts = new XMLDateParts();
            PortTimeZone portTimeZone = portTimeZoneRepository.GetSinglePortTimeZone(timeZone);

            if (portTimeZone != null && !string.IsNullOrEmpty(portTimeZone.UTCOffset))
            {
                xMLDateParts.TimeZoneSign = portTimeZone.UTCOffset.Substring(0, 1);
                xMLDateParts.TimeZoneHoursValue = ConvertStringToInteger(portTimeZone.UTCOffset.Substring(1, 2));
                xMLDateParts.TimeZoneMinutsValue = ConvertStringToInteger(portTimeZone.UTCOffset.Substring(4, 2));
                xMLDateParts.TimeZone = new TimeSpan(xMLDateParts.TimeZoneHoursValue, xMLDateParts.TimeZoneMinutsValue, 0);
            }

            return xMLDateParts;
        }
    }

    public class XMLDateParts
    {        
        public XMLDateParts()
        {
            DateTimeValue = null;
            TimeZoneHoursValue = 0;
            TimeZoneMinutsValue = 0;
            TimeZoneSign = "";
            TimeZone = new TimeSpan(0, 0, 0);
        }
        public DateTime? DateTimeValue { get; set; }
        public int TimeZoneHoursValue { get; set; }
        public int TimeZoneMinutsValue { get; set; }
        public string TimeZoneSign { get; set; }
        public TimeSpan TimeZone { get; set; }
    }
}