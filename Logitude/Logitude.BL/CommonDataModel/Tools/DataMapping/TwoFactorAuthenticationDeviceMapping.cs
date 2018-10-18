
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TwoFactorAuthenticationDeviceMapping
    {
    
        public static void MapEntity(TwoFactorAuthenticationDevicePM entityPM, TwoFactorAuthenticationDevice entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.TwoFactorkey = entityPM.TwoFactorkey;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.AuthenticationCode = entityPM.AuthenticationCode;
            }
            entityPOCO.CodeExpirationDate = entityPM.CodeExpirationDate;
            entityPOCO.DeviceDescription = entityPM.DeviceDescription;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.LastLoginDate = entityPM.LastLoginDate;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.LastLoginIP = entityPM.LastLoginIP;
            entityPOCO.UserId = entityPM.UserId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
    



        }
    }
}
