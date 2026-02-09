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

    public class CCUCARLQueryService : EntityQueryService<CCUCARL, CCUCARLKeys, CCUCARLPM, CCUCUSTITEM, CCUCUSTITEMKeys>
    {
        public CCUCARLQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCARLRepository(context);
            mapping = new CCUCARLDataMapping();
        }

        public CCUCARLPM GetSingle(int FILENO, int LINENO, int tenant, bool getComposition)
        {
            var keys = new CCUCARLKeys() { FILENO = FILENO, LINENO = LINENO , TENANT=tenant  };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCARL entityPOCO)
        {
            return new CCUCARLKeys() {FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, TENANT = entityPOCO.TENANT };
        }
    }
}
