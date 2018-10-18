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
    public class CTBMISHGURQueryService : EntityQueryService<CTBMISHGUR, CTBMISHGURKeys, CTBMISHGURPM, object, CTBMISHGURKeys>
    {
        public CTBMISHGURQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBMISHGURRepository(context);
            mapping = new CTBMISHGURDataMapping();
        }

        public CTBMISHGURPM GetSingle(string SUGMISHGUR, bool getFromCache)
        {
            var keys = new CTBMISHGURKeys() { SUGMISHGUR = SUGMISHGUR};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBMISHGUR entityPOCO)
        {
            return new CTBMISHGURKeys() { SUGMISHGUR = entityPOCO.SUGMISHGUR };
        }
    }
}

