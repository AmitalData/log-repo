using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityAMs;

namespace CommunicationWorkerRole.Services.ShipmentOrderDocuments
{
    public class ShipmentOrderDocumentsService
    {
        private readonly DbQueueService queueService;
        private readonly QueueResponse queueResponse;

        private IWebFreightContext webFreightContext;
        private IGlobalContext objectContext;
        private ObjectTableRepository objectTableRepository;
        private DocumentsFilingQuery documentsFilingQuery;
        private APILogsService apiLogsService;
        private APILogsQuery aPILogsQuery;
        private SettingQuery settingQuery;
        private string shipmentOrderId;
        private string documentFilingId;
        private int? tenant;
        private string messageId;
        private APILogsPM apiLog;
        private ObjectTable objectTable;
        private ShipmentOrderDocumentsFilingAMMap shipmentOrderDocumentsFilingAMMap;

        private string URI = "";//"http://localhost:9996/api/";
        private string token;
        private readonly APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
        {
            PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",//"e2f5a308-475f-478d-a732-7ee9d9a0239e",
            SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
        };

        public ShipmentOrderDocumentsService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            SetQueueResponseFields();
            InitiallizeServices();
            InitiallizeFields();
        }

        private void SetQueueResponseFields()
        {
            if (queueService == null || queueResponse == null) return;
            shipmentOrderId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("ShipmentOrderId") ? queueResponse.MessageValues["ShipmentOrderId"].ToString() : "";
            documentFilingId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("DocumentFilingId") ? queueResponse.MessageValues["DocumentFilingId"].ToString() : "";
            tenant = GetTenantValueFromQueueResponse(queueResponse);
            messageId = queueResponse.MessageId;
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return null;
            if (!queueResponse.MessageValues.Keys.Contains("Tenant")) return null;

            string tenantString = queueResponse.MessageValues["Tenant"].ToString();
            if (string.IsNullOrEmpty(tenantString)) return null;

            return int.Parse(tenantString);
        }

        private void InitiallizeServices()
        {
            if (tenant == null) return;
            webFreightContext = WebFreightContext.GetContext(tenant.Value);
            objectContext = GlobalContext.GetContext();

            objectTableRepository = new ObjectTableRepository(webFreightContext);
            documentsFilingQuery = new DocumentsFilingQuery(tenant.Value);
            apiLogsService = new APILogsService(webFreightContext, tenant.Value);
            aPILogsQuery = new APILogsQuery(tenant.Value);
            settingQuery = new SettingQuery(new SettingRepository(objectContext));
            shipmentOrderDocumentsFilingAMMap = new ShipmentOrderDocumentsFilingAMMap(tenant.Value, objectTableRepository);
        }

        private void InitiallizeFields()
        {
            if (tenant == null) return;
            objectTable = objectTableRepository.GetObjectTableByName("DocumentsFiling", tenant.Value, true);
            URI = settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }


        public void ExecuteQueue()
        {
            if (queueService == null || queueResponse == null || tenant == null || string.IsNullOrEmpty(shipmentOrderId)) return;
            GetApiLog();
            GetToken();
            try
            {
                DocumentsFilingAM documentFilingAM = GetDocumentsFilingAM();
                SendDocumentsFilingAM(documentFilingAM);
                queueService.Complete();
            }
            catch (Exception ex)
            {
                HandleQueueException(ex);
            }

        }


        private DocumentsFilingAM GetDocumentsFilingAM()
        {
            var documentsFiling = documentsFilingQuery.GetSinglePM(documentFilingId, tenant.Value);
            return shipmentOrderDocumentsFilingAMMap.Map(documentsFiling, shipmentOrderId);
        }
        private void SendDocumentsFilingAM(DocumentsFilingAM documentFilingAM)
        {
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Sending Shipment Order Document" + DateTime.Now, TrimXmlString(documentFilingAM), null, null, "");
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", messageId);

                var serializedObject = JsonConvert.SerializeObject(documentFilingAM);
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                string uri = URI + "ImporterShipmentOrderDocuments";
                var result = client.PostAsync(uri, content);
                result.Wait();
                if (result.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    HandleRequestError(result);
                }
                var responseData = result.Result.Content.ReadAsStringAsync().Result;
                var msg = "New Document Sent To Shipment Order Importer Successfully " + DateTime.Now;
                APILogsUtility.UpdateAPILogStatus(apiLog.Id, tenant.Value, "D", queueResponse.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, null, responseData, null, "");
            }
        }

        private static string TrimXmlString(DocumentsFilingAM documentFilingAM)
        {
            var objectString = LogitudeXmlSerializer.SerializeObjectToXmlString(documentFilingAM);
            int start = objectString.IndexOf("<FileData>");
            int end = objectString.IndexOf("</FileData>", start);
            string result = objectString.Substring(start + 10, end - start - 10);
            return objectString.Replace(result, result.Substring(0, 10));
        }

        private void HandleRequestError(Task<HttpResponseMessage> result)
        {
            APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            if (EXC == null)
            {
                return;
            }
            var Failmsg = EXC.ErrorType + " Fail To Send Document To Shipment Order Importer " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, tenant.Value, "F", queueResponse.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
        }




        private void HandleQueueException(Exception ex)
        {
            ExceptionHandler.HandleException(ex, DateTime.Now, tenant.Value, "", "WorkerRole", "", null);

            var Failmsg = ex.Message + " " + DateTime.Now;
            string errorMessage = GetErrorMessage(ex);

            if (queueResponse.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }

            if (queueResponse.RetryNumber > 1 && queueResponse.RetryNumber <= 2)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 10));
            }

            if (queueResponse.RetryNumber < 3)
            {
                return;
            }

            queueService.CompleteAsFailed();
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, tenant.Value, "F", queueResponse.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
        }


        private static string GetErrorMessage(Exception ex)
        {
            string errorMessage = ex.Message + Environment.NewLine;

            if (ex.InnerException != null) errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
            return errorMessage;
        }

        private void GetApiLog()
        {
            apiLog = aPILogsQuery.GetSingleByCorrelationIdAndTenant(messageId, tenant.Value);
            if (apiLog != null) return;
            apiLog = CreateNewApiLogInstance();
            apiLogsService.Create(apiLog);
        }

        private void GetToken()
        {
            var client = new HttpClient();
            string AuthURI = URI + "APIAuthentication";
            var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PostAsync(AuthURI, content);
            result.Wait();
            var tempUser = result.Result.Content.ReadAsStringAsync().Result;
            ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
            this.token = User.Token;
        }


        private APILogsPM CreateNewApiLogInstance()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant.Value),
                CorrelationId = messageId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = messageId,
                ObjectTableId = objectTable.Id,
                Tenant = tenant.Value,
                BatchNumber = messageId,
                Subject = "Start Building Queues For ImporterShipmentOrderDocuments Controller"
            };
        }

    }
}
