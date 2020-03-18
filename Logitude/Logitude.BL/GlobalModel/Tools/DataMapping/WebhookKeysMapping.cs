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
            BuildSearchFields(entityPM, poco);

        }
        private static void BuildSearchFields(WebhookKeysPM entityPM, WebhookKeys entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PartnerName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Description);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AccessKey);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
