using System;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModel;

using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.Helpers;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace CommunicationWorkerRole
{
    public class AgentsSharedLogisticsWorkerRole : WorkerEntryPoint
    {


        public override void Run()
        {


            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {

                            string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                            bool processEnebled = true;

                            if (processEnebled)
                            {
                                if (commLog != null)
                                {
                                    if (commLog.CommunicationStatusTypeCode == "D")
                                    {
                                        queueservice.Complete();
                                    }
                                    else
                                    {

                                        if (commLog.Document != null)
                                        {
                                            string filename = commLog.DocumentId + "." + commLog.Document.Extension;


                                            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                                            {
                                                FileName = commLog.Document.Id,
                                                FolderName = commLog.Document.Folder,
                                                Extension = commLog.Document.Extension,
                                                Tenant = commLog.Document.Tenant,
                                                FileSize = commLog.Document.FileSize,
                                            };
                                            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                                            byte[] datainByte = storageservice.Read(fileInfo);

                                            if (datainByte != null)
                                            {
                                            
                                                if (commLog.Subject != "Status Update")
                                                {
                                                    ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(datainByte);
                                                    string manifestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(manifestSL);

                                                    ICommonDataContext agentContext = CommonDataContext.GetContext(manifestSL.DestinationAgentTenant);
                                                    ContactRepository contactRepository = new ContactRepository(manifestSL.DestinationAgentTenant);
                                                    PortRepository portRepository = new PortRepository(manifestSL.DestinationAgentTenant);
                                                    AgentRepository agentRepository = new AgentRepository(manifestSL.DestinationAgentTenant);
                                                    Agent destinationAgent = agentRepository.GetSingleAgentBySharedKey(manifestSL.AgentSharedKey, manifestSL.DestinationAgentTenant);
                                                    Contact systemContact = contactRepository.GetSingleContactByEmail("system@tenant" + manifestSL.DestinationAgentTenant + ".com", manifestSL.DestinationAgentTenant, false);

                                                    #region Trans Port
                                                    Port fromPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.MainCarriageFromPort.CountryCode + manifestSL.MainCarriageFromPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                    Port toPort = null;
                                                    if (manifestSL.FinalDistenationPort != null)
                                                    {
                                                        toPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.FinalDistenationPort.CountryCode + manifestSL.FinalDistenationPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                    }
                                                    else
                                                    {
                                                        toPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.MainCarriageToPort.CountryCode + manifestSL.MainCarriageToPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                    }

                                                    if (manifestSL.ShipmentPickUp != null)
                                                    {
                                                        if (manifestSL.ShipmentPickUp.FromPort != null)
                                                        {
                                                            Port pickUpFromPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.ShipmentPickUp.FromPort.CountryCode + manifestSL.ShipmentPickUp.FromPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                            manifestSL.ShipmentPickUp.FromPortId = pickUpFromPort!=null ? pickUpFromPort.Id:null;
                                                        }
                                                        if (manifestSL.ShipmentPickUp.ToPort != null)
                                                        {
                                                            Port pickUpToPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.ShipmentPickUp.ToPort.CountryCode + manifestSL.ShipmentPickUp.ToPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                            manifestSL.ShipmentPickUp.ToPortId = pickUpToPort != null ? pickUpToPort.Id : null;
                                                        }
                                                    }
                                                    if (manifestSL.ShipmentDelivery != null)
                                                    {
                                                        if (manifestSL.ShipmentDelivery.FromPort != null)
                                                        {
                                                            Port pickUpFromPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.ShipmentDelivery.FromPort.CountryCode + manifestSL.ShipmentDelivery.FromPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                            manifestSL.ShipmentDelivery.FromPortId = pickUpFromPort != null ? pickUpFromPort.Id : null;
                                                        }
                                                        if (manifestSL.ShipmentDelivery.ToPort != null)
                                                        {
                                                            Port pickUpToPort = WcfServicesHelper.GetPortOrCopyToTenant(manifestSL.ShipmentDelivery.ToPort.CountryCode + manifestSL.ShipmentDelivery.ToPort.Code, manifestSL.DestinationAgentTenant, portRepository);
                                                            manifestSL.ShipmentDelivery.ToPortId = pickUpToPort != null ? pickUpToPort.Id : null;
                                                        }
                                                    }
                                                    #endregion

                                                    #region Cancel Old Mainfest
                                                    if (commLog.Subject == "Update Shared Agent")
                                                    {

                                                        AgentSharedManifestHelper agentSharedManifestHelper = new AgentSharedManifestHelper();
                                                        List<ManifestSL> oldManifestSLLists = agentSharedManifestHelper.GetAgentShareManifestSLByEntityId(commLog.EntityId, tenant);
                                                        foreach (ManifestSL oldManifestSL in oldManifestSLLists)
                                                        {
                                                            if (oldManifestSL.AgentSharedManifestId != manifestSL.AgentSharedManifestId)
                                                            {
                                                                AgentSharedManifestRepository agentSharedManifestRepository = new AgentSharedManifestRepository(tenant);
                                                                AgentSharedManifest agentSharedManifest = agentSharedManifestRepository.GetSingleAgentSharedManifest(oldManifestSL.AgentSharedManifestId, oldManifestSL.DestinationAgentTenant);
                                                                if (agentSharedManifest != null && !agentSharedManifest.CancelledBySenderAgent)
                                                                {
                                                                    ShipmentQuery shipmentQuery = new ShipmentQuery(agentSharedManifest.Tenant);
                                                                    bool isCreate = shipmentQuery.CheckIfShipmentCreateFromManinfest(agentSharedManifest.Id, agentSharedManifest.Tenant);
                                                                    if (isCreate)
                                                                    {
                                                                       agentSharedManifestHelper.SendEmail(manifestSL.AgentSharedKey, manifestSL.ShipmentNumber, tenant);
                                                                    }
                                                                    else
                                                                    {
                                                                        agentSharedManifest.StatusCode = "CANC";
                                                                        agentSharedManifest.CancelledBySenderAgent = true;
                                                                        agentSharedManifestRepository.Update(agentSharedManifest);
                                                                        agentSharedManifestRepository.SubmitChanges();
                                                                    }
                                                                }
                                                            }
                                                        }


                                                    }
                                                    #endregion


                                                    AgentSharedManifestService service = new AgentSharedManifestService(agentContext, manifestSL.DestinationAgentTenant);
                                                    AgentSharedManifestPM agentSharedPM = new AgentSharedManifestPM()
                                                    {

                                                        CreateDate = TenantServerConfigration.GetCurrentDateTime(manifestSL.DestinationAgentTenant),
                                                        Tenant = manifestSL.DestinationAgentTenant,
                                                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(manifestSL.DestinationAgentTenant),
                                                        UpdatedByUserId = systemContact!=null ? systemContact.Id:null,
                                                        AgentReference = manifestSL.ShipmentNumber,
                                                        Master = manifestSL.TransportModeId == "A" ? manifestSL.LongMaster : manifestSL.MasterNumber,
                                                        ManifestXML = manifestXML,
                                                        ShipmentTypeId = manifestSL.ShipmentTypeId,
                                                        StatusCode = "WAIT",
                                                        TransportModeId = manifestSL.TransportModeId,
                                                        DirectionId = manifestSL.DirectionId,
                                                        FromPortId = fromPort!=null ? fromPort.Id :null,
                                                        ToPortId = toPort!=null ? toPort.Id:null,
                                                        GrossWeight = manifestSL.GrossWeight,
                                                        ChargeableWeight = manifestSL.ChargeableWeight,
                                                        TEU = manifestSL.TEU,
                                                        PackagesQuantity = manifestSL.PackagesQuantity,
                                                        AgentId = destinationAgent != null ? destinationAgent.Id : null,
                                                        ShipmentLevelCode = manifestSL.ShipmentLevelCode,
                                                    };

                                                    if (!string.IsNullOrEmpty(manifestSL.AgentSharedManifestId)) agentSharedPM.Id = manifestSL.AgentSharedManifestId;
                                                    else agentSharedPM.Id = IdCounter.GetNumber("AgentSharedManifest", manifestSL.DestinationAgentTenant);


                                                        service.Create(agentSharedPM);
                                                }
                                                else
                                                {
                                                    string url = LogitudeSettings.LogitudeURL + "/EntityExternalUpdate.aspx";
                                                    WebRequest request = WebRequest.Create(url);       
                                                    request.Method = "POST";
                                                    byte[] byteArray = datainByte;
                                                    // Set the ContentType property of the WebRequest.
                                                    request.ContentType = "application/x-www-form-urlencoded";
                                                    // Set the ContentLength property of the WebRequest.
                                                    request.ContentLength = byteArray.Length;
                                                    // Get the request stream.
                                                    Stream dataStream = request.GetRequestStream();
                                                    // Write the data to the request stream.
                                                    dataStream.Write(byteArray, 0, byteArray.Length);
                                                    // Close the Stream object.
                                                    dataStream.Close();
                                                    // Get the response.
                                                    WebResponse webResponse = request.GetResponse();
                                                    // Display the status.
                                                    Console.WriteLine(((HttpWebResponse)webResponse).StatusDescription);
                                                    // Get the stream containing content returned by the server.
                                                    dataStream = webResponse.GetResponseStream();
                                                    // Open the stream using a StreamReader for easy access.
                                                    StreamReader reader = new StreamReader(dataStream);
                                                    // Read the content.
                                                    string responseFromServer = reader.ReadToEnd();
                                                    // Display the content.
                                                    Console.WriteLine(responseFromServer);
                                                    // Clean up the streams.
                                                    reader.Close();
                                                    dataStream.Close();
                                                    webResponse.Close();
                                                }
                                            }


                                            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
                                            //waitingCommLog.Logs = logs;
                                            commLog.CommunicationStatusTypeCode = "D";
                                            commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                            commLog.DoneDateUTC = DateTime.UtcNow;
                                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                                            commLogrepository.Update(commLog);
                                            commLogrepository.SubmitChanges();



                                        }

                                        queueservice.Complete();
                                        LogDoneItemInMemory();

                                    }
                                }
                                else
                                {
                                    if (response.RetryNumber <= 11)
                                    {
                                        if (response.RetryNumber < 3)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 1));
                                        }

                                        if (response.RetryNumber >= 3 && response.RetryNumber <= 5)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 5 && response.RetryNumber <= 10)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                                + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(3000);
                                        }
                                        if (response.RetryNumber == 11)
                                        {

                                            queueservice.Delay(new TimeSpan(0, 0, 2, 0));
                                            AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                            Thread.Sleep(10000);

                                        }
                                    }
                                    else
                                    {
                                        queueservice.Complete();
                                        AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at AgentsSharedLogistics worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "AgentsSharedLogistics worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }


        public override bool OnStart()
        {

            ConnectClient();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AgentsSharedLogistics";
         
            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        DbQueueService queueservice;
        string queueName = "AgentsSharedLogisticsQueue";
        public void ConnectClient()
        {
            try
            {
 

                queueservice = new DbQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

    }
}
