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
    public class RTBPORTQueryService : EntityQueryService<RTBPORT, RTBPORTKeys, RTBPORTPM, object, RTBPORTKeys>
    {
        public RTBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new RTBPORTRepository(context);
            mapping = new RTBPORTDataMapping();
        }

        public RTBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new RTBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(RTBPORT entityPOCO)
        {
            return new RTBPORTKeys() { PORTID = entityPOCO.PORTID };
        }
    }
}

