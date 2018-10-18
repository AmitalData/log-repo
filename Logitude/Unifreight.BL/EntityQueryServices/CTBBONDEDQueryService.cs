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
    public class CTBBONDEDQueryService : EntityQueryService<CTBBONDED, CTBBONDEDKeys, CTBBONDEDPM, object, CTBBONDEDKeys>
    {
        public CTBBONDEDQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBBONDEDRepository(context);
            mapping = new CTBBONDEDDataMapping();
        }

        public CTBBONDEDPM GetSingle(string WAREHOUSEID, bool getFromCache)
        {
            var keys = new CTBBONDEDKeys() { WAREHOUSEID = WAREHOUSEID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBBONDED entityPOCO)
        {
            return new CTBBONDEDKeys() { WAREHOUSEID = entityPOCO.WAREHOUSEID };
        }
    }
}

