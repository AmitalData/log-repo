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
        private Container container;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string containerObjectTableId;
        private string loggedContactId;
        private string containerId;
        private string oceanInsightsEnvelopeParameters;
        private IShipmentsContext shipmentContext;
        private ShipmentContainerStatusRepository shipmentContainerStatusRepository;
        private ContainerRepository containerRepository;
        private ContainerStatusRepository containerStatusRepository;
        private ShipmentRepository shipmentRepository;
        private ShipmentPackageRepository shipmentPackageRepository;

        private string oceanInsightsId;
        private string container_number;
        private string carrier_scac;
        private string container_status;
        private string details;
        private string weight;
        private string createdDate;
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
                    this.containerStatusRepository = new ContainerStatusRepository(shipmentContext);
                    this.shipmentRepository = new ShipmentRepository(shipmentContext);
                    this.shipmentPackageRepository = new ShipmentPackageRepository(shipmentContext);

                    this.GetContainerDataByContainerNumber();
                    this.AddContainerStatusCommunicationLog();
                    this.CreateShipmentContainerStatus();
                    ContainerUpdatedFields containerUpdatedFields = this.BuildContainerUpdatedFields();
                    this.UpdateContainer(containerUpdatedFields);
                    this.UpdateShipment(containerUpdatedFields);
                    this.Save();
                }

                scope.Complete();
            }
        }
        private void GetContainerDataByContainerNumber()
        {
            var containerNumber = this.oceanInsight?.ContainerNumber;
            if (!string.IsNullOrEmpty(containerNumber))
            {
                container = containerRepository.GetContainerByContainerNumberAndTenant(containerNumber, logitudeTenant.Value);
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

            if (!shipmentContainerStatusRepository.DoesRecordExist(iHash))
            {
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
            }
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
                
                //DateTime.TryParse(createdDate, out result);
                return TryParseNullable(createdDate);
                //return DateTime.ParseExact(createdDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                //return Convert.ToDateTime(createdDate, CultureInfo.InvariantCulture);
            }

            return null;
        }
        private DateTime? ComputeDepartureDate()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return TryParseNullable(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return TryParseNullable(ATD_actual);
            }

            else if (!string.IsNullOrEmpty(ETD_last))
            {
                return TryParseNullable(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return TryParseNullable(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeArrivalDate()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return TryParseNullable(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return TryParseNullable(ATA_actual);
            }

            else if (!string.IsNullOrEmpty(ETA_last))
            {
                return TryParseNullable(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return TryParseNullable(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return TryParseNullable(ETA_predection);
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
            string information = shipmentId + tenant.ToString(); //+ this.DeparturePortId + this.ArrivalPortId + this.container_number + this.EventLocationCode + this.EventLocationDateString;
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
                if (containerUpdatedFields != null)
                {
                    this.FillContainerFieldsNewValues("MainCarriageETD", containerUpdatedFields.MainCarriageETD);
                    this.FillContainerFieldsNewValues("MainCarriageETA", containerUpdatedFields.MainCarriageETA);
                    this.FillContainerFieldsNewValues("MainCarriageATD", containerUpdatedFields.MainCarriageATD);
                    this.FillContainerFieldsNewValues("MainCarriageATA", containerUpdatedFields.MainCarriageATA);
                    this.FillContainerFieldsNewValues("EmptyPickupLocation", containerUpdatedFields.EmptyPickupLocation);
                    this.FillContainerFieldsNewValues("EstimatedEmptyPickupDate", containerUpdatedFields.EstimatedEmptyPickupDate);
                    this.FillContainerFieldsNewValues("ActualEmptyPickupDate", containerUpdatedFields.ActualEmptyPickupDate);
                    this.FillContainerFieldsNewValues("EstimatedGateInDate", containerUpdatedFields.EstimatedGateInDate);
                    this.FillContainerFieldsNewValues("ActualGateInDate", containerUpdatedFields.ActualGateInDate);
                    this.FillContainerFieldsNewValues("DepartureLocation", containerUpdatedFields.DepartureLocation);
                    this.FillContainerFieldsNewValues("DestinationLocation", containerUpdatedFields.DestinationLocation);
                    container.CurrentStatus = containerUpdatedFields.CurrentStatus;
                    container.CurrentLocation = containerUpdatedFields.CurrentLocation;
                    container.CurrentStatusDate = containerUpdatedFields.CurrentStatusDate;
                    container.HasContainerException = containerUpdatedFields.HasContainerException;
                    container.UpdateDate = TenantServerConfigration.GetCurrentDateTime(logitudeTenant.Value);
                    containerRepository.Update(container);
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
                return TryParseNullable(ETD_last);
            }

            else if (!string.IsNullOrEmpty(ETD_initial))
            {
                return TryParseNullable(ETD_initial);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageETA()
        {
            if (!string.IsNullOrEmpty(ETA_last))
            {
                return TryParseNullable(ETA_last);
            }

            else if (!string.IsNullOrEmpty(ETA_initial))
            {
                return TryParseNullable(ETA_initial);
            }

            else if (!string.IsNullOrEmpty(ETA_predection))
            {
                return TryParseNullable(ETA_predection);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATD()
        {
            if (!string.IsNullOrEmpty(ATD_detected))
            {
                return TryParseNullable(ATD_detected);
            }

            else if (!string.IsNullOrEmpty(ATD_actual))
            {
                return TryParseNullable(ATD_actual);
            }

            return null;
        }
        private DateTime? ComputeMainCarriageATA()
        {
            if (!string.IsNullOrEmpty(ATA_detected))
            {
                return TryParseNullable(ATA_detected);
            }

            else if (!string.IsNullOrEmpty(ATA_actual))
            {
                return TryParseNullable(ATA_actual);
            }

            return null;
        }
        private DateTime? ComputeEstimatedEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_last))
            {
                return TryParseNullable(emptyPickup_last);
            }

            else if (!string.IsNullOrEmpty(emptyPickup_initial))
            {
                return TryParseNullable(emptyPickup_initial);
            }

            return null;
        }
        private DateTime? ComputeActualEmptyPickupDate()
        {
            if (!string.IsNullOrEmpty(emptyPickup_actual))
            {
                return TryParseNullable(emptyPickup_actual);
            }            

            return null;
        }
        private DateTime? ComputeEstimatedGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_last))
            {
                return TryParseNullable(gateInDate_last);
            }

            else if (!string.IsNullOrEmpty(gateInDate_initial))
            {
                return TryParseNullable(gateInDate_initial);
            }

            return null;
        }
        private DateTime? ComputeActualGateInDate()
        {
            if (!string.IsNullOrEmpty(gateInDate_actual))
            {
                return TryParseNullable(gateInDate_actual);
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
        private void FillContainerFieldsNewValues(string propertyName, object newValue)
        {
            PropertyInfo propertyInfo = this.container.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(this.container, newValue);
            }
        }
        private void UpdateShipment(ContainerUpdatedFields containerUpdatedFields)
        {
            Shipment shipment = this.shipmentRepository.GetSingleShipment(oceanInsight.ShipmentId, logitudeTenant.Value);
            if (shipment != null && containerUpdatedFields != null)
            {
                int? packagesCount = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(oceanInsight.ShipmentId, logitudeTenant.Value)?.Count();
                if(packagesCount != null)
                {
                    if(packagesCount == 1)
                    {
                        this.UpdateShipmentDates(containerUpdatedFields, shipment);
                        this.CreateEvent();
                    }

                    else
                    {

                    }
                }
            }
        }
        private void UpdateShipmentDates(ContainerUpdatedFields containerUpdatedFields, Shipment shipment)
        {
            this.FillShipmentFieldsNewValues("MainCarriageETD", containerUpdatedFields.MainCarriageETD, shipment);
            this.FillShipmentFieldsNewValues("MainCarriageETA", containerUpdatedFields.MainCarriageETA, shipment);
            this.FillShipmentFieldsNewValues("MainCarriageATD", containerUpdatedFields.MainCarriageATD, shipment);
            this.FillShipmentFieldsNewValues("MainCarriageATA", containerUpdatedFields.MainCarriageATA, shipment);
            shipmentRepository.Update(shipment);
        }
        private void FillShipmentFieldsNewValues(string propertyName, object newValue, Shipment shipment)
        {
            PropertyInfo propertyInfo = shipment.GetType().GetProperty(propertyName);

            if (propertyInfo != null && newValue != null)
            {
                propertyInfo.SetValue(shipment, newValue);
            }
        }
        private void CreateEvent()
        {
            // and create a shipment event that explains what was
            //updated and to what value(Event Name = Ocean Insight Shipment Update, Notes: “ETD
            //of main carriage was updated from XXXX to XXXX”)
        }
        private void Save()
        {
            shipmentContext.SaveChanges();
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

        public  DateTime? TryParseNullable(string val)
        {
            DateTime outValue;
            return DateTime.TryParse(val, out outValue) ? (DateTime?)outValue : null;
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