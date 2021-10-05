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
    public class ETBVENDQueryService : EntityQueryService<ETBVEND, ETBVENDKeys, ETBVENDPM, object, ETBVENDKeys>
    {
        public ETBVENDQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBVENDRepository(context);
            mapping = new ETBVENDDataMapping();
        }

        public ETBVENDPM GetSingle(string VENDORID, bool getFromCache)
        {
            var keys = new ETBVENDKeys() { VENDORID = VENDORID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBVEND entityPOCO)
        {
            return new ETBVENDKeys() { VENDORID = entityPOCO.VENDORID };
        }
    }
}

