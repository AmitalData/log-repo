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
        public List<CB_PreferencePM> GetCB_PreferenceByUserIdAndTenant(string userId, int tenant = 0)
        {
            List<CB_Preference> CB_Preferences = repository.GetCB_PreferenceByUserIdAndTenant(userId, tenant);
            List<CB_PreferencePM> preferencesPM = new List<CB_PreferencePM>(); // Initialize the list
            CB_PreferencePM preferencePM = null;
            if (CB_Preferences != null && CB_Preferences.Count > 0)
            {
                foreach (var item in CB_Preferences)
                {
                    preferencePM = this.GetEntityPM(item, false, null);
                    if (preferencePM != null)
                    {
                        preferencesPM.Add(preferencePM);
                    }
                }
            }
            return preferencesPM;
        }

    }
}
