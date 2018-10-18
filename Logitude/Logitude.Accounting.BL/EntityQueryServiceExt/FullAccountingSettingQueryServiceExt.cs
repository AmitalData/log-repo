using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class FullAccountingSettingQueryServiceExt : IFullAccountingSettingQueryServiceExt
    {
        public FullAccountingSettingQueryServiceExt()
        {

        }
        public FullAccountingSettingPM GetFullAccountingSettingByTenant(int tenant)
        {
            FullAccountingSettingQueryService query = new FullAccountingSettingQueryService(tenant);
            return query.GetSingleFullAccountingSetting(tenant);
        }
    }
}
