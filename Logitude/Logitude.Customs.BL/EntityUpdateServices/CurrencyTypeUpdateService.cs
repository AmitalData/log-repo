using Logitude.BL.Helpers;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CurrencyTypeUpdateService : EntityUpdateService<CurrencyType, CurrencyTypePM, EntityPM>
    {
        protected override void OnUpdating(CurrencyTypePM entityPM)
        {
            TableLastUpdateClass.UpdateTableHistory(0, "Customs.CurrencyType");
            base.OnUpdating(entityPM);
        }
    }
}
