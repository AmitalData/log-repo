using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class PendingByKeywordQueryService : EntityQueryService<PendingByKeyword, PendingByKeywordKeys, PendingByKeywordPM, object, PendingByKeywordKeys>
    {
        public List<string> GetCourierPendingReasonCodeBykeyWords(string keyWordsList, string SearchByFieldCode, int tenant)
        {
            return repository.GetCourierPendingReasonCodeBykeyWords(keyWordsList, SearchByFieldCode, tenant);
        }


        public void DeleteById(string id)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                PendingByKeywordPM entityPM = new PendingByKeywordQueryService(context)
                    .GetSingle(id, false, false);

                entityPM.ChangeSetOp = ChangeSetOperation.Delete;

                new PendingByKeywordUpdateService(context, new Dictionary<string, IContext>(), Tenant)
                    .Update(entityPM, true);                

                scope.Complete();
            }
        }
    }
}
