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
    public class CTBPKDTQueryService : EntityQueryService<CTBPKDT, CTBPKDTKeys, CTBPKDTPM, object, CTBPKDTKeys>
    {
        public CTBPKDTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBPKDTRepository(context);
            mapping = new CTBPKDTDataMapping();
        }

        public CTBPKDTPM GetSingle(string PACKDETAIL, bool getFromCache)
        {
            var keys = new CTBPKDTKeys() { PACKDETAIL = PACKDETAIL };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBPKDT entityPOCO)
        {
            return new CTBPKDTKeys() { PACKDETAIL = entityPOCO.PACKDETAIL };
        }
    }
}


