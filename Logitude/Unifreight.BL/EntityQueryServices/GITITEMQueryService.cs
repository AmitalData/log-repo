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
using Simplog.Server.Infrastructure;

namespace Unifreight.BL.EntityQueryServices
{
    public class GITITEMQueryService : EntityQueryService<GITITEM, GITITEMKeys, GITITEMPM, object, GITITEMKeys>
    {
        public GITITEMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GITITEMRepository(context);
            mapping = new GITITEMDataMapping();
        }

        public GITITEMPM GetSingle(string COUNTER, bool getFromCache)
        {
            var keys = new GITITEMKeys() { COUNTER = COUNTER };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GITITEM entityPOCO)
        {
            return new GITITEMKeys() { COUNTER = entityPOCO.COUNTER.ToString() };
        }

        public override void GetComposition(EntityKeyFields entityKeys, GITITEMPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var myGITITEMkeys = entityKeys as GITITEMKeys;

            var myGITITEMCRQueryService = new GITITEMCRQueryService(amitalContext);
            entityPM.GITITEMCRPMs = myGITITEMCRQueryService.GetMulti(myGITITEMkeys, true);
            
            base.GetComposition(entityKeys, entityPM);
        }
    }
}

