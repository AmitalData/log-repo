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
    public class GTRTRANUpdateService : EntityUpdateService<GTRTRAN, GTRTRANPM, EntityPM>
    {
        public GTRTRANUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTRTRANRepository(context);

            Mapping = new GTRTRANDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTRTRANPM entityPM)
        {
            return new GTRTRANKeys() { PARTNERID = entityPM.PARTNERID, TABLEID = entityPM.TABLEID, PARTNERCODE = entityPM.PARTNERCODE, LOCALCODE = entityPM.LOCALCODE };
        }

        protected override void OnCreating(GTRTRANPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}
