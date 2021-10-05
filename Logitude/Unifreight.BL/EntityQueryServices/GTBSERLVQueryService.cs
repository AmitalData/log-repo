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
    public class GTBSERLVQueryService : EntityQueryService<GTBSERLV, GTBSERLVKeys, GTBSERLVPM, object, GTBSERLVKeys>
    {
        public GTBSERLVQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBSERLVRepository(context);
            mapping = new GTBSERLVDataMapping();
        }

        public GTBSERLVPM GetSingle(string SERVLEVELID, bool getFromCache)
        {
            var keys = new GTBSERLVKeys() { SERVLEVELID = SERVLEVELID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBSERLV entityPOCO)
        {
            return new GTBSERLVKeys() { SERVLEVELID = entityPOCO.SERVLEVELID };
        }
    }
}

