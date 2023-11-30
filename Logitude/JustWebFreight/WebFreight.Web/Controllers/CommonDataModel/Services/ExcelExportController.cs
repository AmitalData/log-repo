using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.CommonDataModel.Services
{
    public class ExcelExportController : ApiController
    {
        public HttpResponseMessage GetExportRoleFeaturesToCSVFile()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ExcelExportService excelExportService = new ExcelExportService();

            byte[] result =    excelExportService.ExportRoleFeaturesToCSVFile();
            return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage PostImportRoleFeatures(ImageParameter parameter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ImportRoleFeature", parameter.Tenant, authToken.Tenant);

                if (parameter != null && !string.IsNullOrEmpty(parameter.Base64String))
                {
                    ExcelExportService excelExportService = new ExcelExportService();

                    excelExportService.ImportRoleFeatures(Convert.FromBase64String(parameter.Base64String));
                }
              return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        



    }
}