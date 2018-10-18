using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class CFIMSVPAGEQueryService : EntityQueryService<CFIMSVPAGE, CFIMSVPAGEKeys, CFIMSVPAGEPM, object, CFIMSVPAGEKeys>
    {
        public CFIMSVPAGEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVPAGERepository(context);
            mapping = new CFIMSVPAGEDataMapping();
        }

        public CFIMSVPAGEPM GetSingle(long FILENO, bool getFromCache)
        {
            var keys = new CFIMSVPAGEKeys() { FILENO = FILENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVPAGE entityPOCO)
        {
            return new CFIMSVPAGEKeys() { FILENO = entityPOCO.FILENO };
        }
    }
}

