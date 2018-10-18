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
    public class CTBIDNTPQueryService : EntityQueryService<CTBIDNTP, CTBIDNTPKeys, CTBIDNTPPM, object, CTBIDNTPKeys>
    {
        public CTBIDNTPQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBIDNTPRepository(context);
            mapping = new CTBIDNTPDataMapping();
        }

        public CTBIDNTPPM GetSingle(string IDENTIFITYPE, bool getFromCache)
        {
            var keys = new CTBIDNTPKeys() { IDENTIFITYPE = IDENTIFITYPE};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBIDNTP entityPOCO)
        {
            return new CTBIDNTPKeys() { IDENTIFITYPE = entityPOCO.IDENTIFITYPE };
        }
    }
}

