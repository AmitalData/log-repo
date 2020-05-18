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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityUpdateServices;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class ExceptionReasonController : ApiController
    {
        public HttpResponseMessage GetExceptionReasonByUnifreightStatus(string unifreightStatusCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                ExceptionReasonQueryService exceptionReasonQueryService = new ExceptionReasonQueryService(customContext);
                List<ExceptionReasonPM> exceptionReasonPMList = exceptionReasonQueryService.GetExceptionReasonByUnifreightStatus(unifreightStatusCode, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Count = exceptionReasonPMList.Count();
                response.Result = exceptionReasonPMList;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage DeleteExceptionReasonByUnifreightStatus(string ExceptionReasonCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                ExceptionReasonQueryService exceptionReasonQueryService = new ExceptionReasonQueryService(customContext);
                ExceptionReasonPM exceptionReasonPM = exceptionReasonQueryService.GetSingle(ExceptionReasonCode, false, false);
                exceptionReasonPM.ChangeSetOp = ChangeSetOperation.Update;
                exceptionReasonPM.UnifreightStatusCode = null;

                ExceptionReasonUpdateService exceptionReasonUpdateService = new ExceptionReasonUpdateService(customContext, new Dictionary<string, IContext>(), authToken.Tenant);
                exceptionReasonUpdateService.Update(exceptionReasonPM, true);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}