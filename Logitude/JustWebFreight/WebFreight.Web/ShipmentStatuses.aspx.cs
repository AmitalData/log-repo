using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Parameter = Logitude.Server.Tools.Parameter;

namespace WebFreight.Web
{
    public partial class ShipmentStatuses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpRequest iRequest = this.Request;

                if (iRequest != null)
                {
                    string iString = "";

                    using (var reader = new StreamReader(Request.InputStream))
                    {
                        iString = reader.ReadToEnd();
                    }

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(iString);
                    XmlNodeList mydataList = xmldoc.GetElementsByTagName("mydata");
                    string PartnerId = string.Empty;
                    if (!string.IsNullOrEmpty(iString))
                    {
                        foreach (XmlNode item in mydataList)
                        {
                            foreach (XmlNode item1 in item.ChildNodes)
                            {
                                if (item1.Name == "master_customer_id")
                                {
                                    PartnerId = item1.InnerText;
                                }
                            }
                        }

                        //XmlNodeList nodeList = xmldoc.GetElementsByTagName("Master_Customer_ID");
                        
                        //if (nodeList[0] != null)
                        //{
                        //    PartnerId = nodeList[0].InnerText;
                        //}
                        this.AddExternalTaskQueue(iString, PartnerId);
                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ShipmentStatuses Page", "ShipmentStatuses Method", null);
                throw ex;
            }
        }

        private void AddExternalTaskQueue(string messageData, string PartnerId)
        {
            ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(0);
            var MyCode = helper.GetLogitudeCodeTranslation(PartnerId, "LastMile", "HybridPartner");
            ICommonDataContext commonContext = CommonDataContext.GetContext(0);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commonContext);
            HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
            HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePM(MyCode, 0);
            //ShipmentQuery shipmentQuery = new ShipmentQuery(0); 
            //var ForwarderShipment = shipmentQuery.GetSinglePM(ShipmentId, tenant);
            if (Partner != null)
            {
                ObjectTableQuery tablesQuery = new ObjectTableQuery(0);
                ObjectTablePM objectTable = tablesQuery.GetObjectTableByName("Shipment", 0);
                List<QueueTask> tasks = new List<QueueTask>();
                var data = messageData;//LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(Shipment);
                tasks.Add(new QueueTask() { Action = "StatusReceived", Parameters = new List<Logitude.Server.Tools.Parameter>() { new Logitude.Server.Tools.Parameter { Name = "ShipmentStatus", Value = data } } });
                var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                var Tenant = (int)Partner.PartnerTenant;
                
              
                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = ByteData.Length,
                    Tenant = Tenant,
                    Id = IdCounter.GetNumber("Document", Tenant),
                    HasFile = true,
                    Folder = "ExternalTasksQueue",
                };
                documentrepository.Add(document);
                documentrepository.SubmitChanges();
                var commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    InOut = "O",
                    //EntityId = OceanInsightsRequest.Id,
                    ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                    Subject = "ShipmentStatus",
                    Tenant = Tenant,
                    CommunicationLogTypeCode = "Q",
                    CommunicationStatusTypeCode = "W",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    LastStatusDateUTC = DateTime.UtcNow,
                    QueueName = "externaltasksqueue" + Tenant + 1,
                    Priority = 1,

                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();
                SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, Tenant);
            }

        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });
    
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }
    }
}