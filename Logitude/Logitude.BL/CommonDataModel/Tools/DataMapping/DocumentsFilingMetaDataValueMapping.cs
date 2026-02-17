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
    public class DocumentsFilingMetaDataValueMapping
    {
        public static void MapEntity(DocumentsFilingMetaDataValuePM entityPM, DocumentsFilingMetaDataValue entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.DocumentsFilingId = entityPM.DocumentsFilingId;
            }

            entityPOCO.DocumentsMetaDataTypeId = entityPM.DocumentsMetaDataTypeId;
            entityPOCO.MetaDataValue = entityPM.MetaDataValue;
         
          
        }
    }
}
