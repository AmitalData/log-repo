using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ExpenseAllocationFlowService
    {
         bool isNewEntity;
        private int tenant;
        public ExpenseAllocationFlow Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ExpenseAllocationFlowPM entityPM;
        private IInvoiceContext objectContext;
        private ExpenseAllocationFlowRepository entityRepository;
        private APInvoiceNormalService aPInvoiceNormalService;
        public ExpenseAllocationFlowService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ExpenseAllocationFlowRepository(objectContext);
        }

        public void Create(ExpenseAllocationFlowPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ExpenseAllocationFlow", tenant).ToString();
            this.Poco = new ExpenseAllocationFlow();
            this.Poco.Id = this.entityPM.Id;
            ExpenseAllocationFlowMapping.MapEntity(entityPM, Poco, isNewEntity);
            if (theEntityPm.RunDate != null && theEntityPm.RunDate.Date == DateTime.Today.Date)
            {
                RunTaskNow();
            }
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ExpenseAllocationFlowPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleById(theEntityPm.Id, entityPM.Tenant);
            ExpenseAllocationFlowMapping.MapEntity(entityPM, Poco, isNewEntity);
            if(theEntityPm.RunDate != null && theEntityPm.RunDate.Date == DateTime.Today.Date)
            {
                RunTaskNow();               
            }           
             entityRepository.Update(Poco);
             entityRepository.SubmitChanges();
           

        }

        public void RunTaskNow()
        {
            ExpenseAllocationSettingQuery query = new ExpenseAllocationSettingQuery();
            ExpenseAllocationSettingPM settingPM = query.GetSinglePM(entityPM.SettingId,entityPM.Tenant);
            if(settingPM == null)
            {
                throw new Exception("Expense Allocation Setting not found.");
            }

            APInvoiceQuery invoiceQuery = new APInvoiceQuery(settingPM.Tenant);
            var apInvoice = invoiceQuery.GetSinglePM(settingPM.EntityId, settingPM.Tenant);
            if(apInvoice == null)
            {
                throw new Exception("AP Invoice not found.");
            }
            Poco.JournalId = AddJournalAndJournalLines(apInvoice, settingPM);

            AddTask(settingPM);
        }
        public void AddTask(ExpenseAllocationSettingPM settingPM)
        {
            ExpenseAllocationFlow expenseAllocationFlow = new ExpenseAllocationFlow();
            expenseAllocationFlow.Id = IdCounter.GetNumber("ExpenseAllocationFlow", tenant).ToString();
            expenseAllocationFlow.Tenant = this.entityPM.Tenant;
            expenseAllocationFlow.SettingId = this.entityPM.SettingId;
            expenseAllocationFlow.Status = "";
            expenseAllocationFlow.RunDate = DateTime.Now.AddDays(5);
            expenseAllocationFlow.JournalId =null;

            entityRepository.Add(expenseAllocationFlow);
            entityRepository.SubmitChanges();


        }


        private string AddJournalAndJournalLines(APInvoicePM theEntityPm, ExpenseAllocationSettingPM settingPM)
        {
            int tenant = theEntityPm.Tenant;
            aPInvoiceNormalService = new APInvoiceNormalService(this.ObjectContext, theEntityPm);
          
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            FullAccountingSettingPM accountingSettings = getFullAccountingSettings(theEntityPm.Tenant);
            if (tenantPOCO.AccountingActivated)
                {                  
                    JournalPM journal = new JournalPM();
                    journal.Tenant = tenant;
                    journal.JournalNumber = "1";
                    journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.TypeCode = "0";
                    journal.StatusCode = "6";
                    journal.CreatedByUserId = theEntityPm.CreatedByUserId;
                    journal.AccountingEntityCode = "4";
                    journal.AccountingEntityId = theEntityPm.Id;
                    journal.AccountingEntityReference = theEntityPm.InvoiceNumber;
                    journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                    journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;
                    journal.ChangeSetOp = ChangeSetOperation.Insert;

                    bool differentCurrencies = false;
                    List<string> currencies = new List<string>();
                    if (theEntityPm.IsExternalEntity && theEntityPm.InvoiceLines != null && theEntityPm.InvoiceLines != null && theEntityPm.InvoiceLines.Count > 1)
                    {
                        currencies = theEntityPm.InvoiceLines.Select(line => line.ForiegnCurrencyId).Distinct().ToList();
                        if (currencies != null && currencies.Count > 1)
                        {
                            differentCurrencies = true;
                        }
                    }
                    int counter = 0;
                    Accounting.Def.EntityPMs.GLAccountPM glAccount = aPInvoiceNormalService.GetInvoiceGLAccount(theEntityPm);
                    JournalLinePM journalLine = new JournalLinePM();
                    if (!differentCurrencies)
                    {
                        journalLine = CreateJournalLinePM(theEntityPm, ++counter, journal.Id, glAccount.Id, theEntityPm.IsPrepaidExpenses ? accountingSettings?.PrepaidExpensesGLAccountId : null);

                        journalLine.LocalAmount = (decimal)theEntityPm.AmountInLocalCurrency / settingPM.NumberOfPayments;
                        journalLine.CurrencyId = theEntityPm.InvoiceCurrencyId;
                        journalLine.ForeignAmount = (decimal)theEntityPm.AmountInInvoiceCurrency / settingPM.NumberOfPayments;
                        journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;

                        journal.JournalLines.Add(journalLine);
                    }
                    else
                    {
                        foreach (var curr in currencies)
                        {
                            journalLine = CreateJournalLinePM(theEntityPm, ++counter, journal.Id, glAccount.Id, theEntityPm.IsPrepaidExpenses ? accountingSettings?.PrepaidExpensesGLAccountId : null);

                            journalLine.LocalAmount = ((decimal)theEntityPm.InvoiceLines.Where(ln => ln.ForiegnCurrencyId == curr).Sum(ln => ln.LocalCurrencyAmount)) / settingPM.NumberOfPayments;
                            journalLine.CurrencyId = curr;
                            journalLine.ForeignAmount = ((decimal)theEntityPm.InvoiceLines.Where(ln => ln.ForiegnCurrencyId == curr).Sum(ln => ln.ForiegnCurrencyAmount)) / settingPM.NumberOfPayments;
                            journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceLines.Where(ln => ln.ForiegnCurrencyId == curr).FirstOrDefault().ForiegnExchangeRate;

                            journal.JournalLines.Add(journalLine);
                        }
                    }

                    journalLine = new JournalLinePM();

                    List<JournalLinePM> journalDebitLines = new List<JournalLinePM>();
                if (theEntityPm.InvoiceLines[0].VatRecognizedPercentage == 0 || theEntityPm.InvoiceLines[0].VatRecognizedPercentage == null)
                {
                    if (theEntityPm.InvoiceLines[0].VatPercentage == null || theEntityPm.InvoiceLines[0].VatRecognizedPercentage == null)
                    {
                        theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized = theEntityPm.InvoiceLines[0].LocalCurrencyAmount;
                    }
                    else
                    {
                        theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized = theEntityPm.InvoiceLines[0].LocalCurrencyAmount + ((theEntityPm.InvoiceLines[0].VatPercentage / 100) * theEntityPm.InvoiceLines[0].LocalCurrencyAmount);
                    }

                }
                else
                {
                    theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized = (theEntityPm.InvoiceLines[0].LocalCurrencyAmount + ((theEntityPm.InvoiceLines[0].VatPercentage / 100) * ((1 - theEntityPm.InvoiceLines[0].VatRecognizedPercentage) * theEntityPm.InvoiceLines[0].LocalCurrencyAmount)));
                }
                theEntityPm.InvoiceLines[0].ForiegnAmountWithRecognizedVat = theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized != null ? theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized / theEntityPm.InvoiceLines[0].ForiegnExchangeRate : theEntityPm.InvoiceLines[0].LocalAmountWithVatRecognized;



                List<JournalLinePM> journalLines = (from d in theEntityPm.InvoiceLines?.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete)
                                                        select new JournalLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ActionCode = AccountingActionCodes.Debit,
                                                            ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                                                            JournalId = journal.Id,
                                                            DebitAccountId =  d.ChargeTypeGLAccountId,
                                                            CreditAccountId = theEntityPm.VendorGLAccountId,
                                                            Line = ++counter,
                                                            DocumentDate = theEntityPm.InvoiceDate.Value,
                                                            AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant),
                                                            DueDate = theEntityPm.DueDate.Value,
                                                            LocalAmount = (Math.Round(d.VatRecognizedPercentage == null ? (decimal)d.LocalCurrencyAmount.Value : (decimal)d.LocalAmountWithVatRecognized.Value, 2)) / settingPM.NumberOfPayments,
                                                            CurrencyId = d.ForiegnCurrencyId,
                                                            ForeignAmount= ( d?.ForiegnAmountWithRecognizedVat != null)   ? Math.Round((decimal)d.ForiegnAmountWithRecognizedVat, 2) / settingPM.NumberOfPayments : Math.Round((decimal)d.ForiegnAmountWithRecognizedVat, 2),
                                                            ExchangeRate = (decimal)d.ForiegnExchangeRate,
                                                            Reference1 = theEntityPm.InvoiceNumber,
                                                            Reference2 = theEntityPm.MainEntityReference,
                                                            Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber,
                                                            Notes = !string.IsNullOrEmpty(d.Notes) && !string.IsNullOrWhiteSpace(d.Notes) ? d.Notes : theEntityPm.InternalNotes,
                                                            ExcludeFromTaxReport = d.ExcludeFromTaxReport,
                                                        }).ToList();

                    journalDebitLines.AddRange(journalLines);
                    journal.JournalLines.AddRange(journalLines);
                    var totalDebitLines = journal.JournalLines.Where(d => d.ActionCode == AccountingActionCodes.Debit).Sum(d => d.LocalAmount);
                    counter = journal.JournalLines.Count();
                    var journalCreditAmount = journal.JournalLines.Where(d => d.ActionCode == AccountingActionCodes.Credit).FirstOrDefault().LocalAmount;
                    var difference = journalCreditAmount - totalDebitLines;

                    if (Math.Abs(difference) <= (decimal)0.06)
                    {
                        JournalLinePM largestJournalAmount = journalDebitLines.Where(d => d.LocalAmount == journalDebitLines.Max(a => a.LocalAmount)).FirstOrDefault();
                        journal.JournalLines.Where(d => d.Line == largestJournalAmount.Line).ToList().ForEach(d => { d.LocalAmount = d.LocalAmount + difference; d.ForeignAmount = d.ForeignAmount + difference; });
                    }


                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride(string.Empty, 1)) as IJournalUpdateServiceExt;
                    AddAccountingEntitieJournal(journal, AccountingEntityJournalActions.APInvoiceApprove,settingPM.Id);
                    journalUpdate.Update(journal);
                    return journal.Id;

                }

            return null;
           
        }

        private JournalLinePM CreateJournalLinePM(APInvoicePM theEntityPm, int lineNo, string journalId, string glAccountId, string prepaidExpensesGLAccountId = null)
        {
            APInvoiceNormalService aPInvoiceNormalService = new APInvoiceNormalService(this.ObjectContext, theEntityPm);
             int tenant = theEntityPm.Tenant;
            JournalLinePM journalLine = new JournalLinePM();
            journalLine.Tenant = tenant;
            journalLine.JournalId = journalId;
            journalLine.Line = lineNo;
            journalLine.ActionCode = AccountingActionCodes.Credit;
            journalLine.ActionTypeCodeEnum = JournalActionTypeEnum.Credit;
            journalLine.DocumentDate = theEntityPm.InvoiceDate.Value;
            journalLine.AccountingDate = theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            journalLine.DueDate = theEntityPm.DueDate.Value;

            journalLine.Reference1 = theEntityPm.InvoiceNumber;
            journalLine.Reference2 = theEntityPm.MainEntityReference;
            journalLine.Reference3 = !string.IsNullOrEmpty(theEntityPm.HouseNumber) ? theEntityPm.HouseNumber : theEntityPm.MasterNumber;
            journalLine.Notes = theEntityPm.InternalNotes;
            journalLine.CreditAccountId = glAccountId;

            journalLine.DebitAccountId =  aPInvoiceNormalService.SetDebitAccountForSingleLineAPInvoice(theEntityPm);
            journalLine.ChangeSetOp = ChangeSetOperation.Insert;
            return journalLine;
        }

        private void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingEntityJournalUpdateServiceExt service = ContainerAccessor.Container.Resolve(typeof(IAccountingEntityJournalUpdateServiceExt), "AccountingEntityJournalUpdateServiceExt", new ParameterOverride(string.Empty, 1)) as IAccountingEntityJournalUpdateServiceExt;
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }

        private FullAccountingSettingPM getFullAccountingSettings(int tenant)
        {
            FullAccountingSettingPM accountingSettings;
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride(string.Empty, 1)) as IFullAccountingSettingQueryServiceExt;
            accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
            return accountingSettings;
        }
    }
}