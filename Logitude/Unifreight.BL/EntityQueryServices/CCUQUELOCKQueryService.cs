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
    public class CCUQUELOCKQueryService : EntityQueryService<CCUQUELOCK, CCUQUELOCKKeys, CCUQUELOCKPM, object, CCUQUELOCKKeys>
    {
        public CCUQUELOCKQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUQUELOCKRepository(context);
            mapping = new CCUQUELOCKDataMapping();
        }

        public CCUQUELOCKPM GetSingle(string ENTNAME, string FILENO, bool getComposition)
        {
            var keys = new CCUQUELOCKKeys() { ENTNAME = ENTNAME, FILENO = FILENO };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUQUELOCK entityPOCO)
        {
            return new CCUQUELOCKKeys() { ENTNAME = entityPOCO.ENTNAME, FILENO = entityPOCO.FILE_NO, };
        }
    }
}
