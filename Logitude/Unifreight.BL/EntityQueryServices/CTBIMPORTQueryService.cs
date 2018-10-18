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
    public class CTBIMPORTQueryService : EntityQueryService<CTBIMPORT, CTBIMPORTKeys, CTBIMPORTPM, object, CTBIMPORTKeys>
    {
        public CTBIMPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBIMPORTRepository(context);
            mapping = new CTBIMPORTDataMapping();
        }

        public CTBIMPORTPM GetSingle(string IMPORTERID, bool getFromCache)
        {
            var keys = new CTBIMPORTKeys() { IMPORTERID = IMPORTERID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBIMPORT entityPOCO)
        {
            return new CTBIMPORTKeys() { IMPORTERID = entityPOCO.IMPORTERID };
        }
    }
}

