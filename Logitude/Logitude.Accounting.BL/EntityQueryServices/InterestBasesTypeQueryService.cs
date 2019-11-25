using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class InterestBasesTypeQueryService
    {

        public override void GetComposition(EntityKeyFields entityKeys, InterestBasesTypePM entityPM)
        {
            IAccountingContext context = MainContext as IAccountingContext;
            InterestBasesTypeKeys activityKeys = entityKeys as InterestBasesTypeKeys;
            InterestBasesPeriodQueryService queryService = new InterestBasesPeriodQueryService(context);
            entityPM.InterestBasesPeriods = queryService.GetMulti(activityKeys, true);
        }
    }
}
