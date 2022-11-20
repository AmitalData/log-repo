using CommunicationWorkerRole.Services.Logbox;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.InfrastructureModel;

namespace CommunicationWorkerRole.Services.SignUp
{
    public class QueueMessageCloudToLogboxSender
    {
        private static string signUpControllerURL = "SignUp";
        public static void Send(SignUpInfoClass signUpInfo)
        {
            string URI = CustomerTenantsURLService.Get();
            string Token = APICredentialsAuthenticationService.Authenticate(URI);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", Token);
                signUpInfo.IsCreateLogboxTenantFromCloudPassed = true;
                string serializedObject = JsonConvert.SerializeObject(signUpInfo);
                StringContent content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var result = client.PostAsync(URI + signUpControllerURL, content);

                result.Wait();
                BuildAPILogs(result, signUpInfo);
            }
        }

        private static void BuildAPILogs(Task<HttpResponseMessage> result, SignUpInfoClass signUpInfo)
        {
            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                BuildSuccededAPILog(result, signUpInfo);
            }
            else
            {
                BuildFailedAPILog(result, signUpInfo);
            }
        }

        private static void BuildSuccededAPILog(Task<HttpResponseMessage> result, SignUpInfoClass signUpInfo)
        {
            string responseData = result.Result.Content.ReadAsStringAsync().Result;
            APILogsPM aPILogsPM = GetNewLogPM(signUpInfo);
            const string doneStatusCode = "D";
            aPILogsPM.Status = doneStatusCode;
            CreateAPILog(aPILogsPM, signUpInfo, responseData);
        }

        private static void BuildFailedAPILog(Task<HttpResponseMessage> result, SignUpInfoClass signUpInfo)
        {
            APIException aPIException = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            APILogsPM aPILogsPM = GetNewLogPM(signUpInfo);
            const string failedStatusCode = "F";
            aPILogsPM.Status = failedStatusCode;
            string aPIExceptionErrorMessage = aPIException != null ? aPIException.ErrorMessage : ""; 
            CreateAPILog(aPILogsPM, signUpInfo, aPIExceptionErrorMessage);
        }

        private static APILogsPM GetNewLogPM(SignUpInfoClass signUpInfo)
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", signUpInfo.Tenant),
                CorrelationId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                Tenant = signUpInfo.Tenant
            };
        }

        private static void CreateAPILog(APILogsPM LogPM, SignUpInfoClass signUpInfo, string responseData)
        {
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(signUpInfo.Tenant);
            APILogsService apiLogsService = new APILogsService(webFreightContext, signUpInfo.Tenant);
            const string customerTenantAccessObjectTableName = "CustomerTenantAccess";
            string customerTenantAccessObjectTableId = GetObjectTableIdByName(customerTenantAccessObjectTableName, signUpInfo.Tenant);

            string message = "Create New Tenant From Cloud With Email: " + signUpInfo.Email;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))
            {
                LogPM.ObjectTableId = customerTenantAccessObjectTableId;
                LogPM.Subject = "Create New Tenant From Cloud";
                apiLogsService.Create(LogPM);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, signUpInfo.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, message, message, responseData, null, "");
                scope.Complete();
            };
        }

        private static string GetObjectTableIdByName(string objectTableName, int tenant)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objectTabelRepository.GetObjectTableByName(objectTableName, tenant, true);
            if (objecttable == null) return "";

            return objecttable.Id;
        }
    }
}
