using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
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
    class ImporterPortService
    {
        public string URI = "";
        private APILogsPM logPM;
        private readonly IWebFreightContext webFreightContext;
        private readonly int tenant;
        private readonly string correlationId;
        private readonly QueueResponse response;

        public ImporterPortService(QueueResponse queueResponse)
        {
            tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            URI = CustomerTenantsURLService.Get();
            webFreightContext = WebFreightContext.GetContext(tenant);
            correlationId = queueResponse.MessageId;
            response = queueResponse;
        }
        public void Run(string portId, int tenant)
        {
            string token = APICredentialsAuthenticationService.Authenticate(URI);
            PortPM portPM = GetPortPM(portId, tenant);
            logPM = GetLogPM("Send Port To Importer By ImporterPorts Controller", portPM != null ? portPM.Code : portId);
            if (portPM == null || portPM.InActive)
            {
                APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "F", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "Port with Id: " + portId + " is missing, or it is inactive.", LogitudeXmlSerializer.SerializeObjectToXmlString(portId), null, null, "");
                return;
            }

            logPM.Refrence = portPM.Code;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", logPM.NumberOfRetries + 1, DateTime.Now, DateTime.UtcNow, "In Progress", LogitudeXmlSerializer.SerializeObjectToXmlString(portId), null, null, "");
            bool IsPortExist = IsPortExistInForwarderSide(tenant, token, portPM);
            using (var client = new HttpClient())
            {
                string ImporterPortsURI = URI + "ImporterPorts";
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", correlationId);
                PortAM portAM = BuildPortAM(portPM);
                if (IsPortExist)
                {
                    UpdatePortInForwarder(client, ImporterPortsURI, portAM);
                }
                else
                {
                    CreatePortInForwarder(client, ImporterPortsURI, portAM);
                }
            }
        }

        private void CreatePortInForwarder(HttpClient client, string ImporterPortsURI, PortAM portAM)
        {
            logPM.Subject = "Send Port To Importers By ImporterPorts Controller";
            string msg = "Start Sending Port To Importers " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(portAM), null, null, "");
            var serializedObject = JsonConvert.SerializeObject(portAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PostAsync(ImporterPortsURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "Port Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send Port To Importer Tenant " + DateTime.Now);
            }
        }
        private void UpdatePortInForwarder(HttpClient client, string ImporterPortsURI, PortAM portAM)
        {
            logPM.Subject = "Send updates Of Ports To Importer By ImporterPorts Controller";
            string msg = "Start Sending Updates Of Port To Importer" + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(logPM.Id, tenant, "I", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(portAM), null, null, "");

            var serializedObject = JsonConvert.SerializeObject(portAM);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            var result = client.PutAsync(ImporterPortsURI, content);
            result.Wait();
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MarkProcessAsDone(result, "Updates Of Port Sent To Importer Successfully " + DateTime.Now);
            }
            else
            {
                MarkProcessAsFailed(result, " Faild To Send Port Updates To Importer Tenant " + DateTime.Now);
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
                ExceptionHandler.HandleException(exception, DateTime.Now, tenant, "", "Import worker role", "", null);
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

        private static PortAM BuildPortAM(PortPM portPM)
        {
            return new PortAM()
            {
                Tenant = portPM.Tenant,
                Code = portPM.Code,
                EnglishName = portPM.EnglishName,
                LocalName = portPM.LocalName,
                Notes = portPM.Notes,
                InActive = portPM.InActive,
                IsAir = portPM.IsAir,
                IsOcean = portPM.IsOcean,
                IsInland = portPM.IsInland,
                CountryCode = portPM.CountryCode,
                StateCode = portPM.StateCode,
                TimeZoneCode = portPM.PortTimeZoneCode,
            };
        }

        private bool IsPortExistInForwarderSide(int tenant, string token, PortPM portPM)
        {
            string GetURI = URI + "ImporterPorts/GetIfPortExists?tenant=" + tenant + "&portCode=" + portPM.Code + "&countryCode=" + portPM.CountryCode;
            bool IsPortExist = false;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", token);
                using (var apiResponse = client.GetAsync(GetURI))
                {
                    apiResponse.Wait();
                    if (apiResponse.Result.IsSuccessStatusCode)
                    {
                        IsPortExist = FillExistenceResult(IsPortExist, apiResponse);
                    }
                }
            }
            return IsPortExist;
        }

        private static bool FillExistenceResult(bool IsPortExist, System.Threading.Tasks.Task<HttpResponseMessage> apiResponse)
        {
            var IsPortExistJsonString = apiResponse.Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject(IsPortExistJsonString);
            if (result != null)
            {
                IsPortExist = (bool)result;
            }
            return IsPortExist;
        }

        private static PortPM GetPortPM(string portId, int tenant)
        {
            PortQuery portQuery = new PortQuery(tenant);
            PortPM portPM = portQuery.GetSinglePM(portId, tenant);
            return portPM;
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

        private static APILogsPM MapLogPMFromPoco(APILogs Log)
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
                QueueType = "Port",
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
                QueueType = "Port",
                Subject = subject,
                Refrence = reference,
            };
        }
    }
}
