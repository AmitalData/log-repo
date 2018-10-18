using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Controllers.CommonDataModel.Services
{
    public class ExcelExportFeaturesController : ApiController
    {
        public HttpResponseMessage GetExportFeaturesToCSVFile()
        {
            try
            {
                ExcelExportService excelExportService = new ExcelExportService();

                byte[] result = excelExportService.ExportFeaturesToCSVFile();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PostImportFeaturePackages(ImageParameter parameter)
        {
            try
            {
                if (parameter != null && !string.IsNullOrEmpty(parameter.Base64String))
                {
                    ExcelExportService excelExportService = new ExcelExportService();

                    excelExportService.ImportFeaturePackages(Convert.FromBase64String(parameter.Base64String));
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