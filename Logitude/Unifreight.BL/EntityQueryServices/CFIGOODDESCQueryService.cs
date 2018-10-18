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
    public class CFIGOODDESCQueryService : EntityQueryService<CFIGOODDESC, CFIGOODDESCKeys, CFIGOODDESCPM, object, CFIGOODDESCKeys>
    {
        public CFIGOODDESCQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIGOODDESCRepository(context);
            mapping = new CFIGOODDESCDataMapping();
        }

        public CFIGOODDESCPM GetSingle(long FILENO, bool getFromCache)
        {
            var keys = new CFIGOODDESCKeys() { FILENO = FILENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIGOODDESC entityPOCO)
        {
            return new CFIGOODDESCKeys() { FILENO = entityPOCO.FILENO };
        }
    }
}

