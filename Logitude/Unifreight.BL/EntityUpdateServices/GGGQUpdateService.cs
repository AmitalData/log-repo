using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GGGQUpdateService : EntityUpdateService<GGGQ, GGGQPM, EntityPM>
    {      
        public GGGQUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GGGQRepository(context);

            Mapping = new GGGQDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GGGQPM entityPM)
        {
            return new GGGQKeys() { QUEID = entityPM.QUEID };
        }

        protected override void OnCreating(GGGQPM entityPM, EntityPM entityParentPM)
        {
            entityPM.CREATEDATE = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now;
            entityPM.QUEID = CommCounterUtil.GetUnique30(entityPM.CREATEDATE);
            
            ///entityPM.COMPUTERID = Environment.MachineName;            
        }

        protected override void UpdateComposition(GGGQPM entityPM)
        {
            var myGGGQCUpdateService = new GGGQCUpdateService(this.MainContext as AmitalContext);
            myGGGQCUpdateService.UpdateMulti(entityPM.GGGQCPMs, new List<GGGQCPM>(), entityPM, true);
        }
    }
}
