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
    public partial class SLAEscalationQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, SLAEscalationPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            SLAEscalationKeys SLAEscalationKeys = entityKeys as SLAEscalationKeys;

            SLAEscalationRecepientQueryService service = new SLAEscalationRecepientQueryService(context);
            entityPM.SLAEscalationRecepients = service.GetMulti(SLAEscalationKeys, true);
        }

        public SLAEscalationPM GetSinglePMByTenant(int tenant)
        {
            ICRMContext context = MainContext as ICRMContext;
            SLAEscalation entity = repository.GetSingleByTenant(tenant);

            if (entity != null)
            {
                EntityPM = new SLAEscalationPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
                this.GetComposition(new SLAEscalationKeys() { Id = EntityPM.Id }, EntityPM);
            }

            return EntityPM;
        }


        public List<SLAEscalationPM> GetSLAEscalationByHeaderId(string headerId, int tenant)
        {
            List<SLAEscalationPM> result = new List<SLAEscalationPM>();
            List<SLAEscalation> activityHistories = repository.GetSLAEscalationsBySLAHeaderId(headerId, tenant);

            foreach (SLAEscalation entity in activityHistories)
            {
                EntityPM = new SLAEscalationPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            return result;
        }
    }
}
