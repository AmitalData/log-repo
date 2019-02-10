using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.Server.Tools;
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

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ImporterDepositionController : ApiController
    {
        public HttpResponseMessage PostImporterDepositions(ImporterDepositionAM importerDepositionAM)
        {
            try 
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ImporterDepositionHelper importerDepositionHelper = new ImporterDepositionHelper();
                string logId = importerDepositionHelper.AddAPILogs(importerDepositionAM);
                importerDepositionHelper.StartImporterDeposition(importerDepositionAM);
                var msg = "Importer Deposition Send to cloud Successfully";
                APILogsUtility.UpdateAPILogStatus(logId, importerDepositionAM.CustomerTenant, "D", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(importerDepositionAM), null, null, "");

                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}