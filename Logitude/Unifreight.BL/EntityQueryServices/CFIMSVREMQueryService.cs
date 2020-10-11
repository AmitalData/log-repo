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
    public class CFIMSVREMQueryService : EntityQueryService<CFIMSVREM, CFIMSVREMKeys, CFIMSVREMPM, object, CFIMSVREMKeys>
    {
        public CFIMSVREMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFIMSVREMRepository(context);
            mapping = new CFIMSVREMDataMapping();
        }

        public CFIMSVREMPM GetSingle(long FILENO, string COMID, int PAGENUM, int TOP, int LEFT, int HEIGHT, int WIDTH, bool getFromCache)
        {

        var keys = new CFIMSVREMKeys() { FILENO = FILENO, COMID = COMID, PAGENUM = PAGENUM, TOP = TOP, LEFT = LEFT, HEIGHT = HEIGHT, WIDTH = WIDTH };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIMSVREM entityPOCO)
        {
            return new CFIMSVREMKeys() { FILENO = entityPOCO.FILENO, COMID = entityPOCO.COMID, PAGENUM = entityPOCO.PAGENUM, TOP = entityPOCO.TOP, LEFT = entityPOCO.LEFT, HEIGHT = entityPOCO.HEIGHT, WIDTH = entityPOCO.WIDTH };
        }
    }
}

