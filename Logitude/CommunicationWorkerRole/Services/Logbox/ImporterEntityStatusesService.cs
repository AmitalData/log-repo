using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityAMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Net.Http;
using System.Text;

namespace CommunicationWorkerRole.Services.Logbox
{
    class ImporterEntityStatusesService
    {
        public string URI = "";
        private APILogsPM logPM;
        private readonly IWebFreightContext webFreightContext;
        private readonly int tenant;
        private readonly string correlationId;
        private readonly QueueResponse response;

        public ImporterEntityStatusesService(QueueResponse queueResponse)
        {
            tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            URI = CustomerTenantsURLService.Get();
            webFreightContext = WebFreightContext.GetContext(tenant);
            correlationId = queueResponse.MessageId;
            response = queueResponse;
        }
        public void Run(string entityStatusId, int tenant)
        {
            string token = APICredentialsAuthenticationService.Authenticate(URI);
            EntityStatusPM entityStatusPM = GetEntityStatusPM(entityStatusId, tenant);
            logPM = GetLogPM("Send EntityStatus To Importer By ImporterEntityStatuses Controller", entityStatusPM != null ? entityStatusPM.Code : entityStatusId);
            if (entityStatusPM == null || entityStatusPM.InActive)
            {
                APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "F", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "EntityStatus with Id: " + entityStatusId + " is missing, or it is inactive.", LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusPM), null, null, "");
                return;
            }

            logPM.Refrence = entityStatusPM.Code;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "In Progress", LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusPM), null, null, "");
            bool isEntityStatusExist = IsEntityStatusExistInForwarderSide(tenant, token, entityStatusPM);
            using (var client = new HttpClient())
            {
                string importerEntityStatusesURI = URI + "ImporterEntityStatuses";
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", correlationId);
                EntityStatusAM entityStatusAM = BuildEntityStatusAM(entityStatusPM);
                if (isEntityStatusExist)
                {
                    UpdateEntityStatusInForwarder(client, importerEntityStatusesURI, entityStatusAM);
                }
                else
                {
                    CreateEntityStatusInForwarder(client, importerEntityStatusesURI, entityStatusAM);
                }
            }
        }

        private APILogsPM GetLogPM(string subject, string reference)
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            if (Log != null)
            {
                return MapLogPMFromPoco(Log);
            }
            APILogsPM LogPM = GetNewLog(subject, reference);
            new APILogsService(webFreightContext, tenant).Create(LogPM);
            return LogPM;
        }

        private APILogsPM MapLogPMFromPoco(APILogs Log)
        {
            return new APILogsPM()
            {
                Id = Log.Id,
                CorrelationId = Log.CorrelationId,
                CreateDate = Log.CreateDate,
                CreateDateUTC = Log.CreateDateUTC,
                Direction = Log.Direction,
                EntityId = Log.EntityId,
                LastUpdateDate = Log.LastUpdateDate,
                LastUpdateDateUTC = Log.LastUpdateDateUTC,
                NumberOfRetries = Log.NumberOfRetries++,
                ObjectTableId = Log.ObjectTableId,
                ExpirationDate = Log.ExpirationDate,
                Refrence = Log.Refrence,
                Status = "I",
                Tenant = Log.Tenant,
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId,
                QueueType = "EntityStatus",
                Subject = Log.Subject,
            };
        }

        private APILogsPM GetNewLog(string subject, string reference)
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = correlationId,
                QueueType = "EntityStatus",
                Subject = subject,
                Refrence = reference,
            };
        }

        private static EntityStatusPM GetEntityStatusPM(string entityStatusId, int tenant)
        {
            EntityStatusQuery entityStatusQuery = new EntityStatusQuery(tenant);
            EntityStatusPM entityStatusPM = entityStatusQuery.GetSinglePM(entityStatusId, tenant);
            return entityStatusPM;
        }

        private bool IsEntityStatusExistInForwarderSide(int tenant, string token, EntityStatusPM entityStatusPM)
        {
            string GetURI = URI + "ImporterEntityStatuses/GetIfEntityStatusExists?tenant=" + tenant + "&entityStatusCode=" + entityStatusPM.Code + "&objectTableName=" + entityStatusPM.ObjectTableName;
            bool isEntityStatusExist = false;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", token);
                using (var apiResponse = client.GetAsync(GetURI))
                {
                    apiResponse.Wait();
                    isEntityStatusExist = FillExistenceResult(apiResponse);
                }
            }
            return isEntityStatusExist;
        }

        private bool FillExistenceResult(System.Threading.Tasks.Task<HttpResponseMessage> apiResponse)
        {
            if (!apiResponse.Result.IsSuccessStatusCode) MarkProcessAsFailed(apiResponse, " Faild Check Existenc EntityStatus");

            string isEntityStatusExistJsonResult = apiResponse.Result.Content.ReadAsStringAsync().Result;
            object result = JsonConvert.DeserializeObject(isEntityStatusExistJsonResult);
            return result != null && (bool)result;
        }

        private static EntityStatusAM BuildEntityStatusAM(EntityStatusPM entityStatusPM)
        {
            return new EntityStatusAM()
            {
                Tenant = entityStatusPM.Tenant,
                Code = entityStatusPM.Code,
                Name = entityStatusPM.Name,
                DisplayName = entityStatusPM.DisplayName,
                ObjectTableName = entityStatusPM.ObjectTableName,
                StatusWeight = entityStatusPM.StatusWeight,
                StatusLocalWeight = entityStatusPM.StatusLocalWeight,
                EntityStatusTypeCode = entityStatusPM.EntityStatusTypeCode,
                AllowPartial = entityStatusPM.AllowPartial,
                IsDigitalPortal = entityStatusPM.IsDigitalPortal,
            };
        }

        private void UpdateEntityStatusInForwarder(HttpClient client, string importerEntityStatusesURI, EntityStatusAM entityStatusAM)
        {
            logPM.Subject = "Send updates Of EntityStatuses To Importer By ImporterEntityStatuses Controller";
            string msg = "Start Sending Updates Of EntityStatus To Importer" + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusAM), null, null, "");

            var serializedObject = JsonConvert.SerializeObject(entityStatusAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PutAsync(importerEntityStatusesURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "Updates Of EntityStatus Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send EntityStatus Updates To Importer Tenant " + DateTime.Now);
            }
        }

        private void CreateEntityStatusInForwarder(HttpClient client, string importerEntityStatusesURI, EntityStatusAM entityStatusAM)
        {
            logPM.Subject = "Send EntityStatus To Importers By ImporterEntityStatuses Controller";
            string msg = "Start Sending EntityStatus To Importers " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusAM), null, null, "");
            var serializedObject = JsonConvert.SerializeObject(entityStatusAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PostAsync(importerEntityStatusesURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "EntityStatus Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send EntityStatus To Importer Tenant " + DateTime.Now);
            }
        }

        private void MarkProcessAsDone(System.Threading.Tasks.Task<HttpResponseMessage> result,string doneMsg)
        {
            string responseData;
            try
            {
                responseData = result.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception, DateTime.Now, tenant, "", "EntityStatus Import worker role", "", null);
                responseData = exception.Message;
            }
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "D", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, doneMsg, null, responseData, null, "");
        }
        
        private void MarkProcessAsFailed(System.Threading.Tasks.Task<HttpResponseMessage> result, string failMsg)
        {
            APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            if (EXC == null) 
                return;
            failMsg = EXC.ErrorType + failMsg;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, failMsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
        }
    }
}
