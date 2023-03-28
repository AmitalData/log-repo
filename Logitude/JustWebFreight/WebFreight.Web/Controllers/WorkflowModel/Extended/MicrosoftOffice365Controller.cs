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

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class MicrosoftOffice365Controller : ApiController
    {
        [ActionName("Test")]
        public HttpResponseMessage GetTest()
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

        public HttpResponseMessage PostSubscription(string workflowNumber, string microsoftEmailAccessToken, string microsoftEmailRefreshToken)
        {
            if (string.IsNullOrEmpty(microsoftEmailAccessToken) || string.IsNullOrEmpty(workflowNumber))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty access token or workflow number");
            }

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("ServiceProviderSubscription", "NEW", authToken.Tenant);

                    //string refreshToken = "0.ASUARhRrpPSaeUCHrTME7p7XWFnKtpUwjqtFucvkdl6ClxolABs.AgABAAEAAAD--DLA3VO7QrddgJg7WevrAgDs_wUA9P-_yOGQiFxkFEYmJRp4M0WosEF7JBVKt_bBrlClF5_Yxw97HceTyWzp8Bivs87zDC-DLvE2LfRvo2vB6dOXsTlc3eTEX5EqdJk98UnU4S7ZjJHXN_PUQjdQa4JcovG9DKhhpQZFiVCo5P4if0ASYuZOK0_SElKO4p5vFKmbMuLHu5Hj6osKdMR58kgiHX4rC1v4UG-HQWuzus1KPXlyz3EbBltqvjHZM680cIleeVRAzDfqyFgd5WxvRby9S5-E8QWEM9JeyG2RWKLoXb3AJdcpUf_wcRlq5o9aqnBEf6g5ZZtjN9pb8lalgz4ChLEkYGRas6Hj9vllS_VeMGFHhLD92ErxWAW2UumEGfDm6Lxa_tJyFy3jbgIy2CUZMKmXQYUX7a-jZZRU741xRlo1Jz-CJZ-XWwVKgzZ0g_tbR-rPfjA708KcXDYDFCXv6QaemZhZ-k1h-JE-4R-znqhAGYuMqT4j7vxtmfclaJTlcd9N575ng9z8fLYEP5phw3x_r59BufuBJwZbnXODq2e4tZjDsf-PqrZliz-8BhP6W2ZQD8ppR6eWOSRCE0B4sAcNa1HyOuSpwujWuzEra24dvgMVCmBy95Rb0sPzEkPYKRHsIEWfIuHJyIT9XjN6D2-YMFuLuNkPeGitcVR1Y24bfE6iEbrRmFjUuGTnizH2ZLVL7RbfEMPeXe6cNBY318sH6-l7YRDqNX9xwhQmrSefFndg-qL9JzgBIsg1UyQ9b7cJjINWjTsW5Wc8SdZmLC481m5sycP7bIkDCtdGFBZEjot0ZAcNCUM4ANemKKsqM3L21DCah7LfFe-iRhqSkCN7yP2W27fEmT5DgnhO1kw1goHgddVyVkTA-LLB0N0JBYuYFWRMi7ga0DL6kJypAbI-UfC4";


                    var subscription = new GraphClientSubscriptionsService().SetAccessToken(microsoftEmailAccessToken).Create(new CreateSubscriptionRequest
                    {
                        ChangeType = "created",
                        NotificationUrl = "https://5bb1-82-213-2-230.eu.ngrok.io/api/MicrosoftOffice365/WebHook?workflowNumber=" + workflowNumber + "&tenant=" + authToken.Tenant,//$"{_notificationHost}/listen",
                        Resource = "me/mailfolders/inbox/messages",
                        ClientState = Guid.NewGuid().ToString(),
                        ExpirationDateTime = DateTime.Now.AddDays(2),//DateTimeOffset.UtcNow.AddDays(2)
                        LatestSupportedTlsVersion = "v1_2"
                    });



                    ServiceProviderSubscriptionPM entityPM = new ServiceProviderSubscriptionPM
                    {
                        Tenant = authToken.Tenant,
                        UserEmail = null,
                        AccessToken = null,
                        RefreshToken = null,
                        EmailProvider = null,
                        WorkflowNumber = null,
                        AdditionalSettings = null,
                        AccessTokenExpirationDateTime = DateTime.Now,
                        SubscriptionExpirationDateTime = DateTime.Now,
                        WebhookParams = null,
                        ChangeSetOp = ChangeSetOperation.Insert
                    };


                    IWorkflowContext workflowContext = WorkflowContext.GetContext(entityPM.Tenant);
                    ServiceProviderSubscriptionUpdateService service = new ServiceProviderSubscriptionUpdateService(workflowContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(entityPM, true);


                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpPost]
        [ActionName("WebHook")]
        public IHttpActionResult WebHook([FromUri] string workflowNumber, int tenant, string validationToken = null)
        {
            if (!string.IsNullOrEmpty(validationToken))
            {
                return Ok(validationToken);
            }

            var test = "";



            return StatusCode(HttpStatusCode.Accepted);
        }




    }
}