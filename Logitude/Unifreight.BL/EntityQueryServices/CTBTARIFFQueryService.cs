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
    public class CTBTARIFFQueryService : EntityQueryService<CTBTARIFF, CTBTARIFFKeys, CTBTARIFFPM, object, CTBTARIFFKeys>
    {
        public CTBTARIFFQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBTARIFFRepository(context);
            mapping = new CTBTARIFFDataMapping();
        }

        public CTBTARIFFPM GetSingle(string TARIFFID, bool getFromCache)
        {
            var keys = new CTBTARIFFKeys() { TARIFFID = TARIFFID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBTARIFF entityPOCO)
        {
            return new CTBTARIFFKeys() { TARIFFID = entityPOCO.TARIFFID };
        }
    }
}

