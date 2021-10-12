using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
  public partial   class DecCargoSplitConQueryService
    {

      public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, DecCargoSplitConPM entityPM)
      {
          ICustomContext context = MainContext as CustomContext;
          DecCargoSplitConKeys DecCargoSplitConKeys = entityKeys as DecCargoSplitConKeys;
          DecCargoSplitConsItemQueryService DecCargoSplitConsItemitionQueryService = new DecCargoSplitConsItemQueryService(context);
          entityPM.DecCargoSplitConsItems = DecCargoSplitConsItemitionQueryService.GetMulti(DecCargoSplitConKeys, true);


          if (entityPM.DecCargoSplitConsItems != null)
          {
              if (entityPM.DecCargoSplitConsItems.Count > 0)
              {
                  entityPM.DecCargoSplitConsItemLastLineNumber = entityPM.DecCargoSplitConsItems.Max(m => m.ItemLine);
              }
          }
      }
        public List<int?> GetConsiPackageSequeList(string declarationid)
        {
            DecCargoSplitConRepository DecCargoSplitConsRepository = new DecCargoSplitConRepository(context);
            return DecCargoSplitConsRepository.GetConsiPackageSequeList(declarationid);
        }

    }
}
