using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ExternalLinkMapping
    {
        public static void MapEntity(ExternalLinkPM entityPM, ExternalLink poco, bool isNewEntity = true)
        {            
            poco.Id = entityPM.Id;
            poco.Ref = entityPM.Ref;
            poco.Link = entityPM.Link;
            poco.ExpirationDate = entityPM.ExpirationDate;
            poco.ActivityLog = entityPM.ActivityLog;
            poco.Params = entityPM.Params;
            poco.Tenant = entityPM.Tenant;
        }


        public static ExternalLinkPM MapPM(ExternalLink poco, ExternalLinkPM entityPM = null)
        {
            if (entityPM == null)
                entityPM = new ExternalLinkPM();

            entityPM.Id = poco.Id;
            entityPM.Ref = poco.Ref;
            entityPM.Link = poco.Link;
            entityPM.ExpirationDate = poco.ExpirationDate;
            entityPM.ActivityLog = poco.ActivityLog;
            entityPM.Params = poco.Params;
            entityPM.Tenant = poco.Tenant;

            return entityPM;
        }
    }
}
