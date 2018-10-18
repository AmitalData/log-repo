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
    public class CTBLOADQueryService : EntityQueryService<CTBLOAD, CTBLOADKeys, CTBLOADPM, object, CTBLOADKeys>
    {
        public CTBLOADQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBLOADRepository(context);
            mapping = new CTBLOADDataMapping();
        }

        public CTBLOADPM GetSingle(string LPORTID, bool getFromCache)
        {
            var keys = new CTBLOADKeys() { LPORTID = LPORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBLOAD entityPOCO)
        {
            return new CTBLOADKeys() { LPORTID = entityPOCO.LPORTID };
        }
    }
}

