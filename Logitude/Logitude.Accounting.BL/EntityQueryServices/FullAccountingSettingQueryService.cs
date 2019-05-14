using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class FullAccountingSettingQueryService : EntityQueryService<FullAccountingSetting, FullAccountingSettingKeys, FullAccountingSettingPM, object, FullAccountingSettingKeys>
    {
        public FullAccountingSettingPM GetSingleFullAccountingSetting(int tenant)
        {

            TenantPM tenantpm = TenantQuery.GetSingleTenantPM(tenant, false);
            if (tenantpm != null)
            {
                FullAccountingSettingPM fullAccountingSetting = this.GetEntityPM(this.repository.GetSingleFullAccountingSetting(tenant));
                if (fullAccountingSetting == null)
                {
                    return null;
                    fullAccountingSetting = new FullAccountingSettingPM();
                }
                fullAccountingSetting.AccountingActivationDate = tenantpm.AccountingActivationDate;
                fullAccountingSetting.AccountingActivated = tenantpm.AccountingActivated;
                return fullAccountingSetting;
            }
            else
            {
                return null;
            }

        }
        public static FullAccountingSettingPM Get(int tenant)
        {
            string key = "FullAccountingSettingPM," + tenant.ToString();

            var val = CacheManager.GetOrInsertNewObject<FullAccountingSettingPM>(key, () =>
            {
                var full = new FullAccountingSettingQueryService(tenant);
                var fullPm = full.GetSingleFullAccountingSetting(tenant);
                return fullPm ;
            });
            return val;
        }
    }
}
