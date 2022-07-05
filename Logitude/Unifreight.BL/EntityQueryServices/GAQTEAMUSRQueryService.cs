using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class GAQTEAMUSRQueryService : EntityQueryService<GAQTEAMUSR, GAQTEAMUSRKeys, GAQTEAMUSRPM, object, GAQTEAMUSRKeys>
    {
        public GAQTEAMUSRQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GAQTEAMUSRRepository(context);
            mapping = new GAQTEAMUSRDataMapping();
        }

        public GAQTEAMUSRPM GetSingle(string TEAMID, bool getFromCache)
        {
            var keys = new GAQTEAMUSRKeys() { TEAMID = TEAMID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GAQTEAMUSR entityPOCO)
        {
            return new GAQTEAMUSRKeys() { TEAMID = entityPOCO.TEAMID };
        }
    }
}

