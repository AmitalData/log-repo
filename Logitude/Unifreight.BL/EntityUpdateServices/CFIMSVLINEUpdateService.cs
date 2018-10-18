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
    public class CFIMSVLINEUpdateService : EntityUpdateService<CFIMSVLINE, CFIMSVLINEPM, EntityPM>
    {
        public CFIMSVLINEUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVLINERepository(context);

            Mapping = new CFIMSVLINEDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVLINEPM entityPM)
        {
            return new CFIMSVLINEKeys() { FILENO = entityPM.FILENO, COMID = entityPM.COMID, PAGENUM = entityPM.PAGENUM, LINENUM = entityPM.LINENUM };
        }

        protected override void OnCreating(CFIMSVLINEPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}

