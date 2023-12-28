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
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityQueryServices;

namespace Unifreight.BL.EntityUpdateServices
{
    public class YCULTASKUpdateService : EntityUpdateService<YCULTASK, YCULTASKPM, EntityPM>
    {         
        public YCULTASKUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new YCULTASKRepository(context);

            Mapping = new YCULTASKDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
       
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(YCULTASKPM entityPM)
        {
            return new YCULTASKKeys() { TASKID = entityPM.TASKID };
        }

        protected override void OnCreating(YCULTASKPM entityPM, EntityPM entityParentPM)
        {
            //entityPM.LOGTIME = DateTime.Now;
            entityPM.LOGTIME  = DateTime.Now;
            //if (string.IsNullOrWhiteSpace(entityPM.TASKID))
            {
                entityPM.TASKID = CommCounterUtil.GetUnique30(entityPM.LOGTIME);
            }
            ///entityPM.COMPUTERID = Environment.MachineName;

        }
    }
}
