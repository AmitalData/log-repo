using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;



using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityAMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;

using Simplog.Data.CommonDataModel;

using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;

using Logitude.BL.CommonDataModel.Tools.EntityService;

using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Newtonsoft.Json;
using Logitude.Server.Tools.QueueService;

using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Transactions;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Microsoft.ServiceBus.Messaging;
using Logitude.SystemLogs;




namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ImporterDepositionController : ApiController
    {
        public HttpResponseMessage PostImporterDeposition(ImporterDepositionAM importerDepositionAM)
        {
            try 
            {
				using (TransactionScope scope = TransactionFactory.GetTransaction())
				{
					string token = HttpContext.Current.Request.Headers["Token"];
					AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
					SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

					ImporterDepositionHelper importerDepositionHelper = new ImporterDepositionHelper();
					string logId = importerDepositionHelper.AddAPILogs(importerDepositionAM,null, importerDepositionAM.CustomerTenant);
					importerDepositionHelper.StartImporterDeposition(importerDepositionAM);
					var msg = "Importer Deposition Send to cloud Successfully";
					APILogsUtility.UpdateAPILogStatus(logId, importerDepositionAM.CustomerTenant, "D", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(importerDepositionAM), null, null, "");

					scope.Complete();
					return Request.CreateResponse(HttpStatusCode.OK, msg);
				}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostSendVDCStatusToUNF(ShipmentAdditionalCloudDataAM Data)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnTenant(Data.Tenant);
                    ICommonDataContext commonContext = CommonDataContext.GetContext(Data.Tenant);
                    CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                    DocumentRepository documentrepository = new DocumentRepository(commonContext);
                    ObjectTableRepository objecttableRep = new ObjectTableRepository(Data.Tenant);
                    ObjectTable objectTable = null;

                    objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);

                    List<QueueTask> tasks = new List<QueueTask>();
                    tasks.Add(new QueueTask()
                    {
                        Action = "StatusUpdate",
                        Parameters = new List<Logitude.Server.Tools.Parameter>() {
                new Logitude.Server.Tools.Parameter { Name = "ShipmentNumber", Value = Data.ShipmentNumber},
                new Logitude.Server.Tools.Parameter { Name = "Code", Value = Data.Code},
                new Logitude.Server.Tools.Parameter { Name = "Date", Value = Data.Date},
                new Logitude.Server.Tools.Parameter { Name = "Time", Value = Data.Time},
                new Logitude.Server.Tools.Parameter { Name = "Remarks", Value = Data.Remarks},
                new Logitude.Server.Tools.Parameter { Name = "Direction", Value = Data.Direction}
                }
                    });
                    var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                    Document document = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = ByteData.Length,
                        Tenant = Convert.ToInt32(Data.Tenant),
                        Id = IdCounter.GetNumber("Document", Data.Tenant),
                        HasFile = true,
                        Folder = "ExternalTasksQueue",
                    };
                    documentrepository.Add(document);
                    documentrepository.SubmitChanges();
                    var commLog = new CommunicationLog()
                    {
                        Id = IdCounter.GetNumber("CommunicationLog", Data.Tenant),
                        LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Data.Tenant),
                        InOut = "O",
                        ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                        Subject = "Status Update",
                        Tenant = Data.Tenant,
                        CommunicationLogTypeCode = "Q",
                        CommunicationStatusTypeCode = "W",
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(Data.Tenant),
                        DocumentId = document.Id,
                        CreateDateUTC = DateTime.UtcNow,
                        LastStatusDateUTC = DateTime.UtcNow,
                        QueueName = "externaltasksqueue" + Data.Tenant + 1,
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
                        Tenant = Data.Tenant,
                        FileSize = ByteData.Length,

                    };

                    storageservice.Write(ByteData, fileInfo);

                    if (!string.IsNullOrEmpty(commLog.QueueName))
                    {
                        try
                        {
                            Communications.UpdateCommunicationLogStatus(commLog.Id, Data.Tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue ImporterDeposition" + DateTime.Now.ToString(), null);
                            SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, Data.Tenant);
                            Communications.UpdateCommunicationLogStatus(commLog.Id, Data.Tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  ImporterDeposition" + DateTime.Now.ToString(), null);

                        }
                        catch (Exception ex)
                        {
                            string errorMessage = ex.Message;

                            if (!string.IsNullOrEmpty(ex.StackTrace))
                            {
                                errorMessage += Environment.NewLine + ex.StackTrace;
                            }

                            Communications.UpdateCommunicationLogStatus(commLog.Id, Data.Tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue ImporterDeposition" + DateTime.Now.ToString(), errorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

                        }
                    }
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
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
       
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }
    }
}