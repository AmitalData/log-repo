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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CustomWebServices.BL.XLSReports;
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

                DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationId, true, false);

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

        

        public HttpResponseMessage GetWorkSpacePendingTab(string IntegratorId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext context = CustomContext.GetContext(tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(tenant);
                var counts = declarationCourierStatusQueryService.GetWorkSpacePendingTab(tenant, IntegratorId);


                return Request.CreateResponse(HttpStatusCode.OK, counts);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetQueriesCounts(string IntegratorId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext context = CustomContext.GetContext(tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(tenant);
              var counts=  declarationCourierStatusQueryService.GetQueriesCounts(tenant, IntegratorId);


                return Request.CreateResponse(HttpStatusCode.OK, counts);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSLAReport2Excel(string tenant,string fromDate,string toDate,string integratorCode,string reportType)
        {
            try
            {
                var slaReports = new SlaReport();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var item = slaReports.GetSlaReport(fromDate,toDate,integratorCode,reportType,tenant);
                response.Content = new StreamContent(new MemoryStream(item));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName =Guid.NewGuid().ToString() + ".xls";
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetLastMileReport2Excel(string tenant, string hatraFromDate, string hatraToDate, string lastMileFromDate, string LastMileToDate, string airline, string trucker,string mawb)
        {
            try
            {
                var lastMileReport = new LastMileReport(tenant);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var item = lastMileReport.GetLastMileReport(hatraFromDate, hatraToDate, lastMileFromDate, LastMileToDate, airline, trucker, mawb);
                response.Content = new StreamContent(new MemoryStream(item));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = Guid.NewGuid().ToString() + ".xls";
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}