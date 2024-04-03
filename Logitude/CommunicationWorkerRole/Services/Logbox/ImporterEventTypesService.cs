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
    class ImporterEventTypesService
    {
        public string URI = "";
        private APILogsPM logPM;
        private readonly IWebFreightContext webFreightContext;
        private readonly int tenant;
        private readonly string correlationId;
        private readonly QueueResponse response;

        public ImporterEventTypesService(QueueResponse queueResponse)
        {
            tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            URI = CustomerTenantsURLService.Get();
            webFreightContext = WebFreightContext.GetContext(tenant);
            correlationId = queueResponse.MessageId;
            response = queueResponse;
        }
        public void Run(string eventTypeId, int tenant)
        {
            string token = APICredentialsAuthenticationService.Authenticate(URI);
            EventTypePM eventTypePM = GetEventTypePM(eventTypeId, tenant);
            logPM = GetLogPM("Send EventType To Importer By ImporterEventTypes Controller", eventTypePM != null ? eventTypePM.Code : eventTypeId);
            if (eventTypePM == null || eventTypePM.InActive)
            {
                APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "F", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "EventType with Id: " + eventTypeId + " is missing, or it is inactive.", LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypePM), null, null, "");
                return;
            }

            logPM.Refrence = eventTypePM.Code;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "In Progress", LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypePM), null, null, "");
            bool isEventTypeExist = IsEventTypeExistInForwarderSide(tenant, token, eventTypePM);
            using (var client = new HttpClient())
            {
                string importerEventTypesURI = URI + "ImporterEventTypes";
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", correlationId);
                EventTypeAM eventTypeAM = BuildEventTypeAM(eventTypePM);
                if (isEventTypeExist)
                {
                    UpdateEventTypeInForwarder(client, importerEventTypesURI, eventTypeAM);
                }
                else
                {
                    CreateEventTypeInForwarder(client, importerEventTypesURI, eventTypeAM);
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
                QueueType = "EventType",
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
                QueueType = "EventType",
                Subject = subject,
                Refrence = reference,
            };
        }

        private static EventTypePM GetEventTypePM(string eventTypeId, int tenant)
        {
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventTypePM = eventTypeQuery.GetSingleEventTypePMIncludeObjectTable(eventTypeId, tenant);
            return eventTypePM;
        }

        private bool IsEventTypeExistInForwarderSide(int tenant, string token, EventTypePM eventTypePM)
        {
            string GetURI = URI + "ImporterEventTypes/GetIfEventTypeExists?tenant=" + tenant + "&eventTypeCode=" + eventTypePM.Code + "&objectTableName=" + eventTypePM.ObjectTableName;
            bool isEventTypeExist = false;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", token);
                using (var apiResponse = client.GetAsync(GetURI))
                {
                    apiResponse.Wait();
                    isEventTypeExist = FillExistenceResult(apiResponse);
                }
            }
            return isEventTypeExist;
        }

        private bool FillExistenceResult(System.Threading.Tasks.Task<HttpResponseMessage> apiResponse)
        {
            if (!apiResponse.Result.IsSuccessStatusCode) MarkProcessAsFailed(apiResponse, " Faild Check Existenc EventType");

            string isEventTypeExistJsonResult = apiResponse.Result.Content.ReadAsStringAsync().Result;
            object result = JsonConvert.DeserializeObject(isEventTypeExistJsonResult);
            return result != null && (bool)result;
        }

        private static EventTypeAM BuildEventTypeAM(EventTypePM eventTypePM)
        {
            return new EventTypeAM()
            {
                Tenant = eventTypePM.Tenant,
                Code = eventTypePM.Code,
                EnglishName = eventTypePM.EnglishName,
                LocalName = eventTypePM.LocalName,
                ObjectTableName = eventTypePM.ObjectTableName,
                EntityStatusCode = eventTypePM.EntityStatusCode,
                IsFollowUp = eventTypePM.IsFollowUp,
                IsCustomerView = eventTypePM.IsCustomerView,
                IsAgentView = eventTypePM.IsAgentView,
                FollowUpEnglishName = eventTypePM.FollowUpEnglishName,
                FollowUpLocalName = eventTypePM.FollowUpLocalName,
                EventTrigger = eventTypePM.EventTrigger,
            };
        }

        private void UpdateEventTypeInForwarder(HttpClient client, string importerEventTypesURI, EventTypeAM eventTypeAM)
        {
            logPM.Subject = "Send updates Of EventTypes To Importer By ImporterEventTypes Controller";
            string msg = "Start Sending Updates Of EventType To Importer" + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypeAM), null, null, "");

            var serializedObject = JsonConvert.SerializeObject(eventTypeAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PutAsync(importerEventTypesURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "Updates Of EventType Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send EventType Updates To Importer Tenant " + DateTime.Now);
            }
        }

        private void CreateEventTypeInForwarder(HttpClient client, string importerEventTypesURI, EventTypeAM eventTypeAM)
        {
            logPM.Subject = "Send EventType To Importers By ImporterEventTypes Controller";
            string msg = "Start Sending EventType To Importers " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypeAM), null, null, "");
            var serializedObject = JsonConvert.SerializeObject(eventTypeAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PostAsync(importerEventTypesURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "EventType Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send EventType To Importer Tenant " + DateTime.Now);
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
                ExceptionHandler.HandleException(exception, DateTime.Now, tenant, "", "EventType Import worker role", "", null);
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
