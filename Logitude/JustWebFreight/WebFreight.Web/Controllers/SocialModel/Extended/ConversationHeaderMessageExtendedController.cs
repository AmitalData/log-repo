using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
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

namespace WebFreight.Web.Controllers.SocialModel.Extended
{
    public class ConversationHeaderMessageExtendedController : ApiController
    {
        public HttpResponseMessage GetAllConversationMessageForHeaderQuery(string conversationHeaderId ,string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

                ConversationHeaderMessageQueryService service = new ConversationHeaderMessageQueryService(socialContext);
                List<ConversationHeaderMessagePM> lists = service.GetAllConversationMessageListForHeader(conversationHeaderId, userId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, lists);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


      

    }
}