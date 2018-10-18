using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
  public partial   class DecCargoSplitConsItemQueryService
    {

      public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, DecCargoSplitConsItemPM entityPM)
      {
          ICustomContext context = MainContext as CustomContext;
          DecCargoSplitConsItemKeys DecCargoSplitConsItemKeys = entityKeys as DecCargoSplitConsItemKeys;
          DecCargoSplitConsPackDetQueryService DecCargoSplitConsPackDetitionQueryService = new DecCargoSplitConsPackDetQueryService(context);
          entityPM.DecCargoSplitConsPackDets = DecCargoSplitConsPackDetitionQueryService.GetMulti(DecCargoSplitConsItemKeys, true);


          if (entityPM.DecCargoSplitConsPackDets != null)
          {
              if (entityPM.DecCargoSplitConsPackDets.Count > 0)
              {
                  entityPM.DecCargoSplitConsPackDetLastLineNumber = entityPM.DecCargoSplitConsPackDets.Max(m => m.PackageLine);
              }
          }
      }

    }
}
