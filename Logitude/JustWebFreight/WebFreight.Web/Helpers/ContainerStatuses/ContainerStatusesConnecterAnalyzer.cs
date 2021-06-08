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

namespace WebFreight.Web.Helpers.Analyzers
{
    public class ContainerStatusesConnecterAnalyzer
    {   
        private AnalyzeQueue analyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private CommunicationLogRepository myCommunicationLogRepository;
        private int tenant;
        private List<QueueTask> externalTasksQueueTasks;

        public ContainerStatusesConnecterAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.tenant = analyzeQueue.Tenant;
                this.analyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.myCommunicationLogRepository = new CommunicationLogRepository(this.tenant);
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
                XmlSerializer serializer = new XmlSerializer(typeof(string));
                externalTasksQueueTasks = (List<QueueTask>)serializer.Deserialize(memorystream);
            }

            catch (Exception ex)
            {
                analyzeQueue.Status = "F";
                analyzeQueue.ErrorMessage = "ContainerStatusesConnecterAnalyzer failed: " + ex.Message;
                analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
                analyzeQueueRepository.Update(analyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }

            if (externalTasksQueueTasks != null)
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
                this.AnalyzeOceanInsightsParametersXML();
                this.DoneAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private void AnalyzeOceanInsightsParametersXML()
        {
            var oceanInsightsQueueTask = externalTasksQueueTasks.Where(a => a.Action == "OceanInsights.PushUpdate").FirstOrDefault();
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
            XmlNodeList xnList = xmlDoc.SelectNodes("//event");
            foreach (XmlNode xn in xnList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    if (item.Attributes != null)
                    {
                        if(item.Attributes["Type"] != null && item.Attributes["Type"].Value == "shipment_id")
                        {
                            if(item.Attributes["Type"].Value == "shipment_id")
                            {
                                var shipmentId = item.FirstChild.InnerText;
                            }
                            if (item.Attributes["Type"].Value == "shipment_id")
                            {
                                var shipmentId = item.FirstChild.InnerText;
                            }
                        }
                    }
                }
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
                    CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(analyzeQueue.CommunicationLogId, tenant);
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

                        myCommunicationLogRepository.Update(commLog);
                        myCommunicationLogRepository.SubmitChanges();
                    }
                }
            }

            analyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(analyzeQueue.Tenant);
            analyzeQueueRepository.Update(analyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}
