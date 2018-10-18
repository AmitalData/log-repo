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

    public class GDMFILEVERQueryService : EntityQueryService<GDMFILEVER, GDMFILEVERKeys, GDMFILEVERPM, GDMFILINGPM, GDMFILINGKeys>
    {
        public GDMFILEVERQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDMFILEVERRepository(context);
            mapping = new GDMFILEVERDataMapping();
        }



        public GDMFILEVERPM GetSingle(string COMID, int VERSION  )
        {
            var keys = new GDMFILEVERKeys() { COMID = COMID, VERSION = VERSION };
            return base.GetSingle(keys, false, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDMFILEVER entityPOCO)
        {
            return new GDMFILEVERKeys() { COMID = entityPOCO.COMID, VERSION = entityPOCO.VERSION };
        }

        
    }
}


