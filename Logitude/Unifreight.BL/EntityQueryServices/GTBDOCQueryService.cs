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

    public class GTBDOCQueryService : EntityQueryService<GTBDOC, GTBDOCKeys, GTBDOCPM, object, GTBDOCKeys>
    {
        public GTBDOCQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBDOCRepository(context);
            mapping = new GTBDOCDataMapping();
        }

        public GTBDOCPM GetSingle(string DOCID, bool getComposition)
        {
            var keys = new GTBDOCKeys() { DOCID = DOCID };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBDOC entityPOCO)
        {
            return new GTBDOCKeys() { DOCID = entityPOCO.DOCID };
        }
    }
}
