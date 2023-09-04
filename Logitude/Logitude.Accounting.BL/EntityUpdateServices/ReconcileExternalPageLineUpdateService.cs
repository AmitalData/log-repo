using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.Validators;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconcileExternalPageLineUpdateService : EntityUpdateService<ReconcileExternalPageLine, ReconcileExternalPageLinePM, ReconcileExternalPagePM>
    {
        protected override void OnCreating(ReconcileExternalPageLinePM entityPM, ReconcileExternalPagePM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "")
                entityPM.Id = IdCounter.GetNumber("ReconcileExternalPageLine", entityPM.Tenant);

            entityPM.ReconcileExternalPageId = entityParentPM.Id;
            //this.UpdateBankAccount(entityPM);
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(ReconcileExternalPageLinePM entityPM, ReconcileExternalPageLine entityPOCO)
        {
            if(entityPM.IsReconciled != entityPOCO.IsReconciled)
            {
                //this.UpdateBankAccount(entityPM);
            }
            base.OnUpdating(entityPM, entityPOCO);
        }

        internal void UpdateBankAccount(ReconcileExternalPageLinePM entityPM)
        {
            BankAccountQueryService qs = new BankAccountQueryService((MainContext as IAccountingContext));
            ReconcileExternalPageQueryService reconcileExternalPageQs = new ReconcileExternalPageQueryService((MainContext as IAccountingContext));
            var reconcileExternalPage = reconcileExternalPageQs.GetSingle(entityPM.ReconcileExternalPageId, false, false);
            if (reconcileExternalPage != null)
            {
                BankAccountUpdateService service = new BankAccountUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                var bankAccount = qs.GetByGLAccountId(reconcileExternalPage.GLAccountId, entityPM.Tenant);
                if (bankAccount != null)
                {
                    var totalOpenPagesLines = 0;
                    if (entityPM.IsReconciled)
                    {
                        Int32.TryParse(bankAccount.TotalOpenPagesLines, out totalOpenPagesLines);
                        if (totalOpenPagesLines > 0)
                        {
                            totalOpenPagesLines--;
                        }
                        bankAccount.TotalOpenPagesLines = totalOpenPagesLines.ToString();
                    }
                    else
                    {
                        Int32.TryParse(bankAccount.TotalOpenExternalTransactions, out totalOpenPagesLines);
                        if (totalOpenPagesLines > 0)
                        {
                            totalOpenPagesLines++;
                        }
                        bankAccount.TotalOpenPagesLines = totalOpenPagesLines.ToString();
                    }
                    bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                    service.Update(bankAccount, true);
                }
            }
        }

        internal void Update_InProgressExternalReconcile(List<string> listReconcileExternalPageLineId, int tenant, bool Value_ExternalReconcileInProgress)
        {
            var reconcileExternalPageLineQueryService = new ReconcileExternalPageLineQueryService(MainContext as IAccountingContext);
            var pmList = reconcileExternalPageLineQueryService.GetPageLinesPMsByIdList(listReconcileExternalPageLineId, tenant);

            foreach (var item in pmList)
            {
                if (Value_ExternalReconcileInProgress == true)//while prepare check while streaming do not check !!
                {
                    if (item.InProgressExternalReconcile)
                    {
                        throw new ApplicationException("יש התאמות בתהליך");
                    }
                }
                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                item.InProgressExternalReconcile = /*true*/ Value_ExternalReconcileInProgress;
            }
            this.UpdateMulti(pmList, new List<ReconcileExternalPageLinePM>(), new ReconcileExternalPagePM(), true);
        }
    }
}
