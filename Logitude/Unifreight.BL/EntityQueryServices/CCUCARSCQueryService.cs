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

    public class CCUCARSCQueryService : EntityQueryService<CCUCARSC, CCUCARSCKeys, CCUCARSCPM, CCUCUSTITEM, CCUCUSTITEMKeys>
    {
        public CCUCARSCQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCARSCRepository(context);
            mapping = new CCUCARSCDataMapping();
        }

        public CCUCARSCPM GetSingle(int FILENO, int LINENO, int tenant,  bool getComposition)
        {
            var keys = new CCUCARSCKeys() { FILENO = FILENO, LINENO = LINENO , TENANT =tenant };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCARSC entityPOCO)
        {
            return new CCUCARSCKeys() {FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, TENANT = entityPOCO.TENANT };
        }
    }
}
