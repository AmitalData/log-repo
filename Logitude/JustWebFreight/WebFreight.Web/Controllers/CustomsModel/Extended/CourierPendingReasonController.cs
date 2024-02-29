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
    public class CourierPendingReasonController : ApiController
    {
        public HttpResponseMessage GetCourierPendingReasonByUnifreightStatus(string unifreightStatusCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(customContext);
                List<CourierPendingReasonPM> courierPendingReasonPMList = courierPendingReasonQueryService.GetCourierPendingReasonByUnifreightStatus(unifreightStatusCode, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Count = courierPendingReasonPMList.Count();
                response.Result = courierPendingReasonPMList;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteCourierPendingReasonUnifreightStatus(string courierPendingReasonCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(customContext);
                CourierPendingReasonPM courierPendingReasonPM = courierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(courierPendingReasonCode, tenant);

                //CourierPendingReasonPM courierPendingReasonPM = courierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(courierPendingReasonCode, tenant);
                courierPendingReasonPM.ChangeSetOp = ChangeSetOperation.Update;
                courierPendingReasonPM.UnifreightStatusCode = null;

                CourierPendingReasonUpdateService courierPendingReasonUpdateService = new CourierPendingReasonUpdateService(customContext, new Dictionary<string, IContext>(), authToken.Tenant);
                courierPendingReasonUpdateService.Update(courierPendingReasonPM,true);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetSingleCourierPendingReasonByCode(string code)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
                
                CourierPendingReasonQueryService courierPendingReasonQuery = new CourierPendingReasonQueryService(MyContext);
                CourierPendingReasonPM courierPendingReasonPM = courierPendingReasonQuery.GetSingleCourierPendingReasonByCode(code, authToken.Tenant);


                return Request.CreateResponse(HttpStatusCode.OK, courierPendingReasonPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);

                CourierPendingReasonListQueryService courierPendingReasonQuery = new CourierPendingReasonListQueryService(MyContext);
                CourierPendingReasonList courierPendingReasonList = courierPendingReasonQuery.GetSingleByCode(code, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, courierPendingReasonList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}