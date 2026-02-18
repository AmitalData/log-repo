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

    public class CCUTRANSPVALQueryService : EntityQueryService<CCUTRANSPVAL, CCUTRANSPVALKeys, CCUTRANSPVALPM, CCUFILEM, CCUFILEMKeys>
    {
        public CCUTRANSPVALQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUTRANSPVALRepository(context);
            mapping = new CCUTRANSPVALDataMapping();
        }

        public CCUTRANSPVALPM GetSingle(int FILENO, int LINENO, bool getComposition)
        {
            var keys = new CCUTRANSPVALKeys() { FILENO = FILENO, LINENO = LINENO };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUTRANSPVAL entityPOCO)
        {
            return new CCUTRANSPVALKeys() {FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO };
        }
    }
}
