using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CommonIIGInterface;


namespace Logitude.CustomsMessaging.Helpers
{
    public class ForbiddenSignsUtil
    {
        private char _defaultChar = '_';
        public string ReplaceForbiddenChars(string original, string forbiddenChars)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(forbiddenChars))
                return original;

            return original.Any(forbiddenChars.Contains)
                ? new string(original.Select(c => forbiddenChars.Contains(c) ? _defaultChar : c).ToArray())
                : original;
        }


        public string GetForbiddenSigns(int tenant)
        {

            ICustomContext _context = CustomContext.GetContext(tenant);
            CustomsSettingQueryService customsSettingQuery = new CustomsSettingQueryService(_context);
            CustomsSettingPM CustomsSetting = customsSettingQuery.GetSingleByTenant(tenant);

            return CustomsSetting.ForbiddenSigns;
        }
    }
}
