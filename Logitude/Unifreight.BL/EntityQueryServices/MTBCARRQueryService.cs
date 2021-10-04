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
    public class MTBCARRQueryService : EntityQueryService<MTBCARR, MTBCARRKeys, MTBCARRPM, object, MTBCARRKeys>
    {
        public MTBCARRQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new MTBCARRRepository(context);
            mapping = new MTBCARRDataMapping();
        }

        public MTBCARRPM GetSingle(string AIRLINEID, bool getFromCache)
        {
            var keys = new MTBCARRKeys() { AIRLINEID = AIRLINEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(MTBCARR entityPOCO)
        {
            return new MTBCARRKeys() { AIRLINEID = entityPOCO.AIRLINEID };
        }
    }
}

