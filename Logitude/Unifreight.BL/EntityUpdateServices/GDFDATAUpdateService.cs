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
    public class GDFDATAUpdateService : EntityUpdateService<GDFDATA, GDFDATAPM, EntityPM>
    {
        public GDFDATAUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDFDATARepository(context);

            Mapping = new GDFDATADataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDFDATAPM entityPM)
        {
            return new GDFDATAKeys() { DISTRID = entityPM.DISTRID, DEFID = entityPM.DEFID, BRANCHID = entityPM.BRANCHID, CARDID = entityPM.CARDID };
        }

        protected override void OnCreating(GDFDATAPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}

