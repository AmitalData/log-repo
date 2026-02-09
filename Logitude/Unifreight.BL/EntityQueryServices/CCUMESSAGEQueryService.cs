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
    public class CCUMESSAGEQueryService : EntityQueryService<CCUMESSAGE, CCUMESSAGEKeys, CCUMESSAGEPM, CCUFILEMPM , CCUFILEMKeys>
    {        
        public CCUMESSAGEQueryService(AmitalContext context)
        {
            MainContext  = context;
            Repository = new CCUMESSAGERepository(context);
            mapping = new CCUMESSAGEDataMapping();
        }

        public CCUMESSAGEPM GetSingle(int FILENO, int LINENO,int tenant,  bool getComposition)
        {
            var keys = new CCUMESSAGEKeys() { FILENO = FILENO, LINENO = LINENO , Tenant= tenant };
            return  base.GetSingle(keys , getComposition ,false );        
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUMESSAGE entityPOCO)
        {
            return new CCUMESSAGEKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, Tenant = entityPOCO.TENANT };
        }
    }
}
