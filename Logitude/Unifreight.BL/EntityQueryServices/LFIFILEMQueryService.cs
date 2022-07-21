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
    public class LFIFILEMQueryService : EntityQueryService<LFIFILEM, LFIFILEMKeys, LFIFILEMPM, object, LFIFILEMKeys>
    {
        public LFIFILEMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new LFIFILEMRepository(context);
            mapping = new LFIFILEMDataMapping();
        }

        public LFIFILEMPM GetSingle(int DELIVERYNO, bool getFromCache)
        {
            var keys = new LFIFILEMKeys() { DELIVERYNO = DELIVERYNO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(LFIFILEM entityPOCO)
        {
            return new LFIFILEMKeys() { DELIVERYNO = entityPOCO.DELIVERYNO };
        }
    }
}

