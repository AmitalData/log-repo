using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Logitude.Server.Tools;
using System.Collections.Generic;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Text;
using Simplog.Data.ShipmentsModel;
using Logitude.Server.Tools.Counters;
using System.Security.Cryptography;
using System.Reflection;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.Helpers.Analyzers
{
    public class ContainerStatusesConnecterAnalyzer
    {
        private AnalyzeQueue analyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private CommunicationLogRepository communicationLogRepository;
        private int tenant;
        private ArrayOfQueueTask externalTasksQueues;
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private int? logitudeTenant = null;
        private ICommonDataContext commonContext;
        private List<LogitudeOceanInsightsRequest> oceanInsights;
        private ContainerPM container;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string objectTableId;
        private string loggedContactId;
        private string containerId;
        private string oceanInsightsEnvelopeParameters;
        private IShipmentsContext shipmentContext;
        private ShipmentContainerStatusRepository shipmentContainerStatusRepository;
        private ContainerRepository containerRepository;
        private ContainerQuery containerQuery;
        private ContainerStatusRepository containerStatusRepository;
        private ShipmentRepository shipmentRepository;
        private ShipmentQuery shipmentQuery;
        private string objectTableName = "Container";
        private ShipmentPM shipmentPM;
        private PortRepository portRepository;
        private ComputingPartnerTranslationHelper computingPartnerTranslator;
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
        private string gateInDate_last;
        private string gateInDate_initial;
        private string gateInDate_actual;
        private string departureLocation;
        private string destinationLocation;
        private string origin_loc_locode;
        string origin_pickup_planned_initial = null;
        string origin_pickup_planned_last = null;
        string origin_pickup_actual = null;
        string pol_loc_locode = null;
        string pol_loaded_planned_initial = null;
        string pol_loaded_planned_last = null;
        string pol_loaded_actual = null;
        string ts_count = null;
        string tsp1_loc_locode = null;
        string tsp1_vslarrival_planned_initial = null;
        string tsp1_vslarrival_planned_last = null;
        string tsp1_vslarrival_actual = null;
        string tsp1_vslarrival_detected = null;
        string tsp1_discharge_planned_last = null;
        string tsp1_discharge_actual = null;
        string tsp1_loaded_planned_initial = null;
        string tsp1_loaded_planned_last = null;
        string tsp1_loaded_actual = null;
        string tsp1_vsldeparture_planned_initial = null;
        string tsp1_vsldeparture_planned_last = null;
        string tsp1_vsldeparture_actual = null;
        string tsp1_vsldeparture_detected = null ; 
        string tsp1_discharge_planned_initial = null;
        string tsp2_loc_locode = null;
        string tsp2_vslarrival_planned_initial = null;
        string tsp2_vslarrival_planned_last = null;
        string tsp2_vslarrival_actual = null;
        string tsp2_vslarrival_detected = null;
        string tsp2_discharge_planned_initial = null;
        string tsp2_discharge_planned_last = null;
        string tsp2_discharge_actual = null;
        string tsp2_loaded_planned_initial = null;
        string tsp2_loaded_planned_last = null;
        string tsp2_loaded_actual = null;
        string tsp2_vsldeparture_planned_initial = null;
        string tsp2_vsldeparture_planned_last = null;
        string tsp2_vsldeparture_actual = null;
        string tsp2_vsldeparture_detected = null;
        string tsp3_loc_locode = null;
        string tsp3_vslarrival_planned_initial = null;
        string tsp3_vslarrival_planned_last = null;
        string tsp3_vslarrival_actual = null;
        string tsp3_vslarrival_detected = null;
        string tsp3_discharge_planned_initial = null;
        string tsp3_discharge_planned_last = null;
        string tsp3_discharge_actual = null;
        string tsp3_loaded_planned_initial = null;
        string tsp3_loaded_planned_last = null;
        string tsp3_loaded_actual = null;
        string tsp3_vsldeparture_planned_initial = null;
        string tsp3_vsldeparture_planned_last = null;
        string tsp3_vsldeparture_actual = null;
        string tsp3_vsldeparture_detected = null;
        string tsp4_loc_locode = null;
        string tsp4_vslarrival_planned_initial = null;
        string tsp4_vslarrival_planned_last = null;
        string tsp4_vslarrival_actual = null;
        string tsp4_vslarrival_detected = null;
        string tsp4_discharge_planned_initial = null;
        string tsp4_discharge_planned_last = null;
        string tsp4_discharge_actual = null;
        string tsp4_loaded_planned_initial = null;
        string tsp4_loaded_planned_last = null;
        string tsp4_loaded_actual = null;
        string tsp4_vsldeparture_planned_initial = null;
        string tsp4_vsldeparture_planned_last = null;
        string tsp4_vsldeparture_actual = null;
        string tsp4_vsldeparture_detected = null;
        string leg1_vessel_name = null;
        string leg1_voyage = null;
        string leg2_vessel_name = null;
        string leg2_voyage = null;
        string leg3_vessel_name = null;
        string leg3_voyage = null;
        string leg4_vessel_name = null;
        string leg4_voyage = null;
        string leg5_vessel_name = null;
        string leg5_voyage = null;
        string pod_loc_locode = null;
        string pod_discharge_planned_initial = null;
        string pod_discharge_planned_last = null;
        string pod_discharge_actual = null;
        string pod_departure_planned_initial = null;
        string pod_departure_planned_last = null;
        string pod_departure_actual = null;
        string dlv_loc_locode = null;
        string dlv_delivery_planned_initial = null;
        string dlv_delivery_planned_last = null;
        string dlv_delivery_actual = null;
        string lif_loc_locode = null;
        string lif_arrival_planned_initial = null;
        string lif_arrival_planned_last = null;
        string lif_arrival_actual = null;
        string lif_departure_planned_initial = null;
        string lif_departure_planned_last = null;
        string lif_departure_actual = null;
        string empty_return_loc_locode = null;
        string empty_return_planned_initial = null;
        string empty_return_planned_last = null;
        string empty_return_actual = null;
        string customs_release_date = null;
        string carrier_release_date = null;
        string customs_release_state = null;
        string carrier_release_state = null;
        string availability_date = null;
        string availability_loc =  null;
        string POLShipmentUpdateIndicator = null;
        string PODShipmentUpdateIndicator = null;
        string computingPartnerCode;
        private bool IsUpdatingPackages = false;

        public ContainerStatusesConnecterAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.tenant = analyzeQueue.Tenant;
                this.analyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(this.tenant);                
                this.computingPartnerCode = "G-OCI";
            }
        }

        public void Run()
        {
            if (analyzeQueue != null)
            {
                this.Deserialize();
            }
        }
        private void Deserialize()
        {
            try
            {
                MemoryStream memorystream = new MemoryStream(analyzeQueue.MessageBody);
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfQueueTask));
                externalTasksQueues = (ArrayOfQueueTask)serializer.Deserialize(memorystream);
            }

            catch (Exception ex)
            {
                analyzeQueue.Status = "F";
                analyzeQueue.ErrorMessage = "ContainerStatusesConnecterAnalyzer failed: " + ex.Message;
                analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                throw ex;
            }

            if (externalTasksQueues != null)
            {
                this.AnalyzeData(analyzeQueue.From);
            }
        }
        private void AnalyzeData(string from)
        {
            try
            {
                this.ConnectAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
                throw ex;
            }
        }
        private void ConnectAnalyzeQueue()
        {
            try
            {
                this.ConnectAnalyzeQueueToTenantAndEntity();
                this.AnalyzeOceanInsightsParametersXML();
                this.GetLogitudeOceanInsights();
                this.ProcessLogitudeTenant();
                this.DoneAnalyzeQueue();
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }
        private void ConnectAnalyzeQueueToTenantAndEntity()
        {
            if (!analyzeQueue.ConnectedToTenant)
            {
                analyzeQueue.ConnectedToTenant = true;
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }
            if (!analyzeQueue.ConnectedToEntity)
            {
                analyzeQueue.ConnectedToEntity = true;
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }
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
            }
        }
        private void GetDepartureLocationElement(XmlNode node)
        {
            XmlElement departureLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loc").FirstOrDefault();
            if (departureLocationElement != null)
            {
                departureLocation = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                pol_loc_locode = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
        }
        private void GetDestinationLocationElement(XmlNode node)
        {
            XmlElement destinationLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_loc").FirstOrDefault();
            if (destinationLocationElement != null)
            {
                destinationLocation = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                pod_loc_locode = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;

            }
        }
        private void GetOriginLocationElement(XmlNode node)
        {
            XmlElement origin_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "origin_loc").FirstOrDefault();
            if (origin_locElement != null)
            {
                origin_loc_locode = origin_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
            }
        }
        private void GetTransshipment1Leg(XmlNode node)
        {
            XmlElement tsp1_locElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "tsp1_loc").FirstOrDefault();
            if (tsp1_locElement != null)
            {
                tsp1_loc_locode = tsp1_locElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
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
                availability_loc = availabilityemptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "locode").FirstOrDefault()?.InnerText;
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
                            this.AddContainerStatusCommunicationLog(item);
                            this.CreateLogitudeOceanInsightsResponse();
                            if (IsUpdatingShipmentAndContainer())
                            {
                                this.CreateShipmentContainerStatus(item);
                                this.UpdateContainer();
                                this.UpdatePackage();
                                this.UpdateShipment();
                                this.SaveShipment(shipmentPM);
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
            this.computingPartnerTranslator = new ComputingPartnerTranslationHelper(logitudeTenant.Value);
        }
        private bool IsUpdatingShipmentAndContainer()
        {
            if (this.eventCode != null && this.eventCode != "20" &&
                                (Int32.Parse(this.eventCode) >= 0 && Int32.Parse(this.eventCode) <= 31)
                                && !string.IsNullOrEmpty(this.container_number))
            {
                return true;
            }

            return false;
        }

        private void GetShipmentById(LogitudeOceanInsightsRequest oceanInsight)
        {
            shipmentPM = shipmentQuery.GetSinglePM(oceanInsight?.ShipmentId, logitudeTenant.Value);
        }
        private void GetContainerDataByContainerNumber(LogitudeOceanInsightsRequest oceanInsight)
        {
            container_number = this.GetContainerNumber(oceanInsight);
            container = containerQuery.GetContainerByNumberAndShipmentIdAndTenant(container_number, oceanInsight.ShipmentId, logitudeTenant.Value);
            containerId = container?.Id;
            shipmentPackagesId = this.GetShipmentPackagesId(container, oceanInsight);
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
                containerNumber = this.GetContainerNumberFromShipmentContainers(oceanInsight);
            }

            return containerNumber;
        }
        private string GetShipmentPackagesId(ContainerPM container, LogitudeOceanInsightsRequest oceanInsight)
        {
            string shipmentPackagesId = "";
            if (container != null)
            {
                shipmentPackagesId = container.ShipmentPackagesId;
            }
            else
            {
                shipmentPackagesId = this.GetShipmentPackagesIdFromShipmentContainers(oceanInsight);
            }

            return shipmentPackagesId;
        }
        private string GetContainerNumberFromShipmentContainers(LogitudeOceanInsightsRequest oceanInsight)
        {
            string containerNumber = "";
            var package = shipmentPM?.ShipmentPackages?.Where(a => a.ContainerNumber == container_number_FromXML).FirstOrDefault();
            if (package != null)
            {
                containerNumber = package.ContainerNumber;
            }
            else
            {
                this.objectTableName = "Shipment"; // Container number does not found; open the log under shipment 
            }
            return containerNumber;
        }
        private string GetShipmentPackagesIdFromShipmentContainers(LogitudeOceanInsightsRequest oceanInsight)
        {
            string shipmentPackagesId = "";
            var package = shipmentPM?.ShipmentPackages?.Where(a => a.ContainerNumber == container_number_FromXML).FirstOrDefault();
            if (package != null)
            {
                shipmentPackagesId = package.Id;
                containerId = package.ContainerEntityId;
            }

            return shipmentPackagesId;
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
            };

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

        private void CreateLogitudeOceanInsightsResponse()
        {
            if (container != null)
            {
                LogitudeOceanInsightsResponseRepository logitudeOceanInsightsResponseRepository = new LogitudeOceanInsightsResponseRepository(tenant);
                LogitudeOceanInsightsResponse logitudeOceanInsightsResponse = logitudeOceanInsightsResponseRepository.GetLogitudeOceanInsightsResponseByContainerNumberAndScac(container_number, carrier_scac, tenant);
                if (logitudeOceanInsightsResponse == null)
                {
                    logitudeOceanInsightsResponse = new LogitudeOceanInsightsResponse()
                    {
                        Id = IdCounter.GetNumber("LogitudeOceanInsightsResponse", tenant),
                        FirstResponseDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        LastResponseDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        ContainerNumber = container_number,
                        SCACCode = carrier_scac,
                        Tenant = logitudeTenant != null ? logitudeTenant.Value: tenant,
                        CarrierName = GetCarrierName()
                    };
                    logitudeOceanInsightsResponseRepository.Add(logitudeOceanInsightsResponse);
                }
                else
                {
                    logitudeOceanInsightsResponse.LastResponseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    logitudeOceanInsightsResponseRepository.Update(logitudeOceanInsightsResponse);
                }

                logitudeOceanInsightsResponseRepository.SubmitChanges();
            }
        }

        private string GetCarrierName()
        {
            string carrierName = "";
            CardRepository cardRepository = new CardRepository(tenant);
            Card shippingLine = cardRepository.GetSingleCard(container.MainCarriageCarrierId, tenant);
            carrierName = shippingLine?.EnglishName;
            return carrierName;
        }

        private void CreateShipmentContainerStatus(LogitudeOceanInsightsRequest oceanInsight)
        {
            string iHash = this.GetHashedData(oceanInsight.ShipmentId);
            DateTime logDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? eventDate = this.GetEventDate();
            double containerWeight = this.GetContainerWeight();
            DateTime? departureDate = this.ComputeDepartureDate();
            DateTime? arrivalDate = this.ComputeArrivalDate();
            string departureDateInfo = this.ComputeDepartureDateInfo();
            string arrivalDateInfo = this.ComputeArrivalDateInfo();
            string statusDetails = this.GetStatusDetails();

            ShipmentContainerStatus containerStatus = new ShipmentContainerStatus()
            {
                Id = IdCounter.GetNumber("ShipmentContainerStatus", this.tenant),
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
        private double GetContainerWeight()
        {
            double containerWeight = 0;
            if (!string.IsNullOrEmpty(weight))
            {
                Double.TryParse(weight, out containerWeight);
            }

            return containerWeight;
        }
        private DateTime? GetEventDate()
        {
            if (!string.IsNullOrEmpty(createdDate))
            {
                return ConvertStringToDateTime(createdDate);
            }

            return null;
        }
        private DateTime? ComputeDepartureDate()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return ConvertStringToDateTime(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return ConvertStringToDateTime(ATD_actual);
            }

            else if (!string.IsNullOrEmpty(ETD_last))
            {
                return ConvertStringToDateTime(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return ConvertStringToDateTime(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeArrivalDate()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return ConvertStringToDateTime(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return ConvertStringToDateTime(ATA_actual);
            }

            else if (!string.IsNullOrEmpty(ETA_last))
            {
                return ConvertStringToDateTime(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return ConvertStringToDateTime(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return ConvertStringToDateTime(ETA_predection);
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
        private string GetHashedData(string shipmentId)
        {
            string information = shipmentId + tenant.ToString() + this.container_number;
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;
        }
        private void UpdateContainer()
        {
            if (container != null)
            {
                ContainerUpdatedFields containerUpdatedFields = this.BuildContainerUpdatedFields();
                this.FillFieldsNewValues("MainCarriageETD", containerUpdatedFields.MainCarriageETD, container);
                this.FillFieldsNewValues("MainCarriageETA", containerUpdatedFields.MainCarriageETA, container);
                this.FillFieldsNewValues("MainCarriageATD", containerUpdatedFields.MainCarriageATD, container);
                this.FillFieldsNewValues("MainCarriageATA", containerUpdatedFields.MainCarriageATA, container);
                this.FillFieldsNewValues("EmptyPickupLocation", containerUpdatedFields.EmptyPickupLocation, container);
                this.FillFieldsNewValues("EstimatedEmptyPickupDate", containerUpdatedFields.EstimatedEmptyPickupDate, container);
                this.FillFieldsNewValues("ActualEmptyPickupDate", containerUpdatedFields.ActualEmptyPickupDate, container);
                this.FillFieldsNewValues("EstimatedPOLArrival", containerUpdatedFields.EstimatedPOLArrival, container);
                this.FillFieldsNewValues("ActualPOLArrival", containerUpdatedFields.ActualPOLArrival, container);
                this.FillFieldsNewValues("DepartureLocation", containerUpdatedFields.DepartureLocation, container);
                this.FillFieldsNewValues("DestinationLocation", containerUpdatedFields.DestinationLocation, container);

                container.CurrentStatus = containerUpdatedFields.CurrentStatus;
                container.CurrentLocation = containerUpdatedFields.CurrentLocation;
                container.CurrentStatusDate = containerUpdatedFields.CurrentStatusDate;
                container.HasContainerException = containerUpdatedFields.HasContainerException;
                container.UpdateDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
                container.OriginLocation = containerUpdatedFields.OriginLocation;
                container.EstimatedOriginPickup = containerUpdatedFields.EstimatedOriginPickup;
                container.ActualOriginPickup = containerUpdatedFields.ActualOriginPickup;
                container.POLLocation = containerUpdatedFields.POLLocation;
                container.EstimatedPOLLoaded = containerUpdatedFields.EstimatedPOLLoaded;
                container.ActualPOLLoaded = containerUpdatedFields.ActualPOLLoaded;
                container.EstimatedPOLVesselDeparture = containerUpdatedFields.EstimatedPOLVesselDeparture;
                container.ActualPOLVesselDeparture = containerUpdatedFields.ActualPOLVesselDeparture;
                container.TransshipmentCount = containerUpdatedFields.TransshipmentCount;
                container.Transshipment1Location = containerUpdatedFields.Transshipment1Location;
                container.EstimatedTrans1VesselArrival = containerUpdatedFields.EstimatedTrans1VesselArrival;
                container.ActualTransshipment1VesselArrival = containerUpdatedFields.ActualTransshipment1VesselArrival;
                container.EstimatedTransshipment1Discharge = containerUpdatedFields.EstimatedTransshipment1Discharge;
                container.ActualTransshipment1Discharge = containerUpdatedFields.ActualTransshipment1Discharge;
                container.EstimatedTransshipment1Loaded = containerUpdatedFields.EstimatedTransshipment1Loaded;
                container.ActualTransshipment1Loaded = containerUpdatedFields.ActualTransshipment1Loaded;
                container.EstimatedTrans1VesselDeparture = containerUpdatedFields.EstimatedTrans1VesselDeparture;
                container.ActualTrans1VesselDeparture = containerUpdatedFields.ActualTrans1VesselDeparture;
                container.Transshipment2Location = containerUpdatedFields.Transshipment2Location;
                container.EstimatedTrans2VesselArrival = containerUpdatedFields.EstimatedTrans2VesselArrival;
                container.ActualTransshipment2VesselArrival = containerUpdatedFields.ActualTransshipment2VesselArrival;
                container.EstimatedTransshipment2Discharge = containerUpdatedFields.EstimatedTransshipment2Discharge;
                container.ActualTransshipment2Discharge = containerUpdatedFields.ActualTransshipment2Discharge;
                container.EstimatedTransshipment2Loaded = containerUpdatedFields.EstimatedTransshipment2Loaded;
                container.ActualTransshipment2Loaded = containerUpdatedFields.ActualTransshipment2Loaded;
                container.EstimatedTrans2VesselDeparture = containerUpdatedFields.EstimatedTrans2VesselDeparture;
                container.ActualTrans2VesselDeparture = containerUpdatedFields.ActualTrans2VesselDeparture;
                container.Transshipment3Location = containerUpdatedFields.Transshipment3Location;
                container.EstimatedTrans3VesselArrival = containerUpdatedFields.EstimatedTrans3VesselArrival;
                container.ActualTransshipment3VesselArrival = containerUpdatedFields.ActualTransshipment3VesselArrival;
                container.EstimatedTransshipment3Discharge = containerUpdatedFields.EstimatedTransshipment3Discharge;
                container.ActualTransshipment3Discharge = containerUpdatedFields.ActualTransshipment3Discharge;
                container.EstimatedTransshipment3Loaded = containerUpdatedFields.EstimatedTransshipment3Loaded;
                container.ActualTransshipment3Loaded = containerUpdatedFields.ActualTransshipment3Loaded;
                container.EstimatedTrans3VesselDeparture = containerUpdatedFields.EstimatedTrans3VesselDeparture;
                container.ActualTrans3VesselDeparture = containerUpdatedFields.ActualTrans3VesselDeparture;
                container.Transshipment4Location = containerUpdatedFields.Transshipment4Location;
                container.EstimatedTrans4VesselArrival = containerUpdatedFields.EstimatedTrans4VesselArrival;
                container.ActualTransshipment4VesselArrival = containerUpdatedFields.ActualTransshipment4VesselArrival;
                container.EstimatedTransshipment4Discharge = containerUpdatedFields.EstimatedTransshipment4Discharge;
                container.ActualTransshipment4Discharge = containerUpdatedFields.ActualTransshipment4Discharge;
                container.EstimatedTransshipment4Loaded = containerUpdatedFields.EstimatedTransshipment4Loaded;
                container.ActualTransshipment4Loaded = containerUpdatedFields.ActualTransshipment4Loaded;
                container.EstimatedTrans4VesselDeparture = containerUpdatedFields.EstimatedTrans4VesselDeparture;
                container.ActualTrans4VesselDeparture = containerUpdatedFields.ActualTrans4VesselDeparture;
                container.Leg1Vessel = containerUpdatedFields.Leg1Vessel;
                container.Leg1Voyage = containerUpdatedFields.Leg1Voyage;
                container.Leg2Vessel = containerUpdatedFields.Leg2Vessel;
                container.Leg2Voyage = containerUpdatedFields.Leg2Voyage;
                container.Leg3Vessel = containerUpdatedFields.Leg3Vessel;
                container.Leg3Voyage = containerUpdatedFields.Leg3Voyage;
                container.Leg4Vessel = containerUpdatedFields.Leg4Vessel;
                container.Leg4Voyage = containerUpdatedFields.Leg4Voyage;
                container.Leg5Vessel = containerUpdatedFields.Leg5Vessel;
                container.Leg5Voyage = containerUpdatedFields.Leg5Voyage;
                container.PODLocation = containerUpdatedFields.PODLocation;
                container.EstimatedPODVesselArrival = containerUpdatedFields.EstimatedPODVesselArrival;
                container.ActualPODVesselArrival = containerUpdatedFields.ActualPODVesselArrival;
                container.ActualPODVesselArrival = containerUpdatedFields.ActualPODVesselArrival;
                container.EstimatedPODDischarge = containerUpdatedFields.EstimatedPODDischarge;
                container.ActualPODDischarge = containerUpdatedFields.ActualPODDischarge;
                container.EstimatedPODDeparture = containerUpdatedFields.EstimatedPODDeparture;
                container.ActualPODDeparture = containerUpdatedFields.ActualPODDeparture;
                container.DeliveryLocation = containerUpdatedFields.DeliveryLocation;
                container.EstimatedDelivery = containerUpdatedFields.EstimatedDelivery;
                container.ActualDelivery = containerUpdatedFields.ActualDelivery;
                container.LIFLocation = containerUpdatedFields.LIFLocation;
                container.EstimatedLIFArrival = containerUpdatedFields.EstimatedLIFArrival;
                container.ActualLIFArrival = containerUpdatedFields.ActualLIFArrival;
                container.EstimatedLIFDeparture = containerUpdatedFields.EstimatedLIFDeparture;
                container.ActualLIFDeparture = containerUpdatedFields.ActualLIFDeparture;
                container.EmptyReturnLocation = containerUpdatedFields.EmptyReturnLocation;
                container.EstimatedEmptyReturn = containerUpdatedFields.EstimatedEmptyReturn;
                container.ActualEmptyReturn = containerUpdatedFields.ActualEmptyReturn;
                container.CustomsReleaseState = containerUpdatedFields.CustomsReleaseState;
                container.CustomsReleaseDate = containerUpdatedFields.CustomsReleaseDate;
                container.CarrierReleaseState = containerUpdatedFields.CarrierReleaseState;
                container.CarrierReleaseDate = containerUpdatedFields.CarrierReleaseDate;
                container.AvailablityDate = containerUpdatedFields.AvailablityDate;
                container.AvailabilityLocation = containerUpdatedFields.AvailabilityLocation;
                container.EmptyPickupLocationPortId = this.GetPortId(containerUpdatedFields.EmptyPickupLocation);
                container.DeliveryLocationPortId = this.GetPortId(containerUpdatedFields.DeliveryLocation);
                container.EmptyReturnLocationPortId = this.GetPortId(containerUpdatedFields.EmptyReturnLocation);
                container.AvailabilityLocationPortId = this.GetPortId(containerUpdatedFields.AvailabilityLocation);
                container.OriginLocationPortId = this.GetPortId(containerUpdatedFields.OriginLocation);
                container.LIFLocationPortId = this.GetPortId(containerUpdatedFields.LIFLocation);
                container.POLLocationPortId = this.GetPortId(containerUpdatedFields.POLLocation);
                container.PODLocationPortId = this.GetPortId(containerUpdatedFields.PODLocation);
                container.Transshipment1LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment1Location);
                container.Transshipment2LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment2Location);
                container.Transshipment3LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment3Location);
                container.Transshipment4LocationPortId = this.GetPortId(containerUpdatedFields.Transshipment4Location);
                this.SaveContainer();
            }
        }
        private void UpdatePackage()
        {
            this.IsUpdatingPackages = false;
            DateTime todatDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
            DateTime? eventData = this.GetEventDate();
            ShipmentPackagePM package = shipmentPM.ShipmentPackages.Where(a => a.Id == this.shipmentPackagesId).FirstOrDefault();
            string oceanInsightsSource = "OIN";
            if (package != null)
            {
                if (eventData == null)
                {
                    eventData = todatDate;
                }

                if (package.LastStatusDate == null)
                {
                    package.LastStatusCode = container_status;
                    package.LastStatusDate = eventData;
                    package.ContainerStatusSourceCode = oceanInsightsSource;
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    this.IsUpdatingPackages = true;
                }
                else if (eventData > package.LastStatusDate)
                {
                    package.LastStatusCode = container_status;
                    package.LastStatusDate = eventData;
                    package.ContainerStatusSourceCode = oceanInsightsSource;
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    this.IsUpdatingPackages = true;
                }
            }
        }
        private ContainerUpdatedFields BuildContainerUpdatedFields()
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
            containerUpdatedFields.EmptyPickupLocation = this.GetTranslatedPortCode(emptyPickupLocation);            
            containerUpdatedFields.DepartureLocation = this.departureLocation;
            containerUpdatedFields.DestinationLocation = this.destinationLocation;
            containerUpdatedFields.CurrentStatusDate = this.GetEventDate();
            containerUpdatedFields.CurrentStatus = this.GetContainerStatusName();
            containerUpdatedFields.CurrentLocation = this.ComputeCurrentStatusLocation();
            containerUpdatedFields.OriginLocation = this.GetTranslatedPortCode(origin_loc_locode);
            containerUpdatedFields.EstimatedOriginPickup = this.ComputeEstimatedOriginPickup();
            containerUpdatedFields.ActualOriginPickup = this.ComputeActualOriginPickup();
            containerUpdatedFields.POLLocation = this.GetTranslatedPortCode(pol_loc_locode);
            containerUpdatedFields.EstimatedPOLLoaded = this.ComputeEstimatedPOLLoaded();
            containerUpdatedFields.ActualPOLLoaded = this.ComputeActualPOLLoaded();
            containerUpdatedFields.EstimatedPOLVesselDeparture = this.ComputeEstimatedPOLVesselDeparture();
            containerUpdatedFields.ActualPOLVesselDeparture = ComputeActualPOLVesselDeparture();
            containerUpdatedFields.TransshipmentCount = ts_count;
            containerUpdatedFields.Transshipment1Location = this.GetTranslatedPortCode(tsp1_loc_locode);
            containerUpdatedFields.EstimatedTrans1VesselArrival = this.ComputeEstimatedTrans1VesselArrival();
            containerUpdatedFields.ActualTransshipment1VesselArrival = this.ComputeActualTransshipment1VesselArrival();
            containerUpdatedFields.EstimatedTransshipment1Discharge = this.ComputeEstimatedTransshipment1Discharge();
            containerUpdatedFields.ActualTransshipment1Discharge = ComputeActualTransshipment1Discharge();
            containerUpdatedFields.EstimatedTransshipment1Loaded = this.ComputeEstimatedTransshipment1Loaded();
            containerUpdatedFields.ActualTransshipment1Loaded = ComputeActualTransshipment1Loaded();
            containerUpdatedFields.EstimatedTrans1VesselDeparture = this.ComputeEstimatedTransshipment1VesselDeparture();
            containerUpdatedFields.ActualTrans1VesselDeparture = ComputeActualTransshipment1VesselDeparture();
            containerUpdatedFields.Transshipment2Location = this.GetTranslatedPortCode(tsp2_loc_locode);
            containerUpdatedFields.EstimatedTrans2VesselArrival = this.ComputeEstimatedTrans2VesselArrival();
            containerUpdatedFields.ActualTransshipment2VesselArrival = this.ComputeActualTransshipment2VesselArrival();
            containerUpdatedFields.EstimatedTransshipment2Discharge = this.ComputeEstimatedTransshipment2Discharge();
            containerUpdatedFields.ActualTransshipment2Discharge = ComputeActualTransshipment2Discharge();
            containerUpdatedFields.EstimatedTransshipment2Loaded = this.ComputeEstimatedTransshipment2Loaded();
            containerUpdatedFields.ActualTransshipment2Loaded = ComputeActualTransshipment2Loaded();
            containerUpdatedFields.EstimatedTrans2VesselDeparture = this.ComputeEstimatedTransshipment2VesselDeparture();
            containerUpdatedFields.ActualTrans2VesselDeparture = ComputeActualTransshipment2VesselDeparture();
            containerUpdatedFields.Transshipment3Location = this.GetTranslatedPortCode(tsp3_loc_locode);
            containerUpdatedFields.EstimatedTrans3VesselArrival = this.ComputeEstimatedTrans3VesselArrival();
            containerUpdatedFields.ActualTransshipment3VesselArrival = this.ComputeActualTransshipment3VesselArrival();
            containerUpdatedFields.EstimatedTransshipment3Discharge = this.ComputeEstimatedTransshipment3Discharge();
            containerUpdatedFields.ActualTransshipment3Discharge = ComputeActualTransshipment3Discharge();
            containerUpdatedFields.EstimatedTransshipment3Loaded = this.ComputeEstimatedTransshipment3Loaded();
            containerUpdatedFields.ActualTransshipment3Loaded = ComputeActualTransshipment3Loaded();
            containerUpdatedFields.EstimatedTrans3VesselDeparture = this.ComputeEstimatedTransshipment3VesselDeparture();
            containerUpdatedFields.ActualTrans3VesselDeparture = ComputeActualTransshipment3VesselDeparture();
            containerUpdatedFields.Transshipment4Location = this.GetTranslatedPortCode(tsp4_loc_locode);
            containerUpdatedFields.EstimatedTrans4VesselArrival = this.ComputeEstimatedTrans4VesselArrival();
            containerUpdatedFields.ActualTransshipment4VesselArrival = this.ComputeActualTransshipment4VesselArrival();
            containerUpdatedFields.EstimatedTransshipment4Discharge = this.ComputeEstimatedTransshipment4Discharge();
            containerUpdatedFields.ActualTransshipment4Discharge = ComputeActualTransshipment4Discharge();
            containerUpdatedFields.EstimatedTransshipment4Loaded = this.ComputeEstimatedTransshipment4Loaded();
            containerUpdatedFields.ActualTransshipment4Loaded = ComputeActualTransshipment4Loaded();
            containerUpdatedFields.EstimatedTrans4VesselDeparture = this.ComputeEstimatedTransshipment4VesselDeparture();
            containerUpdatedFields.ActualTrans4VesselDeparture = ComputeActualTransshipment4VesselDeparture();
            containerUpdatedFields.Leg1Vessel = leg1_vessel_name;
            containerUpdatedFields.Leg1Voyage = leg1_voyage;
            containerUpdatedFields.Leg2Vessel = leg2_vessel_name;
            containerUpdatedFields.Leg2Voyage = leg2_voyage;
            containerUpdatedFields.Leg3Vessel = leg3_vessel_name;
            containerUpdatedFields.Leg3Voyage = leg3_voyage;
            containerUpdatedFields.Leg4Vessel = leg4_vessel_name;
            containerUpdatedFields.Leg4Voyage = leg4_voyage;
            containerUpdatedFields.Leg5Vessel = leg5_vessel_name;
            containerUpdatedFields.Leg5Voyage = leg5_voyage;
            containerUpdatedFields.PODLocation = this.GetTranslatedPortCode(pod_loc_locode);
            containerUpdatedFields.EstimatedPODVesselArrival = ComputeEstimatedPODVesselArrival();
            containerUpdatedFields.ActualPODVesselArrival = ComputeActualPODVesselArrival();
            containerUpdatedFields.EstimatedPODDischarge = ComputeEstimatedPODDischarge();
            containerUpdatedFields.ActualPODDischarge = ComputeActualPODDischarge();
            containerUpdatedFields.EstimatedPODDeparture = ComputeEstimatedPODDeparture();
            containerUpdatedFields.ActualPODDeparture = ComputeActualPODDeparture();
            containerUpdatedFields.DeliveryLocation = this.GetTranslatedPortCode(dlv_loc_locode);
            containerUpdatedFields.EstimatedDelivery = ComputeEstimatedDelivery();
            containerUpdatedFields.ActualDelivery = ComputeActualDelivery();
            containerUpdatedFields.LIFLocation = this.GetTranslatedPortCode(lif_loc_locode);
            containerUpdatedFields.EstimatedLIFArrival = ComputeEstimatedLIFArrival();
            containerUpdatedFields.ActualLIFArrival = ComputeActualLIFArrival();
            containerUpdatedFields.EstimatedLIFDeparture = ComputeEstimatedLIFDeparture();
            containerUpdatedFields.ActualLIFDeparture = this.ComputeActualLIFDeparture();
            containerUpdatedFields.EmptyReturnLocation = this.GetTranslatedPortCode(empty_return_loc_locode);
            containerUpdatedFields.EstimatedEmptyReturn = this.ComputeEstimatedEmptyReturn();
            containerUpdatedFields.ActualEmptyReturn = this.ComputeActualEmptyReturn();
            containerUpdatedFields.CustomsReleaseState = customs_release_state;
            containerUpdatedFields.CustomsReleaseDate = this.ComputeCustomsReleaseDate();
            containerUpdatedFields.CarrierReleaseState = carrier_release_state;
            containerUpdatedFields.CarrierReleaseDate = this.ComputeCarrierReleaseDate();
            containerUpdatedFields.AvailablityDate = this.ComputeAvailablityDate();
            containerUpdatedFields.AvailabilityLocation = this.GetTranslatedPortCode(availability_loc);

            return containerUpdatedFields;
        }
        private DateTime? ComputeMainCarriageETD()
        {
            if (!string.IsNullOrEmpty(ETD_last))
            {
                return ConvertStringToDateTime(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return ConvertStringToDateTime(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageETA()
        {
            if (!string.IsNullOrEmpty(ETA_last))
            {
                return ConvertStringToDateTime(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return ConvertStringToDateTime(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return ConvertStringToDateTime(ETA_predection);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATD()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return ConvertStringToDateTime(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return ConvertStringToDateTime(ATD_actual);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATA()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return ConvertStringToDateTime(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return ConvertStringToDateTime(ATA_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_last))
            {
                return ConvertStringToDateTime(emptyPickup_last);
            }

            else if (!string.IsNullOrEmpty(emptyPickup_initial))
            {
                return ConvertStringToDateTime(emptyPickup_initial);
            }

            return null;
        }
        private DateTime? ComputeActualEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_actual))
            {
                return ConvertStringToDateTime(emptyPickup_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_last))
            {
                return ConvertStringToDateTime(gateInDate_last);
            }

            else if (!string.IsNullOrEmpty(gateInDate_initial))
            {
                return ConvertStringToDateTime(gateInDate_initial);
            }

            return null;
        }
        private DateTime? ComputeActualGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_actual))
            {
                return ConvertStringToDateTime(gateInDate_actual);
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
                return ConvertStringToDateTime(origin_pickup_planned_last);
            }

            else if (!string.IsNullOrEmpty(origin_pickup_planned_initial))
            {
                return ConvertStringToDateTime(origin_pickup_planned_initial);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPOLLoaded()
        {
            if (!string.IsNullOrEmpty(pol_loaded_planned_last))
            {
                return ConvertStringToDateTime(pol_loaded_planned_last);
            }

            else if (!string.IsNullOrEmpty(pol_loaded_planned_initial))
            {
                return ConvertStringToDateTime(pol_loaded_planned_initial);
            }

            return null;
        }
        private DateTime? ComputeActualOriginPickup()
        {
            if (!string.IsNullOrEmpty(origin_pickup_actual))
            {
                return ConvertStringToDateTime(origin_pickup_actual);
            }
            return null;
        }
        private DateTime? ComputeActualPOLLoaded()
        {
            if (!string.IsNullOrEmpty(pol_loaded_actual))
            {
                return ConvertStringToDateTime(pol_loaded_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ETD_last))
            {
                return ConvertStringToDateTime(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return ConvertStringToDateTime(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeActualPOLVesselDeparture()
        {
            if (!string.IsNullOrEmpty(ATD_actual))
            {
                return ConvertStringToDateTime(ATD_actual);
            }
            else if (!string.IsNullOrEmpty(ATD_detected))
            {
                return ConvertStringToDateTime(ATD_detected);

            }
            return null;
        }
        private DateTime? ComputeEstimatedTrans1VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp1_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(tsp1_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(tsp1_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(tsp1_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp1_vslarrival_actual))
            {
                return ConvertStringToDateTime(tsp1_vslarrival_actual);
            } 
            else if (!string.IsNullOrEmpty(tsp1_vslarrival_detected))
            {
                return ConvertStringToDateTime(tsp1_vslarrival_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(tsp1_discharge_planned_last))
            {
                return ConvertStringToDateTime(tsp1_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp1_discharge_planned_initial))
            {
                return ConvertStringToDateTime(tsp1_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Discharge()
        {
            if (!string.IsNullOrEmpty(tsp1_discharge_actual))
            {
                return ConvertStringToDateTime(tsp1_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(tsp1_loaded_planned_last))
            {
                return ConvertStringToDateTime(tsp1_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp1_loaded_planned_initial))
            {
                return ConvertStringToDateTime(tsp1_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1Loaded()
        {
            if (!string.IsNullOrEmpty(tsp1_loaded_actual))
            {
                return ConvertStringToDateTime(tsp1_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp1_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(tsp1_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp1_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(tsp1_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment1VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp1_vsldeparture_actual))
            {
                return ConvertStringToDateTime(tsp1_vsldeparture_actual);
            }
            else if (!string.IsNullOrEmpty(tsp1_vsldeparture_detected))
            {
                return ConvertStringToDateTime(tsp1_vsldeparture_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans2VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp2_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(tsp2_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(tsp2_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(tsp2_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp2_vslarrival_actual))
            {
                return ConvertStringToDateTime(tsp2_vslarrival_actual);
            }
            else if (!string.IsNullOrEmpty(tsp2_vslarrival_detected))
            {
                return ConvertStringToDateTime(tsp2_vslarrival_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(tsp2_discharge_planned_last))
            {
                return ConvertStringToDateTime(tsp2_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp2_discharge_planned_initial))
            {
                return ConvertStringToDateTime(tsp2_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Discharge()
        {
            if (!string.IsNullOrEmpty(tsp2_discharge_actual))
            {
                return ConvertStringToDateTime(tsp2_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(tsp2_loaded_planned_last))
            {
                return ConvertStringToDateTime(tsp2_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp2_loaded_planned_initial))
            {
                return ConvertStringToDateTime(tsp2_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2Loaded()
        {
            if (!string.IsNullOrEmpty(tsp2_loaded_actual))
            {
                return ConvertStringToDateTime(tsp2_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp2_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(tsp2_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp2_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(tsp2_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment2VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp2_vsldeparture_actual))
            {
                return ConvertStringToDateTime(tsp2_vsldeparture_actual);
            }
            else if (!string.IsNullOrEmpty(tsp2_vsldeparture_detected))
            {
                return ConvertStringToDateTime(tsp2_vsldeparture_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans3VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp3_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(tsp3_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(tsp3_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(tsp3_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp3_vslarrival_actual))
            {
                return ConvertStringToDateTime(tsp3_vslarrival_actual);
            }
            else if (!string.IsNullOrEmpty(tsp3_vslarrival_detected))
            {
                return ConvertStringToDateTime(tsp3_vslarrival_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(tsp3_discharge_planned_last))
            {
                return ConvertStringToDateTime(tsp3_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp3_discharge_planned_initial))
            {
                return ConvertStringToDateTime(tsp3_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Discharge()
        {
            if (!string.IsNullOrEmpty(tsp3_discharge_actual))
            {
                return ConvertStringToDateTime(tsp3_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(tsp3_loaded_planned_last))
            {
                return ConvertStringToDateTime(tsp3_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp3_loaded_planned_initial))
            {
                return ConvertStringToDateTime(tsp3_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3Loaded()
        {
            if (!string.IsNullOrEmpty(tsp3_loaded_actual))
            {
                return ConvertStringToDateTime(tsp3_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp3_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(tsp3_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp3_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(tsp3_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment3VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp3_vsldeparture_actual))
            {
                return ConvertStringToDateTime(tsp3_vsldeparture_actual);
            }
            else if (!string.IsNullOrEmpty(tsp3_vsldeparture_detected))
            {
                return ConvertStringToDateTime(tsp3_vsldeparture_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTrans4VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp4_vslarrival_planned_last))
            {
                return ConvertStringToDateTime(tsp4_vslarrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(tsp4_vslarrival_planned_initial))
            {
                return ConvertStringToDateTime(tsp4_vslarrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselArrival()
        {
            if (!string.IsNullOrEmpty(tsp4_vslarrival_actual))
            {
                return ConvertStringToDateTime(tsp4_vslarrival_actual);
            }
            else if (!string.IsNullOrEmpty(tsp4_vslarrival_detected))
            {
                return ConvertStringToDateTime(tsp4_vslarrival_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(tsp4_discharge_planned_last))
            {
                return ConvertStringToDateTime(tsp4_discharge_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp4_discharge_planned_initial))
            {
                return ConvertStringToDateTime(tsp4_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Discharge()
        {
            if (!string.IsNullOrEmpty(tsp4_discharge_actual))
            {
                return ConvertStringToDateTime(tsp4_discharge_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(tsp4_loaded_planned_last))
            {
                return ConvertStringToDateTime(tsp4_loaded_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp4_loaded_planned_initial))
            {
                return ConvertStringToDateTime(tsp4_loaded_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4Loaded()
        {
            if (!string.IsNullOrEmpty(tsp4_loaded_actual))
            {
                return ConvertStringToDateTime(tsp4_loaded_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp4_vsldeparture_planned_last))
            {
                return ConvertStringToDateTime(tsp4_vsldeparture_planned_last);
            }
            else if (!string.IsNullOrEmpty(tsp4_vsldeparture_planned_initial))
            {
                return ConvertStringToDateTime(tsp4_vsldeparture_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualTransshipment4VesselDeparture()
        {
            if (!string.IsNullOrEmpty(tsp4_vsldeparture_actual))
            {
                return ConvertStringToDateTime(tsp4_vsldeparture_actual);
            }
            else if (!string.IsNullOrEmpty(tsp4_vsldeparture_detected))
            {
                return ConvertStringToDateTime(tsp4_vsldeparture_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ETA_last))
            {
                return ConvertStringToDateTime(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return ConvertStringToDateTime(ETA_initial);
            }

            return null;
        }
        private DateTime? ComputeActualPODVesselArrival()
        {
            if (!string.IsNullOrEmpty(ATA_actual))
            {
                return ConvertStringToDateTime(ATA_actual);
            }
            else if (!string.IsNullOrEmpty(ATA_detected))
            {
                return ConvertStringToDateTime(ATA_detected);
            }

            return null;
        }
        private DateTime? ComputeEstimatedPODDischarge()
        {
            if (!string.IsNullOrEmpty(pod_discharge_planned_last))
            {
                return ConvertStringToDateTime(pod_discharge_planned_last);
            }

            else if (!string.IsNullOrEmpty(pod_discharge_planned_initial))
            {
                return ConvertStringToDateTime(pod_discharge_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualPODDischarge()
        {
            if (!string.IsNullOrEmpty(pod_discharge_actual))
            {
                return ConvertStringToDateTime(pod_discharge_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedPODDeparture()
        {
            if (!string.IsNullOrEmpty(pod_departure_planned_last))
            {
                return ConvertStringToDateTime(pod_departure_planned_last);
            }

            else if (!string.IsNullOrEmpty(pod_departure_planned_initial))
            {
                return ConvertStringToDateTime(pod_departure_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualPODDeparture()
        {
            if (!string.IsNullOrEmpty(pod_departure_actual))
            {
                return ConvertStringToDateTime(pod_departure_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedDelivery()
        {
            if (!string.IsNullOrEmpty(dlv_delivery_planned_last))
            {
                return ConvertStringToDateTime(dlv_delivery_planned_last);
            }

            else if (!string.IsNullOrEmpty(dlv_delivery_planned_initial))
            {
                return ConvertStringToDateTime(dlv_delivery_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualDelivery()
        {
            if (!string.IsNullOrEmpty(dlv_delivery_actual))
            {
                return ConvertStringToDateTime(dlv_delivery_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedLIFArrival()
        {
            if (!string.IsNullOrEmpty(lif_arrival_planned_last))
            {
                return ConvertStringToDateTime(lif_arrival_planned_last);
            }

            else if (!string.IsNullOrEmpty(lif_arrival_planned_initial))
            {
                return ConvertStringToDateTime(lif_arrival_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualLIFArrival()
        {
            if (!string.IsNullOrEmpty(lif_arrival_actual))
            {
                return ConvertStringToDateTime(lif_arrival_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedLIFDeparture()
        {
            if (!string.IsNullOrEmpty(lif_departure_planned_last))
            {
                return ConvertStringToDateTime(lif_departure_planned_last);
            }

            else if (!string.IsNullOrEmpty(lif_departure_planned_initial))
            {
                return ConvertStringToDateTime(lif_departure_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualLIFDeparture()
        {
            if (!string.IsNullOrEmpty(lif_departure_actual))
            {
                return ConvertStringToDateTime(lif_departure_actual);
            }
            return null;
        }
        private DateTime? ComputeEstimatedEmptyReturn()
        {
            if (!string.IsNullOrEmpty(empty_return_planned_last))
            {
                return ConvertStringToDateTime(empty_return_planned_last);
            }

            else if (!string.IsNullOrEmpty(empty_return_planned_initial))
            {
                return ConvertStringToDateTime(empty_return_planned_initial);
            }
            return null;
        }
        private DateTime? ComputeActualEmptyReturn()
        {
            if (!string.IsNullOrEmpty(empty_return_actual))
            {
                return ConvertStringToDateTime(empty_return_actual);
            }

            return null;
        }
        private DateTime? ComputeCustomsReleaseDate()
        {
            if (!string.IsNullOrEmpty(customs_release_date))
            {
                return ConvertStringToDateTime(customs_release_date);
            }

            return null;
        }
        private DateTime? ComputeCarrierReleaseDate()
        {
            if (!string.IsNullOrEmpty(carrier_release_date))
            {
                return ConvertStringToDateTime(carrier_release_date);
            }

            return null;
        }
        private DateTime? ComputeAvailablityDate()
        {
            if (!string.IsNullOrEmpty(availability_date))
            {
                return ConvertStringToDateTime(availability_date);
            }

            return null;
        }

        private void UpdateShipment()
        {
            if (FeatureToggleHelper.HasFeatureToggle("OIU", this.logitudeTenant.Value))
            {
                if (shipmentPM == null)
                {
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }

                else
                {
                    this.POLShipmentUpdateIndicator = this.GetPOLShipmentUpdateIndicator(container.POLLocationPortId);
                    this.PODShipmentUpdateIndicator = this.GetPODShipmentUpdateIndicator(container.PODLocationPortId);

                    if (!string.IsNullOrEmpty(POLShipmentUpdateIndicator) || !string.IsNullOrEmpty(PODShipmentUpdateIndicator))
                    {
                        this.StartProcessingUpdateShipment();
                    }
                }
            }
        }
        private string GetPortId(string portCode)
        {
            Port port = portRepository.GetOceanPortByCombinedCode(portCode, this.logitudeTenant.Value);
            if (port != null)
            {
                return port.Id;
            }

            return null;
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
        private string GetPOLShipmentUpdateIndicator(string portId)
        {
            string POLShipmentUpdateIndicator = null;

            if (!string.IsNullOrEmpty(portId))
            {
                if (shipmentPM.PreCarriageFromPortId == portId)
                {
                    POLShipmentUpdateIndicator = "Pre Carriage";
                }

                else if (shipmentPM.MainCarriageFromPortId == portId)
                {
                    POLShipmentUpdateIndicator = "Main Carriage";
                }
            }

            return POLShipmentUpdateIndicator;
        }
        private string GetPODShipmentUpdateIndicator(string portId)
        {
            string PODShipmentUpdateIndicator = null;

            if (!string.IsNullOrEmpty(portId))
            {
                if (shipmentPM.OnCarriageToPortId == portId)
                {
                    PODShipmentUpdateIndicator = "On Carriage";
                }

                else if (shipmentPM.MainCarriageToPortId == portId)
                {
                    PODShipmentUpdateIndicator = "Main Carriage";
                }
            }

            return PODShipmentUpdateIndicator;
        }
        private void StartProcessingUpdateShipment()
        {
            shipmentPM.IsUpdatedOceanInsightsAnalyzer = true;  
            this.UpdateShipmentDates();
        }
        private void UpdateShipmentDates()
        {
            this.UpdatePOLDates();
            this.UpdatePODDates();
        }
        private void UpdatePOLDates()
        {
            if (POLShipmentUpdateIndicator == "Pre Carriage")
            {
                this.FillFieldsNewValues("PreCarriageETD", container.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.PreCarriageATD == null)
                {
                    this.FillFieldsNewValues("PreCarriageATD", container.ActualPOLVesselDeparture, shipmentPM);
                }
            }

            else if(POLShipmentUpdateIndicator == "Main Carriage")
            {
                shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = true;
                this.FillFieldsNewValues("MainCarriageETD", container.EstimatedPOLVesselDeparture, shipmentPM);

                if (shipmentPM.MainCarriageATD == null)
                {
                    this.FillFieldsNewValues("MainCarriageATD", container.ActualPOLVesselDeparture, shipmentPM);
                }
            }
        }
        private void UpdatePODDates()
        {
            if (PODShipmentUpdateIndicator == "On Carriage")
            {
                this.FillFieldsNewValues("OnCarriageETA", container.EstimatedPODVesselArrival, shipmentPM);

                if (shipmentPM.OnCarriageATA == null)
                {
                    this.FillFieldsNewValues("OnCarriageATA", container.ActualPODVesselArrival, shipmentPM);
                }                
            }

            else if (PODShipmentUpdateIndicator == "Main Carriage")
            {
                shipmentPM.IsUpdatedOceanInsightsMainCarriageDates = true;
                this.FillFieldsNewValues("MainCarriageETA", container.EstimatedPODVesselArrival, shipmentPM);

                if (shipmentPM.MainCarriageATA == null)
                {
                    this.FillFieldsNewValues("MainCarriageATA", container.ActualPODVesselArrival, shipmentPM);
                }
            }
        }
        private void FillFieldsNewValues(string propertyName, object newValue, object entity)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }
        private void SaveContainer()
        {
            ContainerService containerService = new ContainerService(shipmentContext, logitudeTenant.Value);
            containerService.Update(container);
        }
        private void SaveShipment(ShipmentPM shipmentPM)
        {
            if (shipmentPM.IsUpdatedOceanInsightsAnalyzer || this.IsUpdatingPackages)
            {
                string systemEmail = "system@tenant" + this.logitudeTenant.Value + ".com";
                ShipmentService service = new ShipmentService(shipmentContext, shipmentPM, systemEmail);
                service.Update(true);
            }
        }
        private void DoneAnalyzeQueue()
        {
            analyzeQueue.Status = "D";
            analyzeQueue.ErrorMessage = null;
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
        private void OnCatchAnalyzingError(Exception ex)
        {
            analyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            analyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            analyzeQueue.ErrorMessage = analyzeQueue.ErrorMessage.Length > 7950 ? analyzeQueue.ErrorMessage.Substring(0, 7950) : analyzeQueue.ErrorMessage;
            analyzeQueue.StackTrace = analyzeQueue.StackTrace.Length > 7950 ? analyzeQueue.StackTrace.Substring(0, 7950) : analyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                analyzeQueue.Status = "F";
            }

            else
            {
                analyzeQueue.Retries++;

                if (analyzeQueue.Retries >= 5)
                {
                    analyzeQueue.Status = "F";
                }
            }

            if (analyzeQueue.Status == "F")
            {
                if (analyzeQueue.ConnectedToTenant && analyzeQueue.CommunicationLogId != null)
                {
                    this.communicationLogRepository = new CommunicationLogRepository(this.tenant);
                    CommunicationLog commLog = communicationLogRepository.GetSingleCommunicationLog(analyzeQueue.CommunicationLogId, tenant);
                    if (commLog != null)
                    {
                        commLog.CommunicationStatusTypeCode = "F";
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        commLog.ExceptionMessage = analyzeQueue.ErrorMessage;

                        if (analyzeQueue.StackTrace != null)
                        {
                            commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + analyzeQueue.StackTrace;
                        }

                        communicationLogRepository.Update(commLog);
                        communicationLogRepository.SubmitChanges();
                    }
                }
            }

            analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
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
    }

    [XmlRoot("ArrayOfQueueTask")]
    public class ArrayOfQueueTask
    {
        [XmlElement("QueueTask")]
        public List<QueueTask> QueueTask { get; set; }

        public ArrayOfQueueTask()
        {
            this.QueueTask = new List<QueueTask>();
        }
    }

    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }
        public List<Parameter> Parameters { get; set; }
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
        public string AvailabilityLocation { get; set; }
    }

    public class ContainerGroup
    {
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public List<Container> GroupList { get; set; }
    }
}