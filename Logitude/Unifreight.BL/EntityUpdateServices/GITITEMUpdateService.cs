using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;

using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GITITEMUpdateService : EntityUpdateService<GITITEM, GITITEMPM, EntityPM>
    {
        public GITITEMUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GITITEMRepository(context);

            Mapping = new GITITEMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GITITEMPM entityPM)
        {
            return new GITITEMKeys() { COUNTER = entityPM.COUNTER.ToString() };
        }

        protected override void OnCreating(GITITEMPM entityPM, EntityPM entityParentPM)
        {
        }

    }
}

