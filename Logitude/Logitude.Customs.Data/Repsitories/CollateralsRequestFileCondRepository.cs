
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CollateralsRequestFileCondRepository : IRepository<CollateralsRequestFileCond>
    {

        public List<CollateralsRequestFileCond> GetMulti(EntityKeyFields entityKeys)
        {

            CustomsCollateralsAnswerKeys keys = entityKeys as CustomsCollateralsAnswerKeys;
            List<CollateralsRequestFileCond> conditions = (from a in context.CollateralsRequestFileConds
                                                           where a.CustomsCollateralId == keys.CustomsCollateralId && a.LineNumber == keys.LineNumber
                                                           select a).ToList();
            return conditions;
        }
    }

}
