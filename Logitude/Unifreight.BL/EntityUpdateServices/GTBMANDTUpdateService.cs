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
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GTBMANDTUpdateService : EntityUpdateService<GTBMANDT, GTBMANDTPM, EntityPM>
    {
        public GTBMANDTUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBMANDTRepository(context);

            Mapping = new GTBMANDTDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBMANDTPM entityPM)
        {
            return new GTBMANDTKeys() { CLIENTCODE = entityPM.CLIENTCODE, FORMNAME = entityPM.FORMNAME, ENTITY = entityPM.ENTITY, FIELDNAME = entityPM.FIELDNAME };
        }

        protected override void OnCreating(GTBMANDTPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}
