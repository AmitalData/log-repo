using System.Linq;
using System.Web.Http;
using System;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Web;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using Logitude.SystemLogs;
using System.Net;
using System.Net.Http;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalUploaderController : ApiController
    {
        [HttpPost]
        [Route("DigitalUploader/PostDocument")]
        public HttpResponseMessage PostDocument(DigitalUploaderInfo info)
        {
            int tenant = 0;
            string email = "";

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                    // Create Docs In
                    tenant = authToken.Tenant;
                    email = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, info.CardId);

                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    ContactRepository contactRepository = new ContactRepository(tenant);
                    Contact loggedContact = contactRepository.GetSingleContactByEmailAndTenant(email, tenant);
                    DocumentsFilingService service = new DocumentsFilingService(context, tenant);

                    var documentId = service.UploadDigitalDocument(info, tenant, loggedContact);
                    ImageParameter imageParameterfilter = UploadImage(documentId, info, tenant);
                    
                    HandelNoteInformation(info, loggedContact);
                    AddUploadEvent(info, loggedContact.Id, tenant);
                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, imageParameterfilter);
                }
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        #region private 

        private static void HandelNoteInformation(DigitalUploaderInfo info, Contact loggedContact)
        {
            info.Notes = $"{loggedContact.EnglishName} - {loggedContact.CompanyName} {Environment.NewLine}"
                         + $"{info.FileName} {Environment.NewLine}"
                         + $"{info.DocumentTypeName} {Environment.NewLine}"
                         + info.Notes;
        }

        private ImageParameter UploadImage(string documentId, DigitalUploaderInfo info, int tenant)
        {
            // Upload Image 
            ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();
            var filter = new ImageParameter()
            {
                Base64String = info.Base64String,
                EntityId = documentId,
                Extension = info.FileExtension,
                FileSize = info.FileSize,
                FileName = info.FileName,
                Tenant = tenant,
                Buffersize = info.Buffersize,
                BufferNumber = -1
            };

            ImageParameter imageParameterfilter = imageLibraryControllerHelper.UploadAttachementOrChunk(filter);
            return imageParameterfilter;
        }

        private void AddUploadEvent(DigitalUploaderInfo info, string loggedContactId, int tenant)
        {      
            UserRepository userRepository = new UserRepository(tenant);
            string loggedUserId = loggedContactId;
            bool isContactUser = false;
            isContactUser = userRepository.IsContactIdExist(loggedContactId, tenant);
            if (!isContactUser)
            {
                var loggedUserEmail = "system@tenant" + tenant + ".com";
                var loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);
                loggedUserId = loggedUser.Id;
            }

            var eventCode = "DOUP";
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = eventCode,
                UserId = loggedUserId,
                EntityId = info.EntityId,
                ObjectTableName = info.ObjectTableName,
                Notes = info.Notes
            });
        }
        
        #endregion private
    }
}