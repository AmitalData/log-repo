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
    public class GTBITEMSMSVQueryService : EntityQueryService<GTBITEMSMSV, GTBITEMSMSVKeys, GTBITEMSMSVPM, object, GTBITEMSMSVKeys>
    {
        public GTBITEMSMSVQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBITEMSMSVRepository(context);
            mapping = new GTBITEMSMSVDataMapping();
        }

        public GTBITEMSMSVPM GetSingle(string PRATID, bool getFromCache)
        {
            var keys = new GTBITEMSMSVKeys() { PRATID = PRATID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBITEMSMSV entityPOCO)
        {
            return new GTBITEMSMSVKeys() { PRATID = entityPOCO.PRATID };
        }
    }
}

