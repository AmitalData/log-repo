using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using System.Web;
using System.Text;
using System.Xml;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using WebFreight.Web.DataContracts;

using Logitude.BL.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentTypeCopyExtendedController : ApiController
    {
        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("DocumentTypeCopy", "READ", authToken.Tenant);
        }
        public HttpResponseMessage GetDocumentTypeCopiesWithoutLimitedOneForAutomations(string documentTypeId, string limitedPrintCopyId, int tenant)
        {
            try
            {
                Authentication();
                SecurityUtility.AuthenticationOnTenant(tenant);
                DocumentTypeCopyQuery documentTypeCopyQuery = new DocumentTypeCopyQuery(tenant);
                List<DocumentTypeCopyPM> documentTypeCopies = documentTypeCopyQuery.GetDocumentTypeCopiesWithoutLimitedOneForAutomations(documentTypeId, limitedPrintCopyId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, documentTypeCopies);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}