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
    public class MFIFILEMQueryService : EntityQueryService<MFIFILEM, MFIFILEMKeys, MFIFILEMPM, object, MFIFILEMKeys>
    {
        public MFIFILEMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new MFIFILEMRepository(context);
            mapping = new MFIFILEMDataMapping();
        }

        public MFIFILEMPM GetSingle(int FILENO, bool getFromCache)
        {
            var keys = new MFIFILEMKeys() { FILENO = FILENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(MFIFILEM entityPOCO)
        {
            return new MFIFILEMKeys() { FILENO = entityPOCO.FILENO };
        }
    }
}

