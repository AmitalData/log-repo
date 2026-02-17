using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class WebhookKeysMapping
    {

        public static void MapEntity(WebhookKeysPM entityPM, WebhookKeys poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
                poco.CreatedByUserName = entityPM.CreatedByUserName;
            }

            poco.Description = entityPM.Description; 
            poco.AccessKey = entityPM.AccessKey;
            poco.InActive = entityPM.InActive;            
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserName = entityPM.UpdatedByUserName;
            poco.PartnerName = entityPM.PartnerName; 
       
        }
    }
}
