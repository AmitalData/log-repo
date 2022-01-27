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
    public class ESPSPEDQueryService : EntityQueryService<ESPSPED, ESPSPEDKeys, ESPSPEDPM, object, ESPSPEDKeys>
    {
        public ESPSPEDQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ESPSPEDRepository(context);
            mapping = new ESPSPEDDataMapping();
        }

        public ESPSPEDPM GetSingle(int SPD_NO, bool getFromCache)
        {
            var keys = new ESPSPEDKeys() { SPD_NO = SPD_NO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ESPSPED entityPOCO)
        {
            return new ESPSPEDKeys() { SPD_NO = entityPOCO.SPDNO };
        }
    }
}

