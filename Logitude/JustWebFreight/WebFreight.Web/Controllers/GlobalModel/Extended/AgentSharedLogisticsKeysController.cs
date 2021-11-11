using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.BlobServiceReference;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.GlobalModel.Extended
{
    public class AgentSharedLogisticsKeysController : ApiController
    {
        public HttpResponseMessage GetSingle(string key)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                AgentSharedLogisticsKey agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(key);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, agentSharedLogisticsKey);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleByAgentId(string agentId, int tenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                AgentRepository agentRepository = new AgentRepository(tenant);
                Agent agent = agentRepository.GetSingleAgent(tenant, agentId);

                AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                AgentSharedLogisticsKey agentSharedLogisticsKey = null;
                if (!string.IsNullOrEmpty(agent.AgentSharedLogisticsKey))
                {
                    agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(agent.AgentSharedLogisticsKey);
                }

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, agentSharedLogisticsKey);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage PostAgentSharedLogisticsKeyInvitation(string agentId, string invitedEmail, int tenant)
        {

            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                TenantRepository tenantRep = new TenantRepository(commonContext);
                Tenant senderTenant = tenantRep.GetSingleTenant(tenant);

                AgentService agentService = new AgentService(commonContext, tenant);
                AgentQuery agentQuery = new AgentQuery(tenant);

                AgentPM agentPM = agentQuery.GetSinglePM(agentId, tenant);

                string email = HttpContext.Current.User.Identity.Name;
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

                AgentSharedLogisticsKey agentSharedLogisticsKey = new AgentSharedLogisticsKey()
                {
                    SharedKey = agentPM.Id + StringHelper.GetRandomString(10),
                    Agent1Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserEmail = loggedContact.Email,
                    StatusCode = "W",
                };

                agentSharedLogisticsKeyRepository.Add(agentSharedLogisticsKey);
                agentSharedLogisticsKeyRepository.SubmitChanges();

                agentPM.AgentSharedLogisticsKey = agentSharedLogisticsKey.SharedKey;
                agentService.Update(agentPM);

                string logo = "logo" + tenant;
                string subject = senderTenant.Company + " invites you to share manifests via Logitude";
                string fromEmail =  loggedContact.Email;
                StringBuilder HtmlTemplate = new StringBuilder();
                HtmlTemplate.Append("<p style='text-align:left'>");
                HtmlTemplate.Append("Hello,");
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append(loggedContact.EnglishName + " from " + senderTenant.Company + " is sending you this invitation to activate the sharing manifests process.");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Please use the following shared key: "+ agentSharedLogisticsKey.SharedKey + " to accept the invitation from Shared Logistics tab in your agent card.");

                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append("Best Regards,");
                HtmlTemplate.Append("<br />");
                HtmlTemplate.Append(loggedContact.EnglishName);
                HtmlTemplate.Append("<br /><br />");
                HtmlTemplate.Append("<img width='290' height='101' src='cid:" + logo + "' />");

                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                {
                    From = fromEmail,
                    To = invitedEmail,
                    Subject = subject,
                    EmailBody = HtmlTemplate.ToString(),
                    LoggingUserId = loggedContact.Id,
                    Tenant = tenant,
                };
                Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
                
                /*
                 *  Waiting for approval
                    Active
                    Inactive
                 * */
                return Request.CreateResponse(HttpStatusCode.OK, agentSharedLogisticsKey);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
 


        //public HttpResponseMessage Post(TenantManagementPM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string logKey = PerformanceLogger.LogCurrentTime();
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //                SecurityUtility.CheckContactFeature("TenantManagement", "NEW", authToken.Tenant);

        //                IGlobalContext MyContext = GlobalContext.GetContext();
        //                TenantManagementService service = new TenantManagementService(MyContext, entityPM.Id);
        //                service.Create(entityPM);


        //                scope.Complete();
        //                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

        //                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}


        public HttpResponseMessage Put(AgentSharedLogisticsKey entity, string updatedByAgentId,bool isAccepted)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //string logKey = PerformanceLogger.LogCurrentTime();
                    //using (TransactionScope scope = TransactionFactory.GetTransaction())
                    // {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    //SecurityUtility.CheckContactFeature("Shipment", "AgentSharedManifest", authToken.Tenant);

                    AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                    //AgentSharedLogisticsKey storedKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(entity.SharedKey);

                    //if(storedKey.StatusCode == "I")
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, "This key is expired!");
                    //}

                    agentSharedLogisticsKeyRepository.Update(entity);
                    agentSharedLogisticsKeyRepository.SubmitChanges();

                    //scope.Complete();
                    //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    if (!string.IsNullOrEmpty(updatedByAgentId) && isAccepted)
                    {
                        ICommonDataContext commonContext = CommonDataContext.GetContext(entity.Agent2Tenant);
                        ContactRepository contactRepository = new ContactRepository(commonContext);
                        AgentService agentService = new AgentService(commonContext, entity.Agent2Tenant);
                        AgentQuery agentQuery = new AgentQuery(entity.Agent2Tenant);

                        AgentPM agentPM = agentQuery.GetSinglePM(updatedByAgentId, entity.Agent2Tenant);
                        agentPM.AgentSharedLogisticsKey = entity.SharedKey;
                        agentService.Update(agentPM);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                    //}
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}