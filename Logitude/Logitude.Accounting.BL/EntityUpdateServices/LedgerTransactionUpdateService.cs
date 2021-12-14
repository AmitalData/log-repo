using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class LedgerTransactionUpdateService : EntityUpdateService<LedgerTransaction, LedgerTransactionPM, EntityPM>
    {
        protected override void OnCreating(LedgerTransactionPM entityPM, EntityPM entityParentPM)
        {
          if (entityPM.IsReconciled == null)
            {
                entityPM.IsReconciled = false;
            }
        }

        protected override void OnUpdating(LedgerTransactionPM entityPM)
        {
            if (entityPM.IsReconciled == null)
            {
                entityPM.IsReconciled = false;
            }
        }

        public bool _CancelledAction;
        protected override void OnUpdating(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO)
        {

            switch (entityPM.ChangeSetOp)
            {
                case Simplog.Server.Infrastructure.ChangeSetOperation.None:
                    return;
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                    throw new Exception("cannot insert LedgerTransactionPM move to Store Procedure");
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                    throw new Exception("cannot Delete LedgerTransactionPM !!!!!");
                    break;
                default:
                    break;
            }

            if (!_CancelledAction)
            {
                if (entityPM.OpenAmount != 0)
                {
                    if (entityPOCO.OpenAmount < 0 && entityPM.OpenAmount > 0)
                    {
                        throw new Exception("(entityPOCO.OpenAmount < 0 && entityPM.OpenAmount > 0)");
                    }

                    if (entityPOCO.OpenAmount > 0 && entityPM.OpenAmount < 0)
                    {
                        throw new Exception("(entityPOCO.OpenAmount > 0 && entityPM.OpenAmount < 0)");
                    }
                }
            }
            base.OnUpdating(entityPM, entityPOCO);
        }

        public void DelSertOpenRecilationDrafts(List<LedgerTransactionPM> OpenRecilationDrafts)
        {

            string gLAccountId; int tenant ;
            var pairs=OpenRecilationDrafts.GroupBy(r => new { r.AccountId, r.Tenant });
                var pairsCount = pairs.Count();
                if (pairsCount > 1)
                {
                    throw new Exception("only one combination allowed Of {AccountId +Tenant }");
                }
                if (pairsCount == 0)
                {
                    //to delete 
                    throw new Exception("only one combination allowed Of {AccountId +Tenant } (pairsCount == 0) ==>No Items On Match List,To Delete ? ");
                }
            
            var repeateTrans= OpenRecilationDrafts.GroupBy(r => r.Id).Where( g=> g.Count()>1).Select( g=>g.Key).ToList();
            if (repeateTrans.Count>0)
            {
                throw new Exception("Client Side should send send Unique Id List :" + string.Join(",",repeateTrans.ToArray()));
            }
  
            gLAccountId =pairs.First().Key.AccountId;
            tenant=pairs.First().Key.Tenant;

            using (var scope = TransactionFactory.GetTransaction())
            {
                (this.Repository as LedgerTransactionRepository).ResetDraftOpenReconciliation(gLAccountId, tenant);

                var transIdList=OpenRecilationDrafts.Select(r => r.Id).ToList();
                LedgerTransactionQueryService qs = new LedgerTransactionQueryService((MainContext as IAccountingContext));
                var listPM=qs.GetLedgerTransactionPMsByIdList(transIdList, tenant);
                foreach (var pm in listPM)
                {
                    if (pm.IsReconciled)
                    {
                        throw new Exception("DelSertOpenRecilationDrafts but pm.IsReconciled " + pm.Id);
                    }
                    pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    pm.AmountToReconcile = OpenRecilationDrafts.First(r => r.Id == pm.Id).AmountToReconcile;
                    pm.Mark = true;
                }
                this.UpdateMulti(listPM, new List<LedgerTransactionPM>(), new EntityPM(), true);
                scope.Complete();
            }
        }
        public void UpdateInReconcileProgress(List<String> listTransactionId,int tenant,bool Value_inReconcileProgress)
        {

            //using (var scope = TransactionFactory.GetTransaction())//we alreary in a scope !!-but while straming we r in Serlazed TRans
            {
                var ledgerTransactionQueryService = new LedgerTransactionQueryService(MainContext as IAccountingContext);
                var pmList = ledgerTransactionQueryService.GetLedgerTransactionPMsByIdList(listTransactionId, tenant);

                foreach (var item in pmList)
                {
                    if (Value_inReconcileProgress == true)//while prepare check while streaming do not check !!
                    {
                        if (item.InReconcileProgress)
                        {
                            throw new Exception("Already InReconcileProgress");
                        }
                    }
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    item.InReconcileProgress = /*true*/ Value_inReconcileProgress;
                }
                bool supperssSaveOnUpdateMultiDueIsFaster = true;
                this.UpdateMulti(pmList, new List<LedgerTransactionPM>(), new EntityPM(), !supperssSaveOnUpdateMultiDueIsFaster);
                if (supperssSaveOnUpdateMultiDueIsFaster)
                {
                    this.SubmitChanges();
                }
                //scope.Complete();
            }
        }

        internal void Update_InProgressExternalReconcile(List<string> listTransactionId, int tenant, bool Value_ExternalReconcileInProgress)
        {
            var ledgerTransactionQueryService = new LedgerTransactionQueryService(MainContext as IAccountingContext);
            var pmList = ledgerTransactionQueryService.GetLedgerTransactionPMsByIdList(listTransactionId, tenant);

            foreach (var item in pmList)
            {
                if (Value_ExternalReconcileInProgress == true)//while prepare check while streaming do not check !!
                {
                    if (item.InProgressExternalReconcile)
                    {
                        throw new Exception("Already InReconcileProgress");
                    }
                }
                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                item.InProgressExternalReconcile = /*true*/ Value_ExternalReconcileInProgress;
            }
            bool supperssSaveOnUpdateMultiDueIsFaster = true;
            this.UpdateMulti(pmList, new List<LedgerTransactionPM>(), new EntityPM(), !supperssSaveOnUpdateMultiDueIsFaster);
            if (supperssSaveOnUpdateMultiDueIsFaster)
            {
                this.SubmitChanges();
            }
        }

        protected override void Trace(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO, string changesXml)
        {
            if(entityPM.IsExternalReconcile != entityPOCO.IsExternalReconcile)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                bool showLocals = !contact.DontShowLocalLabels;
                var eventNotes = string.Concat("Line No:  "+ entityPM.JournalLineNumber + ", Is Externally Reconciled changed: ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), entityPOCO.IsExternalReconcile, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), entityPM.IsExternalReconcile, "\n");

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.JournalId,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "Journal",
                    IsAddedManually = false,
                    EventTypeCode = "JUP",
                    Notes = eventNotes,

                });
            }
            
        }
    }
}
