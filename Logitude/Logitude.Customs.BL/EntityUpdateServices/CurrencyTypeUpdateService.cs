using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CurrencyTypeUpdateService : EntityUpdateService<CurrencyType, CurrencyTypePM, EntityPM>
    {


        protected override void UpdateComposition(CurrencyTypePM entityPM)
        {
            int Tenant = 0;
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                Tenant = authToken.Tenant;

            }
            catch (Exception)
            {

                
            }

            var currencyTypeTenantQueryService = new CurrencyTypeTenantQueryService(Tenant);
            var pm =currencyTypeTenantQueryService.GetPMByCode(Tenant, entityPM.Code);
            if (pm == null)
            {
                pm = new CurrencyTypeTenantPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Code = entityPM.Code
            };
            }
            else
            {
                pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
            pm.Tenant = Tenant;
            pm.TenantInactive = entityPM.TenantInactive;
            var currencyTypeTenantUpdateService = new CurrencyTypeTenantUpdateService(MainContext, new Dictionary<string, IContext>(), pm.Tenant);
            currencyTypeTenantUpdateService.Update(pm, true);
            base.UpdateComposition(entityPM);

        }

        protected override void OnUpdating(CurrencyTypePM entityPM)
        {
            TableLastUpdateClass.UpdateTableHistory(0, "Customs.CurrencyType");
            base.OnUpdating(entityPM);
        }
    }
}
