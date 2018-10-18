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
    public class CCUSIGNUMQueryService : EntityQueryService<CCUSIGNUM, CCUSIGNUMKeys, CCUSIGNUMPM, CCUMSHGRPM , CCUMSHGRKeys>
    {        
        public CCUSIGNUMQueryService(AmitalContext context)
        {
            MainContext  = context;
            Repository = new CCUSIGNUMRepository(context);
            mapping = new CCUSIGNUMDataMapping();
        }

        public CCUSIGNUMPM GetSingle(int FILENO, int LINENOMSHGR, int LINENOSIGN, bool getComposition)
        {
            var keys = new CCUSIGNUMKeys() { FILENO = FILENO, LINENOMSHGR = LINENOMSHGR, LINENOSIGN = LINENOSIGN };
            return  base.GetSingle(keys , getComposition ,false );        
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUSIGNUM entityPOCO)
        {
            return new CCUSIGNUMKeys() { FILENO = entityPOCO.FILENO, LINENOMSHGR = entityPOCO.LINENOMSHGR, LINENOSIGN = entityPOCO.LINENOSIGN };
        }
    }
}
