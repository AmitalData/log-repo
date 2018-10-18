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
    public class CTBINCOTERMQueryService : EntityQueryService<CTBINCOTERM, CTBINCOTERMKeys, CTBINCOTERMPM, object, CTBINCOTERMKeys>
    {
        public CTBINCOTERMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBINCOTERMRepository(context);
            mapping = new CTBINCOTERMDataMapping();
        }

        public CTBINCOTERMPM GetSingle(string PTERMID, bool getFromCache)
        {
            var keys = new CTBINCOTERMKeys() { PTERMID = PTERMID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBINCOTERM entityPOCO)
        {
            return new CTBINCOTERMKeys() { PTERMID = entityPOCO.PTERMID };
        }
    }
}

