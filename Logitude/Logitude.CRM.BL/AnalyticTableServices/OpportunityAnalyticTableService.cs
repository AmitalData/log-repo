using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.AnalyticTableServices;
using System;
using System.Data.Entity;

namespace Logitude.CRM.BL.AnalyticTableServices
{
    public class OpportunityAnalyticTableService : AnalyticTableService<Opportunity, OpportunityAnalytic>
    {
        public OpportunityAnalyticTableService(DbContext context) : base(context)
        {

        }

        protected override void CustomMap(Opportunity entity, OpportunityAnalytic analyticTable)
        {
           
        }
    }
}
