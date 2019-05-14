using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffVersionQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TariffVersionPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext;
            TariffVersionKeys tariffVersionKeys = entityKeys as TariffVersionKeys;

            TariffLineQueryService tariffLineQueryService = new TariffLineQueryService(context);
            entityPM.TariffLines = tariffLineQueryService.GetMulti(tariffVersionKeys, true);
        }
    }
}
