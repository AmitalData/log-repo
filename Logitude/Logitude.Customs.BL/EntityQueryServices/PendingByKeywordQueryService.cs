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

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class PendingByKeywordQueryService : EntityQueryService<PendingByKeyword, PendingByKeywordKeys, PendingByKeywordPM, object, PendingByKeywordKeys>
    {
        public string GetCourierPendingReasonCodeBykeyWords(string keyWordsList, int tenant)
        {
            return repository.GetCourierPendingReasonCodeBykeyWords(keyWordsList, tenant);
        }
    }
}
