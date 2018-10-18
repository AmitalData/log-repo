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
    public class CTBAPPROVQueryService : EntityQueryService<CTBAPPROV, CTBAPPROVKeys, CTBAPPROVPM, object, CTBAPPROVKeys>
    {
        public CTBAPPROVQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBAPPROVRepository(context);
            mapping = new CTBAPPROVDataMapping();
        }

        public CTBAPPROVPM GetSingle(string APPROVCODEID, bool getFromCache)
        {
            var keys = new CTBAPPROVKeys() { APPROVCODEID = APPROVCODEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBAPPROV entityPOCO)
        {
            return new CTBAPPROVKeys() { APPROVCODEID = entityPOCO.APPROVCODEID };
        }
    }
}

