using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.SocialModel.Extended
{
    public class ConversationHeadersExtendedController : ApiController
    {
        public HttpResponseMessage PostGetMessageByFilter(MessageFilters messageFilters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

                ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);

                messageFilters.Technology = "Angular";
                List<ConversationHeaderPM> result = service.GetMessagePMsByFilter(messageFilters, true, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCountUnReadConversationHeaderPMs(string userid ,  string entityId, string objectTableId, string areaMessage)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
                ConversationHeaderQueryService service = new ConversationHeaderQueryService(socialContext);
                int coountMessage =  service.GetCountUnReadMessagePMsByFilter(userid, authToken.Tenant, entityId, objectTableId, areaMessage);

                return Request.CreateResponse(HttpStatusCode.OK, coountMessage);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetLoggedContactMessageInfo(string userId, int tenant)
        {
            try
            {
                string result = "";
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                string color = "";
                ContactRepository contactRepository = new ContactRepository(authToken.Tenant);
                Contact contact = contactRepository.GetSingleContact(userId, tenant);

                if (contact != null)
                {
                    ColorIndexRepository colorIndexRepository = new ColorIndexRepository(authToken.Tenant);
                    color = colorIndexRepository.GetSingleHasColor(contact.IndexColor);
                    result = color + "@" + contact.ImageDetailId;
                }


                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}