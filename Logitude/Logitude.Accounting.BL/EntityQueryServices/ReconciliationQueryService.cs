using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logitude.Accounting.Data.Repositories;


namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ReconciliationQueryService : EntityQueryService<Reconciliation,ReconciliationKeys,ReconciliationPM,object,ReconciliationKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ReconciliationPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            ReconciliationKeys reconciliationKeys = entityKeys as ReconciliationKeys;
            ReconciliationLineQueryService reconciliationLineQueryService = new ReconciliationLineQueryService(context);
            entityPM.ReconciliationLines = reconciliationLineQueryService.GetMulti(reconciliationKeys, true);

            //if (entityPM.ReconciliationLines.Count > 0)
            //{
            //    entityPM.LastLineNumber = entityPM.ReconciliationLines.Max(m => m.Line);
            //}

            base.GetComposition(entityKeys, entityPM);
        }

        public List<ReconciliationPM> GetReconciliationsByIds(List<string> recoIds, int tenant)
        {
            List<Reconciliation> recoLines = (from a in context.Reconciliations
                                                  where recoIds.Contains(a.Id) && a.Tenant == tenant
                                                  select a).ToList();

            List<ReconciliationPM> pms = recoLines.Select(poco => GetEntityPM(poco)).ToList();

            return pms;
        }
        public ReconciliationPM GetByNumber(string number, int tenant)
        {
            Reconciliation reco = (from a in context.Reconciliations
                                              where a.Number == number && a.Tenant == tenant
                                              select a).FirstOrDefault();

            ReconciliationPM pm = GetEntityPM(reco);

            return pm;
        }

    }// class ReconciliationQueryService
}
