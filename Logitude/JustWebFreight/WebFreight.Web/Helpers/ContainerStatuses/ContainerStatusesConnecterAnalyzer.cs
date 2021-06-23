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
using System.Net;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Security;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Text;
using Simplog.Data.ShipmentsModel;
using Logitude.Server.Tools.Counters;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;

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
        private LogitudeOceanInsightsRequest oceanInsight;
        private ContainerPM container;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string containerObjectTableId;
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

        private string oceanInsightsId;
        private string container_number;
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

        public ContainerStatusesConnecterAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.tenant = analyzeQueue.Tenant;
                this.analyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.logitudeOceanInsightsRequestRepository = new LogitudeOceanInsightsRequestRepository(this.tenant);                
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
                this.ConnectingOceanInsightRequestToTenant();
                this.ProcessLogitudeTenant(); // choose more suitable name later                
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
                    this.ReadOceanInsightsParametersXMLFields();
                }
            }
        }
        private void ReadOceanInsightsParametersXMLFields()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(oceanInsightsEnvelopeParameters);
            XmlNodeList xnList = xmlDoc.SelectNodes("//container");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    this.ReadEventSectionFields(item);
                    this.ReadShipmentSectionFields(item); 
                }
            }
        }
        private void ReadEventSectionFields(XmlNode node)
        {
            if (node.ChildNodes != null && node.Name == "event")
            {
                oceanInsightsId = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "shipment_id").FirstOrDefault()?.InnerText;
                details = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "details").FirstOrDefault()?.InnerText;
                createdDate = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "created").FirstOrDefault()?.InnerText;
                eventCode = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "code").FirstOrDefault()?.InnerText;
            }
        }
        private void ReadShipmentSectionFields(XmlNode node)
        {
            if (node.ChildNodes != null && node.Name == "shipment")
            {
                container_number = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
                carrier_scac = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_scac").FirstOrDefault()?.InnerText;
                container_status = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "status").FirstOrDefault()?.InnerText;
                weight = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "weight").FirstOrDefault()?.InnerText;
                ETD_initial = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "Pol_vsldeparture_planned_initial").FirstOrDefault()?.InnerText;
                ETD_last = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "Pol_vsldeparture_planned_last").FirstOrDefault()?.InnerText;
                ATD_actual = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "Pol_vsldeparture_actual").FirstOrDefault()?.InnerText;
                ATD_detected = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "Pol_vsldeparture_detected").FirstOrDefault()?.InnerText;
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

                XmlElement emptyPickupLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "empty_pickup_loc").FirstOrDefault();
                XmlElement departureLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pol_loc").FirstOrDefault();
                XmlElement destinationLocationElement = node.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "pod_loc").FirstOrDefault();

                if (emptyPickupLocationElement != null)
                {
                    emptyPickupLocation = emptyPickupLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                }

                if (departureLocationElement != null)
                {
                    departureLocation = departureLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                }

                if (destinationLocationElement != null)
                {
                    destinationLocation = destinationLocationElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "name").FirstOrDefault()?.InnerText;
                }
            }
        }
        private void ConnectingOceanInsightRequestToTenant()
        {
            if (!string.IsNullOrEmpty(this.oceanInsightsId))
            {
                this.GetLogitudeTenantByOceanInsightsId();
            }

            if(string.IsNullOrEmpty( this.oceanInsightsId) || this.oceanInsight == null)
            {
                this.GetLogitudeTenantByOceanInsightsContainerNumberAndScac();
            }            
        }
        private void GetLogitudeTenantByOceanInsightsId()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                oceanInsight = this.logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsRequestByOceanInsigntId(this.oceanInsightsId);
                if (oceanInsight != null)
                {
                    this.logitudeTenant = oceanInsight.Tenant;
                }

                scope.Complete();
            }
        }
        private void GetLogitudeTenantByOceanInsightsContainerNumberAndScac()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                oceanInsight = this.logitudeOceanInsightsRequestRepository.GetSingleLogitudeOceanInsightsRequestByContainerNumberAndScac(this.container_number, this.carrier_scac);
                if (oceanInsight != null)
                {
                    this.logitudeTenant = oceanInsight.Tenant;
                }
                scope.Complete();
            }
        }
        private void ProcessLogitudeTenant()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.logitudeTenant != null)
                {
                    this.shipmentContext = ShipmentsContext.GetContext(logitudeTenant.Value);
                    this.shipmentContainerStatusRepository = new ShipmentContainerStatusRepository(shipmentContext);
                    this.containerRepository = new ContainerRepository(shipmentContext);
                    this.containerQuery = new ContainerQuery(containerRepository);
                    this.containerStatusRepository = new ContainerStatusRepository(shipmentContext);
                    this.shipmentRepository = new ShipmentRepository(shipmentContext);
                    this.shipmentQuery = new ShipmentQuery(shipmentRepository);

                    this.GetContainerDataByContainerNumber();
                    this.AddContainerStatusCommunicationLog();

                    if (this.eventCode == "0")
                    {
                        this.CreateShipmentContainerStatus();                        
                        this.UpdateContainer();                        
                        this.UpdateShipment();
                    }
                }

                scope.Complete();
            }
        }
        private void GetContainerDataByContainerNumber()
        {
            var containerNumber = this.oceanInsight?.ContainerNumber;
            if (!string.IsNullOrEmpty(containerNumber))
            {
                container = containerQuery.GetContainerByContainerNumberAndTenant(containerNumber, logitudeTenant.Value);
                containerId = container?.Id;
            }
        }
        private void AddContainerStatusCommunicationLog()
        {
            if (!string.IsNullOrEmpty(containerId))
            {
                this.commonContext = CommonDataContext.GetContext(this.logitudeTenant.Value);
                this.communicationLogRepository = new CommunicationLogRepository(this.logitudeTenant.Value);

                this.GetCommuniactionLogObjectTableId();
                this.GetLoggedContactId();
                this.BuildCommunicationLog();
            }
        }
        private void GetCommuniactionLogObjectTableId()
        {
            var objectTableName = "Container";
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(logitudeTenant.Value);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                containerObjectTableId = objectTable.Id;
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
        private void BuildCommunicationLog()
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
                LoggingObjectTableId = containerObjectTableId,
                LoggingEntityId = containerId,
                LoggingEntityReference = container_number,
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
        private void CreateShipmentContainerStatus()
        {
            string iHash = this.GetHashedData(oceanInsight.ShipmentId);

            DateTime logDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? eventDate = this.GetEventDate();
            double containerWeight = this.GetContainerWeight();
            DateTime? departureDate = this.ComputeDepartureDate();
            DateTime? arrivalDate = this.ComputeArrivalDate();
            string departureDateInfo = this.ComputeDepartureDateInfo();
            string arrivalDateInfo = this.ComputeArrivalDateInfo();

            ShipmentContainerStatus containerStatus = new ShipmentContainerStatus()
            {
                Id = IdCounter.GetNumber("ShipmentContainerStatus", this.tenant),
                Tenant = this.logitudeTenant.Value,
                ShipmentId = this.oceanInsight.ShipmentId,
                StatusSource = "OIN",
                ContainerStatusCode = this.container_status,
                Details = this.details,
                RecordHash = iHash,
                Weight = containerWeight,
                ReceivingDate = logDate,
                EventDate = eventDate,
                ContainerId = this.container.ShipmentPackagesId,
                ContainerNumber = this.container_number,
                DepartureDate = departureDate,
                ArrivalDate = arrivalDate,
                TimeOfDepartureInfo = departureDateInfo,
                TimeOfArrivalInfo = arrivalDateInfo,
            };

            shipmentContainerStatusRepository.Add(containerStatus);
            shipmentContainerStatusRepository.SubmitChanges();
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
                this.FillFieldsNewValues("EstimatedGateInDate", containerUpdatedFields.EstimatedGateInDate, container);
                this.FillFieldsNewValues("ActualGateInDate", containerUpdatedFields.ActualGateInDate, container);
                this.FillFieldsNewValues("DepartureLocation", containerUpdatedFields.DepartureLocation, container);
                this.FillFieldsNewValues("DestinationLocation", containerUpdatedFields.DestinationLocation, container);
                container.CurrentStatus = containerUpdatedFields.CurrentStatus;
                container.CurrentLocation = containerUpdatedFields.CurrentLocation;
                container.CurrentStatusDate = containerUpdatedFields.CurrentStatusDate;
                container.HasContainerException = containerUpdatedFields.HasContainerException;
                container.UpdateDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);

                this.SaveContainer();
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
            containerUpdatedFields.EstimatedGateInDate = this.ComputeEstimatedGateInDate();
            containerUpdatedFields.ActualGateInDate = this.ComputeActualGateInDate();
            containerUpdatedFields.EmptyPickupLocation = this.emptyPickupLocation;
            containerUpdatedFields.DepartureLocation = this.departureLocation;
            containerUpdatedFields.DestinationLocation = this.destinationLocation;
            containerUpdatedFields.CurrentStatusDate = this.GetEventDate();
            containerUpdatedFields.CurrentStatus = this.GetContainerStatusName();
            containerUpdatedFields.CurrentLocation = this.ComputeCurrentStatusLocation();
            //containerUpdatedFields.HasContainerException = "";

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
        private void UpdateShipment()
        {
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(oceanInsight.ShipmentId, logitudeTenant.Value);

            if (shipmentPM == null)
            {
                throw new Exception("Analyzing shipment faild, shipment not found");
            }

            else
            {
                bool isSavingShipment = false;
                List<Container> shipmentContainers = containerRepository.GetContainesrByShipmentId(shipmentPM.Id, logitudeTenant.Value).ToList();
                if (shipmentContainers.Count() == 1)
                {
                    shipmentPM.IsUpdatedOceanInsightsAnalyzer = true;
                    this.UpdateShipmentDates(shipmentContainers.FirstOrDefault(), shipmentPM);
                    isSavingShipment = true;
                }

                else
                {
                    List<Container> nullValuesContainers = shipmentContainers.Where(d => d.MainCarriageATA == null && d.MainCarriageATD == null 
                    && d.MainCarriageETA == null && d.MainCarriageETD == null
                    && string.IsNullOrEmpty(d.DepartureLocation) && string.IsNullOrEmpty(d.DestinationLocation)).ToList();
                    
                    shipmentContainers = shipmentContainers.Except(nullValuesContainers).ToList();

                    List<Container> shipmentContainers_grouped = (from s in shipmentContainers
                                                                  group s by new
                                                                  {
                                                                      s.DepartureLocation,
                                                                      s.DestinationLocation,
                                                                      s.MainCarriageETD,
                                                                      s.MainCarriageETA,
                                                                      s.MainCarriageATD,
                                                                      s.MainCarriageATA,
                                                                  } into m
                                                                  select new Container()
                                                                  {
                                                                      MainCarriageETD = m.Key.MainCarriageETD,
                                                                      MainCarriageETA = m.Key.MainCarriageETA,
                                                                      MainCarriageATD = m.Key.MainCarriageATD,
                                                                      MainCarriageATA = m.Key.MainCarriageATA,
                                                                  }).ToList();

                    if (shipmentContainers_grouped != null)
                    {
                        if (shipmentContainers_grouped.Count() == 1)
                        {
                            shipmentPM.IsUpdatedOceanInsightsAnalyzer = true;
                            this.UpdateShipmentDates(shipmentContainers_grouped.FirstOrDefault(), shipmentPM);
                            this.UpdateContainersException(shipmentContainers, true);
                            isSavingShipment = true;
                        }

                        else
                        {
                            this.UpdateContainersException(shipmentContainers, false);
                            containerRepository.SubmitChanges();
                        }
                    }
                }

                if (isSavingShipment)
                {
                    this.SaveShipment(shipmentPM);
                }
            }
        }
        private void UpdateContainersException(List<Container> shipmentContainers, bool sameConatiner)
        {
            foreach(Container item in shipmentContainers)
            {
                item.HasContainerException = false;

                if (!sameConatiner)
                {
                    if(item.DepartureLocation != departureLocation || item.DestinationLocation != destinationLocation 
                        || item.MainCarriageATA != ComputeMainCarriageATA()
                        || item.MainCarriageATD != ComputeMainCarriageATD()
                        || item.MainCarriageETA != ComputeMainCarriageETA()
                        || item.MainCarriageETD != ComputeMainCarriageETD())
                    {
                        item.HasContainerException = true;
                    }
                }

                containerRepository.Update(item);
            }
        }
        private void UpdateShipmentDates(Container container, ShipmentPM shipment)
        {
            this.FillFieldsNewValues("MainCarriageETD", container.MainCarriageETD, shipment);
            this.FillFieldsNewValues("MainCarriageETA", container.MainCarriageETA, shipment);
            this.FillFieldsNewValues("MainCarriageATD", container.MainCarriageATD, shipment);
            this.FillFieldsNewValues("MainCarriageATA", container.MainCarriageATA, shipment);
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
            string systemEmail = "system@tenant" + this.logitudeTenant.Value + ".com";
            ShipmentService service = new ShipmentService(shipmentContext, shipmentPM, systemEmail);
            service.Update();
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
        public  DateTime? ConvertStringToDateTime(string XMLValue)
        {
            string dateTimeString = this.GetCorrectDateTimeString(XMLValue);

            if(!string.IsNullOrEmpty(dateTimeString))
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
                if(XMLValue.Length > 16)
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
        public DateTime? EstimatedGateInDate { get; set; }
        public DateTime? ActualGateInDate { get; set; }
        public string EmptyPickupLocation { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public bool HasContainerException { get; set; }
    }
}