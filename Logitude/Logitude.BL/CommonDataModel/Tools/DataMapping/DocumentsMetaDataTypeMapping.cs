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
    public class DocumentsMetaDataTypeMapping
    {
        public static void MapEntity(DocumentsMetaDataTypePM entityPM, DocumentsMetaDataType entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.Code = entityPM.Code;
            entityPOCO.EnglishName = entityPM.EnglishName;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.CustomsMetaDataCode = entityPM.CustomsMetaDataCode;
            entityPOCO.Format = entityPM.Format;
           
        }

        
    }
}
