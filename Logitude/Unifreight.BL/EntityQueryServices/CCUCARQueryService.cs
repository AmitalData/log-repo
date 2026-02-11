
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

    public class CCUCARQueryService : EntityQueryService<CCUCAR, CCUCARKeys, CCUCARPM, CCUCUSTITEM, CCUCUSTITEMKeys>
    {
        public CCUCARQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCARRepository(context);
            mapping = new CCUCARDataMapping();
        }

        public CCUCARPM GetSingle(int FILENO, int LINENO,int tenant, bool getComposition)
        {
            var keys = new CCUCARKeys() { FILENO = FILENO, LINENO = LINENO, Tenant= tenant };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCAR entityPOCO)
        {
            return new CCUCARKeys() {FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO , Tenant= entityPOCO.TENANT };
        }
    }
}
