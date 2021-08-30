using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GGGQCUpdateService : EntityUpdateService<GGGQC, GGGQCPM, GGGQPM>
    {
        public GGGQCUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GGGQCRepository(context);

            Mapping = new GGGQCDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GGGQCPM entityPM)
        {
            return new GGGQCKeys() { QUEID = entityPM.QUEID };
        }

        protected override void OnCreating(GGGQCPM entityPM, GGGQPM entityParentPM)
        {
            entityPM.QUEID = entityParentPM.QUEID;
        }

        
    }
}
