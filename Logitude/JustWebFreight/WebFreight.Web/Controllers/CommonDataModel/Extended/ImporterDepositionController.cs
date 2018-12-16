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
                importerDepositionHelper.StartImporterDeposition(importerDepositionAM);
                importerDepositionHelper.AddAPILogs(importerDepositionAM);

                return Request.CreateResponse(HttpStatusCode.OK, "Importer Deposition Send to cloud Successfully");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}