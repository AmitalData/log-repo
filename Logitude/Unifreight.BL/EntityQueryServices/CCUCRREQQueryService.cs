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

    public class CCUCRREQQueryService : EntityQueryService<CCUCRREQ, CCUCRREQKeys, CCUCRREQPM, CCUSUPITEM, CCUSUPITEMKeys>
    {
        public CCUCRREQQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCRREQRepository(context);
            mapping = new CCUCRREQDataMapping();
        }

        public CCUCRREQPM GetSingle(int FILENO, int LINENO, bool getComposition)
        {
            var keys = new CCUCRREQKeys() { FILENO = FILENO, LINENO = LINENO };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCRREQ entityPOCO)
        {
            return new CCUCRREQKeys() { ENTNAME = entityPOCO.ENTNAME, FILENO = entityPOCO.FILENO, ACCLINENO = entityPOCO.ACCLINENO, ITEMLINE = entityPOCO.ITEMLINE, LINENO = entityPOCO.LINENO };
        }

        public List<CCUCRREQPM> GetFiles105Documents(int FILENO, int ACCLINENO, int ITEMLINE)
        {
            var listPoco = (this.Repository as CCUCRREQRepository).GetFiles105Documents(FILENO, ACCLINENO, ITEMLINE);
            var listPM = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return listPM;
        }
    }
}
