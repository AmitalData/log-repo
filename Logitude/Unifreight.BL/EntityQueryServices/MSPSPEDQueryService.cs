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
    public class MSPSPEDQueryService : EntityQueryService<MSPSPED, MSPSPEDKeys, MSPSPEDPM, object, MSPSPEDKeys>
    {
        public MSPSPEDQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new MSPSPEDRepository(context);
            mapping = new MSPSPEDDataMapping();
        }

        public MSPSPEDPM GetSingle(int SPDNO, bool getFromCache)
        {
            var keys = new MSPSPEDKeys() { SPDNO = SPDNO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(MSPSPED entityPOCO)
        {
            return new MSPSPEDKeys() { SPDNO = entityPOCO.SPDNO };
        }
    }
}

