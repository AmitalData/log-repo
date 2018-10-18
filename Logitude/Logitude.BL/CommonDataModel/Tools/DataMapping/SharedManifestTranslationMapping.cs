using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class SharedManifestTranslationMapping
    {
        public static void MapEntity(SharedManifestTranslationPM entityPM, SharedManifestTranslation poco, bool isNewEntity)
        {
            poco.Id = entityPM.Id;
            poco.Tenant = entityPM.Tenant;
            poco.ObjectTableName = entityPM.ObjectTableName;
            poco.MyCode = entityPM.MyCode;
            poco.AgentCode = entityPM.AgentCode;
            poco.AgentId = entityPM.AgentId;
            poco.CreatedByUserId = entityPM.CreatedByUserId;
            poco.CreateDate = entityPM.CreateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
            poco.UpdateDate = entityPM.UpdateDate;

        }

        
    }
}
