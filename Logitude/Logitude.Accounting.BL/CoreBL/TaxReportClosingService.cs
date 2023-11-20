using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class TaxReportClosingService
    {
        private const string RequiredAccountsMessage = "Please make sure you select all required accounts in full accounting settings";
        int tenant;
        string taxReportId;
        TaxReportPM taxReportPM;
        FullAccountingSettingPM fullAccountingSettings;
        TenantPM tenantPM;
        public JournalPM journalPM;
        const string TaxReportLineInputType = "I";
        const string TaxReportLineOutType = "O";
        public TaxReportClosingService(int tenant, string taxReportId, bool cancelCreatedJournal = false)
        {
            this.tenant = tenant;
            this.taxReportId = taxReportId;
            if (!cancelCreatedJournal)
            {
                GetRelatedEntities();
                Validate();
            }
            else {
                GetTaxReport();
            }
        }

        private void Validate()
        {
            EnsureAbilityToCreateClosingJournal();
            ValidateDate();
            ValidateFullAccountingSettingsFields();
        }

        private void GetRelatedEntities()
        {
            GetTaxReport();
            GetFullAccountingSetting();
            GetTenantPM();
        }
        private void GetTaxReport()
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(tenant);
            taxReportPM = taxReportQueryService.GetSingle(taxReportId, false, false);
        }
        private void GetFullAccountingSetting()
        {
            FullAccountingSettingQueryService settingQueryService = new FullAccountingSettingQueryService(tenant);
            fullAccountingSettings = settingQueryService.GetSingleFullAccountingSetting(tenant);
        }
        private void GetTenantPM()
        {
            tenantPM = TenantQuery.GetSingleTenantPM(tenant);
        }
        private void EnsureAbilityToCreateClosingJournal()
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(tenant);
            List<TaxReportLine> reconciledLines = null;
            var canHaveClosingJournal = taxReportQueryService.CheckIfTaxReportCanHaveClosingJournal(taxReportId, fullAccountingSettings.VATOutputGLAccountId, tenant, ref reconciledLines);
            if (!canHaveClosingJournal)
            {
                string error_text = TranslateTextsClass.Translate("TaxReport.O.ClosingJournalValidationMessage", tenant);
                if (reconciledLines != null && reconciledLines.Count > 0)
                {
                    reconciledLines = reconciledLines.OrderBy(rl => rl.Journal.JournalNumber).ThenBy(rl => rl.Line).ToList();
                    const int MAX = 5;
                    if (reconciledLines.Count > MAX)
                    {
                        reconciledLines = reconciledLines.Take(MAX).ToList();
                    }
                    string lines = String.Join(",", reconciledLines.Select(rl => rl.Line)); 

                    string journals = String.Join(",", reconciledLines.Select(rl => rl.Journal.JournalNumber));

                    string problem = TranslateTextsClass.Translate("Accounting.O.TaxRepProblem", tenant);
                    if (String.IsNullOrEmpty(problem)) problem = @"הבעיה מצויה בשורה";

                    string problem_2 = TranslateTextsClass.Translate("Accounting.O.TaxRepProblem_2", tenant);
                    if (String.IsNullOrEmpty(problem)) problem_2 = @"בדוח זה בפקודת יומן מספר";

                    error_text += ". " + problem + " " + journals; 
                    error_text += " " + problem_2 + " " + lines;
                }
                throw new ApplicationException(error_text);
            }
        }

        private void ValidateDate()
        {
            if(taxReportPM.TaxReportMonth >= TenantServerConfigration.GetStartOfCurrentMonthDate(tenant))
                throw new ApplicationException(TranslateTextsClass.Translate("TaxReport.O.CantCloseThisMonth", tenant));
        }
        private void ValidateFullAccountingSettingsFields()
        {
            if (fullAccountingSettings.TaxInstitutionGLAccountId == null
                || fullAccountingSettings.DefaultDifferencesGLAccountId == null
                || fullAccountingSettings.VATInputsGLAccountId == null
                || fullAccountingSettings.VATOutputGLAccountId == null)
                throw new ApplicationException(RequiredAccountsMessage);
        }

        public void CloseTaxReport()
        {
            CreateJournal();
            CreateJournalAdditionalDatas();
            SetTaxReportAsTransmittedAndClosingJournal();
        }

        public void CancelClosingJournal()
        {

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                VoidJournal();
                SetTaxReportAsCancelled();
                CancelReconciliations();
                scope.Complete();
            }
        }

        private void VoidJournal()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            JournalQueryService journalQuery = new JournalQueryService(MyContext);
            IQueryable<JournalPM> journalPMsQuery = journalQuery.GetJournalsByAccountingEntityId(taxReportId, tenant);
            var journalPMsList = journalPMsQuery.Where(r => r.AccountingEntityCode == AccountingEntityValues.TaxReport).ToList();
            if (journalPMsList.Count == 1)
            {
                journalPM = journalPMsList.FirstOrDefault();
            }
            else if (journalPMsList.Count == 0)
            {
                throw new ApplicationException("Couldn't find any Journal for this tax report");
            }
            else
            {
                throw new ApplicationException("Find more then 1 Journal for this tax report");
            }
            var service = new JournalVoidUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            var StornoOverrideM = new StornoOverrideM()
            {
                AccountingEntityCode = journalPM.AccountingEntityCode,
                AccountingEntityId = journalPM.AccountingEntityId,
                AccountingEntityReference = journalPM.AccountingEntityReference,
            };
            journalPM = service.VoidJournal(journalPM.Id, tenant, StornoOverrideM);
            AddAccountingEntityJournal(AccountingEntityJournalActions.TaxReportCancelClosingJournal);
        }

        private void CancelReconciliations() {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            var Reconciliations = transactionsQuery.GetReconciliationsByJournalId(journalPM.Id, tenant);
            ReconciliationUpdateService service = new ReconciliationUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            foreach (var item in Reconciliations) {
                item.IsCancelled = true;
                service.InitializeEntityPM(item);
                item.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(item, true);
            }
        }

        private void CreateJournal()
        {
            InitializeJournal();
            
            CreateJournalLines();
            
            AddAccountingEntityJournal(AccountingEntityJournalActions.TaxReportClosingJournal);

            SubmitJournal();
        }
        private void CreateJournalAdditionalDatas()
        {
            JournalAdditionalDataPM inputData = CreateAddionalDataForInputAccount();
            SubmitJournalAddionalData(inputData);

            JournalAdditionalDataPM outputData = CreateAddionalDataForOutputAccount();
            SubmitJournalAddionalData(outputData);
        }

        private void SubmitJournalAddionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(journalPM.Tenant);
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(accountingContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }

        private JournalAdditionalDataPM CreateAddionalDataForInputAccount()
        {
            return new JournalAdditionalDataPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                TaxReportId = taxReportId,
                TaxReportTransmitStatusCode = TaxReportLineTransmitStatusValues.Fortransmit,
                JournalId = journalPM.Id,
                JournalLineNumber = 4
            };

            
        }
        private JournalAdditionalDataPM CreateAddionalDataForOutputAccount()
        {
            return new JournalAdditionalDataPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                TaxReportId = taxReportId,
                TaxReportTransmitStatusCode = TaxReportLineTransmitStatusValues.Fortransmit,
                JournalId = journalPM.Id,
                JournalLineNumber = 2
            };
        }
        private void SetTaxReportAsTransmittedAndClosingJournal()
        {
            taxReportPM.StatusCode = VatReportStatusValues.TransmittedAndClosingJournal;

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportClosingservice.SetTaxReportAsTransmittedAndClosingJournal(*1*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
            ULog(text, stopLogAt);

            SubmitTaxReport();
        }

        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "TaxReportPMToPOCO", new DateTime(2023, 6, 1));
        }


        private void SetTaxReportAsCancelled()
        {
            taxReportPM.IsCancelled = true;
            taxReportPM.StatusCode = VatReportStatusValues.Cancelled;

            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportClosingservice.SetTaxReportAsCancelled(*1*): " + taxReportPM.Id + " taxReportPM.StatusCode : " + taxReportPM.StatusCode;
            ULog(text, stopLogAt);

            SubmitTaxReport();
        }

        private void SubmitJournal()
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            var journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            journalUpdateService.Update(journalPM, true);
        }
        private void SubmitTaxReport()
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
            taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
            taxReportUpdateService.Update(taxReportPM, true);
        }

        private void InitializeJournal()
        {
            var loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            var currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            journalPM = new JournalPM()
            {
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                UpdateDate = currentDate,
                UpdatedByUserId = loggedContact.Id,
                ApproveDate = currentDate,
                ApprovedByUserId = loggedContact.Id,
                CreateDate = currentDate,
                CreatedByUserId = loggedContact.Id,
                IsVoided = false,

                AccountingDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                TypeCode = JournalTypeValues.Regular,
                StatusCode = JournalStatusTypeValues.Approved,
                AccountingEntityCode = AccountingEntityValues.TaxReport,
                AccountingEntityId = taxReportId,
                AccountingEntityReference = taxReportPM.TaxReportNumber,
                JournalLines = new List<JournalLinePM>()
            };


        }

        private void CreateJournalLines()
        {
            CreateTaxInstitutionCreditLine();
            CreateVatInputCreditLine();
            CreateDifferencesCreditLine();
            CreateVatOutputDebitLine();
        }
        private void AddAccountingEntityJournal(string actionName, string childEntityId = null)
        {
            IAccountingContext context = AccountingContext.GetContext(journalPM.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, IContext>(), tenant);
            service.AddAccountingEntitieJournal(journalPM, actionName, childEntityId);
        }
        private void CreateTaxInstitutionCreditLine()
        {
            var line = new JournalLinePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                Line = 1,
                ActionCode = JournalActionType.Credit,

                DueDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                DocumentDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                AccountingDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),

                CurrencyId = tenantPM.CurrencyId,
                LocalAmount = taxReportPM.AmountForPayRefund.Value,
                ForeignAmount = taxReportPM.AmountForPayRefund.Value,
                ExchangeRate = 1,

                CreditAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,
                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
        }
        private void CreateVatOutputDebitLine()
        {
            var line = new JournalLinePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                Line = 4,
                ActionCode = JournalActionType.Debit,

                DueDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                DocumentDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                AccountingDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),

                CurrencyId = tenantPM.CurrencyId,
                LocalAmount = taxReportPM.OutputTaxAmount.Value + (taxReportPM.OutputTaxAmountRound ?? 0),
                ForeignAmount = taxReportPM.OutputTaxAmount.Value + (taxReportPM.OutputTaxAmountRound ?? 0),
                ExchangeRate = 1,

                DebitAccountId = fullAccountingSettings.VATOutputGLAccountId,
                CreditAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,

                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
            if (line.LocalAmount > 0 || line.ForeignAmount > 0) {
                CreateInternalReconciliationForVATOutputGLAccount(line);
            }
        }
        private void CreateVatInputCreditLine()
        {
            var line = new JournalLinePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                Line = 2,
                ActionCode = JournalActionType.Credit,

                DueDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                DocumentDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                AccountingDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),

                CurrencyId = tenantPM.CurrencyId,
                LocalAmount = taxReportPM.EquipmentInputsTaxAmount.Value + taxReportPM.OtherInputsTaxAmount.Value + (taxReportPM.InputsTaxAmountRound ?? 0),
                ForeignAmount = taxReportPM.EquipmentInputsTaxAmount.Value + taxReportPM.OtherInputsTaxAmount.Value + (taxReportPM.InputsTaxAmountRound ?? 0),
                ExchangeRate = 1,

                DebitAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,
                CreditAccountId = fullAccountingSettings.VATInputsGLAccountId,

                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
            if (line.LocalAmount > 0 || line.ForeignAmount > 0)
            {
                CreateInternalReconciliationForVATInputGLAccount(line);
            }
        }
        private void CreateDifferencesCreditLine()
        {
            var line = new JournalLinePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                Line = 3,
                ActionCode = JournalActionType.Credit,

                DueDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                DocumentDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),
                AccountingDate = TenantServerConfigration.GetLastOfMonthDate(taxReportPM.TaxReportMonth),

                CurrencyId = tenantPM.CurrencyId,
                LocalAmount = (taxReportPM.OutputTaxAmountRound ?? 0) - (taxReportPM.InputsTaxAmountRound ?? 0),
                ForeignAmount = (taxReportPM.OutputTaxAmountRound ?? 0) - (taxReportPM.InputsTaxAmountRound ?? 0),
                ExchangeRate = 1,

                DebitAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,
                CreditAccountId = fullAccountingSettings.DefaultDifferencesGLAccountId,

                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportDifferencJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
        }

        private string GetTaxReportJournalLineNote()
        {
            return "דו“ח מע“מ " + taxReportPM.TaxReportMonth.ToString("MM.yyyy");
        }
        private string GetTaxReportDifferencJournalLineNote()
        {
            return "דו“ח מע“מ " + taxReportPM.TaxReportMonth.ToString("MM.yyyy") + " - עיגול סכומים";
        }

        private void CreateInternalReconciliationForVATOutputGLAccount(JournalLinePM journalLinePM) {
            // GetReportLinesPMs
            var outputLines = GetTaxReportLines(TaxReportLineOutType);

            if (outputLines.Any()) {
                var ledgerTranasactions = GetLedgerTransactionsForOutputTaxReportLines(outputLines);
                AddJournalReconciles(ledgerTranasactions, journalLinePM);
            }
        }

        private void CreateInternalReconciliationForVATInputGLAccount(JournalLinePM journalLinePM)
        {
            var inputLines = GetTaxReportLines(TaxReportLineInputType);
            if (inputLines.Any())
            {
                var ledgerTranasactions = GetLedgerTransactionsForInputTaxReportLines(inputLines);
                AddJournalReconciles(ledgerTranasactions, journalLinePM);
            }

        }

        private void AddJournalReconciles(List<LedgerTransaction> ledgerTranasctions, JournalLinePM journalLinePM)
        {
            foreach (var transaction in ledgerTranasctions)
            {
                journalPM.JournalReconciles.Add(new JournalReconcilePM()
                {
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    JournalId = journalPM.Id,
                    Line = journalLinePM.Line,
                    LedgerTransactionId = transaction.Id,
                    CurrencyId = transaction.OpenAmountCurrencyId,
                    ReconciliationAmount = transaction.OpenAmount,
                    IsPartial = false
                });
            }
        }

        private List<TaxReportLine> GetTaxReportLines(string taxReportLineType)
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(tenant);
            return taxReportQueryService.GetReportLines(taxReportId, tenant).Where(d => d.OutputOrInput == taxReportLineType && d.TransmitStatusCode == TaxReportLineTransmitStatusValues.Fortransmit).ToList();
        }

        private List<LedgerTransaction> GetLedgerTransactionsForOutputTaxReportLines(List<TaxReportLine> taxReportLines)
        {
            LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
            var journalIds = taxReportLines.Select(x => x.JournalId).ToList();
            List<LedgerTransaction> ltList = ledgerTransactionRepository.GetLedgerTransactionsByJournalIdsAndAccountId(journalIds, fullAccountingSettings.VATOutputGLAccountId, tenant);
            List<LedgerTransaction> rv = ltList.Where(lt => lt.IsReconciled != true && lt.InReconcileProgress != true).ToList();
            if (ltList.Count > rv.Count) {
                var ErrorsInltList = ltList.Where(lt => lt.IsReconciled != false || lt.InReconcileProgress != false).ToList();
                var reconiledLines = taxReportLines.Where(taxReportLine => ErrorsInltList.Any(error => taxReportLine.JournalId == error.JournalId)).ToList();
                string errorText = "";
                foreach (var item in reconiledLines)
                {
                    errorText += $" ישנה התאמה בשורה {item.Line} בסכום {item.TotalInvoiceAmount} אסמכתא {item.Reference} לא ניתן לבצע את פקודה הסגירה.\n\n";

                }
                throw new ApplicationException(errorText);
            }
            return rv;
        }

        private List<LedgerTransaction> GetLedgerTransactionsForInputTaxReportLines(List<TaxReportLine> taxReportLines)
        {

            LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
            var ledgerTranasctionsIds = taxReportLines.Select(x => x.LedgerTransactionId).ToList();
            List<LedgerTransaction> ltList = ledgerTransactionRepository.GetLedgerTransactionsByIds(ledgerTranasctionsIds, tenant).ToList();
            List<LedgerTransaction> rv = ltList.Where(lt => lt.IsReconciled != true && lt.InReconcileProgress != true).ToList();
            return rv;
        }
    }
    public class TaxReportClosingJournalServiceArguments
    {

    }

    public struct JournalActionType
    {
        public const string NotValid = "0";
        public const string Credit = "1";
        public const string Debit = "2";
        public const string DebitAndCredit = "3";
        public const string DebitCreditAndVatdeduction = "4";
    }
}
