using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
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

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationCourierStatusWebServiceController: ApiController
    {

        public HttpResponseMessage GetSetManualProcesscode(string declarationId, string manualProcessCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext context = CustomContext.GetContext(tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(tenant);
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), tenant);

                DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationId, false, false);

                declarationCourierStatusPM.ManualProcessCode = manualProcessCode;
                declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;

                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null);
                calculateDeclarationCourierStatus.CalcFastIndividualProcess(declarationCourierStatusPM);

                declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}