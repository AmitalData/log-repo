using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestBasesTypeUpdateService
    {
        protected override void OnCreating(InterestBasesTypePM entityPM, EntityPM entityParentPM)
        {
          
        }

        protected override void OnUpdating(InterestBasesTypePM entityPM, InterestBasesType entityPOCO)
        {
        }

        protected override void UpdateComposition(InterestBasesTypePM entityPM)
        {
            InterestBasesPeriodUpdateService mementoLineUpdateService = new InterestBasesPeriodUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            mementoLineUpdateService.UpdateMulti(entityPM.MementoLines, entityPM.DeletedMementoLines, entityPM, false);
        }
    }
}
