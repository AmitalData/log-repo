using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
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
using WebFreight.Web.Controllers.CustomsModel.Extended;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class CustomsCollateralWebServiceController : ApiController
    {
        [HttpPut]
        public HttpResponseMessage Multi([FromBody] CustomsCollateralWebServiceMultiParams body)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

               List<CustomsCollateral> CustomsCollateralList = new CustomsCollateralQueryService(authToken.Tenant).UpdateMulti(body.ids, body.declarationId, body.selectAll, body.customsCollateralsAnswerPM, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, CustomsCollateralList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public class CustomsCollateralWebServiceMultiParams 
        {
            public string[] ids { get; set; }
            public string declarationId { get; set; }
            public bool selectAll { get; set; }
            public CustomsCollateralsAnswerPM customsCollateralsAnswerPM { get; set; }
        }
    }
}