using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class NotificationWebServiceController : ApiController
    {

        public HttpResponseMessage GetNotificationsByDefinitionCode(string objectTableId, string entityId, int tenant)
        {
            try
            {
                /*string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);*/
                ICustomContext customContext = CustomContext.GetContext(tenant);

                NotificationQueryService notificationQuery = new NotificationQueryService(customContext);
                List<NotificationPM> notifications = notificationQuery.GetNotificationByDefinitionCode(objectTableId, entityId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, notifications);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSetNotificationsStatus([FromUri] List<string> Ids, string status)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                NotificationQueryService notificationQuery = new NotificationQueryService(customContext);

                NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                List<NotificationPM> notifications = new List<NotificationPM>();

                notifications = notificationQuery.GetNotificationsPMsByIds(Ids, tenant);

                foreach (NotificationPM notificationPM in notifications)
                {

                    if (status == "Read")
                    {
                        notificationPM.IsSeenByAssignee = true;
                    }

                    else if (status == "Unread")
                    {
                        notificationPM.IsSeenByAssignee = false;
                    }

                    else if (status == "Close")
                    {
                        notificationPM.IsClosedByAssignee = true;
                    }

                    else if (status == "Open")
                    {
                        notificationPM.IsClosedByAssignee = false;
                    }
                    notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                    service.Update(notificationPM, true);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

            


        }

        public HttpResponseMessage PostSendNotificationReplyRequest(MessageToAgentRequestParams requestParams)
        {
            try
            {
                MessageToAgentResponseData responseData = null;

                var service = new DOC_NG_5101_GNMessageToAgentMessagingService();
                responseData = service.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}