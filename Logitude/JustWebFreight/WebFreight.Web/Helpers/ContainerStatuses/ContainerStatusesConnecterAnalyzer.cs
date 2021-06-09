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

namespace WebFreight.Web.Helpers.Analyzers
{
    public class ContainerStatusesConnecterAnalyzer
    {
        private AnalyzeQueue analyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private CommunicationLogRepository communicationLogRepository;
        private int tenant;
        private ArrayOfQueueTask externalTasksQueues;
        private string oceanInsightsId;
        private string container_number;
        private string carrier_scac;
        private LogitudeOceanInsightsRequestRepository logitudeOceanInsightsRequestRepository;
        private int? logitudeTenant = null;
        private ICommonDataContext commonContext;
        private LogitudeOceanInsightsRequest oceanInsight;
        private string communicationLogTo = "OceanInsightStatusRequest";
        private string communicationLogSubject = "Shipment Containers Statuses";
        private string containerObjectTableId;
        private string loggedContactId;
        private string containerId;

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
            }
        }
        private void ConnectAnalyzeQueue()
        {
            try
            {
                this.ConnectAnalyzeQueueToTenantAndEntity();
                this.AnalyzeOceanInsightsParametersXML();
                this.ConnectingOceanInsightRequestToTenant();
                this.AddContainerStatusCommunicationLog();
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
                    var oceanInsightsEnvelopeParameters = oceanInsightsParameters.Value;
                    this.ReadOceanInsightsParametersXMLFields(oceanInsightsEnvelopeParameters);
                }
            }
        }
        private void ReadOceanInsightsParametersXMLFields(string oceanInsightsEnvelopeParameters)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(oceanInsightsEnvelopeParameters);
            XmlNodeList xnList = xmlDoc.SelectNodes("//container");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    if (item.ChildNodes != null && item.Name == "event")
                    {
                        oceanInsightsId = item.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "shipment_id").FirstOrDefault()?.InnerText;
                    }
                    if (item.ChildNodes != null && item.Name == "shipment")
                    {
                        container_number = item.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "container_number").FirstOrDefault()?.InnerText;
                        carrier_scac = item.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName == "carrier_scac").FirstOrDefault()?.InnerText;
                    }
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
            this.GetContainerIdByContainerNumber();
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
        private void GetContainerIdByContainerNumber()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.logitudeTenant != null)
                {
                    var containerNumber = this.oceanInsight?.ContainerNumber;
                    if (!string.IsNullOrEmpty(containerNumber))
                    {
                        ContainerRepository containerRepository = new ContainerRepository(logitudeTenant.Value);
                        containerId = containerRepository.GetContainerByContainerNumberAndTenant(containerNumber, logitudeTenant.Value)?.Id;
                    }
                }
                scope.Complete();
            }
        }
        private void AddContainerStatusCommunicationLog()
        {
            if (this.logitudeTenant != null && !string.IsNullOrEmpty(containerId))
            {
                this.commonContext = CommonDataContext.GetContext(this.logitudeTenant.Value);
                this.communicationLogRepository = new CommunicationLogRepository(this.logitudeTenant.Value);
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    this.GetCommuniactionLogObjectTableId();
                    this.GetLoggedContactId();
                    this.BuildCommunicationLog();
                    scope.Complete();
                }
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
            ContactRepository contactRepository = new ContactRepository(this.commonContext);
            var loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), logitudeTenant.Value);
            this.loggedContactId = loggedContact.Id;
        }
        private void BuildCommunicationLog()
        {
            byte[] documentXML = LogitudeXmlSerializer.SerializeObject(externalTasksQueues);
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
                ByteData = documentXML,
            };

            Communications.AddCommunicationLog(logParams);
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
}