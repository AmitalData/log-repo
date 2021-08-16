using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityAMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Newtonsoft.Json;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Transactions;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Microsoft.ServiceBus.Messaging;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ForwarderExportShipmentsController : ApiController
    {

         

        public HttpResponseMessage Post(NewAExporterShipmentAM Shipment)
        {
            try
            {
                //SecurityUtility.AuthenticationOnTenant(Shipment.Tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(Shipment.Tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentrepository = new DocumentRepository(commonContext);
                ObjectTableRepository objecttableRep = new ObjectTableRepository(Shipment.Tenant);
                ObjectTable objectTable = null;

                objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);
                CustomerTenantAccessQuery CAQuery = new CustomerTenantAccessQuery(Shipment.Tenant);
                CustomerTenantAccessCardQuery CTACQuery = new CustomerTenantAccessCardQuery(Shipment.Tenant);
                var TenantAccess = CAQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(Shipment.Tenant, Shipment.ExporterTenant);
                CustomerTenantAccessCardPM card = CTACQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(TenantAccess.Id, Shipment.Tenant).Where(a => a.StatusTypeCode.ToUpper() != "IA").FirstOrDefault();
                //ShipmentQuery shipmentQuery = new ShipmentQuery(Shipment.Tenant);

                if (card != null)// InCase there is no card on the CustomerTenantAccess WI# 41867
                {
                    Shipment.Customer = new CodeProperties() { Code = card.CustomerCode };
                }
                HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(Shipment.Tenant);
                HybridPartnerPM CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(Shipment.Tenant);
                CommunicationLog commLog = new CommunicationLog();
                if (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner)
                {
                    List<QueueTask> tasks = new List<QueueTask>();
                    var data = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(Shipment);
                    string myAction = "NewExporterShipment";
                    if (Shipment.SendUpdatesToAgentEnabled)
                    {
                        myAction = "UpdateExporterShipment";
                    }

                    tasks.Add(new QueueTask() { Action = myAction, Parameters = new List<Logitude.Server.Tools.Parameter>() { new Logitude.Server.Tools.Parameter { Name = "ImporterShipment", Value = data } } });
                    var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                    Document document = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = ByteData.Length,
                        Tenant = Convert.ToInt32(Shipment.Tenant),
                        Id = IdCounter.GetNumber("Document", Shipment.Tenant),
                        HasFile = true,
                        Folder = "ExternalTasksQueue",
                    };
                    documentrepository.Add(document);
                    documentrepository.SubmitChanges();
                    commLog = new CommunicationLog()
                    {
                        Id = IdCounter.GetNumber("CommunicationLog", Shipment.Tenant),
                        LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Shipment.Tenant),
                        InOut = "O",
                        //EntityId = OceanInsightsRequest.Id,
                        ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                        Subject = myAction,
                        Tenant = Shipment.Tenant,
                        CommunicationLogTypeCode = "Q",
                        CommunicationStatusTypeCode = "W",
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(Shipment.Tenant),
                        DocumentId = document.Id,
                        CreateDateUTC = DateTime.UtcNow,
                        LastStatusDateUTC = DateTime.UtcNow,
                        QueueName = "externaltasksqueue" + Shipment.Tenant + 1,
                        Priority = 1,

                    };

                    communicationLogRepository.Add(commLog);
                    communicationLogRepository.SubmitChanges();
                    string filename = document.Id + "." + document.Extension;
                    string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = Shipment.Tenant,
                        FileSize = ByteData.Length,

                    };

                    storageservice.Write(ByteData, fileInfo);

                    if (!string.IsNullOrEmpty(commLog.QueueName))
                    {
                        try
                        {
                            Communications.UpdateCommunicationLogStatus(commLog.Id, Shipment.Tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), null);
                            SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, Shipment.Tenant);
                            Communications.UpdateCommunicationLogStatus(commLog.Id, Shipment.Tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  Forwarder Shipment " + DateTime.Now.ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex.Message;

                            if (!string.IsNullOrEmpty(ex.StackTrace))
                            {
                                errorMessage += Environment.NewLine + ex.StackTrace;
                            }

                            Communications.UpdateCommunicationLogStatus(commLog.Id, Shipment.Tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), errorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, commLog.Id);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);
                //BrokeredMessage message = new BrokeredMessage();

                //message.Properties["CommunicationLogId"] = communicationLogId;
                //message.Properties["Tenant"] = tenant;
                //QueueClient client = GetQueueClient(queueName);// StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
                //using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
                //{
                //    client.Send(message);
                //    scope.Complete();
                //}
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }

    }
}