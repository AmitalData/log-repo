using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class ApiCredintialsMapping
    {

        public static void MapEntity(ApiCredintialsPM entityPM, ApiCredintials poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
                poco.CreatedBy = entityPM.CreatedBy;
            }

            poco.AllowedIPs = entityPM.AllowedIPs;

            if (!entityPM.HashedPrimaryAccessKey.Contains("-"))
            {
                poco.HashedPrimaryAccessKey = entityPM.HashedPrimaryAccessKey;
            }
            else
            {
                poco.HashedPrimaryAccessKey = PasswordGenerator.GetOldHashedPassword(entityPM.HashedPrimaryAccessKey); 
            }
            if (!entityPM.HashedSeconderyAccessKey.Contains("-"))
            {
                poco.HashedSeconderyAccessKey = entityPM.HashedSeconderyAccessKey;
            }
            else
            {
                poco.HashedSeconderyAccessKey = PasswordGenerator.GetOldHashedPassword(entityPM.HashedSeconderyAccessKey);
            }
            
            poco.maskedPrimaryAccessKey = entityPM.maskedPrimaryAccessKey;
            poco.maskedSeconderyAccessKey = entityPM.maskedSeconderyAccessKey;            
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedBy = entityPM.UpdatedBy;
            poco.UsedFor = entityPM.UsedFor;
            poco.TokenExpirationTime = entityPM.TokenExpirationTime;
           // poco.ComputingPartnerId = entityPM.ComputingPartnerId;
       
        }
    }
}
