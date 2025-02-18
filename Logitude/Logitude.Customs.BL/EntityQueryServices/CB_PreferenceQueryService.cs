using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;


namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CB_PreferenceQueryService : EntityQueryService<CB_Preference, CB_PreferenceKeys, CB_PreferencePM, object, CB_PreferenceKeys>
    {
        public CB_PreferencePM GetCB_PreferenceByUserIdAndTenant(string userId, int tenant = 0)
        {
            CB_Preference CB_Preference = repository.GetCB_PreferenceByUserIdAndTenant(userId, tenant);
            CB_PreferencePM preferencePM = null;
            if (CB_Preference != null)
            {
                preferencePM = this.GetEntityPM(CB_Preference, false, null);
            }
            return preferencePM;
        }

    }
}
