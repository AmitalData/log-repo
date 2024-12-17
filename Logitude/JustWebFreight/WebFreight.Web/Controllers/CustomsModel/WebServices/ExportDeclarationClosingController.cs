using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Logitude.Customs.Data;
using Unifreight.BL.BL;
using System.Collections.Generic;
using Logitude.Customs.Data.Repsitories;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class ExportDeclarationClosingWebServiceController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage UnifreightData(string exportfile)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ExportDeclarationClosing.ExportData exportData = new ExportDeclarationClosing(tenant).GetExportData(exportfile);

                return Request.CreateResponse(HttpStatusCode.OK, exportData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}