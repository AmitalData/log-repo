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

    public class GDMFILINGQueryService : EntityQueryService<GDMFILING, GDMFILINGKeys, GDMFILINGPM, object, GDMFILINGKeys>
    {
        public GDMFILINGQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDMFILINGRepository(context);
            mapping = new GDMFILINGDataMapping();
        }

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, GDMFILINGPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var myGDMFILINGKeys = entityKeys as GDMFILINGKeys;

            var myGDMFILEVERRepository = new GDMFILEVERQueryService(amitalContext);
            entityPM.GDMFILEVERs = myGDMFILEVERRepository.GetMulti(myGDMFILINGKeys, false);
            
        }

        public GDMFILINGPM GetSingle(string COMID, bool getComposition)
        {
            var keys = new GDMFILINGKeys() { COMID = COMID };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDMFILING entityPOCO)
        {
            return new GDMFILINGKeys() { COMID = entityPOCO.COMID };
        }
    }
}
