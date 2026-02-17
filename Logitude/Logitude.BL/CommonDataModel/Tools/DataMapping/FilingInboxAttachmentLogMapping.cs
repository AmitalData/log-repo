using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class FilingInboxAttachmentLogMapping
    {
        public static void MapEntity(FilingInboxAttachmentLogPM entityPM, FilingInboxAttachmentLog poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.DocumentsFilingId = entityPM.DocumentsFilingId;
            poco.FilingInboxAttachmentId = entityPM.FilingInboxAttachmentId;
        }
    }
}
