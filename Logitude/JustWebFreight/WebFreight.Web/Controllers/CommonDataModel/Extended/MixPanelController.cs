using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.MixPanel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class MixPanelController : ApiController
    {
        public HttpResponseMessage Post(MixPanelActionsEvent mixPanelActionsEvent)
        {
            try
            {
                AuthenticationToken authToken = Authentication();
                mixPanelActionsEvent.Email = authToken.Email;
                MixPanelActionsService logBoxActionsMixPanelService = new MixPanelActionsService(authToken.Tenant);
                logBoxActionsMixPanelService.Build(mixPanelActionsEvent);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private AuthenticationToken Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            return authToken;
        }
    }
}