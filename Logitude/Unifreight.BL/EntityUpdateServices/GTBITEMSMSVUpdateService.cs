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
    public class GTBITEMSMSVUpdateService : EntityUpdateService<GTBITEMSMSV, GTBITEMSMSVPM, EntityPM>
    {
        public GTBITEMSMSVUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBITEMSMSVRepository(context);

            Mapping = new GTBITEMSMSVDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBITEMSMSVPM entityPM)
        {
            return new GTBITEMSMSVKeys() { PRATID = entityPM.PRATID};
        }

        protected override void OnCreating(GTBITEMSMSVPM entityPM, EntityPM entityParentPM)
        {
        }

    }
}

