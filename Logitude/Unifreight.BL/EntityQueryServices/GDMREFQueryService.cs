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
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{

    public class GDMREFQueryService : EntityQueryService<GDMREF, GDMREFKeys, GDMREFPM, object, GDMREFKeys>
    {
        public GDMREFQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDMREFRepository(context);
            mapping = new GDMREFDataMapping();
        }

        public GDMREFPM GetSingle(string COMID, string REFID, string REFERENCE, bool getComposition)
        {
            var keys = new GDMREFKeys() { COMID = COMID, REFID = REFID, REFERENCE = REFERENCE };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDMREF entityPOCO)
        {
            return new GDMREFKeys() { COMID = entityPOCO.COMID, REFID = entityPOCO.REFID, REFERENCE = entityPOCO.REFERENCE };
        }
    }
}
