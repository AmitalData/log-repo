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
    public class ETBPORTQueryService : EntityQueryService<ETBPORT, ETBPORTKeys, ETBPORTPM, object, ETBPORTKeys>
    {
        public ETBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBPORTRepository(context);
            mapping = new ETBPORTDataMapping();
        }

        public ETBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new ETBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBPORT entityPOCO)
        {
            return new ETBPORTKeys() { PORTID = entityPOCO.PORTID };
        }
    }
}

