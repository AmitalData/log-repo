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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using WebFreight.Web.DataContracts;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityAMs;
using CommunicationWorkerRole.EntityMapping;

namespace CommunicationWorkerRole.Services.ImporterShipmentOrders
{
    public class ImporterShipmentOrdersService
    {
        private readonly DbQueueService queueService;
        private readonly QueueResponse queueResponse;
        private IWebFreightContext webFreightContext;
        private IGlobalContext objectContext;
        private ObjectFieldRepository objectFieldRepository;
        private ObjectTableRepository objectTableRepository;
        private APILogsRepository aPILogsRepository;
        private APILogsService apiLogsService;
        private SettingQuery settingQuery;
        private ShipmentOrderQueryService shipmentOrderQueryService;
        private ShipmentOrderAmMap shipmentOrderAmMap;

        private string shipmentOrderId;
        private int? tenant;
        private string messageId;
        private ObjectTable objectTable;
        private APILogsPM apiLog;
        private bool isNewLog;
        private string URI = "http://localhost:9996/api/";//"http://localhost:9996/api/";
        private string token;
        private APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
        {
            PrimaryKey = "121060db-b064-4ee3-a65f-b3e6c214e659",//"8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
            SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
        };


        public ImporterShipmentOrdersService(DbQueueService queueService, QueueResponse queueResponse)
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
            tenant = GetTenantValueFromQueueResponse(queueResponse);
            messageId = queueResponse.MessageId;
        }

        private void InitiallizeServices()
        {
            if (tenant == null) return;
            webFreightContext = WebFreightContext.GetContext(tenant.Value);
            objectContext = GlobalContext.GetContext();

            apiLogsService = new APILogsService(webFreightContext, tenant.Value);
            objectFieldRepository = new ObjectFieldRepository(tenant.Value);
            aPILogsRepository = new APILogsRepository(webFreightContext);
            objectTableRepository = new ObjectTableRepository(webFreightContext);
            settingQuery = new SettingQuery(new SettingRepository(objectContext));
            shipmentOrderQueryService = new ShipmentOrderQueryService(tenant.Value);
            shipmentOrderAmMap = new ShipmentOrderAmMap();
        }

        private void InitiallizeFields()
        {
            if (tenant == null) return;
            objectTable = objectTableRepository.GetObjectTableByName("ShipmentOrder", tenant.Value, true);
            URI = settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return null;
            if (!queueResponse.MessageValues.Keys.Contains("Tenant")) return null;

            string tenantString = queueResponse.MessageValues["Tenant"].ToString();
            if (string.IsNullOrEmpty(tenantString)) return null;

            return int.Parse(tenantString);
        }

        public void ExecuteQueue()
        {
            if (queueService == null || queueResponse == null || tenant == null || string.IsNullOrEmpty(shipmentOrderId)) return;
            BuildApiLog();
            GetToken();
            try
            {
                SendApiLog("Start Building Queues For ImporterShipmentOrders Controller");
                ShipmentOrderAM shipmentOrderAM = GetShipmentOrderAM();
                SendShipmentAM(shipmentOrderAM);
                queueService.Complete();
            }
            catch (Exception ex)
            {
                HandleQueueException(ex);
            }

        }

        private void SendShipmentAM(ShipmentOrderAM shipmentOrderAM)
        {
            using (var client = new HttpClient())
            {
                string ImporterShipmentsURI = URI + "ImporterShipmentOrders";
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", messageId);

                var serializedObject = JsonConvert.SerializeObject(shipmentOrderAM);
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var result = client.PostAsync(ImporterShipmentsURI, content);
                result.Wait();
                if (result.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    HandleRequestError(result);
                }
            }
        }

        private void HandleRequestError(Task<HttpResponseMessage> result)
        {
            APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            if (EXC == null)
            {
                return;   
            }
            var Failmsg = EXC.ErrorType + " Fail To Send Shipment Updates To Importer Tenant " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, tenant.Value, "F", queueResponse.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
        }

        private ShipmentOrderAM GetShipmentOrderAM()
        {
            var shipmentOrder = shipmentOrderQueryService.GetSinglePM(shipmentOrderId, tenant.Value);
            return shipmentOrderAmMap.GetForImporterShipmentOrders(tenant.Value, shipmentOrder);
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
            SendApiLog("Build Documents Queues");
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, tenant.Value, "F", queueResponse.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
        }

        private static string GetErrorMessage(Exception ex)
        {
            string errorMessage = ex.Message + Environment.NewLine;

            if (ex.InnerException != null) errorMessage = errorMessage + " (" + ex.InnerException.Message + ")" + Environment.NewLine;

            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
            return errorMessage;
        }

        private void SendApiLog(string subject)
        {
            if (!isNewLog) return;
            apiLog.Subject = subject;
            apiLog.BatchNumber = messageId;
            //apiLog.CustomerId = CustomerId;
            apiLogsService.Create(apiLog);
        }

        private void BuildApiLog()
        {
            isNewLog = true;
            APILogs apiLogs = aPILogsRepository.GetSingleAPILogsByCorrelationId(messageId, tenant.Value);
            if (apiLogs == null)
            {
                apiLog = SetApiLogDefaultFields(CreateNewApiLogInstance());
                return;
            }
            isNewLog = false;
            apiLog = SetApiLogDefaultFields(GetApiLogInstance(apiLogs));
        }

        private APILogsPM SetApiLogDefaultFields(APILogsPM aPILogsPM)
        {
            aPILogsPM.ObjectTableId = objectTable.Id;
            aPILogsPM.Tenant = tenant.Value;
            return aPILogsPM;
        }

        private APILogsPM GetApiLogInstance(APILogs apiLogs)
        {
            return new APILogsPM()
            {
                Id = apiLogs.Id,
                CorrelationId = apiLogs.CorrelationId,
                CreateDate = apiLogs.CreateDate,
                CreateDateUTC = apiLogs.CreateDateUTC,
                Direction = apiLogs.Direction,
                EntityId = apiLogs.EntityId,
                LastUpdateDate = apiLogs.LastUpdateDate,
                LastUpdateDateUTC = apiLogs.LastUpdateDateUTC,
                NumberOfRetries = apiLogs.NumberOfRetries++,
                ObjectTableId = apiLogs.ObjectTableId,
                ExpirationDate = apiLogs.ExpirationDate,
                Refrence = apiLogs.Refrence,
                Status = "I",
                Tenant = apiLogs.Tenant,
                QueueMessageMoreDetailsId = apiLogs.QueueMessageMoreDetailsId
            };
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
                QueueMessageMoreDetailsId = messageId
            };
        }
    }
}
