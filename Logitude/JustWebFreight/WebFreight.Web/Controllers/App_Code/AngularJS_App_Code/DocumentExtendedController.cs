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

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentExtendedController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage GetDocumentById(string documentId, int tenant)
        {
            try
            {
                Authentication(tenant);

                DocumentRepository documentRepository = new DocumentRepository(tenant);
                Document document = documentRepository.GetSingleDocument(tenant, documentId);
                return Request.CreateResponse(HttpStatusCode.OK, document);



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Delete(string documentId, int tenant)
        {
            try
            {
                Authentication(tenant);

                DocumentRepository documentRepository = new DocumentRepository(tenant);
                Document document = documentRepository.GetSingleDocument(tenant, documentId);
                documentRepository.Remove(document);
                documentRepository.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, document);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        private static void Authentication(int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            if (tenant != authToken.Tenant)
            {
                throw new Exception("Sorry! this user is not authorized!");
            }
        }





    }
}