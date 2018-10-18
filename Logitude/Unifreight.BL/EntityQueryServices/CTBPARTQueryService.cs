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
    public class CTBPARTQueryService : EntityQueryService<CTBPART, CTBPARTKeys, CTBPARTPM, object, CTBPARTKeys>
    {
        public CTBPARTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBPARTRepository(context);
            mapping = new CTBPARTDataMapping();
        }

        public CTBPARTPM GetSingle(string PARTIALITYID, bool getFromCache)
        {
            var keys = new CTBPARTKeys() { PARTIALITYID = PARTIALITYID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBPART entityPOCO)
        {
            return new CTBPARTKeys() { PARTIALITYID = entityPOCO.PARTIALITYID };
        }
    }
}


