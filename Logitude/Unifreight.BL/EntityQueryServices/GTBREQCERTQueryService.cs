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
    public class GTBREQCERTQueryService : EntityQueryService<GTBREQCERT, GTBREQCERTKeys, GTBREQCERTPM, object, GTBREQCERTKeys>
    {
        public GTBREQCERTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBREQCERTRepository(context);
            mapping = new GTBREQCERTDataMapping();
        }

        public GTBREQCERTPM GetSingle(string REQCERT, string ENTITY, bool getFromCache)
        {
            var keys = new GTBREQCERTKeys() { REQCERT = REQCERT, ENTITY = ENTITY };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBREQCERT entityPOCO)
        {
            return new GTBREQCERTKeys() { REQCERT = entityPOCO.REQCERT, ENTITY = entityPOCO.ENTITY };
        }
    }
}


