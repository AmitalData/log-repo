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
    public class ATBPTILQueryService : EntityQueryService<ATBPTIL, ATBPTILKeys, ATBPTILPM, object, ATBPTILKeys>
    {
        public ATBPTILQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ATBPTILRepository(context);
            mapping = new ATBPTILDataMapping();
        }

        public ATBPTILPM GetSingle(string BRANID, bool getFromCache)
        {
            var keys = new ATBPTILKeys() { BRANID = BRANID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ATBPTIL entityPOCO)
        {
            return new ATBPTILKeys() { BRANID = entityPOCO.BRANID };
        }
    }
}

