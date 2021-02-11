
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class QuoteDocumentController : ApiController
    {

        public HttpResponseMessage GetDocumentByRef(int tenant, string quoteNo)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				IQuotesContext objectContext = QuotesContext.GetContext(tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

                QuoteDocumentVersionRepository versionRepository = new QuoteDocumentVersionRepository(objectContext);
                QuoteRepository quoteRepository = new QuoteRepository(objectContext);
                DocumentRepository documentRep = new DocumentRepository(commonContext);

                Quote quote = quoteRepository.GetSingleQuoteByNumber(quoteNo, tenant);
                QuoteDocumentVersion lastVersion = versionRepository.GetSingleQuoteDocumentVersion(quote.Id, tenant, quote.LastVersionNumber);
                if (lastVersion != null)
                {
                    Document document = documentRep.GetSingleDocument(tenant, lastVersion.DocumentId);
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = document.Tenant,
                        FileSize = document.FileSize,

                    };

                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    byte[] fileBinary = storageservice.Read(fileInfo);
                    MemoryStream memStream = new MemoryStream(fileBinary);

                    var content = new StreamContent(memStream);//stream, 4096);   //buffer size of 4kB
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);

                    response.Content = content;
                    return response;
                }
                else
                {
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
               
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}
