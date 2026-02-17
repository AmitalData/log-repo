using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class FilingInboxMapping
    {
        public static void MapEntity(FilingInboxPM entityPM, FilingInbox poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.Sender = entityPM.Sender;
            poco.Subject = entityPM.Subject;
            poco.IsDeleted = entityPM.IsDeleted;
            poco.CreateDate = entityPM.CreateDate;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
            poco.BodyDocumentId = entityPM.BodyDocumentId;
            poco.SearchFields = entityPM.SearchFields;
        }
    }
}
