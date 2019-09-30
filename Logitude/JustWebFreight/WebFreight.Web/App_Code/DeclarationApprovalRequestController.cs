using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class DeclarationApprovalRequestController : ApiController
    {
        
        public HttpResponseMessage Put(DeclarationApprovalRequestPM ApprovalRequest)
        {
            try
            {
                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(ApprovalRequest.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, ApprovalRequest.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(ApprovalRequest.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, ApprovalRequest.Tenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", ApprovalRequest.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", ApprovalRequest.Tenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Status = "I",
                        Tenant = ApprovalRequest.Tenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", ApprovalRequest.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = Log.Id,
                        CorrelationId = Log.CorrelationId,
                        CreateDate = Log.CreateDate,
                        CreateDateUTC = Log.CreateDateUTC,
                        Direction = Log.Direction,
                        LastUpdateDate = Log.LastUpdateDate,
                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
                        NumberOfRetries = Log.NumberOfRetries++,
                        ObjectTableId = Log.ObjectTableId,
                        ExpirationDate = Log.ExpirationDate,
                        Status = "I",
                        Tenant = Log.Tenant,

                    };
                }
                LogPM.Subject = "Add Declaration Approval Request To Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                try
                {
                    SecurityUtility.AuthenticationOnTenant(ApprovalRequest.Tenant);
                    ////HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(ApprovalRequest.Tenant);
                    ////var hybridPartner = hybridPartnerQuery.GetSinglePMByPartnerTenant(ApprovalRequest.Tenant);
                    ////CustomerTenantAccessRequestQuery customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(entityAM.CustomerTenant);
                    ////var customerTenantAccessRequest = customerTenantAccessRequestQuery.GetSinglePMByCustomerTenantAndPartnerTenant(hybridPartner.Id, entityAM.CustomerTenant);
                    ////customerTenantAccessRequest.RequestStatus = "A";


                    //var msg = "Start Sending Response To Importer Tenant " + DateTime.Now;
                    //APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(customerTenantAccessRequest), null, null, "");
                    //ICommonDataContext objectContext = CommonDataContext.GetContext(customerTenantAccessRequest.Tenant);

                    //var SystemUser = "system@tenant" + customerTenantAccessRequest.Tenant + ".com";
                    //CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(objectContext, customerTenantAccessRequest.Tenant);
                    //service.Update(customerTenantAccessRequest);
                    //var ResponseData = JsonConvert.SerializeObject(customerTenantAccessRequest.Id);
                    //var Donemsg = "Response Sent To Importer Tenant Successfully " + DateTime.Now;
                    //APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, null, null, "");
                    //IWebFreightContext webfreightcontext = WebFreightContext.GetContext(customerTenantAccessRequest.Tenant);
                    //TableLastUpdateClass.UpdateTableHistory(customerTenantAccessRequest.Tenant, "CustomerTenantAccessRequest", webfreightcontext);
                    return Request.CreateResponse(HttpStatusCode.OK, "OK");

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message + Environment.NewLine;

                    if (ex.InnerException != null)
                    {

                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                    }

                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, ex.Message, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    APIException Responce = new APIException();
                    Responce.ErrorType = ex.GetType().Name;
                    Responce.ErrorMessage = errorMessage;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Responce);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }  
        } 
    }
}