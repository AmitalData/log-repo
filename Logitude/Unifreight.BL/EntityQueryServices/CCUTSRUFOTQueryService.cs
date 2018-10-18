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

    public class CCUTSRUFOTQueryService : EntityQueryService<CCUTSRUFOT, CCUTSRUFOTKeys, CCUTSRUFOTPM, CCUFILEM, CCUFILEMKeys>
    {
        public CCUTSRUFOTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUTSRUFOTRepository(context);
            mapping = new CCUTSRUFOTDataMapping();
        }

        public CCUTSRUFOTPM GetSingle(int FILENO, int LINENO, bool getComposition)
        {
            var keys = new CCUTSRUFOTKeys() { FILENO = FILENO, LINENO = LINENO };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUTSRUFOT entityPOCO)
        {
            return new CCUTSRUFOTKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO };
        }
    }
}
