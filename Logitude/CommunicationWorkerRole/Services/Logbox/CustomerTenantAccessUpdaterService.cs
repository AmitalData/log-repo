using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class CustomerTenantAccessRequestUpdaterService
    {
        public static void Update(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs)
        {
            customerTenantAccessRequestUpdaterArgs.LogPM = GetLogPM(customerTenantAccessRequestUpdaterArgs);
            Task<HttpResponseMessage> result = PutCustomerTenantAccesses(customerTenantAccessRequestUpdaterArgs);
            result.Wait();

            if (result.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                RequestsUpdatedFailed(customerTenantAccessRequestUpdaterArgs, result);
            }

            RequestsUpdatedSuccessfully(customerTenantAccessRequestUpdaterArgs, result);

        }

        private static APILogsPM GetLogPM(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs)
        {
            APILogsPM logPM = customerTenantAccessRequestUpdaterArgs.LogPM;
            logPM.Subject = "Start To Update All Forwarder Requests By CustomerTenantAccess Controller";
            if (customerTenantAccessRequestUpdaterArgs.IsNewLog) { customerTenantAccessRequestUpdaterArgs.ApiLogsService.Create(logPM); }

            return logPM;
        }

        private static Task<HttpResponseMessage> PutCustomerTenantAccesses(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs)
        {
            string serializedCustomerTenantAccessUpdaterObject = GetSerializedCustomerTenantAccessUpdaterObject(customerTenantAccessRequestUpdaterArgs);
            var msg = "Start Updating All Forwarder Requests" + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessRequestUpdaterArgs.LogPM.Id, customerTenantAccessRequestUpdaterArgs.Tenant, customerTenantAccessRequestUpdaterArgs.LogPM.Status, customerTenantAccessRequestUpdaterArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, serializedCustomerTenantAccessUpdaterObject, null, null, "");
            var content = new StringContent(serializedCustomerTenantAccessUpdaterObject, Encoding.UTF8, "application/json");
            var result = customerTenantAccessRequestUpdaterArgs.Client.PutAsync(customerTenantAccessRequestUpdaterArgs.URI + "CustomerTenantAccessExtended", content);
            return result;
        }

        private static string GetSerializedCustomerTenantAccessUpdaterObject(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs)
        {
            string newIsPrivateLabelCustomerFieldValue = customerTenantAccessRequestUpdaterArgs.NewIsPrivateLabelCustomerFieldValue;
            string newIsActiveTenantFieldValue = customerTenantAccessRequestUpdaterArgs.NewIsActiveTenantFieldValue;
            string isPassedTrialEndDateFieldValue = customerTenantAccessRequestUpdaterArgs.IsPassedTrialEndDateFieldValue;

            bool? isPrivateLabel = !string.IsNullOrEmpty(newIsPrivateLabelCustomerFieldValue) ? bool.Parse(newIsPrivateLabelCustomerFieldValue) : (bool?)null;
            bool? isActiveTenant = !string.IsNullOrEmpty(newIsActiveTenantFieldValue) ? bool.Parse(newIsActiveTenantFieldValue) : (bool?)null;
            bool? isPassedTrialEndDate = !string.IsNullOrEmpty(isPassedTrialEndDateFieldValue) ? bool.Parse(isPassedTrialEndDateFieldValue) : (bool?)null;

            CustomerTenantAccessUpdaterAM customerTenantAccessUpdaterAM = new CustomerTenantAccessUpdaterAM()
            {
                Tenant = 0,
                CustomerTenant = customerTenantAccessRequestUpdaterArgs.Tenant,
                IsPrivateLabel = isPrivateLabel,
                IsActive = isActiveTenant,
                IsPassedTrialEndDate = isPassedTrialEndDate,
            };

            string serializedCustomerTenantAccessUpdaterObject = JsonConvert.SerializeObject(customerTenantAccessUpdaterAM);
            return serializedCustomerTenantAccessUpdaterObject;
        }

        private static void RequestsUpdatedFailed(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs, Task<HttpResponseMessage> result)
        {
            APIException aPIException = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            if (aPIException == null) return;

            const string failedStatusCode = "F";
            var Failmsg = aPIException.ErrorType + " Fail To Update All Forwarder Requests" + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessRequestUpdaterArgs.LogPM.Id, customerTenantAccessRequestUpdaterArgs.Tenant, failedStatusCode, customerTenantAccessRequestUpdaterArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(aPIException), null, "");
            throw new Exception(aPIException.ErrorType, new Exception(aPIException.ErrorMessage));
        }

        private static void RequestsUpdatedSuccessfully(CustomerTenantAccessRequestUpdaterArgs customerTenantAccessRequestUpdaterArgs, Task<HttpResponseMessage> result)
        {
            const string doneStatusCode = "D";
            customerTenantAccessRequestUpdaterArgs.Queue.Complete();
            customerTenantAccessRequestUpdaterArgs.LogPM.Status = doneStatusCode;
            var ResponseData = result.Result.Content.ReadAsStringAsync().Result;
            var Donemsg = "All Forwarder Requests Updated Successfully " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessRequestUpdaterArgs.LogPM.Id, customerTenantAccessRequestUpdaterArgs.Tenant, doneStatusCode, customerTenantAccessRequestUpdaterArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
        }
    }

    public class CustomerTenantAccessRequestUpdaterArgs
    {
        public QueueResponse Response { get; set; }
        public int Tenant { get; set; }
        public string NewIsPrivateLabelCustomerFieldValue { get; set; }
        public string NewIsActiveTenantFieldValue { get; set; }
        public string IsPassedTrialEndDateFieldValue { get; set; }
        public APILogsService ApiLogsService { get; set; }
        public bool IsNewLog { get; set; }
        public APILogsPM LogPM { get; set; }
        public HttpClient Client { get; set; }
        public IQueueService Queue { get; set; }
        public string URI { get; set; }
    }
}
