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
    public class CFIMSVFLINEQueryService : EntityQueryService<CFIMSVFLINE, CFIMSVFLINEKeys, CFIMSVFLINEPM, object, CFIMSVFLINEKeys>
    {
        public CFIMSVFLINEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVFLINERepository(context);
            mapping = new CFIMSVFLINEDataMapping();
        }

        public CFIMSVFLINEPM GetSingle(long FILENO, string COMID, int LINENUM, bool getFromCache)
        {
            var keys = new CFIMSVFLINEKeys() { FILENO = FILENO, COMID = COMID, LINENUM = LINENUM };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVFLINE entityPOCO)
        {
            return new CFIMSVFLINEKeys() { FILENO = entityPOCO.FILENO, COMID = entityPOCO.COMID, LINENUM = entityPOCO.LINENUM };
        }
    }
}

