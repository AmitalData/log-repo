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

    public class CCUTAXQueryService : EntityQueryService<CCUTAX, CCUTAXKeys, CCUTAXPM, CCUFILEMPM, CCUFILEMKeys>
    {
        public CCUTAXQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUTAXRepository(context);
            mapping = new CCUTAXDataMapping();
        }

        public CCUTAXPM GetSingle(int FILENO, int LINENO, bool getComposition)
        {
            var keys = new CCUTAXKeys() { FILENO = FILENO, LINENO = LINENO };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUTAX entityPOCO)
        {
            return new CCUTAXKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO };
        }
    }
}
