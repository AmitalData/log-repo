using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentOutMapping
    {
        public static void MapEntity(DocumentOutPM docPM, DocumentOut doc, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(docPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), docPM.Tenant);

            if (isNewState)
            {
                doc.Tenant = docPM.Tenant;
            }
           
            doc.Issued = docPM.Issued;
            doc.Tenant = docPM.Tenant;
            doc.EditableFields = docPM.EditableFields;
            doc.DocumentTemplateId = docPM.DocumentTemplateId;
            doc.EmailTemplateId = docPM.EmailTemplateId;
            doc.XamlDocumentId = docPM.XamlDocumentId;
            doc.NeedsRebuild = docPM.NeedsRebuild;
            doc.IsBlobExist = docPM.IsBlobExist;
            doc.IssuedByUserId = docPM.IssuedByUserId;
            doc.IssuedDate = docPM.IssuedDate;
        }
    }
}
