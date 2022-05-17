using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.ShipmentsModel.Tools.ContainerTracking
{
    public class ContainerTrackingService
    {
        ICommonDataContext commonContext;

        public ShipmentContainerSimulator SimulateVizionApiContainerStatus(ShipmentContainerSimulator simulator)
        {
            VisionContainerStatus vizionContainerStatus = JsonConvert.DeserializeObject<VisionContainerStatus>(simulator.XmlString);
            var result = CallApi<object>("http://localhost:9996/", "api/ContainerTracking/PostVizionUpdateContainerStatus", vizionContainerStatus, Method.POST);
            simulator.Success = true;
            return simulator;
        }


        public void UpdateStatusFromVizion(HttpRequestMessage request, VisionContainerStatus containerStatus)
        {
            var tenant = 1;
            InitializeContext(tenant);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var document = AddDocument(tenant, request);
                var commLog = AddCommunicationLog(containerStatus, tenant, document);
                SendCommunicationLogMessage(commLog, tenant);
                scope.Complete();
            }
        }
        private Document AddDocument(int tenant, HttpRequestMessage request)
        {
            var commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            var byteData = request.Content.ReadAsByteArrayAsync().Result;
            Document document = CreateDocument(tenant, byteData);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = CreateBlobFile(document, byteData);
            storageservice.Write(byteData, fileInfo);
            return document;
        }
        private BlobFileInfo CreateBlobFile(Document document, byte[] byteData)
        {
            return new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = byteData.Length,

            };
        }
        private Document CreateDocument(int tenant, byte[] byteData)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "json",
                FileSize = byteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "ContainerTrackingStatus",
            };
        }
        private CommunicationLog AddCommunicationLog(VisionContainerStatus containerStatus, int tenant, Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);
            var commLog = CreateCommunicationLog(objectTable, tenant, containerStatus, document);
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }
        private CommunicationLog CreateCommunicationLog(ObjectTable objectTable, int tenant, VisionContainerStatus containerStatus, Document document)
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                InOut = "O",
                //EntityId = OceanInsightsRequest.Id,
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "Vizion Update Container Status",
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "VizionUpdateContainerStatus",
                Priority = 1,
                EntityReference = containerStatus.payload.container_id

            };
        }
        private void SendCommunicationLogMessage(CommunicationLog commLog, int tenant)
        {
            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), null);
                    SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  ImporterApprovalReceived " + DateTime.Now.ToString(), null);
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errorMessage += Environment.NewLine + ex.StackTrace;
                    }

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue ImporterApprovalReceived " + DateTime.Now.ToString(), errorMessage);

                }
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Vizion Update Container Status Fail", null, null);
            }
        }
        private void InitializeContext(int tenant)
        {
            commonContext = CommonDataContext.GetContext(tenant);
        }
        public T CallApi<T>(string BaseUrl, string url, object o, Method m, string token = null)
        {

            RestSharp.RestClient restClient = new RestSharp.RestClient(BaseUrl);
            RestRequest restRequest = new RestRequest(url, m) { RequestFormat = DataFormat.Json };
            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Token", token);
            }
            restRequest.AddJsonBody(o);
            var restResponse = restClient.ExecuteAsync<T>(restRequest).Result;
            if (restResponse.StatusCode == HttpStatusCode.OK || restResponse.StatusCode == HttpStatusCode.Accepted)
            {

                return restResponse.Data;
            }
            else if (restResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("URL NotFound");
            }
            else
            {
                JObject jObject = JObject.Parse(restResponse.Content);
                if (jObject["ErrorMessage"] != null)
                {
                    var ErrorMessage = jObject["ErrorMessage"].ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                    throw new Exception(ErrorMessage);
                }
                else
                {
                    var ErrorMessage = "Response Status Code: " + restResponse.StatusCode.ToString() + "\n" + jObject.ToString();
                    throw new Exception(ErrorMessage);
                }

            }


        }


    }
}
