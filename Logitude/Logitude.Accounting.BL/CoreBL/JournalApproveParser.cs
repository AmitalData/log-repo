using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.Validators;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.BL;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
namespace Logitude.Accounting.BL.CoreBL
{
    public interface IJournalApproveParser
    {
        void OnApproveUpdatingFillArrangeJournalPMResetControlAccount();
        bool ParseIt();
    }
    public class JournalApproveParser : IJournalApproveParser
    {
        bool _FeatureTotalDateType = true;
        string _LocalAccountingCurrencyId;
        private JournalPM _JournalPM;
        private bool _includeIdCounter;
        private bool p;

        private ValidationContext _JournalValidatorContext;
        private List<string> _ErrorsList;
        bool _JournalForeignAmountCanBeUnEqual = true;
        public JournalApproveParser(JournalPM entityPM, bool includeIdCounter, ValidationContext journalValidatorContext)
        {
            this._JournalPM = entityPM;
            _includeIdCounter = includeIdCounter;
            this._JournalValidatorContext = journalValidatorContext;
            LedgerTransactions = new List<LedgerTransactionPM>();
            if (_JournalPM.StatusCode == "4") {
                _JournalPM.StatusCode = "6";
            }
            //_PRIVATEOLD_GLAccountTotalByMonths = new List<GLAccountTotalByMonthPM>();

        }

        public virtual void OnApproveUpdatingFillArrangeJournalPMResetControlAccount()
        {
            if (_JournalPM.StatusCode != "6" )
            {
                throw new ApplicationException("occure only OnApproveUpdating");
            }
            if (!String.IsNullOrWhiteSpace(_JournalPM.QueueId))
            {
                throw new JournalApproveException("Parsing allowed b4 Streaming to Account (QueueId!=nuul) ", WhatTODOJournalApproveEnum.ClearQueue);
            }
            var isStreaming2AccountingStage = !String.IsNullOrWhiteSpace(_JournalPM.ApprovedByUserId);
            if (isStreaming2AccountingStage)
            {
                return;
            }
            _JournalPM.ApprovedByUserId = AuthenticationUtil
                //.GetAuthenticatedUser();
                .ResolveUserId(_JournalPM.Tenant);
            _JournalPM.ApproveDate = DateTime.UtcNow;//forgot how to get server time 
            bool? GLAccountTaxChecked = null;
            foreach (var journalLine in _JournalPM.JournalLines)
            {

                if (journalLine.EnsureSettingActionTypeCodeEnum() == JournalActionTypeEnum.DebitCreditAndVatdeduction)
                {
                    if (!GLAccountTaxChecked.HasValue)
                    {
                        GLAccountTaxChecked = AccountTaxChecked();
                    }
                }
                journalLine.EnsureAllDecimalPrecisionIfChangeChangeUpdate();


                var currDebitControlAccountId = GetControlAccountId(journalLine.DebitAccountId);
                if (journalLine.DebitControlAccountId != currDebitControlAccountId)
                {
                    journalLine.DebitControlAccountId = currDebitControlAccountId;
                    if (journalLine.ChangeSetOp == ChangeSetOperation.None)
                    {
                        journalLine.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }


                var currCreditControlAccountId = GetControlAccountId(journalLine.CreditAccountId);
                if (journalLine.CreditControlAccountId != currCreditControlAccountId)
                {
                    journalLine.CreditControlAccountId = currCreditControlAccountId;
                    if (journalLine.ChangeSetOp == ChangeSetOperation.None)
                    {
                        journalLine.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
        }

        private bool AccountTaxChecked()
        {
            var full = FullAccountingSettingQueryService.Get(_JournalPM.Tenant);

            var pm = GetValidGLAccount(full.VATOutputGLAccountId);
            return true;
        }

        private string GetControlAccountId(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return null;
            }

            GLAccountPM pm = GetValidGLAccount(accountId);
            var controlId = pm.ControlAccountId;
            if (!String.IsNullOrWhiteSpace(controlId))
            {
                var controlpm = GetValidGLAccount(controlId);
            }

            return controlId;
        }

        private GLAccountPM GetValidGLAccount(string accountId)
        {
            GLAccountPM pm = null;
            GLAccountQueryService qs;
            qs = new GLAccountQueryService(_JournalPM.Tenant);
            pm = qs.GetSingle(accountId, false, true);
            if (pm == null)
            {
                throw new ApplicationException("accountId not found" + accountId);
            }
            if (pm.Tenant != _JournalPM.Tenant)
            {
                throw new ApplicationException("accountId not found in tenant " + accountId);
            }
            
            ValidationResult res = GLAccountValidator./*IsGLAccountValid*/IsGLAccountValidCacheDueFromJournal(pm);
            if (res != null)
            {
                throw new ApplicationException("GLAccountValidator.IsGLAccountValid :" + res.ErrorMessage);
            }
            if (pm.AccountTypeCode != "1" && String.IsNullOrWhiteSpace(pm.ControlAccountId))
            {
                throw new ApplicationException("GLAccount is not a card (AccountTypeCode != 1 ) and there is no ControlAccountId " + pm.SearchFields);
            }

            return pm;
        }
        public bool StreamingJournalAlreadChecked = false;
        public virtual bool ParseIt()
        {
            try
            {
                string logtext = "";
                if (_JournalPM.StatusCode == "6" )
                {
                    logtext = "JournalApproveParser.ParseIt(), Point 1, Journal " + _JournalPM.JournalNumber + ", T=" + _JournalPM.Tenant.ToString()
                        + ", Status=" + _JournalPM.StatusCode
                        + ", QueueId=" + _JournalPM.QueueId;
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

                    OnApproveUpdatingFillArrangeJournalPMResetControlAccount();
                }

                if (!StreamingJournalAlreadChecked)
                {
                    CheckJournal();
                }

                if (!String.IsNullOrWhiteSpace(_JournalPM.QueueId))
                {
                    throw new //Exception("Parsing allowed b4 Streaming to Account ");
                        JournalApproveException("Parsing allowed b4 Streaming to Account ", WhatTODOJournalApproveEnum.ClearQueue);
                }

                TenantQuery tenantQuery = new TenantQuery(_JournalPM.Tenant);
                TenantPM tPM = tenantQuery.GetSinglePM(_JournalPM.Tenant);
                _LocalAccountingCurrencyId = tPM.CurrencyId;
                if (_JournalPM.JournalLines.Count < 1)
                {
                    throw new ApplicationException("JournalApproveParser(" + this._JournalPM.Id + "): No Journal line ");
                }

                logtext = "JournalApproveParser.ParseIt(), Point 2, Journal " + _JournalPM.JournalNumber + ", T=" + _JournalPM.Tenant.ToString()
                    + ", Status=" + _JournalPM.StatusCode
                    + ", QueueId=" + _JournalPM.QueueId;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

                CreateLedger_MapByJournalActionType();

                CheckLedgerTransactions();

                logtext = "JournalApproveParser.ParseIt(), Point 5, Journal " + _JournalPM.JournalNumber + ", T=" + _JournalPM.Tenant.ToString()
                    + ", Status=" + _JournalPM.StatusCode
                    + ", QueueId=" + _JournalPM.QueueId;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

                CreateGLAccountTotalByMonthFromLedger();
                CheckGLAccountTotalByMonth();
                if (_JournalPM.StatusCode == "6" )
                {
                    CreateControlGLAccountTotalByMonthFromLedger();
                    CheckControlGLAccountTotalByMonths();
                }

                logtext = "JournalApproveParser.ParseIt(), Point 8, Journal " + _JournalPM.JournalNumber + ", T=" + _JournalPM.Tenant.ToString()
                    + ", Status=" + _JournalPM.StatusCode
                    + ", QueueId=" + _JournalPM.QueueId;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

                CheckTotalByMonthDateType();


                logtext = "JournalApproveParser.ParseIt(), Point 9, Journal " + _JournalPM.JournalNumber + ", T=" + _JournalPM.Tenant.ToString()
                    + ", Status=" + _JournalPM.StatusCode
                    + ", QueueId=" + _JournalPM.QueueId;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_ErrorsList != null && _ErrorsList.Count > 0)
                {


                    string errorString = String.Empty;
                    foreach (string error in _ErrorsList)
                    {
                        errorString = errorString + error + ",";
                    }

                    errorString = errorString.Remove(errorString.Length - 1);
                    var errorText = errorString + ", Number=" + _JournalPM.ExternalNo + @"/" + _JournalPM.Id;
                    //ThrowException(errorText);
                    throw new ApplicationException(errorText);
                    //throw new ApplicationException(errorString);
                }
            }
            return true;
        }

        public void CreateLedger_MapByJournalActionType()
        {
            foreach (JournalLinePM item in _JournalPM.JournalLines)
            {
                switch (item.EnsureSettingActionTypeCodeEnum())
                {
                    case JournalActionTypeEnum.Credit:
                        AddCredit(item);
                        break;
                    case JournalActionTypeEnum.Debit:
                        AddDebit(item, false);
                        break;
                    case JournalActionTypeEnum.DebitAndCredit:
                        AddCredit(item);
                        AddDebit(item, false);
                        break;
                    case JournalActionTypeEnum.DebitCreditAndVatdeduction:
                        AddCredit(item);
                        AddDebit(item, true);
                        AddTaxDebit(item);
                        break;
                    case JournalActionTypeEnum.NotValid:
                    default:
                        throw new ApplicationException("JournalApproveParser():JournalActionType is must ");
                        break;
                }


            }
        }

        private void CheckTotalByMonthDateType()
        {

            if (!this.GLAccountTotalByMonths.Any())
            {
                ThrowExceptionAxiom("GLAccountTotalByMonths is must product (ControlGLAccountTotalByMonths is not must)");
            }

            var totgDateTypeCode = (
                from tot in this.GLAccountTotalByMonths
                group tot by tot.DateTypeCode into gDateTypeCode
                select new {
                    //gDateTypeCode.Key,
                    LocalAmountDebitT = gDateTypeCode.Sum(t => t.LocalAmountDebit),
                    LocalAmountCreditT = gDateTypeCode.Sum(t => t.LocalAmountCredit),
                    ForeignAmountDebitT = gDateTypeCode.Sum(t => t.ForeignAmountDebit),
                    ForeignAmountCreditT = gDateTypeCode.Sum(t => t.ForeignAmountCredit),
                }
                ).ToList();
            if (totgDateTypeCode.Count != 3)
            {
                ThrowExceptionAxiom("GLAccountTotalByMonths Group by  DateTypeCode.Count != 3 (accounting,due,document) ");
            }
            var distincttotgDateTypeCode = totgDateTypeCode.Distinct().ToList();
            if (distincttotgDateTypeCode.Count != 1)
            {
                ThrowExceptionAxiom("All GLAccountTotalByMonths Group by DateTypeCode  have to be same Sum(LocalAmountDebit/LocalAmountCredit/ForeignAmountCredit)");
            }


            if (!this.ControlGLAccountTotalByMonths.Any())
            {
                return;
            }

            var totgDateTypeCodeControl = (
                from tot in this.ControlGLAccountTotalByMonths
                group tot by tot.DateTypeCode into gDateTypeCode
                select new
                {
                    //gDateTypeCode.Key,
                    LocalAmountDebitT = gDateTypeCode.Sum(t => t.LocalAmountDebit),
                    LocalAmountCreditT = gDateTypeCode.Sum(t => t.LocalAmountCredit),
                    ForeignAmountDebitT = gDateTypeCode.Sum(t => t.ForeignAmountDebit),
                    ForeignAmountCreditT = gDateTypeCode.Sum(t => t.ForeignAmountCredit),
                }
                ).ToList();
            if (totgDateTypeCodeControl.Count != 3)
            {
                ThrowExceptionAxiom("ControlGLAccountTotalByMonths Group by  DateTypeCode.Count != 3 (accounting,due,document) ");
            }
            var totgDateTypeCodeControlDistinct = totgDateTypeCodeControl.Distinct().ToList();
            if (totgDateTypeCodeControlDistinct.Count != 1)
            {
                ThrowExceptionAxiom("All ControlGLAccountTotalByMonths Group by DateTypeCode  have to be same Sum(LocalAmountDebit/LocalAmountCredit/ForeignAmountCredit)");
            }

        }

        private void CreateGLAccountTotalByMonthFromLedger()
        {
            List<GLAccountTotalByMonthPM> my;
            var GLAccountTotalAccountingdate =
            (from rec in this.LedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 rec.AccountId,
                 rec.CurrencyId,
                 rec.AccountingDate.Year,
                 rec.AccountingDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });

            var GLAccountTotalDocumentDate =
            (from rec in this.LedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 rec.AccountId,
                 rec.CurrencyId,
                 rec.DocumentDate.Year,
                 rec.DocumentDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.DocumentDate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });


            var GLAccountTotalDueDate =
            (from rec in this.LedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 rec.AccountId,
                 rec.CurrencyId,
                 rec.DueDate.Year,
                 rec.DueDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });

            bool UnionreturnsDistinctvalues = true;
            if (UnionreturnsDistinctvalues)
            {
                GLAccountTotalByMonths = GLAccountTotalAccountingdate.Concat(GLAccountTotalDueDate).Concat(GLAccountTotalDocumentDate).ToList();
            }
            else
            {
                GLAccountTotalByMonths = GLAccountTotalAccountingdate.Union(GLAccountTotalDueDate).Union(GLAccountTotalDocumentDate).ToList();
            }

            foreach (var item in GLAccountTotalAccountingdate)
            {
                if (!GLAccountTotalDueDate.Any(x => x.Tenant == item.Tenant  && x.AccountId == item.AccountId && x.Month == item.Month && x.CurrencyId == item.CurrencyId))
                {
                    GLAccountTotalByMonths.Add(new GLAccountTotalByMonthPM()
                    {
                        Tenant = item.Tenant,
                        AccountId = item.AccountId,
                        DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                        CurrencyId = item.CurrencyId,

                        Year = item.Year,
                        Month = item.Month,

                        LocalAmountCredit = 0,
                        LocalAmountDebit = 0,
                        ForeignAmountCredit = 0,
                        ForeignAmountDebit = 0,

                    });
                }
            }

        }


        private void CreateControlGLAccountTotalByMonthFromLedger()
        {
            var controlLedgerTransactions = this.LedgerTransactions.Where(trans => !string.IsNullOrEmpty(trans.ControlAccountId));
            List<GLAccountTotalByMonthPM> my;
            var GLAccountTotalAccountingdate =
            (from rec in controlLedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 AccountId = rec.ControlAccountId,
                 rec.CurrencyId,
                 rec.AccountingDate.Year,
                 rec.AccountingDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });

            var GLAccountTotalDocumentDate =
            (from rec in controlLedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 AccountId = rec.ControlAccountId,
                 rec.CurrencyId,
                 rec.DocumentDate.Year,
                 rec.DocumentDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.DocumentDate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });


            var GLAccountTotalDueDate =
            (from rec in controlLedgerTransactions
             group rec by new
             {
                 rec.Tenant,
                 AccountId = rec.ControlAccountId,
                 rec.CurrencyId,
                 rec.DueDate.Year,
                 rec.DueDate.Month,
             } into groupByAccountCurrency
             select new GLAccountTotalByMonthPM()
             {
                 Tenant = groupByAccountCurrency.Key.Tenant,
                 AccountId = groupByAccountCurrency.Key.AccountId,
                 DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                 CurrencyId = groupByAccountCurrency.Key.CurrencyId,

                 Year = groupByAccountCurrency.Key.Year,
                 Month = groupByAccountCurrency.Key.Month,

                 LocalAmountCredit = groupByAccountCurrency.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebit = groupByAccountCurrency.Sum(x => x.LocalAmountDebit),
                 ForeignAmountCredit = groupByAccountCurrency.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebit = groupByAccountCurrency.Sum(x => x.ForeignAmountDebit),

             });
            bool UnionreturnsDistinctvalues = true;
            if (UnionreturnsDistinctvalues)
            {
                ControlGLAccountTotalByMonths = GLAccountTotalAccountingdate.Concat(GLAccountTotalDueDate).Concat(GLAccountTotalDocumentDate).ToList();
            }
            else
            {
                ControlGLAccountTotalByMonths = GLAccountTotalAccountingdate.Union(GLAccountTotalDueDate).Union(GLAccountTotalDocumentDate).ToList();
            }
            foreach (var item in GLAccountTotalAccountingdate)
            {
                if (!GLAccountTotalDueDate.Any(x => x.Tenant == item.Tenant && x.AccountId == item.AccountId && x.Month == item.Month && x.CurrencyId == item.CurrencyId))
                {
                    ControlGLAccountTotalByMonths.Add(new GLAccountTotalByMonthPM()
                    {
                        Tenant = item.Tenant,
                        AccountId = item.AccountId,
                        DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                        CurrencyId = item.CurrencyId,

                        Year = item.Year,
                        Month = item.Month,

                        LocalAmountCredit = 0,
                        LocalAmountDebit = 0,
                        ForeignAmountCredit = 0,
                        ForeignAmountDebit = 0,

                    });
                }
            }
        }

        void CheckControlGLAccountTotalByMonths()
        {



            //eyal : While Insert GLAccount there is connect to Control account;
            var myConnectedControlGLAccountTotalByMonthsAccountingdate = ControlGLAccountTotalByMonths.Where(r => r.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate);


            CheckNoSameAccIdJoinTotalAndControlTotal();

            var controlLocalAmount = CheckControlTotalPlusCardTransTotalEqual0(myConnectedControlGLAccountTotalByMonthsAccountingdate);

            /// Check _JournalApproveParser.LedgerTransactionsConnectedControl  == _ConnectedControlGLAccountTotalByMonths

            var transControlGLAccountAsTotalByMonths =
                (from trans in LedgerTransactions
                 where !string.IsNullOrWhiteSpace(trans.ControlAccountId)
                 group trans by
                 new
                 {
                     trans.ControlAccountId,
                     trans.CurrencyId,
                     trans.Tenant,
                     trans.AccountingDate.Year,
                     trans.AccountingDate.Month
                 }
                     into gControlAccountIdCurrencyIdTenant
                 select new GLAccountTotalByMonthPM()
                 {
                     AccountId = gControlAccountIdCurrencyIdTenant.Key.ControlAccountId,
                     CurrencyId = gControlAccountIdCurrencyIdTenant.Key.CurrencyId,
                     Tenant = gControlAccountIdCurrencyIdTenant.Key.Tenant,
                     Year = gControlAccountIdCurrencyIdTenant.Key.Year,
                     Month = gControlAccountIdCurrencyIdTenant.Key.Month,

                     ForeignAmountCredit = gControlAccountIdCurrencyIdTenant.Sum(trans => trans.ForeignAmountCredit),
                     ForeignAmountDebit = gControlAccountIdCurrencyIdTenant.Sum(trans => trans.ForeignAmountDebit),

                     LocalAmountCredit = gControlAccountIdCurrencyIdTenant.Sum(trans => trans.LocalAmountCredit),
                     LocalAmountDebit = gControlAccountIdCurrencyIdTenant.Sum(trans => trans.LocalAmountDebit),

                 }).ToList();


            var totalsAmount = myConnectedControlGLAccountTotalByMonthsAccountingdate
                .Sum(r => r.ForeignAmountDebit);
            var transAmount = transControlGLAccountAsTotalByMonths.Sum(r => r.ForeignAmountDebit);
            if (totalsAmount != transAmount)
            {
                var mmes = "Journal:" + _JournalPM.Id + " control(ForeignAmountDebit)<>transControlGLAccountAsTotalByMonths(ForeignAmountDebit) =" + controlLocalAmount.ToString();
                ThrowExceptionAxiom(mmes);
            }
            totalsAmount = myConnectedControlGLAccountTotalByMonthsAccountingdate
                .Sum(r => r.ForeignAmountCredit);
            transAmount = transControlGLAccountAsTotalByMonths.Sum(r => r.ForeignAmountCredit);
            if (totalsAmount != transAmount)
            {
                var mmes = "Journal:" + _JournalPM.Id + " control(ForeignAmountCredit)<>transControlGLAccountAsTotalByMonths(ForeignAmountCredit) =" + controlLocalAmount.ToString();
                ThrowExceptionAxiom(mmes);
            }
            totalsAmount = myConnectedControlGLAccountTotalByMonthsAccountingdate
                .Sum(r => r.LocalAmountDebit);
            transAmount = transControlGLAccountAsTotalByMonths.Sum(r => r.LocalAmountDebit);
            if (totalsAmount != transAmount)
            {
                var mmes = "Journal:" + _JournalPM.Id + " control(LocalAmountDebit)<>transControlGLAccountAsTotalByMonths(LocalAmountDebit) =" + controlLocalAmount.ToString();
                ThrowExceptionAxiom(mmes);
            }
            totalsAmount = myConnectedControlGLAccountTotalByMonthsAccountingdate
                .Sum(r => r.LocalAmountCredit);
            transAmount = transControlGLAccountAsTotalByMonths.Sum(r => r.LocalAmountCredit);

            if (totalsAmount != transAmount)
            {
                var mmes = "Journal:" + _JournalPM.Id + " control(LocalAmountCredit)<>transControlGLAccountAsTotalByMonths(LocalAmountCredit) =" + controlLocalAmount.ToString();
                ThrowExceptionAxiom(mmes);
            }
        }

        private decimal CheckControlTotalPlusCardTransTotalEqual0(IEnumerable<GLAccountTotalByMonthPM> myConnectedControlGLAccountTotalByMonthsAccountingdate)
        {
            var transWithoutControlAccountItsMustBeCardType = this.LedgerTransactions.Where(trans => String.IsNullOrWhiteSpace(trans.ControlAccountId));
            var cardsLocalAmount = transWithoutControlAccountItsMustBeCardType.Sum(trans => trans.LocalAmountDebit - trans.LocalAmountCredit);
            var controlLocalAmount = myConnectedControlGLAccountTotalByMonthsAccountingdate.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit);
            if (controlLocalAmount + cardsLocalAmount != 0)
            {
                var mmes =
                    //"(not all JL Have ControlAccount  (crads like Tax) ,due that the _ConnectedControlGLAccountTotalByMonths total isnot equal)Journal:" + _JournalPM.Id + " controlLocalAmount =" + controlLocalAmount.ToString();
                    "Journal:" + _JournalPM.Id + "  Control account TotalLocalAmount plus cards TotalLocalAmount must mb zero";
                LogMessagingUtil.Instance.AppendLine("Error Journal:" + _JournalPM.Id + " controlLocalAmount =" + controlLocalAmount.ToString());
                ThrowExceptionAxiom(mmes);
            }
            return controlLocalAmount;
        }

        private void CheckNoSameAccIdJoinTotalAndControlTotal()
        {
            var join1 = (from real in GLAccountTotalByMonths
                         join control in ControlGLAccountTotalByMonths
                         on real.AccountId equals control.AccountId
                         select control.AccountId).ToList();


            if (join1.Count > 0)
            {
                ThrowExceptionAxiom("(controlaccount(TotalByMonths) should not by in regular GLAccountTotalByMonths)_ControlGLAccountTotalByMonths join _JournalApproveParser.GLAccountTotalByMonths have rows " +
                    join1.Aggregate((b, n) => String.Concat(b, ",", n)));
            }
        }



        private void CheckGLAccountTotalByMonth()
        {
            bool existWithoutCurrency = GLAccountTotalByMonths.Where(rec => rec.CurrencyId == null).Any();
            if (existWithoutCurrency)
            {
                ThrowExceptionAxiom("CurrencyId is must !!");
            }
            var totalLocalAmountCredit = GLAccountTotalByMonths.Sum(rec => rec.LocalAmountCredit);
            var totalLocalAmountDebit = GLAccountTotalByMonths.Sum(rec => rec.LocalAmountDebit);
            if (!totalLocalAmountDebit.Equals(totalLocalAmountCredit))
            {

                ThrowExceptionAxiom("CheckGLAccountTotalByMonth() totalLocalAmountDebit != totalLocalAmountCredit");
            }


            var GLAccountTotalByMonthsByCurrencyId = GLAccountTotalByMonths.ToLookup(rec => rec.CurrencyId);
            foreach (var itemCurrencyId in GLAccountTotalByMonthsByCurrencyId)
            {
                var totalForeignAmountCredit = itemCurrencyId.Sum(rec => rec.ForeignAmountCredit);
                var totalForeignAmountDebit = itemCurrencyId.Sum(rec => rec.ForeignAmountDebit);
                if (!totalForeignAmountCredit.Equals(totalForeignAmountDebit))
                {

                    if (!_JournalForeignAmountCanBeUnEqual)
                    {
                        ThrowExceptionAxiom(
                        String.Format("CheckGLAccountTotalByMonth(),totalForeignAmountCredit:{0} != totalForeignAmountDebit:{1} (CurrencyId:{2})", totalForeignAmountCredit, totalForeignAmountDebit, itemCurrencyId.Key)
                        );
                    }
                }

            }
        }


        void ThrowExceptionAxiom(string message)
        {
            _ErrorsList = _ErrorsList ?? new List<string>();
            _ErrorsList.Add(message);
            return;
            throw new ApplicationException(message);
        }
        private void CheckLedgerTransactions()
        {
            var TotalLocalAmountInJornal = _JournalPM.JournalLines
                .Where(jl => jl.EnsureSettingActionTypeCodeEnum() != JournalActionTypeEnum.Debit) // Why credit ? credit is not vat splitded (like debit)
                 .Sum(jl => jl.LocalAmount);

            var totalLocalAmountCredit = LedgerTransactions.Sum(rec => rec.LocalAmountCredit);
            var totalLocalAmountDebit =  LedgerTransactions.Sum(rec => rec.LocalAmountDebit) ;
            if (!totalLocalAmountCredit.Equals(totalLocalAmountDebit))
            {
                ThrowExceptionAxiom("totalLocalAmountDebit != totalLocalAmountCredit");
            }
            if (TotalLocalAmountInJornal != totalLocalAmountCredit)
            {
                ThrowExceptionAxiom("TotalLocalAmountInJornal!=totalLocalAmountCredit");
            }
            var LedgerTransactionsByCurrencyId = LedgerTransactions.ToLookup(rec => rec.CurrencyId);
            foreach (var itemCurrencyId in LedgerTransactionsByCurrencyId)
            {
                var totalForeignAmountCredit = itemCurrencyId.Sum(rec => rec.ForeignAmountCredit);
                var totalForeignAmountDebit = itemCurrencyId.Sum(rec => rec.ForeignAmountDebit);
                if (!totalForeignAmountCredit.Equals(totalForeignAmountDebit))
                {
                    if (!_JournalForeignAmountCanBeUnEqual)
                    {
                        ThrowExceptionAxiom(
                            String.Format("totalForeignAmountCredit:{0} != totalForeignAmountDebit:{1} (CurrencyId:{2})", totalForeignAmountCredit, totalForeignAmountDebit, itemCurrencyId.Key)
                            );
                    }
                }

            }

        }

        private void CheckJournal()
        {

            var validationResult = JournalValidator.IsJournalValid(_JournalPM, _JournalValidatorContext);
            if (validationResult != null)
            {

                string errorString = String.Empty;
                foreach (string error in validationResult.MemberNames)
                {
                    errorString = errorString + error + ",";
                }

                errorString = errorString.Remove(errorString.Length - 1);
                throw new ApplicationException(errorString);


                //                string errorText = validationResult.ErrorMessage + ", Number=" + _JournalPM.ExternalNo + @"/" + _JournalPM.Id + ", " + validationResult.MemberNames.FirstOrDefault();
                //                    //+validationResult.MemberNames.Aggregate((a, b) => string.Concat(a, ",", b));


                ////ThrowException(errorText);
                //                throw new ApplicationException(errorText);
            }
            //_JournalPM.JournalLines.ToLookup(rec => rec.ActionTypeCodeEnum);

        }
        private void AddTaxDebit(JournalLinePM item)
        {
            var journalLineDebitMapping = new JournalLineDebitTaxMapping(item, _JournalPM, GetIJournalValidatorContextDataProvider(), GetIIAccountingSettingResolver());
            journalLineDebitMapping.DoIt();
            LedgerTransactions.Add(journalLineDebitMapping.MyLedgerTransaction);
            //AddMyGLAccountTotalByMonth(journalLineDebitMapping.MyGLAccountTotalByMonth);
        }

        
        private void AddDebit(JournalLinePM item, bool vatExtract)
        {
            var journalLineDebitMapping = new JournalLineDebitMapping(item, _JournalPM, vatExtract, GetIJournalValidatorContextDataProvider(), GetIIAccountingSettingResolver());
            journalLineDebitMapping.DoIt();
            LedgerTransactions.Add(journalLineDebitMapping.MyLedgerTransaction);
            //AddMyGLAccountTotalByMonth(journalLineDebitMapping.MyGLAccountTotalByMonth);
        }

        private IAccountingSettingResolver GetIIAccountingSettingResolver()
        {
            return _JournalValidatorContext.GetService(typeof(IAccountingSettingResolver)) as IAccountingSettingResolver;
        }

        IJournalValidatorContextDataProvider GetIJournalValidatorContextDataProvider()
        {
            return _JournalValidatorContext.GetService(typeof(IJournalValidatorContextDataProvider)) as IJournalValidatorContextDataProvider;
        }
        private void AddCredit(JournalLinePM item)
        {
            var journalLineCreditMapping = new JournalLineCreditMapping(item, _JournalPM, GetIJournalValidatorContextDataProvider(),GetIIAccountingSettingResolver());
            journalLineCreditMapping.DoIt();
            LedgerTransactions.Add(journalLineCreditMapping.MyLedgerTransaction);
            //AddMyGLAccountTotalByMonth(journalLineCreditMapping.MyGLAccountTotalByMonth);
        }
#if false
        void AddMyGLAccountTotalByMonth(//List<GLAccountTotalByMonthPM> MyGLAccountTotalByMonths, 
            GLAccountTotalByMonthPM MyGLAccountTotalByMonth)
        {
            GLAccountTotalByMonthPM currGLAccountTotalByMounth = (from a in _PRIVATEOLD_GLAccountTotalByMonths
                                                                  where a.AccountId == MyGLAccountTotalByMonth.AccountId &&
                                                a.CurrencyId == MyGLAccountTotalByMonth.CurrencyId &&
                                                a.Year == MyGLAccountTotalByMonth.Year &&
                                                a.Month == MyGLAccountTotalByMonth.Month


                                                && a.Tenant == MyGLAccountTotalByMonth.Tenant
                                                                  select a).FirstOrDefault();

            if (currGLAccountTotalByMounth == null)
            {


                //GLAccountTotalByMonthPM myGLAccountTotalByMonth = DefaultMapGLAccountTotalByMounth(MyLedgerTransaction);

                _PRIVATEOLD_GLAccountTotalByMonths.Add(MyGLAccountTotalByMonth);

            }
            else
            {
                currGLAccountTotalByMounth.LocalAmountCredit += MyGLAccountTotalByMonth.LocalAmountCredit;
                currGLAccountTotalByMounth.LocalAmountDebit += MyGLAccountTotalByMonth.LocalAmountDebit;
                currGLAccountTotalByMounth.ForeignAmountDebit += MyGLAccountTotalByMonth.ForeignAmountDebit;
                currGLAccountTotalByMounth.ForeignAmountCredit += MyGLAccountTotalByMonth.ForeignAmountCredit;
            }
        }

        
#endif
        private string GetIdCounter(int Tenant)
        {
            string idCounter = null;
            if (_includeIdCounter)
            {
                idCounter = IdCounter.GetNumber("LedgerTransaction", Tenant);
            }
            else
            {
                //myTaxTransaction.Id = IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
            }
            return idCounter;

        }









        public List<LedgerTransactionPM> LedgerTransactions { get; private set; }

        //List<GLAccountTotalByMonthPM> _PRIVATEOLD_GLAccountTotalByMonths;
        public List<GLAccountTotalByMonthPM> GLAccountTotalByMonths { get; private set; }


        public string ErrorMessage { get; set; }

        public List<GLAccountTotalByMonthPM> ControlGLAccountTotalByMonths { get; private set; }
    }

    public enum WhatTODOJournalApproveEnum
    {
        MakeItFailed,
        ClearQueue,
        
    }

    public class JournalApproveException : Exception
    {
        public WhatTODOJournalApproveEnum WhatTODO { get; private set; }


        public JournalApproveException(string message, WhatTODOJournalApproveEnum whatTODOEnum)
            :base(message)
        {
            this.WhatTODO = whatTODOEnum;
        }
    }
}
