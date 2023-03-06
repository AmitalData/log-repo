using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class CustomerTenantAccessCreatorService
    {
        public static void Create(CustomerTenantAccessCreatorArgs customerTenantAccessCreatorArgs)
        {
            customerTenantAccessCreatorArgs.LogPM = GetLogPM(customerTenantAccessCreatorArgs);
            Task<HttpResponseMessage> result = PostCustomerTenantAccess(customerTenantAccessCreatorArgs);
            result.Wait();

            if (result.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                RequestCreatedFailed(customerTenantAccessCreatorArgs, result);
            }
            RequestCreatedSuccessfully(customerTenantAccessCreatorArgs, result);

        }

        private static Task<HttpResponseMessage> PostCustomerTenantAccess(CustomerTenantAccessCreatorArgs customerTenantAccessCreatorArgs)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(customerTenantAccessCreatorArgs.Tenant);
            CustomerTenantAccessRequestQuery customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessCreatorArgs.Tenant);
            var customerTenantAccessRequest = customerTenantAccessRequestQuery.GetSinglePM(customerTenantAccessCreatorArgs.RequestId, customerTenantAccessCreatorArgs.Tenant);
            TenantQuery tenantQuery = new TenantQuery(customerTenantAccessCreatorArgs.Tenant);
            var Tenant = tenantQuery.GetSinglePMByCustomerId(customerTenantAccessCreatorArgs.Tenant);
            HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
            HybridPartner Partner = hybridPartnerRepository.GetSingleHybridPartner(customerTenantAccessRequest.ForwarderId);

            CustomerTenantAccessAM customerTenantAccessAM = new CustomerTenantAccessAM()
            {
                Tenant = Partner.PartnerTenant,
                CustomerTenant = customerTenantAccessRequest.Tenant,
                CompanyName = Tenant.Company,
                CompanyEmail = Tenant.Email,
                CompanyVat = !string.IsNullOrEmpty(Tenant.VatNumber) ? Tenant.VatNumber.Trim() : Tenant.VatNumber,
                ContactName = Tenant.CustomerName,
                ContactMobile = Tenant.CustomerMobile,
                ContactPhone = Tenant.CustomerPhone,
                IsPrivateLabelCustomer = false,
                StockTypeCode = Tenant.StockTypeCode,
            };

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalTenantRepository globaltenantRep = new GlobalTenantRepository();
                GlobalTenant Globaltenant = globaltenantRep.GetGlobalTenantsByTenant(customerTenantAccessAM.CustomerTenant);
                if (Globaltenant != null && !string.IsNullOrEmpty(Globaltenant.PrivateLabelId))
                {
                    FillPrivateLabelCustomerDetails(customerTenantAccessAM, Globaltenant.PrivateLabelId);
                }
                scope.Complete();
            }

            var serializedCustomerTenantAccessCreatorObject = JsonConvert.SerializeObject(customerTenantAccessAM);

            var msg = "Start Sending Request To Forwarder Tenant " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessCreatorArgs.LogPM.Id, customerTenantAccessCreatorArgs.Tenant, customerTenantAccessCreatorArgs.LogPM.Status, customerTenantAccessCreatorArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(customerTenantAccessAM), null, null, "");
            var content = new StringContent(serializedCustomerTenantAccessCreatorObject, Encoding.UTF8, "application/json");
            var result = customerTenantAccessCreatorArgs.Client.PostAsync(customerTenantAccessCreatorArgs.URI + "CustomerTenantAccess", content);
            return result;
        }

        private static void FillPrivateLabelCustomerDetails(CustomerTenantAccessAM customerTenantAccessAM, string privateLabelId)
        {
            customerTenantAccessAM.IsPrivateLabelCustomer = true;
            try
            {
                TenantManagmentPrivateLabelsRepository tenantManagmentPrivateLabelsRepository = new TenantManagmentPrivateLabelsRepository();
                TenantManagmentPrivateLabels tenantManagmentPrivateLabels = tenantManagmentPrivateLabelsRepository.GetSingleTenantManagmentPrivateLabels(privateLabelId);
                customerTenantAccessAM.PrivateLabelName = tenantManagmentPrivateLabels.PrivateLabelName;
            }catch(Exception exception)
            {
                customerTenantAccessAM.PrivateLabelName = "Private";
            }
        }

        private static APILogsPM GetLogPM(CustomerTenantAccessCreatorArgs customerTenantAccessCreatorArgs)
        {
            APILogsPM logPM = customerTenantAccessCreatorArgs.LogPM;

            logPM.Subject = "Start To Send Request To Forwarder By CustomerTenantAccess Controller";

            if (customerTenantAccessCreatorArgs.IsNewLog)
            {
                customerTenantAccessCreatorArgs.ApiLogsService.Create(logPM);
            }

            return logPM;
        }

        private static void RequestCreatedFailed(CustomerTenantAccessCreatorArgs customerTenantAccessCreatorArgs, Task<HttpResponseMessage> result)
        {
            APIException aPIException = JsonConvert.DeserializeObject<APIException>(result.Result.Content.ReadAsStringAsync().Result);
            if (aPIException != null) return;
            
            const string failedStatusCode = "F";
            var Failmsg = aPIException.ErrorType + " Fail To Send Request To Forwarder Tenant " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessCreatorArgs.LogPM.Id, customerTenantAccessCreatorArgs.Tenant, failedStatusCode, customerTenantAccessCreatorArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(aPIException), null, "");
            throw new Exception(aPIException.ErrorType, new Exception(aPIException.ErrorMessage));
        }

        private static void RequestCreatedSuccessfully(CustomerTenantAccessCreatorArgs customerTenantAccessCreatorArgs, Task<HttpResponseMessage> result)
        {
            const string doneStatusCode = "D";
            customerTenantAccessCreatorArgs.Queue.Complete();
            customerTenantAccessCreatorArgs.LogPM.Status = doneStatusCode;
            var ResponseData = result.Result.Content.ReadAsStringAsync().Result;
            var Donemsg = "Request Sent To Forwarder Successfully " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(customerTenantAccessCreatorArgs.LogPM.Id, customerTenantAccessCreatorArgs.Tenant, doneStatusCode, customerTenantAccessCreatorArgs.Response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ResponseData, null, "");
        }

    }
    public class CustomerTenantAccessCreatorArgs
    {
        public QueueResponse Response { get; set; }
        public int Tenant { get; set; }
        public string RequestId { get; set; }
        public APILogsService ApiLogsService { get; set; }
        public bool IsNewLog { get; set; }
        public APILogsPM LogPM { get; set; }
        public HttpClient Client { get; set; }
        public IQueueService Queue { get; set; }
        public string URI { get; set; }
    }
}
