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
    public class CTBUNLOADQueryService : EntityQueryService<CTBUNLOAD, CTBUNLOADKeys, CTBUNLOADPM, object, CTBUNLOADKeys>
    {
        public CTBUNLOADQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBUNLOADRepository(context);
            mapping = new CTBUNLOADDataMapping();
        }

        public CTBUNLOADPM GetSingle(string ULPORTID, bool getFromCache)
        {
            var keys = new CTBUNLOADKeys() { ULPORTID = ULPORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBUNLOAD entityPOCO)
        {
            return new CTBUNLOADKeys() { ULPORTID = entityPOCO.ULPORTID };
        }
    }
}

