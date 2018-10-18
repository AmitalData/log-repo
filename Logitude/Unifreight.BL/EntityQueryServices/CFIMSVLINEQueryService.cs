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
    public class CFIMSVLINEQueryService : EntityQueryService<CFIMSVLINE, CFIMSVLINEKeys, CFIMSVLINEPM, object, CFIMSVLINEKeys>
    {
        public CFIMSVLINEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVLINERepository(context);
            mapping = new CFIMSVLINEDataMapping();
        }

        public CFIMSVLINEPM GetSingle(long FILENO, string COMID, int PAGENUM, int LINENUM, bool getFromCache)
        {
            var keys = new CFIMSVLINEKeys() { FILENO = FILENO, COMID = COMID, PAGENUM = PAGENUM, LINENUM = LINENUM };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVLINE entityPOCO)
        {
            return new CFIMSVLINEKeys() { FILENO = entityPOCO.FILENO, COMID = entityPOCO.COMID, PAGENUM = entityPOCO.PAGENUM, LINENUM = entityPOCO.LINENUM };
        }
    }
}

