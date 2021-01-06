
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.CloseTables;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.FakeMessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.DataContracts;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class DeclarationReferantDataExtendedController : ApiController
    {

        public HttpResponseMessage GetDeclarationReferantDataDashBoard(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;

                DeclarationReferantDataQueryService declarationReferantDataService = new DeclarationReferantDataQueryService(tenant);
                List<DeclarationReferantDataChartingClass> myResult = declarationReferantDataService.GetDeclarationReferantDataDashBoard(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}