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
    public class CTBRGOWNQueryService : EntityQueryService<CTBRGOWN, CTBRGOWNKeys, CTBRGOWNPM, object, CTBRGOWNKeys>
    {
        public CTBRGOWNQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBRGOWNRepository(context);
            mapping = new CTBRGOWNDataMapping();
        }

        public CTBRGOWNPM GetSingle(short RIGHTID, bool getFromCache)
        {
            var keys = new CTBRGOWNKeys() { RIGHTID = RIGHTID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBRGOWN entityPOCO)
        {
            return new CTBRGOWNKeys() { RIGHTID = entityPOCO.RIGHTID };
        }
    }
}

