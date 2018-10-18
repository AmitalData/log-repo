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
  public partial   class CustomsCollateralsAnswerQueryService
    {

      public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, CustomsCollateralsAnswerPM entityPM)
      {
          ICustomContext context = MainContext as CustomContext;
          CustomsCollateralsAnswerKeys customsCollateralsAnswerKeys = entityKeys as CustomsCollateralsAnswerKeys;
          CollateralsRequestFileCondQueryService collateralsRequestFileConditionQueryService = new CollateralsRequestFileCondQueryService(context);
          entityPM.CollateralsRequestFileConds = collateralsRequestFileConditionQueryService.GetMulti(customsCollateralsAnswerKeys, true);


          if (entityPM.CollateralsRequestFileConds != null)
          {
              if (entityPM.CollateralsRequestFileConds.Count > 0)
              {
                  entityPM.AnswerRequestFileConditionLastLineNumber = entityPM.CollateralsRequestFileConds.Max(m => m.LineNumber);
              }
          }
      }

    }
}
