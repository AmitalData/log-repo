using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class DocumentFileController : ApiController
    {
        public HttpResponseMessage PostFile(DocumentFile documentFile)
        {
            try
            { 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant; 
                if (documentFile.FileData == null)
                {
                    documentFile.FileData = new byte[0];

                } 

                ReportHelper reportHelper = new ReportHelper();
                Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(documentFile); 
                 
                return Request.CreateResponse(HttpStatusCode.OK, newDocument);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



       //  public HttpResponseMessage Delete(string documentId, int tenant)
       //  {
       //     try
       //     {
       //         Authentication(tenant);

       //         DocumentRepository documentRepository = new DocumentRepository(tenant);
       //         Document document = documentRepository.GetSingleDocument(tenant, documentId);
       //         StorageDataArgs storageDataArgs = new StorageDataArgs() { FileName = document.FileName, FolderName = document.Folder, Tenant = tenant };

       //        documentRepository.Remove(document); 
       //        StorageDataService.DeleteFileFromStorage(storageDataArgs);
                 
       //        documentRepository.SubmitChanges();

       //        return Request.CreateResponse(HttpStatusCode.OK, document);
       //    }
       //     catch (Exception ex)
       //    {
       //         return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
       //     }
       // }

        //private static void Authentication(int tenant)
        //{
        //    string token = HttpContext.Current.Request.Headers["Token"];
        //    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //    if (tenant != authToken.Tenant)
        //    {
        //        throw new Exception("Sorry! this user is not authorized!");
        //    }
        //}


    }
}