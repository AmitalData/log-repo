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
    public class DocumentOutCopyMapping
    {
        public static void MapEntity(DocumentOutCopyPM documentOutCopyPM, DocumentOutCopy documentOutCopy, bool isNewState)
        {
            documentOutCopy.DocumentId = documentOutCopyPM.DocumentId;
            documentOutCopy.DocumentOutId = documentOutCopyPM.DocumentOutId;
            documentOutCopy.DocumentTypeCopyId = documentOutCopyPM.DocumentTypeCopyId;
            documentOutCopy.LastPrintDate = documentOutCopyPM.LastPrintDate;
            documentOutCopy.LastPrintedByUserId = documentOutCopyPM.LastPrintedByUserId;
            documentOutCopy.Tenant = documentOutCopyPM.Tenant;
        }
    }
}
