using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
   public class CreateJournalReconcileService
    {
        private IAccountingContext _AccountingContext;
        private JournalPM _JournalPM;
        private static Object thisLock = new Object();
        private const string CreditActionCode = "1";
        private const string DebitActionCode = "2";
        private const string RegularTypeCode = "0";
        private const string ApprovedStatusCode = "2";
        private const string AdjustmentAccountingEntityName = "Adjustment";

        public JournalPM Create(
            IAccountingContext accountingContext,
            int tenant,
            List<ReconciliationLinePM> ReconciliationLines,
            string TheAccountId,
            string AdjustAccountId,
            DateTime AccountDate,
            DateTime? DueDate,
            DateTime? RefDate,
            string Ref1,
            string Ref2,
            string Ref3,
            string Remarks
            )
        {
            lock (thisLock)
            {
                using (var scope = TransactionFactory.GetNewReadCommittedTransaction())
                {
                    DateTime dueDate = new DateTime();
                    DateTime refDate = new DateTime();
                    if (DueDate != null) {
                        dueDate = DueDate.Value;
                    }   else {
                        dueDate = AccountDate;
                    }

                    if (RefDate != null)
                    {
                        refDate = RefDate.Value;
                    }
                    else
                    {
                        refDate = AccountDate;
                    }
                    _AccountingContext = accountingContext;
                    var usrid = AuthenticationUtil.ResolveUserId(tenant);
                    decimal totReconciliationAmountFromUnknownCurrency = ReconciliationLines.Sum(r => r.ReconciliationAmount);
                    if (totReconciliationAmountFromUnknownCurrency == 0)
                    {
                        throw new ApplicationException("Total ReconciliationAmount is zero");
                    }
                    if (ReconciliationLines.Select(r => r.CurrencyId).Distinct().Count() > 1)
                    {
                        throw new ApplicationException("לא אופיין התאמת תנועות  ליוצר ממטבע אחד");
                    }
                    DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                    var qs = new GLAccountQueryService(_AccountingContext);
                    var glPM = qs.GetSingle(TheAccountId, false, false);

                    TenantQuery tenantQuery = new TenantQuery(tenant);
                    TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                    string accountingCurrencyId = tPM.CurrencyId;

                    var theCurrencyId = ReconciliationLines.First().CurrencyId;
                    var ratesTablesRepository = new RatesTableRepository(tenant);
                    var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

                    var myAccountingEntityDetails = new AccountingEntityDetails();
                    var myAccEntityReconciliation10 = myAccountingEntityDetails.GetAll().FirstOrDefault(r => r.EnglishName ==
                    //"Reconciliation"
                    "Adjustment"
                    );
                    if (myAccEntityReconciliation10 == null)
                    {
                        throw new ApplicationException("was Reconciliation then change to Adjustment (and now to what ?? - yaron said it will not change ever !!)");
                    }

                    String theJournalLineCurrencyId = "";

                    if (!String.IsNullOrWhiteSpace(glPM.CurrencyId))
                    {
                        theJournalLineCurrencyId = glPM.CurrencyId;//ohad : ACCOUNT -CURRENCY = NISS + RECONCILE = 0 (LOCAL )  ==>> JOURNAL CURRENCY == NIS

                    }
                    else
                    {
                        theJournalLineCurrencyId = accountingCurrencyId;
                    }

                    theCurrencyId = theJournalLineCurrencyId;
                    RatesTablePM rate = null;
                    rate = ratesTableQuery.GetLastRateByValueDate(tenant, theCurrencyId, accountingCurrencyId,
                       //@now  
                       AccountDate //Ohad :By aAccounting date
                       );
                    if (rate == null && theCurrencyId == accountingCurrencyId)
                    {
                        rate = new RatesTablePM() { Rate = 1 };/// ON THE HOUSE !?!?!?
                    }


                    if (rate == null)
                    {
                        throw new ApplicationException("שער המטבע לא קיים בטבלת שערי המטבעות");
                    }





                    decimal totForeign;//= totReconciliationAmount / (decimal)rate.Rate.GetValueOrDefault();
                    decimal totReconciliationLocalAmount;
                    bool useLocalRecoMethod = (glPM.ReconcileMethodCode == "0");
                    if (useLocalRecoMethod)
                    {
                        totReconciliationLocalAmount = totReconciliationAmountFromUnknownCurrency;
                        totForeign = totReconciliationLocalAmount / (decimal)rate.Rate.GetValueOrDefault();
                    }
                    else
                    {
                        totForeign = totReconciliationAmountFromUnknownCurrency;
                        totReconciliationLocalAmount = totForeign * (decimal)rate.Rate.GetValueOrDefault();
                    }

                    JournalPM journal = new JournalPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = tenant,
                        ///journal.JournalNumber = "1";
                        CreateDate = @now,
                        AccountingDate = AccountDate,//1.1.(yyyy+1)
                        TypeCode = "0", //== REGULAR  //"1" == TEMPLATE,
                        StatusCode = "2",
                        AccountingEntityCode = myAccEntityReconciliation10.Code, //"6",// - Reconciliation
                        AccountingEntityId = null,//Reconciliations.id !!!!!!!!!!!!
                        AccountingEntityReference = null,//Reconciliations.Number !!!!!!!!!!!!
                        DueDate = dueDate,
                        DocumentDate = refDate,
                        UpdateDate = @now,
                        //journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                        ApproveDate = @now,
                        //journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;


                        CreatedByUserId = usrid,
                        ApprovedByUserId = usrid,

                        ExternalNo = null,
                        ExternalSystem = null,
                        OriginalJournalId = null,
                        SearchFields= "OneLineReconciliation",//If the journal is one or split, use SearchFields as an indecter.


                    };

                    //totForeign= TranslateForeignAmount(journal.AccountingDate , totReconciliationAmount)
                    if (totReconciliationLocalAmount < 0)///credit //Ohad :
                    {
                        totReconciliationLocalAmount = -1 * totReconciliationLocalAmount; //Ohad :
                        totForeign = -1 * totForeign; //Ohad :
                        journal.JournalLines.Add(new JournalLinePM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = journal.Tenant,
                            JournalId = journal.Id,
                            AccountingDate = journal.AccountingDate,
                            ActionCode = "2",//- Debit
                            DebitAccountId = TheAccountId,
                            LocalAmount = totReconciliationLocalAmount,
                            CurrencyId = theCurrencyId,
                            ForeignAmount = totForeign,

                            DocumentDate = refDate,
                            DueDate = dueDate,
                            CreditAccountId = AdjustAccountId,
                        });

                        journal.JournalLines.Add(new JournalLinePM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = journal.Tenant,
                            JournalId = journal.Id,
                            AccountingDate = journal.AccountingDate,
                            ActionCode = "1",//- Credit
                            CreditAccountId = AdjustAccountId,
                            LocalAmount = totReconciliationLocalAmount,

                            CurrencyId = theCurrencyId,
                            ForeignAmount = totForeign,
                            DocumentDate = refDate,
                            DueDate = dueDate,
                            DebitAccountId = TheAccountId,
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
                            LocalAmount = totReconciliationLocalAmount,

                            CurrencyId = theCurrencyId,
                            ForeignAmount = totForeign,

                            DocumentDate = refDate,
                            DueDate = dueDate,
                            DebitAccountId = AdjustAccountId

                        });

                        journal.JournalLines.Add(new JournalLinePM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = journal.Tenant,
                            JournalId = journal.Id,
                            AccountingDate = journal.AccountingDate,

                            ActionCode = "2",//- Debit
                            DebitAccountId = AdjustAccountId,
                            LocalAmount = totReconciliationLocalAmount,


                            CurrencyId = theCurrencyId,
                            ForeignAmount = totForeign,

                            DocumentDate = refDate,
                            DueDate = dueDate,
                            CreditAccountId = TheAccountId,

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
                    var journalReconcileRepository = new JournalReconcileRepository(accountingContext);
                    var existingReconciliationLines = journalReconcileRepository.GetReconciliationLinesByLedgerTransactionsIds(listJournalReconciles.Select(x => x.LedgerTransactionId).ToList());
                    foreach (var item in existingReconciliationLines)
                    {
                        if (listJournalReconciles.Any(x => x.LedgerTransactionId == item.TransactionId && x.ReconciliationAmount == item.ReconciliationAmount))
                        {

                            ReconciliationQueryService recoQueryService = new ReconciliationQueryService(accountingContext);

                            ReconciliationPM reco = recoQueryService.GetSingle(item.ReconciliationId, false, false);
                            if (reco != null)
                            {
                                TimeSpan timeElapsed = DateTime.Now - reco.CreateDate;
                                long minTotalMinutes = 5;
                                if (timeElapsed.TotalMinutes < minTotalMinutes)
                                {
                                    throw new ApplicationException("ישנן תנועות שסומנו ונמצאות בתהליך התאמה על ידי משתמש או סשן אחר, יש לבצע רענון לצאת ממסך התאמות ללא שמירת השורות ולהיכנס מחדש.");
                                }

                            }

                        }
                    }
                    journal.JournalReconciles.AddRange(listJournalReconciles);

                    //var listTransactionId = ReconciliationLines.Select(r => r.TransactionId).ToList();


                    //var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    //ledgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, tenant);



                    var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    JournalUP.Update(journal, true);
                    var journalReconciles = journalReconcileRepository
                        .GetJournalReconcilesForJournalsWithoutLedgers(listJournalReconciles.Select(x => x.LedgerTransactionId).ToList(), journal.Tenant);
                    if (journalReconciles.Where(x=>x.JournalId != journal.Id).Any())
                    {
                        throw new ApplicationException("ישנן תנועות שסומנו ונמצאות בתהליך התאמה על ידי משתמש או סשן אחר, יש לבצע רענון לצאת ממסך התאמות ללא שמירת השורות ולהיכנס מחדש.");
                    }

                    scope.Complete();
                    return journal;
                }
            }
        }

        public List<JournalPM> CreateSplitJournals(
            IAccountingContext accountingContext,
            int tenant,
            List<ReconciliationLinePM> ReconciliationLines,
            string TheAccountId,
            string AdjustAccountId,
            DateTime? AccountDate,
            DateTime? DueDate,
            DateTime? RefDate,
            string Ref1,
            string Ref2,
            string Ref3,
            string Remarks
            )
        {
            lock (thisLock)
            {
                using (var scope = TransactionFactory.GetNewReadCommittedTransaction())
                {
                    _AccountingContext = accountingContext;
                    var usrid = AuthenticationUtil.ResolveUserId(tenant);
                    ValidateTotalReconciliationAmount(ReconciliationLines);
                    DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                    var gLAccountQueryService = new GLAccountQueryService(_AccountingContext);
                    var glAccountPM = gLAccountQueryService.GetSingle(TheAccountId, false, false);
                    TenantQuery tenantQuery = new TenantQuery(tenant);
                    TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                    string accountingCurrencyId = tPM.CurrencyId;

                    var theCurrencyId = ReconciliationLines.First().CurrencyId;
                    var ratesTablesRepository = new RatesTableRepository(tenant);
                    var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

                    var myAccountingEntityDetails = new AccountingEntityDetails();
                    var adjustmentAccountingEntityDetails = myAccountingEntityDetails.GetAll().FirstOrDefault(r => r.EnglishName == AdjustmentAccountingEntityName);

                    string theJournalLineCurrencyId = !String.IsNullOrWhiteSpace(glAccountPM.CurrencyId) ? glAccountPM.CurrencyId : accountingCurrencyId;
                    theCurrencyId = theJournalLineCurrencyId;
                    RatesTablePM rate = null;
                    rate = ratesTableQuery.GetLastRateByValueDate(tenant, theCurrencyId, accountingCurrencyId,AccountDate);
                    if (rate == null && theCurrencyId == accountingCurrencyId)
                    {
                        rate = new RatesTablePM() { Rate = 1 };/// ON THE HOUSE !?!?!?
                    }
                    ValidateRate(rate);
                    List<JournalPM> addedJournalPMs = new List<JournalPM>();
                    var journalReconcileRepository = new JournalReconcileRepository(accountingContext);
                    ReconciliationLines.ForEach(reconciliationLine =>
                    {
                        JournalPM journal = CreateJournalForReconciliationLine(reconciliationLine, TheAccountId, AdjustAccountId, DueDate, RefDate, theCurrencyId, rate, Ref1, Ref2, Ref3, Remarks,
                            tenant, @now, usrid, AccountDate, adjustmentAccountingEntityDetails.Code);
                        if(journal != null) {
                            var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                            JournalUP.Update(journal, true);
                            addedJournalPMs.Add(journal);
                        var journalReconciles = journalReconcileRepository
                        .GetJournalReconcilesForJournalsWithoutLedgers(journal.JournalReconciles.Select(x => x.LedgerTransactionId).ToList(), tenant);
                        if (journalReconciles.Where(x => x.JournalId != journal.Id).Any())
                        {
                                throw new ApplicationException("ישנן תנועות שסומנו ונמצאות בתהליך התאמה על ידי משתמש או סשן אחר, יש לבצע רענון לצאת ממסך התאמות ללא שמירת השורות ולהיכנס מחדש.");
                            }
                        }
                    });
                    scope.Complete();
                    return addedJournalPMs;
                }
            }
        }

        private JournalPM CreateJournalForReconciliationLine(ReconciliationLinePM reconciliationLine, string TheAccountId, string AdjustAccountId,
            DateTime? dueDate, DateTime? refDate, string theCurrencyId, RatesTablePM rate, string Ref1, string Ref2, string Ref3, string Remarks,
            int tenant, DateTime @now, string userid, DateTime? accountDate, string accountingEntityCode)
        {
            DateTime accountingDate;
            if (accountDate == null)
            {
                JournalRepository repo = new JournalRepository(tenant);
                accountingDate = repo.GetSingleJournalByNumber(reconciliationLine.JournalNumber, tenant).AccountingDate;
                if (!IsMonthOpenForAccountingDate(accountingDate, tenant))
                {
                    return null;
                }
            }
            else
            {
                accountingDate = (DateTime)accountDate;
            }

            DateTime due = dueDate ?? reconciliationLine.DueDate.GetValueOrDefault();
            DateTime referenceDate = refDate ?? reconciliationLine.RefDate.GetValueOrDefault() ;

            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = tenant,
                CreateDate = @now,
                AccountingDate = accountingDate,
                TypeCode = RegularTypeCode,
                StatusCode = ApprovedStatusCode,
                AccountingEntityCode = accountingEntityCode,
                AccountingEntityId = null,
                AccountingEntityReference = null,
                DueDate = due,
                DocumentDate = refDate,
                UpdateDate = @now,
                ApproveDate = @now,
                CreatedByUserId = userid,
                ApprovedByUserId = userid,
                ExternalNo = null,
                ExternalSystem = null,
                OriginalJournalId = null
            };
            AddJournalLines(journal, reconciliationLine, TheAccountId, AdjustAccountId, due, referenceDate, theCurrencyId, rate, Ref1, Ref2, Ref3, Remarks);
            var journalReconcile = new JournalReconcilePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                JournalId = journal.Id,
                LedgerTransactionId = reconciliationLine.TransactionId,
                Line = reconciliationLine.Line,
                CurrencyId = reconciliationLine.CurrencyId,
                ReconciliationAmount = reconciliationLine.ReconciliationAmount,
                IsPartial = reconciliationLine.IsPartial,

            };
            journal.JournalReconciles.Add(journalReconcile);
            return journal;
        }

        private void ValidateRate(RatesTablePM rate)
        {
            if (rate == null)
            {
                throw new ApplicationException("שער המטבע לא קיים בטבלת שערי המטבעות");
            }
        }

        private void ValidateTotalReconciliationAmount(List<ReconciliationLinePM> ReconciliationLines)
        {
            decimal totReconciliationAmountFromUnknownCurrency = ReconciliationLines.Sum(r => r.ReconciliationAmount);
            if (totReconciliationAmountFromUnknownCurrency == 0)
            {
                throw new ApplicationException("Total ReconciliationAmount is zero");
            }
            if (ReconciliationLines.Select(r => r.CurrencyId).Distinct().Count() > 1)
            {
                throw new ApplicationException("לא אופיין התאמת תנועות  ליוצר ממטבע אחד");
            }
        }

        private void AddJournalLines(JournalPM journal, ReconciliationLinePM reconciliationLine, string TheAccountId, string adjustAccountId,
            DateTime dueDate, DateTime refDate, string theCurrencyId, RatesTablePM rate, string Ref1, string Ref2, string Ref3, string Remarks) {
            var qs = new GLAccountQueryService(_AccountingContext);
            var glPM = qs.GetSingle(TheAccountId, false, false);
            decimal totForeign;//= reconciliationLine.ReconciliationAmount / (decimal)rate.Rate.GetValueOrDefault();
            decimal totReconciliationLocalAmount;
            bool useLocalRecoMethod = (glPM.ReconcileMethodCode == "0");
            decimal totReconciliationAmountFromUnknownCurrency = reconciliationLine.ReconciliationAmount;


            if (useLocalRecoMethod)
            {
                totReconciliationLocalAmount = totReconciliationAmountFromUnknownCurrency;
                totForeign = totReconciliationLocalAmount / (decimal)rate.Rate.GetValueOrDefault();
            }
            else
            {
                totForeign = totReconciliationAmountFromUnknownCurrency;
                totReconciliationLocalAmount = totForeign * (decimal)rate.Rate.GetValueOrDefault();
            }


            if (reconciliationLine.ReconciliationAmount < 0)///credit //Ohad :
            {
                totReconciliationLocalAmount = -1 * totReconciliationLocalAmount;
                totForeign = -1 * totForeign; //Ohad :
                journal.JournalLines.Add(new JournalLinePM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = journal.Tenant,
                    JournalId = journal.Id,
                    AccountingDate = journal.AccountingDate,
                    ActionCode = DebitActionCode,//- Debit
                    DebitAccountId = TheAccountId,
                    LocalAmount = totReconciliationLocalAmount,
                    CurrencyId = theCurrencyId,
                    ForeignAmount = totForeign,
                    DocumentDate = refDate,
                    DueDate = dueDate,
                    CreditAccountId = adjustAccountId,
                });

                journal.JournalLines.Add(new JournalLinePM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = journal.Tenant,
                    JournalId = journal.Id,
                    AccountingDate = journal.AccountingDate,
                    ActionCode = CreditActionCode,//- Credit
                    CreditAccountId = adjustAccountId,
                    LocalAmount = totReconciliationLocalAmount,
                    CurrencyId = theCurrencyId,
                    ForeignAmount = totForeign,
                    DocumentDate = refDate,
                    DueDate = dueDate,
                    DebitAccountId = TheAccountId,
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
                    ActionCode = CreditActionCode,//- Credit 
                    CreditAccountId = TheAccountId,
                    LocalAmount = totReconciliationLocalAmount,
                    CurrencyId = theCurrencyId,
                    ForeignAmount = totForeign,
                    DocumentDate = refDate,
                    DueDate = dueDate,
                    DebitAccountId = adjustAccountId
                });

                journal.JournalLines.Add(new JournalLinePM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = journal.Tenant,
                    JournalId = journal.Id,
                    AccountingDate = journal.AccountingDate,

                    ActionCode = DebitActionCode,//- Debit
                    DebitAccountId = adjustAccountId,
                    LocalAmount = totReconciliationLocalAmount,
                    CurrencyId = theCurrencyId,
                    ForeignAmount = totForeign,
                    DocumentDate = refDate,
                    DueDate = dueDate,
                    CreditAccountId = TheAccountId,
                });
            }

            foreach (var item in journal.JournalLines) {
                        item.Reference1 = Ref1 != null ? Ref1 : reconciliationLine.Reference1;
                        item.Reference2 = Ref2 != null ? Ref2 : reconciliationLine.Reference2;
                        item.Reference3 = Ref3 != null ? Ref3 : reconciliationLine.Reference3;
                        item.Notes = Remarks != null ? Remarks : reconciliationLine.Notes;
                    }
        }

        private bool IsMonthOpenForAccountingDate(DateTime accountingDate, int tenant)
        {
            var typeregular = "1"; //1 Regular רגיל        1,Regular,רגיל   0
            var accountingPeriodQueryService = new AccountingPeriodQueryService(tenant);
            var accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodByType(typeregular, tenant); ;

            return
            JournalValidatorNotStatic
                 .IsMonthOpenForAccountingDate(
                accountingPeriodsByTypeRegular.AsQueryable(),
                 new DateTime(accountingDate.Year, accountingDate.Month, 1)
                 );
        }

    }
}
