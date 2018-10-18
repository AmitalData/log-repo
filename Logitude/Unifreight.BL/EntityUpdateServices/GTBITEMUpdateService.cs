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
    public class GTBITEMUpdateService : EntityUpdateService<GTBITEM, GTBITEMPM, EntityPM>
    {
        public GTBITEMUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBITEMRepository(context);

            Mapping = new GTBITEMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBITEMPM entityPM)
        {
            return new GTBITEMKeys() { PARTNERID = entityPM.PARTNERID, ITEMID = entityPM.ITEMID };
        }

        protected override void OnCreating(GTBITEMPM entityPM, EntityPM entityParentPM)
        {
        }

    }
}

