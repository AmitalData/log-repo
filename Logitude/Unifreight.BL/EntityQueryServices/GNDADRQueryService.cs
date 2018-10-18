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
    public class GNDADRQueryService : EntityQueryService<GNDADR, GNDADRKeys, GNDADRPM, GNDCARDPM, GNDCARDKeys>
    {
        public GNDADRQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GNDADRRepository(context);
            mapping = new GNDADRDataMapping();
        }

        public GNDADRPM GetSingle(string CARDID, int LINE, bool getComposition)
        {
            var keys = new GNDADRKeys() { CARDID = CARDID, LINE = LINE };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GNDADR entityPOCO)
        {
            return new GNDADRKeys() { CARDID = entityPOCO.CARDID, LINE = entityPOCO.LINE };
        }
    }
}

