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
    public class CFIMSVFILEQueryService : EntityQueryService<CFIMSVFILE, CFIMSVFILEKeys, CFIMSVFILEPM, object, CFIMSVFILEKeys>
    {
        public CFIMSVFILEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVFILERepository(context);
            mapping = new CFIMSVFILEDataMapping();
        }

        public CFIMSVFILEPM GetSingle(long FILENO, bool getFromCache)
        {
            var keys = new CFIMSVFILEKeys() { FILENO = FILENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVFILE entityPOCO)
        {
            return new CFIMSVFILEKeys() { FILENO = entityPOCO.FILENO };
        }
    }
}

