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
    public class DocumentTypeCopyMapping
    {
        public static void MapEntity(DocumentTypeCopyPM documentTypeCopyPM, DocumentTypeCopy documentTypeCopy, bool isNewState)
        {
            documentTypeCopy.Code = documentTypeCopyPM.Code;
            documentTypeCopy.Name = documentTypeCopyPM.Name;
            documentTypeCopy.Tenant = documentTypeCopyPM.Tenant;
            documentTypeCopy.IsSelectedByDefault = documentTypeCopyPM.IsSelectedByDefault;
            documentTypeCopy.IndexOrder = documentTypeCopyPM.IndexOrder;
            documentTypeCopy.InActive = documentTypeCopyPM.InActive;
        }
    }
}
