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
    public class CTBSTORAGEQueryService : EntityQueryService<CTBSTORAGE, CTBSTORAGEKeys, CTBSTORAGEPM, object, CTBSTORAGEKeys>
    {
        public CTBSTORAGEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBSTORAGERepository(context);
            mapping = new CTBSTORAGEDataMapping();
        }

        public CTBSTORAGEPM GetSingle(string STORAGESITE, bool getFromCache)
        {
            var keys = new CTBSTORAGEKeys() { STORAGESITE = STORAGESITE };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBSTORAGE entityPOCO)
        {
            return new CTBSTORAGEKeys() { STORAGESITE = entityPOCO.STORAGESITE };
        }
    }
}

