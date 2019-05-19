using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PendingByKeywordUpdateService : EntityUpdateService<PendingByKeyword, PendingByKeywordPM, EntityPM>
    {

        protected override void OnCreating(PendingByKeywordPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.PendingByKeyword", entityPM.Tenant);
        }
    }
}
