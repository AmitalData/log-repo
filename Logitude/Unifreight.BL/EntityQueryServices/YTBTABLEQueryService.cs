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
    public class YTBTABLEQueryService : EntityQueryService<YTBTABLE, YTBTABLEKeys, YTBTABLEPM, object, YTBTABLEKeys>
    {
        public YTBTABLEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new YTBTABLERepository(context);
            mapping = new YTBTABLEDataMapping();
        }

        public YTBTABLEPM GetSingle(string CUSTTB, string TBCODE, bool getFromCache)
        {
            var keys = new YTBTABLEKeys() { CUSTTB = CUSTTB, TBCODE = TBCODE };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(YTBTABLE entityPOCO)
        {
            return new YTBTABLEKeys() { CUSTTB = entityPOCO.CUSTTB, TBCODE = entityPOCO.TBCODE };
        }
    }
}

