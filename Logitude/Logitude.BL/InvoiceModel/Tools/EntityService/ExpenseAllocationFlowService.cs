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
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            if (theEntityPm.RunDate != null && theEntityPm.RunDate.Date == DateTime.Today.Date)
            {
                RunTaskNow(Poco);
            }

        }

        public void Update(ExpenseAllocationFlowPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleById(theEntityPm.Id, entityPM.Tenant);
            ExpenseAllocationFlowMapping.MapEntity(entityPM, Poco, isNewEntity);
                    
             entityRepository.Update(Poco);
             entityRepository.SubmitChanges();
            if (theEntityPm.RunDate != null && theEntityPm.RunDate.Date == DateTime.Today.Date)
            {
                RunTaskNow(Poco);
            }

        }
        public bool ShouldCreateAnotherTask(string settingId,int numberOfPayments)
        {                                 
            return entityRepository.GetListBySettingId(tenant,settingId).Count() < numberOfPayments;
         }

        public void RunTaskNow(ExpenseAllocationFlow theEntity)
        {
            ExpenseAllocationSettingQuery query = new ExpenseAllocationSettingQuery(tenant);
            ExpenseAllocationSettingPM settingPM = query.GetSinglePM(theEntity.SettingId, tenant);
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
            try
            {
                theEntity.JournalId = AddJournalAndJournalLines(apInvoice, settingPM, theEntity.Id);
                entityRepository.Update(theEntity);
                entityRepository.SubmitChanges();
            }
            catch (Exception)
            {
                 theEntity.Status = "failed";
                entityRepository.Update(theEntity);
                entityRepository.SubmitChanges();
                throw;
            }
            
            if (ShouldCreateAnotherTask(settingPM.Id,settingPM.NumberOfPayments))
                    AddTask(settingPM);
        }


        
        public void AddTask(ExpenseAllocationSettingPM settingPM)
        {
            ExpenseAllocationFlow expenseAllocationFlow = new ExpenseAllocationFlow();
            expenseAllocationFlow.Id = IdCounter.GetNumber("ExpenseAllocationFlow", tenant).ToString();
            expenseAllocationFlow.Tenant =tenant;
            expenseAllocationFlow.SettingId = settingPM.Id;
            expenseAllocationFlow.Status = "Done";
            expenseAllocationFlow.RunDate = GetNextRunDate(settingPM.PaymentDateType,settingPM.MonthInterval,DateTime.Now);
            expenseAllocationFlow.JournalId =null;

            entityRepository.Add(expenseAllocationFlow);
            entityRepository.SubmitChanges();


        }


        private string AddJournalAndJournalLines(APInvoicePM theEntityPm, ExpenseAllocationSettingPM settingPM,string expenseAllocationFlowId )
        {
            int tenant = theEntityPm.Tenant;
          
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            FullAccountingSettingPM accountingSettings = getFullAccountingSettings(theEntityPm.Tenant);
            if (tenantPOCO.AccountingActivated)
                {                  
                    JournalPM journal = new JournalPM();
                    journal.Tenant = tenant;
                    journal.JournalNumber = "1";
                    journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.AccountingDate =  TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.DueDate = TenantServerConfigration.GetCurrentDateTime(tenant);
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
                    Accounting.Def.EntityPMs.GLAccountPM glAccount = GetInvoiceGLAccount(theEntityPm);
                    JournalLinePM journalLine = new JournalLinePM();
                    if (!differentCurrencies)
                    {
                        journalLine = CreateJournalLinePM(theEntityPm, ++counter, journal.Id, glAccount.Id, theEntityPm.IsPrepaidExpenses ? accountingSettings?.PrepaidExpensesGLAccountId : null);

                        journalLine.LocalAmount = (decimal)theEntityPm.SubTotalInLocalCurrency / settingPM.NumberOfPayments;
                        journalLine.CurrencyId = theEntityPm.InvoiceCurrencyId;
                        journalLine.ForeignAmount = (decimal)theEntityPm.SubTotalInInvoiceCurrency / settingPM.NumberOfPayments;
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

                foreach (var item in theEntityPm.InvoiceLines)
                {
                    if (item.VatRecognizedPercentage == 0 || item.VatRecognizedPercentage == null)
                    {
                        if (item.VatPercentage == null || item.VatRecognizedPercentage == null)
                        {
                            item.LocalAmountWithVatRecognized = item.LocalCurrencyAmount;
                        }
                        else
                        {
                            item.LocalAmountWithVatRecognized = item.LocalCurrencyAmount + ((item.VatPercentage / 100) * item.LocalCurrencyAmount);
                        }

                    }
                    else
                    {
                        item.LocalAmountWithVatRecognized = (item.LocalCurrencyAmount + ((item.VatPercentage / 100) * ((1 - item.VatRecognizedPercentage) * item.LocalCurrencyAmount)));
                    }
                    item.ForiegnAmountWithRecognizedVat = item.LocalAmountWithVatRecognized != null ? item.LocalAmountWithVatRecognized / item.ForiegnExchangeRate : item.LocalAmountWithVatRecognized;

                }



                List<JournalLinePM> journalLines = (from d in theEntityPm.InvoiceLines?.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete)
                                                        select new JournalLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ActionCode = AccountingActionCodes.Debit,
                                                            ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
                                                            JournalId = journal.Id,
                                                            DebitAccountId =  d.ChargeTypeGLAccountId,
                                                            CreditAccountId = accountingSettings?.PrepaidExpensesGLAccountId,
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
                    AddAccountingEntitieJournal(journal, AccountingEntityJournalActions.APInvoiceApprove, expenseAllocationFlowId);
                    journalUpdate.Update(journal);
                    return journal.Id;

                }

            return null;
           
        }

        private JournalLinePM CreateJournalLinePM(APInvoicePM theEntityPm, int lineNo, string journalId, string glAccountId, string prepaidExpensesGLAccountId = null)
        {
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
            journalLine.CreditAccountId = prepaidExpensesGLAccountId;

            journalLine.DebitAccountId =  SetDebitAccountForSingleLineAPInvoice(theEntityPm);
            journalLine.ChangeSetOp = ChangeSetOperation.Insert;
            return journalLine;
        }
        public string SetDebitAccountForSingleLineAPInvoice(APInvoicePM invoice)
        {
            if (invoice.InvoiceLines.Count == 1)
            {
                APInvoiceLinePM invoiceLine = invoice.InvoiceLines?.Where(a => a.ChangeSetOp != ChangeSetOperation.Delete)?.First();
                return invoiceLine.ChargeTypeGLAccountId;

            }

            else return null;
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
        public GLAccountPM GetInvoiceGLAccount(APInvoicePM invoicePM)
        {
            GLAccountPM glAccount;
            if (invoicePM.VendorGLAccountId != null)
                glAccount = GetGLAccountById(invoicePM.VendorGLAccountId, invoicePM.Tenant);
            else
                glAccount = GetGLAccountByCardId(invoicePM.VendorId, invoicePM.Tenant);
            return glAccount;
        }
        private static GLAccountPM GetGLAccountById(string glaccountId, int tenant)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride(string.Empty, 1)) as IGLAccountQueryServiceExt;
            GLAccountPM glaAccount = glAccountQuery.GetSingleGLAccountPM(glaccountId, tenant);
            return glaAccount;
        }
        private static GLAccountPM GetGLAccountByCardId(string cardId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(cardId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride(string.Empty, 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }
        public static DateTime GetNextRunDate(
     string frequencyPattern,
     int interval,
     DateTime lastRunDate){


            var parts = frequencyPattern.Split('_');
            if (parts.Length == 0)
                throw new ArgumentException("Invalid frequency pattern");

            string type = parts[0]; 
            string option = parts.Length > 1 ? parts[1] : null;
            string value = parts.Length > 2 ? parts[2] : null;

            DateTime nextDate = lastRunDate;

            if (type == "Weekly")
            {  
                DayOfWeek targetDay = ParseDayOfWeek(option);
                nextDate = GetNextWeeklyDate(lastRunDate, targetDay, interval);
            }
            else if (type == "Monthly")
            {
                nextDate = GetNextMonthlyDate(option, value, interval, lastRunDate);
            }
            else
            {
                throw new InvalidOperationException("Unknown frequency type: " + type);
            }

            return nextDate;

            
        }

        private static DateTime GetNextMonthlyDate(string option, string value, int interval, DateTime lastRunDate)
        {
            DateTime targetMonth = lastRunDate.AddMonths(interval);

            switch (option)
            {
                case "Start":
                    return new DateTime(targetMonth.Year, targetMonth.Month, 1);

                case "End":
                    int daysInMonth = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
                    return new DateTime(targetMonth.Year, targetMonth.Month, daysInMonth);

                case "SpecificDate":
                    int day = int.Parse(value);
                    int maxDays = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);
                    if (day > maxDays) day = maxDays;
                    return new DateTime(targetMonth.Year, targetMonth.Month, day);

                case "FirstWeek":
                case "SecondWeek":
                case "ThirdWeek":
                case "FourthWeek":
                    int weekNum = GetWeekNumber(option);
                    DayOfWeek dayOfWeekEnum = ParseDayOfWeek(value);
                    return GetMonthlyWeekday(targetMonth, weekNum, dayOfWeekEnum);

                default:
                    throw new InvalidOperationException("Unknown monthly option: " + option);
            }
        }

        private static int GetWeekNumber(string option)
        {
            if (option == "FirstWeek") return 1;
            if (option == "SecondWeek") return 2;
            if (option == "ThirdWeek") return 3;
            if (option == "FourthWeek") return 4;
            throw new ArgumentException("Invalid week option: " + option);
        }

        private static DateTime GetMonthlyWeekday(DateTime month, int weekNum, DayOfWeek targetDay)
        {
            DateTime firstDay = new DateTime(month.Year, month.Month, 1);
            int offset = ((int)targetDay - (int)firstDay.DayOfWeek + 7) % 7;
            return firstDay.AddDays(offset + (weekNum - 1) * 7);

        }

        private static DayOfWeek ParseDayOfWeek(string day)
        {
            if (string.IsNullOrEmpty(day))
                throw new ArgumentException("Invalid day of week");

            switch (day.ToLower())
            {
                case "sunday": return DayOfWeek.Sunday;
                case "monday": return DayOfWeek.Monday;
                case "tuesday": return DayOfWeek.Tuesday;
                case "wednesday": return DayOfWeek.Wednesday;
                case "thursday": return DayOfWeek.Thursday;
                case "friday": return DayOfWeek.Friday;
                case "saturday": return DayOfWeek.Saturday;
                default:
                    throw new ArgumentException("Invalid day of week: " + day);
            }
        }

        private static DateTime GetNextWeeklyDate(DateTime fromDate, DayOfWeek targetDay, int interval)
        {
            DateTime next = fromDate.AddDays(1);
            while (next.DayOfWeek != targetDay)
                next = next.AddDays(1);

            return next.AddDays((interval - 1) * 7);
        }





    }
}