using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
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
        public TaxReportClosingService(int tenant, string taxReportId)
        {
            this.tenant = tenant;
            this.taxReportId = taxReportId;

            GetRelatedEntities();
            
            Validate();
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
            var canHaveClosingJournal = taxReportQueryService.CheckIfTaxReportCanHaveClosingJournal(taxReportId, tenant);
            if (!canHaveClosingJournal)
                throw new ApplicationException(TranslateTextsClass.Translate("TaxReport.O.ClosingJournalValidationMessage",tenant));
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

                DueDate = TenantServerConfigration.GetStartOfMonthDate(taxReportPM.TaxReportMonth),
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
                LocalAmount = taxReportPM.OutputTaxAmount.Value + taxReportPM.OutputTaxAmountRound.Value,
                ForeignAmount = taxReportPM.OutputTaxAmount.Value + taxReportPM.OutputTaxAmountRound.Value,
                ExchangeRate = 1,

                DebitAccountId = fullAccountingSettings.VATOutputGLAccountId,
                CreditAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,

                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
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
                LocalAmount = taxReportPM.EquipmentInputsTaxAmount.Value + taxReportPM.OtherInputsTaxAmount.Value + taxReportPM.InputsTaxAmountRound.Value,
                ForeignAmount = taxReportPM.EquipmentInputsTaxAmount.Value + taxReportPM.OtherInputsTaxAmount.Value + taxReportPM.InputsTaxAmountRound.Value,
                ExchangeRate = 1,

                DebitAccountId = fullAccountingSettings.TaxInstitutionGLAccountId,
                CreditAccountId = fullAccountingSettings.VATInputsGLAccountId,

                Reference1 = taxReportPM.TaxReportNumber,
                Notes = GetTaxReportJournalLineNote()
            };

            journalPM.JournalLines.Add(line);
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
                LocalAmount = taxReportPM.OutputTaxAmountRound.Value - taxReportPM.InputsTaxAmountRound.Value,
                ForeignAmount = taxReportPM.OutputTaxAmountRound.Value - taxReportPM.InputsTaxAmountRound.Value,
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
            return "“דו“ח מע“מ " + taxReportPM.TaxReportMonth.ToString("MM.yyyy");
        }
        private string GetTaxReportDifferencJournalLineNote()
        {
            return "“דו“ח מע“מ " + taxReportPM.TaxReportMonth.ToString("MM.yyyy") + " - עיגול סכומים“";
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
