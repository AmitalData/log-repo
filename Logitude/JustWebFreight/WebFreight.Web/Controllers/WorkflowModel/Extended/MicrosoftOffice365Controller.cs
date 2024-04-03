using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.BL.EntityUpdateServices;
using MicrosoftGraphClient.GraphServices;
using MicrosoftGraphClient.Models.SubscriptionsService;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Logitude.Workflow.BL.Models.MicrosoftOffice365;
using Logitude.Workflow.BL.QueueMessages;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class MicrosoftOffice365Controller : ApiController
    {
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

                        createSubscription.Tenant = authToken.Tenant;
                        
                        Subscription clientSubscription = CreateClientSubscription(createSubscription);
                        ServiceProviderSubscriptionPM serviceProviderSubscriptionPM = CreateServiceProviderSubscription(createSubscription, clientSubscription);

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

        [HttpPost]
        [ActionName("SubscriptionNotificationListen")]
        public HttpResponseMessage SubscriptionNotificationListen([FromUri] string workflowNumber, int tenant, string validationToken = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(validationToken))
                {
                    return GetValidationTokenResponseMessage(validationToken);
                }

                string requestContentJson = Request.Content.ReadAsStringAsync().Result;
                JObject requestContentJObject = JObject.Parse(requestContentJson);
                List<SubscriptionNotification> subscriptionNotifications = JsonConvert.DeserializeObject<List<SubscriptionNotification>>(requestContentJObject["value"].ToString());
                AddQueueMessages(subscriptionNotifications, workflowNumber, tenant);

                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch(Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }


        private Subscription CreateClientSubscription(CreateSubscription createSubscription)
        {
            Subscription subscription = new GraphClientSubscriptionsService()
                .SetAccessToken(createSubscription.AccessToken)
                .Create(new CreateSubscriptionRequest
                {
                    ChangeType = "created",
                    Resource = "me/mailfolders/inbox/messages",
                    NotificationUrl = GetSubscriptionNotificationUrl(createSubscription.WorkflowNumber, createSubscription.Tenant),
                    ExpirationDateTime = DateTimeOffset.UtcNow.AddHours(20),
                    ClientState = Guid.NewGuid().ToString()
                });
            return subscription;
        }

        private ServiceProviderSubscriptionPM CreateServiceProviderSubscription(CreateSubscription createSubscription, Subscription clientSubscription)
        {
            ServiceProviderSubscriptionPM serviceProviderSubscriptionPM = new ServiceProviderSubscriptionPM
            {
                Tenant = createSubscription.Tenant,
                UserEmail = createSubscription.UserEmail,
                AccessToken = createSubscription.AccessToken,
                RefreshToken = createSubscription.RefreshToken,
                WorkflowNumber = createSubscription.WorkflowNumber,
                AccessTokenExpirationDateTime = createSubscription.AccessTokenExpirationDateTime,
                SubscriptionExpirationDateTime = clientSubscription.ExpirationDateTime,
                WebhookParams = GetUrlQueryStringAsJson(clientSubscription.NotificationUrl),
                AdditionalSettings = null,
                EmailProvider = "MicrosoftOffice365",
                ChangeSetOp = ChangeSetOperation.Insert
            };

            IWorkflowContext workflowContext = WorkflowContext.GetContext(serviceProviderSubscriptionPM.Tenant);
            ServiceProviderSubscriptionUpdateService service = new ServiceProviderSubscriptionUpdateService(workflowContext, new Dictionary<string, IContext>(), serviceProviderSubscriptionPM.Tenant);
            service.Update(serviceProviderSubscriptionPM, true);

            return serviceProviderSubscriptionPM;
        }

        private string GetSubscriptionNotificationUrl(string workflowNumber, int tenant)
        {
            if (!string.IsNullOrEmpty(workflowNumber))
            {
                return LogitudeSettings.LogitudeURL + "/api/MicrosoftOffice365/SubscriptionNotificationListen?workflowNumber=" + workflowNumber + "&tenant=" + tenant;
            }
            return null;
        }

        private string GetUrlQueryStringAsJson(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                NameValueCollection collection = HttpUtility.ParseQueryString(url.Split('?')[1]);
                return JsonConvert.SerializeObject(collection.AllKeys.ToDictionary(i => i, i => collection[i]));
            }
            return null;
        }

        private HttpResponseMessage GetValidationTokenResponseMessage(string validationToken)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(validationToken, Encoding.UTF8, "text/plain")
            };
            return httpResponseMessage;
        }

        private void AddQueueMessages(List<SubscriptionNotification> subscriptionNotifications, string workflowNumber, int tenant)
        {
            if(subscriptionNotifications != null)
            {
                foreach (SubscriptionNotification subscriptionNotification in subscriptionNotifications)
                {
                    new NewOffice365MessageQueueMessage()
                    {
                        WorkflowNumber = workflowNumber,
                        Tenant = tenant,
                        MessageId = subscriptionNotification.ResourceData.Id
                    }.Produce();
                }
            }
        }
    }
}