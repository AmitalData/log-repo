using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using WebFreight.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.CustomsModel.Extended;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using  Unifreight.BL.BL;

namespace WebFreight.Web.Controllers.WebServices
{
    public class GTBFUSTATUWebServiceController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetAllGTBFUSTATU()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                var ReferantUnifreightStatuses = new ReferantUnifreightStatuses();
                int myTenant = authToken.Tenant;
                var statusList = ReferantUnifreightStatuses.GetItemsList(myTenant);
                return Request.CreateResponse(HttpStatusCode.OK, statusList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}