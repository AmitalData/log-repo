using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.ExternalAPIs.V1
{
    public class BlobInfoController : ApiController
    {

        public HttpResponseMessage Post(BlobInfo blobInfo)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
					SecurityUtility.AuthenticateAPICall(authToken.Tenant);
					if (authToken == null)
                    {
                        throw new AutenticationException("Sorry! this user is not authorized!");
                    }
              
                    if (blobInfo.BlobChunk.Length > 100000)
                    {
                        throw new ApplicationException("Blob chunk must not be larger than 100 KB");
                    }


                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    int tenant = authToken.Tenant;
                    DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper(this.GetType().Name);
                    BlobInfo result = documentFileUploadHelper.UploadBlobInfoToStorage(blobInfo, tenant);
                    result.BlobChunk = null;
                    APIHelper.AddCommunicationLog("D",  blobInfo, result, "BlobInfo", null, "BlobInfo API", tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F",  blobInfo, apiExceptionResult.Exception, "BlobInfo", null, "BlobInfo API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", blobInfo, apiExceptionResult.Exception, "BlobInfo", null, "BlobInfo API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}
