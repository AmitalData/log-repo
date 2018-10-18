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
    public class CTBPACKTYPEQueryService : EntityQueryService<CTBPACKTYPE, CTBPACKTYPEKeys, CTBPACKTYPEPM, object, CTBPACKTYPEKeys>
    {
        public CTBPACKTYPEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBPACKTYPERepository(context);
            mapping = new CTBPACKTYPEDataMapping();
        }

        public CTBPACKTYPEPM GetSingle(string PACKTYPEID, bool getFromCache)
        {
            var keys = new CTBPACKTYPEKeys() { PACKTYPEID = PACKTYPEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBPACKTYPE entityPOCO)
        {
            return new CTBPACKTYPEKeys() { PACKTYPEID = entityPOCO.PACKTYPEID };
        }
    }
}

