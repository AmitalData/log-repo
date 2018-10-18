using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial class OpportunityProductUpdateService
    {
        protected override void OnUpdating(OpportunityProductPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
            {
                OpportunityProductKeys entityKeys = new OpportunityProductKeys()
                {
                    OpportunityId = entityPM.OpportunityId,
                    OpportunityProductTypeCode = entityPM.OpportunityProductTypeCode,                    
                };

                OpportunityProductLocationRepository repository1 = new OpportunityProductLocationRepository(entityPM.Tenant);
                List<OpportunityProductLocation> list = repository1.GetMulti(entityKeys);
                foreach (OpportunityProductLocation item in list)
                {
                    repository1.Remove(item);
                }

                repository1.SubmitChanges();
            }
        }

        protected override void UpdateComposition(OpportunityProductPM entityPM)
        {
            OpportunityProductLocationUpdateService productLocationsUpdateService = new OpportunityProductLocationUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            productLocationsUpdateService.UpdateMulti(entityPM.OpportunityProductLocations, entityPM.DeletedOpportunityProductLocations, entityPM, false);
        }
    }
}
