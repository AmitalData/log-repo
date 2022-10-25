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
                var loggedUserEmail = authToken.Email;
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(tenant);

                Contact loggedContact = contactRepository.GetSingleContactByEmailAndTenant(loggedUserEmail, tenant);
                var objecttableId = GetObjectTableId(info.ObjectTableName, tenant);
                DocumentsFilingPM newDocument = new DocumentsFilingPM()
                {
                    DocumentTypeId = info.DocumentTypeId,
                    EntityId = info.EntityId,
                    Tenant = tenant,
                    ObjectTableName = info.ObjectTableName,
                    ObjectTableId = objecttableId,
                    DirectionCode = "I",
                    ReceivedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    FileExtension = info.FileExtension,
                    FileSize = info.FileSize,
                    ReceivedByUserName = loggedContact?.EnglishName,
                };

                newDocument.SearchFields = newDocument.Code + "," + newDocument.DirectionCode + "," + loggedContact?.EnglishName + "," + loggedContact?.LocalName;
                newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                newDocument.CreatedByUserId = loggedContact.Id;
                newDocument.OwnerId = loggedContact.Id;
                newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                newDocument.UpdatedByUserId = loggedContact.Id;
                newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                DocumentsFilingService service = new DocumentsFilingService(MyContext, tenant);
                service.Create(newDocument, null);

                // Upload Image 
                ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();
                var filter = new ImageParameter()
                {
                    Base64String = info.Base64String,
                    EntityId = newDocument.Id,
                    Extension = info.FileExtension,
                    FileSize = info.FileSize,
                    FileName = info.FileName,
                    Tenant = tenant,
                    Buffersize = info.Buffersize
                };

                ImageParameter _filter = imageLibraryControllerHelper.UploadAttachementOrChunk(filter);
                
                // Notes & Events 
                if (!string.IsNullOrEmpty(info.Notes))
                {
                    if (info.Notes.Contains('.')) info.Notes = info.Notes.Split('.')[0];
                }
                var eventCode = "DOUP";
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = authToken.Tenant,
                    EventTypeCode = eventCode,
                    UserId = loggedContact.Id,
                    EntityId = info.EntityId,
                    ObjectTableName = "Shipment",
                    Notes = info.Notes,
                });

                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        private string GetObjectTableId(string objectTableName, int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string objectTableId = objectTableQuery.GetObjectTableIdByName(objectTableName);
            return objectTableId;
        }
    }
}