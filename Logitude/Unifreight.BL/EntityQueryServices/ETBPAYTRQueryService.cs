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
    public class ETBPAYTRQueryService : EntityQueryService<ETBPAYTR, ETBPAYTRKeys, ETBPAYTRPM, object, ETBPAYTRKeys>
    {
        public ETBPAYTRQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBPAYTRRepository(context);
            mapping = new ETBPAYTRDataMapping();
        }

        public ETBPAYTRPM GetSingle(string PTERMID, bool getFromCache)
        {
            var keys = new ETBPAYTRKeys() { PTERMID = PTERMID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBPAYTR entityPOCO)
        {
            return new ETBPAYTRKeys() { PTERMID = entityPOCO.PTERMID };
        }
    }
}

