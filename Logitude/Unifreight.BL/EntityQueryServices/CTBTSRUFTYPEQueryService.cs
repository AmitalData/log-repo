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
    public class CTBTSRUFTYPEQueryService : EntityQueryService<CTBTSRUFTYPE, CTBTSRUFTYPEKeys, CTBTSRUFTYPEPM, object, CTBTSRUFTYPEKeys>
    {
        public CTBTSRUFTYPEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBTSRUFTYPERepository(context);
            mapping = new CTBTSRUFTYPEDataMapping();
        }

        public CTBTSRUFTYPEPM GetSingle(string TSRUFAID, bool getFromCache)
        {
            var keys = new CTBTSRUFTYPEKeys() { TSRUFAID = TSRUFAID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBTSRUFTYPE entityPOCO)
        {
            return new CTBTSRUFTYPEKeys() { TSRUFAID = entityPOCO.TSRUFAID };
        }
    }
}

