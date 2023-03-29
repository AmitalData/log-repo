using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.BL;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.BL.EntityUpdateServices;
using Logitude.Workflow.Data.EntityListQueryServices;
using Logitude.Workflow.BL.EntityQueryServices;
using MicrosoftGraphClient.GraphServices;
using MicrosoftGraphClient.Models.AuthenticationService;
using MicrosoftGraphClient.Models.SubscriptionsService;
using WebFreight.Web.Controllers.WorkflowModel.Models.MicrosoftOffice365;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class MicrosoftOffice365Controller : ApiController
    {
        [HttpGet]
        [ActionName("Test")]
        public HttpResponseMessage Test()
        {
            try
            {
                //abd logitude

                //var token = "eyJ0eXAiOiJKV1QiLCJub25jZSI6IkYxTEhpWUxEWXFLLWZVUE9FSURTaXhwc0pZX0xxd0lLUDNicGIwdzExaEUiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9hNDZiMTQ0Ni05YWY0LTQwNzktODdhZC0zMzA0ZWU5ZWQ3NTgvIiwiaWF0IjoxNjc5OTEzNjk2LCJuYmYiOjE2Nzk5MTM2OTYsImV4cCI6MTY3OTkxOTI2NCwiYWNjdCI6MCwiYWNyIjoiMSIsImFpbyI6IkFWUUFxLzhUQUFBQTYvTmRTOUpBT0dRYkdHUk1qazFvcjZpODR6ZHh6Zko3c3dQcnFOOTJrM2VJb1BDV0tpaUZoUS8xekdFWHhvRUJReUkyaGo5cUJPdjJUdjhBNzNsbVRidHowMmVsbGdJNEtVUFNqOG9sN1hRPSIsImFtciI6WyJwd2QiLCJtZmEiXSwiYXBwX2Rpc3BsYXluYW1lIjoiQVNQLk5FVCBHcmFwaCBOb3RpZmljYXRpb24gV2ViaG9vayBTYW1wbGUiLCJhcHBpZCI6Ijk1YjZjYTU5LThlMzAtNDVhYi1iOWNiLWU0NzY1ZTgyOTcxYSIsImFwcGlkYWNyIjoiMCIsImZhbWlseV9uYW1lIjoiTWFsYWtoIiwiZ2l2ZW5fbmFtZSI6IkFiZCBBbC1SYWhtYW4iLCJpZHR5cCI6InVzZXIiLCJpcGFkZHIiOiI4Mi4yMTMuMi4yMzAiLCJuYW1lIjoiQWJkIEFsLVJhaG1hbiBNYWxha2giLCJvaWQiOiIxN2NjMzljZS0xMTNlLTQxYzYtYWZiYy0yOTlkNjFiODBhYTAiLCJwbGF0ZiI6IjMiLCJwdWlkIjoiMTAwMzIwMDA4MzVEMzg0QiIsInB3ZF9leHAiOiI5NDkwIiwicHdkX3VybCI6Imh0dHBzOi8vcG9ydGFsLm1pY3Jvc29mdG9ubGluZS5jb20vQ2hhbmdlUGFzc3dvcmQuYXNweCIsInJoIjoiMC5BU1VBUmhScnBQU2FlVUNIclRNRTdwN1hXQU1BQUFBQUFBQUF3QUFBQUFBQUFBQWxBQnMuIiwic2NwIjoiTWFpbC5SZWFkIG9wZW5pZCBwcm9maWxlIFVzZXIuUmVhZCBlbWFpbCIsInNpZ25pbl9zdGF0ZSI6WyJrbXNpIl0sInN1YiI6ImtNRUtZcDZRR2ZwWEJmTXVoSm95LUh4YnlZZDROcXJHRlJfemJaZnhrWDgiLCJ0ZW5hbnRfcmVnaW9uX3Njb3BlIjoiQVMiLCJ0aWQiOiJhNDZiMTQ0Ni05YWY0LTQwNzktODdhZC0zMzA0ZWU5ZWQ3NTgiLCJ1bmlxdWVfbmFtZSI6IkFiZEBsb2dpdHVkZXdvcmxkLmNvbSIsInVwbiI6IkFiZEBsb2dpdHVkZXdvcmxkLmNvbSIsInV0aSI6IjVDZjgyUGlUcUVtYURJOVlzZEFCQVEiLCJ2ZXIiOiIxLjAiLCJ3aWRzIjpbImI3OWZiZjRkLTNlZjktNDY4OS04MTQzLTc2YjE5NGU4NTUwOSJdLCJ4bXNfc3QiOnsic3ViIjoiLWpyYTMyNzJBaGUyT2QtMlhlSE9wQk1RSkVzNU9XNWtMNnBXSHdyLW9HTSJ9LCJ4bXNfdGNkdCI6MTMxMjgxNDk5NX0.ew4a8H4n_4iC8z8GxA0clkUplRQvwfNA_e0NxIyWsF4kspKw0vfI7J_Jp1HtiqXpLspFZv_wgFzffd8Uf2Ws_0TCwgWoy8hXWWFKdPLcQlCvsVNgnQBFjTZiZTaCVWWXLAbS78noSaT5q0xKt2XSCeyUJOuIKNZdY8yE2yKdTVRSJJfluhRc_Qd1A8UppRAAAfhY94f37YZnqM_foqlISHMT5SpxEFTw_pG2qPWLumlB2e_1RxwULxm7hOBUGB3tpZQuZnHGplyM6r5sH79t0bInOQrq0Ko1wmB2Ha3zAt2CYu8-d9Sl12VSmZS5y7iCRJm3KpJlsWVaRd1fAHcPYg";
                var messageId = "AQMkADc2MjNmNTdjLWZlMmMtNDcxMC1hNjllLWU0MWM5ZDI4MzZkOABGAAADcNK3uTgMGE2lPz6QQJH3cQcA6nOeFaLEv0CGic42TYl42AAAAgEMAAAA6nOeFaLEv0CGic42TYl42AADL74Q0QAAAA==";

                var refreshToken = "";

                var profileService = new GraphClientService().Profile().SetAccessToken(new RefreshAccessTokenRequest { RefreshToken = refreshToken });
                var profile = profileService.Get();


                var profileService2 = new GraphClientProfileService().SetAccessToken(new RefreshAccessTokenRequest { RefreshToken = refreshToken });
                var profile2 = profileService2.Get();


                var messagesService0 = new GraphClientService().Messages().SetAccessToken(new RefreshAccessTokenRequest { RefreshToken = refreshToken });
                var message0 = messagesService0.Get(messageId);
                var messageEml0 = messagesService0.GetEml(messageId);


                var messagesService = new GraphClientMessagesService().SetAccessToken(new RefreshAccessTokenRequest { RefreshToken = refreshToken });
                var message = messagesService.Get(messageId);
                var messageEml = messagesService.GetEml(messageId);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [ActionName("CreateSubscription")]
        public HttpResponseMessage CreateSubscription(CreateSubscription createSubscription)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("ServiceProviderSubscription", "NEW", authToken.Tenant);
                        int tenant = authToken.Tenant;


                        Subscription subscription = new GraphClientSubscriptionsService()
                            .SetAccessToken(createSubscription.AccessToken)
                            .Create(new CreateSubscriptionRequest
                            {
                                ChangeType = "created",
                                Resource = "me/mailfolders/inbox/messages",
                                NotificationUrl = GetSubscriptionNotificationUrl(createSubscription.WorkflowNumber, tenant),
                                ExpirationDateTime = DateTime.Now.AddDays(2),//DateTimeOffset.UtcNow.AddDays(2)
                                ClientState = Guid.NewGuid().ToString(),
                                LatestSupportedTlsVersion = "v1_2"
                            });

                        
                        ServiceProviderSubscriptionPM serviceProviderSubscriptionPM = new ServiceProviderSubscriptionPM
                        {
                            Tenant = tenant,
                            UserEmail = createSubscription.UserEmail,
                            AccessToken = createSubscription.AccessToken,
                            RefreshToken = createSubscription.RefreshToken,
                            WorkflowNumber = createSubscription.WorkflowNumber,
                            AccessTokenExpirationDateTime = createSubscription.AccessTokenExpirationDateTime,
                            SubscriptionExpirationDateTime = subscription.ExpirationDateTime,
                            WebhookParams = GetUrlQueryStringAsJson(subscription.NotificationUrl),
                            AdditionalSettings = null,
                            EmailProvider = "MicrosoftOffice365",
                            ChangeSetOp = ChangeSetOperation.Insert
                        };


                        IWorkflowContext workflowContext = WorkflowContext.GetContext(serviceProviderSubscriptionPM.Tenant);
                        ServiceProviderSubscriptionUpdateService service = new ServiceProviderSubscriptionUpdateService(workflowContext, new Dictionary<string, IContext>(), serviceProviderSubscriptionPM.Tenant);
                        service.Update(serviceProviderSubscriptionPM, true);


                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, serviceProviderSubscriptionPM);
                    }
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

        private string GetSubscriptionNotificationUrl(string workflowNumber, int tenant)
        {
            if (!string.IsNullOrEmpty(workflowNumber))
            {
                string baseUrl = "https://8f3d-82-213-2-230.eu.ngrok.io";
                return baseUrl + "/api/MicrosoftOffice365/WebHook?workflowNumber=" + workflowNumber + "&tenant=" + tenant;
            }
            return null;
        }

        private string GetUrlQueryStringAsJson(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                NameValueCollection collection = HttpUtility.ParseQueryString("");
                return JsonConvert.SerializeObject(collection.AllKeys.ToDictionary(i => i, i => collection[i]));
            }
            return null;
        }


        [HttpPost]
        [ActionName("WebHook")]
        public HttpResponseMessage WebHook([FromUri] string workflowNumber, int tenant, string validationToken = null)
        {
            if (!string.IsNullOrEmpty(validationToken))
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(validationToken, Encoding.UTF8, "text/plain")
                };
                return httpResponseMessage;
            }

            var test = workflowNumber;
            var test2 = tenant;



            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}