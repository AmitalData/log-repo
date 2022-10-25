using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalUploaderController : ApiController
    {
        [HttpPost]
        [Route("DigitalUploader/PostDocument")]
        public IHttpActionResult PostDocument(DigitalUploaderInfo info)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, info.CardId);
               
                // Create Docs In
                var tenant = authToken.Tenant;
                var email = authToken.Email;
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmailAndTenant(email, tenant);
                DocumentsFilingService service = new DocumentsFilingService(context, tenant);
                var documentId = service.UploadDigitalDoeument(info,tenant, loggedContact);

                ImageParameter imageParameterfilter =  UploadImage(documentId, info, tenant);
                AddUploadEvent(info, loggedContact.Id, tenant);
               
                return Ok(imageParameterfilter);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
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
                Buffersize = info.Buffersize
            };

            ImageParameter imageParameterfilter = imageLibraryControllerHelper.UploadAttachementOrChunk(filter);
            return imageParameterfilter;
        }

        private void AddUploadEvent(DigitalUploaderInfo info, string loggedContactId, int tenant)
        {
            // Notes & Events 
            if (!string.IsNullOrEmpty(info.Notes))
            {
                if (info.Notes.Contains('.')) info.Notes = info.Notes.Split('.')[0];
            }

            var eventCode = "DOUP";
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = eventCode,
                UserId = loggedContactId,
                EntityId = info.EntityId,
                ObjectTableName = info.ObjectTableName,
                Notes = info.Notes,
            });
        }

    }
}