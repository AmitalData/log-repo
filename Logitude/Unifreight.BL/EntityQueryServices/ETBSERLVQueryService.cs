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
    public class ETBSERLVQueryService : EntityQueryService<ETBSERLV, ETBSERLVKeys, ETBSERLVPM, object, ETBSERLVKeys>
    {
        public ETBSERLVQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBSERLVRepository(context);
            mapping = new ETBSERLVDataMapping();
        }

        public ETBSERLVPM GetSingle(string SERVLEVELID, bool getFromCache)
        {
            var keys = new ETBSERLVKeys() { SERVLEVELID = SERVLEVELID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBSERLV entityPOCO)
        {
            return new ETBSERLVKeys() { SERVLEVELID = entityPOCO.SERVLEVELID };
        }
    }
}

