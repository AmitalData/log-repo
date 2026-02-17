using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentFolderMapping
    {
        public static void MapEntity(DocumentFolderPM entityPM, DocumentFolder poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.Code = entityPM.Code;
            }
            poco.EnglishName = entityPM.EnglishName;
            poco.LocalName = entityPM.LocalName;
            poco.IsExternalFolder = entityPM.IsExternalFolder;
            poco.ParentFolderId = entityPM.ParentFolderId;

            poco.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.Code;
           
        }
    }
}
