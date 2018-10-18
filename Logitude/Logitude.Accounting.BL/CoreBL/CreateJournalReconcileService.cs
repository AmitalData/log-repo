using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
   public class CreateJournalReconcileService
    {
        private IAccountingContext _AccountingContext;
        private JournalPM _JournalPM;

        public JournalPM Create(
            IAccountingContext accountingContext,  
            int tenant,
            List<ReconciliationLinePM> ReconciliationLines,
            string TheAccountId,
            string AdjustAccountId,
            DateTime AccountDate,
            string Ref1,
            string Ref2,
            string Ref3,
            string Remarks
            )
        {
            using (var scope = TransactionFactory.GetTransaction())
            {
                _AccountingContext = accountingContext;
                var usrid = AuthenticationUtil.ResolveUserId(tenant);
                var totReconciliationAmount = ReconciliationLines.Sum(r => r.ReconciliationAmount);
                if (totReconciliationAmount == 0)
                {
                    throw new Exception("Total ReconciliationAmount is zero");
                }
                DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                var qs = new GLAccountQueryService(_AccountingContext);
                var glPM = qs.GetSingle(TheAccountId, false, false);
                if (glPM.ReconcileMethodCode == ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString())
                {

                }
                else
                {

                }
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                string accountingCurrencyId = tPM.CurrencyId;

                var theCurrencyId = ReconciliationLines.First().CurrencyId;
                var ratesTablesRepository = new RatesTableRepository(tenant);
                var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

                var myAccountingEntityDetails = new AccountingEntityDetails();
                var myAccEntityReconciliation10=  myAccountingEntityDetails.GetAll().First(r => r.EnglishName == "Reconciliation");

                
                var rate = ratesTableQuery.GetLastRateByValueDate(tenant, theCurrencyId, accountingCurrencyId,
                    //@now  
                    AccountDate //Ohad :By aAccounting date
                    );
                var totForeign = totReconciliationAmount * (decimal)rate.Rate.GetValueOrDefault();
                JournalPM journal = new JournalPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = tenant,
                    ///journal.JournalNumber = "1";
                    CreateDate = @now,
                    AccountingDate = AccountDate,//1.1.(yyyy+1)
                    TypeCode ="0" , //== REGULAR  //"1" == TEMPLATE,
                    StatusCode = "2",
                    AccountingEntityCode = myAccEntityReconciliation10.Code, //"6",// - Reconciliation
                    AccountingEntityId = null,//Reconciliations.id !!!!!!!!!!!!
                    AccountingEntityReference = null,//Reconciliations.Number !!!!!!!!!!!!

                    UpdateDate = @now,
                    //journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                    ApproveDate = @now,
                    //journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;


                    CreatedByUserId = usrid,
                    ApprovedByUserId = usrid,

                    ExternalNo = null,
                    ExternalSystem = null,
                    OriginalJournalId = null,
                    


                };

                //totForeign= TranslateForeignAmount(journal.AccountingDate , totReconciliationAmount)
                if (totReconciliationAmount < 0)///credit //Ohad :
                {
                    totReconciliationAmount = -1 * totReconciliationAmount; //Ohad :
                    totForeign = -1 * totForeign; //Ohad :
                    journal.JournalLines.Add(new JournalLinePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = journal.Tenant,
                        JournalId = journal.Id,
                        AccountingDate = journal.AccountingDate,
                        ActionCode = "2",//- Debit
                        DebitAccountId = TheAccountId,
                        LocalAmount = totReconciliationAmount,
                        CurrencyId = theCurrencyId,
                        ForeignAmount = totForeign,

                        DocumentDate = journal.CreateDate,
                        DueDate = journal.CreateDate,

                    });

                    journal.JournalLines.Add(new JournalLinePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = journal.Tenant,
                        JournalId = journal.Id,
                        AccountingDate = journal.AccountingDate,
                        ActionCode = "1",//- Credit
                        CreditAccountId = AdjustAccountId,
                        LocalAmount = totReconciliationAmount,

                        CurrencyId = theCurrencyId,
                        ForeignAmount = totForeign,
                        DocumentDate = journal.CreateDate,
                        DueDate = journal.CreateDate,
                    });

                }
                else//debit  //Ohad :
                {
                    journal.JournalLines.Add(new JournalLinePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = journal.Tenant,
                        JournalId = journal.Id,
                        AccountingDate = journal.AccountingDate,
                        ActionCode = "1",//- Credit 
                        CreditAccountId = TheAccountId,
                        LocalAmount = totReconciliationAmount,

                        CurrencyId = theCurrencyId,
                        ForeignAmount = totForeign,

                        DocumentDate = journal.CreateDate,
                        DueDate = journal.CreateDate,

                    });

                    journal.JournalLines.Add(new JournalLinePM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = journal.Tenant,
                        JournalId = journal.Id,
                        AccountingDate = journal.AccountingDate,

                        ActionCode = "2",//- Debit
                        DebitAccountId = AdjustAccountId,
                        LocalAmount = totReconciliationAmount,


                        CurrencyId = theCurrencyId,
                        ForeignAmount = totForeign,

                        DocumentDate = journal.CreateDate,
                        DueDate = journal.CreateDate,


                    });
                }
                foreach (var item in journal.JournalLines)
                {
                    item.Reference1 = Ref1;
                    item.Reference2 = Ref2;
                    item.Reference3 = Ref3;
                    item.Notes = Remarks;
                }
                var listJournalReconciles = (from item in ReconciliationLines
                                             select new JournalReconcilePM()
                                             {
                                                 ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                                 Tenant = journal.Tenant,
                                                 JournalId = journal.Id,
                                                 LedgerTransactionId = item.TransactionId,
                                                 Line = item.Line,
                                                 CurrencyId = item.CurrencyId,
                                                 ReconciliationAmount = item.ReconciliationAmount,
                                                 IsPartial = item.IsPartial,

                                             }
                           );
                journal.JournalReconciles.AddRange(listJournalReconciles);

                //var listTransactionId = ReconciliationLines.Select(r => r.TransactionId).ToList();


                //var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                //ledgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, tenant);



                var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                JournalUP.Update(journal, true);

                scope.Complete();
                return journal;
            }
        }

        
    }
}
