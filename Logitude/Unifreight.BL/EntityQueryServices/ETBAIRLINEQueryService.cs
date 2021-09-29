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
    public class ETBAIRLINEQueryService : EntityQueryService<ETBAIRLINE, ETBAIRLINEKeys, ETBAIRLINEPM, object, ETBAIRLINEKeys>
    {
        public ETBAIRLINEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBAIRLINERepository(context);
            mapping = new ETBAIRLINEDataMapping();
        }

        public ETBAIRLINEPM GetSingle(string AIRLINEID, bool getFromCache)
        {
            var keys = new ETBAIRLINEKeys() { AIRLINEID = AIRLINEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBAIRLINE entityPOCO)
        {
            return new ETBAIRLINEKeys() { AIRLINEID = entityPOCO.AIRLINEID };
        }
    }
}

