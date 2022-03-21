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
    public class CFIMSVSTATLQueryService : EntityQueryService<CFIMSVSTATL, CFIMSVSTATLKeys, CFIMSVSTATLPM, object, CFIMSVSTATLKeys>
    {
        public CFIMSVSTATLQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVSTATLRepository(context);
            mapping = new CFIMSVSTATLDataMapping();
        }

        public CFIMSVSTATLPM GetSingle(string GUID, bool getFromCache)
        {
            var keys = new CFIMSVSTATLKeys() { GUID = GUID,};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVSTATL entityPOCO)
        {
            return new CFIMSVSTATLKeys() { GUID = entityPOCO.GUID};
        }
    }
}

