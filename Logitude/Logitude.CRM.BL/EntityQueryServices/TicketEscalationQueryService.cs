using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class TicketEscalationQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TicketEscalationPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            TicketEscalationKeys TicketEscalationKeys = entityKeys as TicketEscalationKeys;
        }

        public TicketEscalationPM GetSinglePMByTenant(int tenant)
        {
            ICRMContext context = MainContext as ICRMContext;
            TicketEscalation entity = repository.GetSingleByTenant(tenant);

            if (entity != null)
            {
                EntityPM = new TicketEscalationPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
                this.GetComposition(new TicketEscalationKeys() { Id = EntityPM.Id }, EntityPM);
            }

            return EntityPM;
        }
    }
}
