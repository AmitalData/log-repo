using Logitude.Tariff.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Services
{
    public class TariffSettingsServices
    {
        public TariffSettingPM GetByToken(string token)
        {
            ApiResponse<TariffSettingPM> response = APICaller.CallGet<TariffSettingPM>(Urls.GetTenantTariffSetting, token);
            return response.Data;
        }

        public TariffSettingPM UpdateByToken(string token, TariffSettingPM tariffSetting)
        {
            ApiResponse<TariffSettingPM> response = APICaller.CallPut<TariffSettingPM>(tariffSetting, Urls.Tariffsettings, token);
            return response.Data;
        }
    }
}
