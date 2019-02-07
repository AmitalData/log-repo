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
    public class CFIMSVREMUpdateService : EntityUpdateService<CFIMSVREM, CFIMSVREMPM, EntityPM>
    {
        public CFIMSVREMUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVREMRepository(context);

            Mapping = new CFIMSVREMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVREMPM entityPM)
        {
            return new CFIMSVREMKeys() { FILENO = entityPM.FILENO, COMID = entityPM.COMID, PAGENUM = entityPM.PAGENUM, TOP = entityPM.TOP, LEFT = entityPM.LEFT, HEIGHT = entityPM.HEIGHT, WIDTH = entityPM.WIDTH };
        }

        protected override void OnCreating(CFIMSVREMPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}

