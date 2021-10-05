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
    public class MTBPORTQueryService : EntityQueryService<MTBPORT, MTBPORTKeys, MTBPORTPM, object, MTBPORTKeys>
    {
        public MTBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new MTBPORTRepository(context);
            mapping = new MTBPORTDataMapping();
        }

        public MTBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new MTBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(MTBPORT entityPOCO)
        {
            return new MTBPORTKeys() { PORTID = entityPOCO.PORTID };
        }
    }
}

