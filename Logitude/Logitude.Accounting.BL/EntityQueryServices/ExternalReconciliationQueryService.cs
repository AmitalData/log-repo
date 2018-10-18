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
    public partial class ExternalReconciliationQueryService : EntityQueryService<ExternalReconciliation,ExternalReconciliationKeys,ExternalReconciliationPM,object,ExternalReconciliationKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ExternalReconciliationPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            ExternalReconciliationKeys reconciliationKeys = entityKeys as ExternalReconciliationKeys;
            ExternalReconciliationLineQueryService reconciliationLineQueryService = new ExternalReconciliationLineQueryService(context);
            entityPM.ExternalReconciliationLines = reconciliationLineQueryService.GetMulti(reconciliationKeys, true);
            base.GetComposition(entityKeys, entityPM);
        }

        public List<LedgerTransaction> GetLedgerTransactionByIds(List<string> ledgerIds, int tenant)
        {
            IAccountingContext context = MainContext as AccountingContext;
            var query = (from a in context.LedgerTransactions
                         where a.Tenant == tenant && ledgerIds.Contains(a.Id)
                         select a).ToList();
            return query;
        }
        public List<ReconcileExternalPageLine> GetBankPagesByIds(List<string> linesIds, int tenant)
        {
            IAccountingContext context = MainContext as AccountingContext;
            var query = (from a in context.ReconcileExternalPageLines
                         where a.Tenant == tenant && linesIds.Contains(a.Id)
                         select a).ToList();
            return query;
        }





    }// class ExternalReconciliationQueryService
}
