
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TenantLoginPolicyMapping
{

    public static void MapEntity(TenantLoginPolicyPM entityPM, TenantLoginPolicy entityPOCO, bool isNewState)
    {
        if (isNewState)
        {
            entityPOCO.Tenant = entityPM.Tenant;
        }

        entityPOCO.IsEnabledForSpecificUsers = entityPM.IsEnabledForSpecificUsers;
        entityPOCO.LoginPolicyCode = entityPM.LoginPolicyCode;
        entityPOCO.TwoFactorInternalIPs = entityPM.TwoFactorInternalIPs;
        entityPOCO.KeepUserLoggedIn = entityPM.KeepUserLoggedIn;
        entityPOCO.ExcludeInternalIPs = entityPM.ExcludeInternalIPs;
        entityPOCO.AllowedIPs = entityPM.AllowedIPs;
        entityPOCO.SessionTimeout = entityPM.SessionTimeout;


    }
}