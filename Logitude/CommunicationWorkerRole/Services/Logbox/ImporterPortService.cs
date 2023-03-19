using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    class ImporterPortService
    {
        public string URI = "";
        private APILogsPM LogPM;
        private IWebFreightContext webFreightContext;
        private int tenant;
        private string correlationId;
        APILogsService apiLogsService;
        QueueResponse Response;
        private bool IsNewLog = false;

        public ImporterPortService(QueueResponse queueResponse)
        {
            this.tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            URI = CustomerTenantsURLService.Get();
            webFreightContext = WebFreightContext.GetContext(tenant);
            correlationId = queueResponse.MessageId;
            this.Response = queueResponse;
        }
        public void Run(string portId, int tenant)
        {
            //Authenticate
            string token = APICredentialsAuthenticationService.Authenticate(URI);
            //APILOG
            LogPM = GetLogPM();

            apiLogsService = new APILogsService(webFreightContext, tenant);
            PortQuery portQuery = new PortQuery(tenant);
            PortPM portPM = portQuery.GetSinglePM(portId, tenant);

            LogPM.Refrence = portPM.Code;

            //APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, "Start Sending Port", LogitudeXmlSerializer.SerializeObjectToXmlString(port), null, null, "");
            //else
            //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(port), null, null, "");

            if (portPM == null) return;
            var GetURI = URI + "ImporterPorts/GetIfPortExists?tenant=" + tenant + "&portCode=" + portPM.Code + "&countryCode=" + portPM.CountryCode;
            bool IsPortExist = false;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", token);
                using (var apiresponse = client.GetAsync(GetURI))
                {
                    apiresponse.Wait();
                    if (apiresponse.Result.IsSuccessStatusCode)
                    {
                        var IsPortExistJsonString = apiresponse.Result.Content.ReadAsStringAsync().Result;
                        var tempResult = JsonConvert.DeserializeObject(IsPortExistJsonString);
                        if (tempResult != null)
                        {
                            IsPortExist = (bool)tempResult;
                        }
                    }
                }
            }
            using (var client = new HttpClient())
            {
                string ImporterPortsURI = URI + "ImporterPorts";
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("CorrelationId", correlationId);
                if (IsPortExist)
                {
                    PortAM portAM = new PortAM()
                    {
                        Id = portPM.Id,
                        Code = portPM.Code,
                        CountryCode = portPM.CountryCode,
                        StateCode = portPM.StateCode,
                        PortTimeZoneCode = portPM.PortTimeZoneCode,
                        EnglishName = portPM.EnglishName,
                        LocalName = portPM.LocalName,
                    };
                    LogPM.Subject = "Send updates Of Ports To Importer By ImporterPorts Controller";
                    if (IsNewLog)
                    {
                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)Response.MessageValues);
                        LogPM.QueueType = "Port";
                        apiLogsService.Create(LogPM);
                    }
                    var msg = "Start Sending Updates Of Port To Importer" + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(portAM), null, null, "");

                    var serializedObject = JsonConvert.SerializeObject(portAM);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = client.PutAsync(ImporterPortsURI, content);
                    result.Wait();
                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        try
                        {
                            var responseData = result.Result.Content.ReadAsStringAsync().Result;
                            var Donemsg = "Updates Of Port Sent To Importer Successfully " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, responseData, null, "");
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "Import worker role, Updates Of Port", "", null);
                            string errorMessage = ex.Message + Environment.NewLine;

                            if (ex.InnerException != null)
                            {
                                errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                            }
                            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                            throw new Exception(ex.Message, new Exception(errorMessage));
                        }
                    }
                    else
                    {
                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                        if (EXC != null)
                        {
                            var Failmsg = EXC.ErrorType + " Fail To Send Shipment Updates To Importer Tenant " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                        }
                    }
                }
                else
                {
                    PortAM portAM = new PortAM()
                    {
                        Id = portPM.Id,
                        Code = portPM.Code,
                        CountryCode = portPM.CountryCode,
                        StateCode = portPM.StateCode,
                        PortTimeZoneCode = portPM.PortTimeZoneCode,
                        EnglishName = portPM.EnglishName,
                        LocalName = portPM.LocalName,
                    };
                    LogPM.Subject = "Send Port To Importers By ImporterPorts Controller";
                    if (IsNewLog)
                    {
                        LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)Response.MessageValues);
                        LogPM.QueueType = "Port";
                        apiLogsService.Create(LogPM);
                    }
                    var msg = "Start Sending Port To Importers " + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, LogPM.Status, Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(portAM), null, null, "");
                    var serializedObject = JsonConvert.SerializeObject(portAM);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = client.PostAsync(ImporterPortsURI, content);
                    result.Wait();

                    if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var temp = result.Result.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var Donemsg = "Port Sent To Importer Successfully " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, temp, null, "");
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "Import worker role, Update CustomerShipmentNumber ", "", null);
                            string errorMessage = ex.Message + Environment.NewLine;

                            if (ex.InnerException != null)
                            {
                                errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                            }

                            errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                            throw new Exception(ex.Message, new Exception(errorMessage));
                        }
                    }
                    else
                    {
                        APIException EXC = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
                        var Failmsg = EXC.ErrorType + " Faild To Send Updates To Importer Tenant " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                        if (EXC != null)
                        {
                            throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                        }
                    }
                }
            }
        }
        private APILogsPM GetLogPM()
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            IsNewLog = false;
            APILogsPM LogPM;
            if (Log == null)
            {
                IsNewLog = true;
                LogPM = CreateNewLog();
            }

            IsNewLog = false;
            LogPM = UpdateLog(Log);
            if (IsNewLog)
            {
                //LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                LogPM.QueueType = "Port";
                apiLogsService.Create(LogPM);
            }
            return LogPM;
        }

        private static APILogsPM UpdateLog(APILogs Log)
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
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId
            };
        }

        private APILogsPM CreateNewLog()
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
                QueueMessageMoreDetailsId = correlationId
            };
        }
    }
}
