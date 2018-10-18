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
using Simplog.Server.Infrastructure;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class GNDCARDQueryService : EntityQueryService<GNDCARD, GNDCARDKeys, GNDCARDPM, object, GNDCARDKeys>
    {
        public GNDCARDQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new GNDCARDRepository(context);
            this.mapping = new GNDCARDDataMapping();
        }

        public GNDCARDPM GetSingle(string CARDID, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new GNDCARDKeys() { CARDID = CARDID };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GNDCARD entityPOCO)
        {
            return new GNDCARDKeys() { CARDID = entityPOCO.CARDID };
        }

        public override void GetComposition(EntityKeyFields entityKeys, GNDCARDPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var GNDCARDKeys = entityKeys as GNDCARDKeys;

            var GNDADRQueryService = new GNDADRQueryService(amitalContext);
            entityPM.GNDADRPMs = GNDADRQueryService.GetMulti(GNDCARDKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}

