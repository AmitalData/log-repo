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
    public class CTBERRORQueryService : EntityQueryService<CTBERROR, CTBERRORKeys, CTBERRORPM, object, CTBERRORKeys>
    {
        public CTBERRORQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBERRORRepository(context);
            mapping = new CTBERRORDataMapping();
        }

        public CTBERRORPM GetSingle(string ERRORCODE, bool getFromCache)
        {
            var keys = new CTBERRORKeys() { ERRORCODE = ERRORCODE };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBERROR entityPOCO)
        {
            return new CTBERRORKeys() { ERRORCODE = entityPOCO.ERRORCODE };
        }
    }
}

