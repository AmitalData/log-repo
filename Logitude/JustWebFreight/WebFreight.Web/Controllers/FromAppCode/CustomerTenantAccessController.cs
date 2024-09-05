using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code
{
    public class CustomerTenantAccessController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }


        public HttpResponseMessage Post(CustomerTenantAccessAM entityAM)
        {
            try
            {
                CustomerTenantAccessPM entityPM = new CustomerTenantAccessPM();
                var MapResult = MapEntityAMToEntityPM(entityAM, entityPM);
                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, entityPM.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, entityPM.Tenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("CustomerTenantAccess", entityPM.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", entityPM.Tenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = entityPM.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        //Refrence = Shipment.ShipmentNumber,
                        Status = "I",
                        Tenant = entityPM.Tenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", entityPM.Tenant, true);
                    LogPM = new APILogsPM()
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

                    };
                }
                LogPM.Subject = "Add Request To Forwarder Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                try
                {
                    SecurityUtility.AuthenticationOnTenant(entityAM.Tenant);
                    //SecurityUtility.CheckContactFeature("CustomerTenantAccess", "NEW", entityAM.Tenant);
                    var msg = "Start Inserting CustomerTenantAccess To Forwarder Tenant " + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(entityPM), null, null, "");
                    if (MapResult == null)
                    {
                        entityPM.Status = "W";
                        ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                        var SystemUser = "system@tenant" + entityPM.Tenant + ".com";
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(entityPM.Tenant);
                        CustomerTenantAccessPM temp = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(entityPM.Tenant, entityPM.CustomerTenant);
                        if (temp != null)
                        {
                            if (temp.Status.ToUpper() != "A")
                            {
                                temp.Status = "W";
                                MapResult = MapEntityAMToEntityPM(entityAM, temp);
                                if (MapResult == null)
                                {
                                    CustomerTenantAccessService Updateservice = new CustomerTenantAccessService(objectContext, temp.Tenant, temp, SystemUser);
                                    Updateservice.Update();
                                }
                                else
                                {
                                    var Failmsg = "Inserting CustomerTenantAccess Faild " + DateTime.Now;
                                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, null, "");
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, MapResult);
                                }
                            }
                            else
                            {
                                var Failmsg = "You already sent a request .. " + DateTime.Now;
                                APIException Responce = new APIException();
                                Responce.ErrorMessage = Failmsg;
                                Responce.ErrorType = "Duplicate Message";
                                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, null, "");
                                return Request.CreateResponse(HttpStatusCode.BadRequest, Responce);
                            }
                        }
                        else
                        {
                            CustomerTenantAccessService service = new CustomerTenantAccessService(objectContext, entityPM.Tenant, entityPM, SystemUser);
                            service.Create();
                            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(entityPM.Tenant, false);
                            if (currentTenant != null)
                            {
                                ContactRepository Repos = new ContactRepository(entityPM.Tenant);
                                var Contact = Repos.GetSingleContact(currentTenant.LogBoxAdminUserId, entityPM.Tenant);
                                var emailMessage = GetEmailMessage(entityPM);

                                string env = entityPM.IsPrivateLabelCustomer? entityAM.PrivateLabelName : "Logbox";
                                var subject = "New Request From "+ env + " - " + entityPM.CompanyName;
                                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                                {
                                    From = SettingUtil.Emails.FromNoReply,
                                    To = Contact.Email,
                                    Subject = subject,
                                    EmailBody = emailMessage,
                                    LoggingUserId = currentTenant.LogBoxAdminUserId,
                                    Tenant = entityPM.Tenant,
                                };
                                Communications.AddEmailCommunicationLogQueue(emailParams, entityPM.Tenant);
                            }

                        }
                        var ResponseData = JsonConvert.SerializeObject(entityPM.Id);
                        var Donemsg = "CustomerTenantAccess Added To Forwarder Tenant Successfully " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, null, null, "");
                        IWebFreightContext webfreightcontext = WebFreightContext.GetContext(entityPM.Tenant);
                        TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomerTenantAccess", webfreightcontext);
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");

                    }
                    else
                    {
                        var Failmsg = "Inserting CustomerTenantAccess Faild " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, null, null, "");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, MapResult);
                    }

                }
                catch (Exception exc)
                {
                    string errorMessage = exc.Message + Environment.NewLine;

                    if (exc.InnerException != null)
                    {

                        errorMessage = errorMessage + " (" + (exc.InnerException.InnerException != null ? exc.InnerException.InnerException.Message : exc.InnerException.Message) + ")" + Environment.NewLine;

                    }

                    errorMessage = errorMessage + exc.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, exc.Message, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    APIException Responce = new APIException();
                    Responce.ErrorType = exc.GetType().Name;
                    Responce.ErrorMessage = errorMessage;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Responce);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private APIException MapEntityAMToEntityPM(CustomerTenantAccessAM entityAM, CustomerTenantAccessPM entityPM)
        {
            APIException Responce = new APIException();
            if (entityAM.Tenant != 0)
            {
                entityPM.Tenant = entityAM.Tenant;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "Tenant field is required.";
                return Responce;
            }
            if (entityAM.CustomerTenant != 0)
            {
                entityPM.CustomerTenant = entityAM.CustomerTenant;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "CustomerTenant field is required.";
                return Responce;
            }
            entityPM.CompanyName = entityAM.CompanyName;
            entityPM.CompanyEmail = entityAM.CompanyEmail;
            entityPM.CompanyVat = entityAM.CompanyVat;
            entityPM.ContactName = entityAM.ContactName;
            entityPM.ContactMobile = entityAM.ContactMobile;
            entityPM.ContactPhone = entityAM.ContactPhone;
            entityPM.IsPrivateLabelCustomer = entityAM.IsPrivateLabelCustomer;
            entityPM.StockTypeCode = entityAM.StockTypeCode;
            return null;
        }

        private string GetEmailMessage(CustomerTenantAccessPM entityPm)
        {
            string emailMessage = "";
            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("<b>Contact Name: </b>" + entityPm.ContactName);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<b>Contact Phone: </b>" + (string.IsNullOrEmpty(entityPm.ContactPhone) ? entityPm.ContactMobile : entityPm.ContactPhone));
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<b>Contact Email: </b>" + entityPm.CompanyEmail);
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<b>Company Vat : </b>" + entityPm.CompanyVat);

            HtmlTemplate.Append("</p>");
            emailMessage = HtmlTemplate.ToString();
            return emailMessage;
        }
    }
}