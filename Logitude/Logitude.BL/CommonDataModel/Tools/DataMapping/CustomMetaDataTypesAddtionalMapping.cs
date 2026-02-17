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
    public class CustomMetaDataTypesAddtionalMapping
    {
        public static void MapEntity(CustomMetaDataTypesAddtionalPM entityPM, CustomMetaDataTypesAddtional entityPoco, bool isNewState)
        {
            if (isNewState)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Code = entityPM.Code;
                entityPoco.Tenant = entityPM.Tenant;
            }

            entityPoco.DocumentsMetaDataTypesCode = entityPM.DocumentsMetaDataTypesCode;


        }
    }
}