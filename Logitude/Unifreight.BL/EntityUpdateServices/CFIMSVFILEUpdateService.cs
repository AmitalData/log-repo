using System;
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
    public class CFIMSVFILEUpdateService : EntityUpdateService<CFIMSVFILE, CFIMSVFILEPM, EntityPM>
    {
        public CFIMSVFILEUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVFILERepository(context);

            Mapping = new CFIMSVFILEDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVFILEPM entityPM)
        {
            return new CFIMSVFILEKeys() { FILENO = entityPM.FILENO };
        }

        protected override void OnCreating(CFIMSVFILEPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}

