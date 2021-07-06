using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class TapagConnectionTableUpdateService : EntityUpdateService<TapagConnectionTable, TapagConnectionTablePM, EntityPM>
    {
        protected override void OnUpdating(TapagConnectionTablePM entityPM)
        {
            //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
            if (declarationPM != null && (declarationPM.IsConnectedToUnifreight || declarationPM.IsAmendment==true ))
            {
                UpdateUnifreight(entityPM);
            }

            //UpdateNotification(entityPM);
        }
    }
}
