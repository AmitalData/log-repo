using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityMapping;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class InterestTransactionListQueryService
    {
        private const string DummyInterestReportIdFilterValue = "999";
        private const string creditActionCodeType = "1";
        
        private IQueryable<InterestTransactionList> GetIqueryableList(IQueryable<InterestTransaction> interestTransactionQuery, int tenant)
        {
            var interestTransactionsForAdustmentsAndRevaluationJournals = GetInterestTransactionsForAdustmentsAndRevaluationJournals(interestTransactionQuery);
            var interestTransactionsForARPayments = GetInterestTransactionsForARPayments(interestTransactionQuery);
            
            IQueryable<InterestTransactionList> query
                = (from interestTransaction in interestTransactionQuery.Include("Currency")

                   join journal in context.Journals.Include("AccountingEntity")
                   on interestTransaction.JournalId equals
                      journal.Id
                    
                   join report in context.InterestReports on interestTransaction.InterestReportId equals report.Id
                   into reportJoinData
                   from report in reportJoinData.DefaultIfEmpty()
                   where journal.AccountingEntityCode != Enums.AccountingEntityValues.Adjustment && journal.AccountingEntityCode != Enums.AccountingEntityValues.Revaluation
                         && journal.AccountingEntityCode != Enums.AccountingEntityValues.Journal && journal.AccountingEntityCode != Enums.AccountingEntityValues.BankAdjustment
                         && journal.AccountingEntityCode != Enums.AccountingEntityValues.ARPayment
                   select new InterestTransactionList()
                   {

                       Id = interestTransaction.Id,
                       Tenant = interestTransaction.Tenant,
                       CreateDateTime = interestTransaction.CreateDateTime,
                       UpdateDateTime = interestTransaction.UpdateDateTime,
                       SearchFields = interestTransaction.SearchFields,
                       GLAccountId = interestTransaction.GLAccountId,
                       InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                       EntityId = interestTransaction.EntityId,
                       OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                       LocalAmount = interestTransaction.LocalAmount,
                       ForeignAmount = interestTransaction.ForeignAmount,
                       CurrencyId = interestTransaction.CurrencyId,
                       InterestValueDate = interestTransaction.InterestValueDate,
                       InterestReportId = interestTransaction.InterestReportId,
                       IsClosed = interestTransaction.IsClosed,
                       IsCancelled = interestTransaction.IsCancelled,
                       CurrencyCode = interestTransaction.Currency.Code,
                       InterestReportNumber = report == null ? null : report.ReportNumber,

                       JournalId = journal.Id,
                       JournalNumber = journal.JournalNumber,
                       AccountingDate = interestTransaction.AccountingDate,

                       Source = journal.AccountingEntityReference,
                       SourceType = journal.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                       SourceTypeCode = journal.AccountingEntityCode,
                       SourceId = journal.AccountingEntityId,
                       AccountingEntityCode = interestTransaction.AccountingEntityCode,
                       Notes=interestTransaction.Notes,
                   }).Union(interestTransactionsForAdustmentsAndRevaluationJournals).Union(interestTransactionsForARPayments);
            return query;
        }

        private IQueryable<InterestTransactionList> GetInterestTransactionsForAdustmentsAndRevaluationJournals(IQueryable<InterestTransaction> interestTransactionQuery) {
            return from interestTransaction in interestTransactionQuery.Include("Currency")

                        join journal in context.Journals.Include("AccountingEntity")
                        on interestTransaction.JournalId equals
                           journal.Id
                        join report in context.InterestReports on interestTransaction.InterestReportId equals report.Id
                        into reportJoinData
                        from report in reportJoinData.DefaultIfEmpty()
                        where journal.AccountingEntityCode == Enums.AccountingEntityValues.Adjustment || journal.AccountingEntityCode == Enums.AccountingEntityValues.Revaluation
                        || journal.AccountingEntityCode == Enums.AccountingEntityValues.Journal || journal.AccountingEntityCode == Enums.AccountingEntityValues.BankAdjustment

                        select new InterestTransactionList()
                        {

                            Id = interestTransaction.Id,
                            Tenant = interestTransaction.Tenant,
                            CreateDateTime = interestTransaction.CreateDateTime,
                            UpdateDateTime = interestTransaction.UpdateDateTime,
                            SearchFields = interestTransaction.SearchFields,
                            GLAccountId = interestTransaction.GLAccountId,
                            InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                            EntityId = interestTransaction.EntityId,
                            OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                            LocalAmount = interestTransaction.LocalAmount,
                            ForeignAmount = interestTransaction.ForeignAmount,
                            CurrencyId = interestTransaction.CurrencyId,
                            InterestValueDate = interestTransaction.InterestValueDate,
                            InterestReportId = interestTransaction.InterestReportId,
                            IsClosed = interestTransaction.IsClosed,
                            IsCancelled = interestTransaction.IsCancelled,
                            CurrencyCode = interestTransaction.Currency.Code,
                            InterestReportNumber = report == null ? null : report.ReportNumber,

                            JournalId = journal.Id,
                            JournalNumber = journal.JournalNumber,
                            AccountingDate = journal.AccountingDate,

                            Source = journal.AccountingEntityReference,
                            SourceType = journal.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                            SourceTypeCode = journal.AccountingEntityCode,
                            SourceId = journal.AccountingEntityId,
                            AccountingEntityCode = interestTransaction.AccountingEntityCode,
                            Notes = interestTransaction.Notes,
                        };
        }
        private IQueryable<InterestTransactionList> GetInterestTransactionsForARPayments(IQueryable<InterestTransaction> interestTransactionQuery)
        {
            return from interestTransaction in interestTransactionQuery.Include("Currency")

                   join journal in context.Journals.Include("AccountingEntity")
                   on interestTransaction.JournalId equals
                   journal.Id
                   join journallines in context.JournalLines on new { journalId = journal.Id, tenant = journal.Tenant} equals
                            new
                            {
                                journalId = journallines.JournalId,
                                tenant = journallines.Tenant
                            }
                   join report in context.InterestReports on interestTransaction.InterestReportId equals report.Id
                   into reportJoinData
                   from report in reportJoinData.DefaultIfEmpty()
                   where journal.AccountingEntityCode == Enums.AccountingEntityValues.ARPayment && journallines.ActionCode == creditActionCodeType
                   && interestTransaction.LocalAmount == journallines.LocalAmount * -1

                   select new InterestTransactionList()
                   {

                       Id = interestTransaction.Id,
                       Tenant = interestTransaction.Tenant,
                       CreateDateTime = interestTransaction.CreateDateTime,
                       UpdateDateTime = interestTransaction.UpdateDateTime,
                       SearchFields = interestTransaction.SearchFields,
                       GLAccountId = interestTransaction.GLAccountId,
                       InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                       EntityId = interestTransaction.EntityId,
                       OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                       LocalAmount = interestTransaction.LocalAmount,
                       ForeignAmount = interestTransaction.ForeignAmount,
                       CurrencyId = interestTransaction.CurrencyId,
                       InterestValueDate = interestTransaction.InterestValueDate,
                       InterestReportId = interestTransaction.InterestReportId,
                       IsClosed = interestTransaction.IsClosed,
                       IsCancelled = interestTransaction.IsCancelled,
                       CurrencyCode = interestTransaction.Currency.Code,
                       InterestReportNumber = report == null ? null : report.ReportNumber,

                       JournalId = journal.Id,
                       JournalNumber = journal.JournalNumber,
                       AccountingDate = journal.AccountingDate,

                       Source = journal.AccountingEntityReference,
                       SourceType = journal.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                       SourceTypeCode = journal.AccountingEntityCode,
                       SourceId = journal.AccountingEntityId,
                       AccountingEntityCode = interestTransaction.AccountingEntityCode,
                       Notes = interestTransaction.Notes,
                   };
        }
        public List<InterestTransactionList> MapListQuery(List<InterestTransactionList> interestTransactions, int tenant, bool? exportToExcell = null)
        {
            List<Currency> currencies = GetTenantCurrencies(tenant);

            List<InterestTransactionList> list
                = (from interestTransaction in interestTransactions

                   select new InterestTransactionList()
                   {
                       CurrencyCode = interestTransaction.CurrencyCode,
                       Id = interestTransaction.Id,
                       Tenant = interestTransaction.Tenant,
                       CreateDateTime = interestTransaction.CreateDateTime,
                       UpdateDateTime = interestTransaction.UpdateDateTime,
                       SearchFields = interestTransaction.SearchFields,
                       GLAccountId = interestTransaction.GLAccountId,
                       InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                       EntityId = interestTransaction.EntityId,
                       OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                       LocalAmount = interestTransaction.LocalAmount,
                       ForeignAmount = interestTransaction.ForeignAmount,
                       CurrencyId = interestTransaction.CurrencyId,
                       InterestValueDate = interestTransaction.InterestValueDate,
                       InterestReportId = interestTransaction.InterestReportId,
                       IsClosed = interestTransaction.IsClosed,
                       IsCancelled = interestTransaction.IsCancelled,
                       InterestReportNumber = interestTransaction.InterestReportNumber,
                       JournalId = interestTransaction.JournalId,
                       JournalNumber = interestTransaction.JournalNumber,
                       Source = exportToExcell == true? getEntityIcon(interestTransaction.SourceTypeCode) + " " + interestTransaction.Source : interestTransaction.Source,
                       SourceTypeCode = interestTransaction.SourceTypeCode,
                       SourceType = interestTransaction.SourceType,
                       SourceId = interestTransaction.SourceId,
                       AccountingDate = interestTransaction.AccountingDate,
                       Notes = interestTransaction.Notes,

                   }).ToList();

            return list;
        }

        private string getEntityIcon(string sourceTypeCode)
        {
            var iconTxt = "";
            switch (sourceTypeCode)
            {
                // 1-Journal
                case "1":
                    {
                        iconTxt = "JR";
                        break;
                    }

                // 2-ARInvoice
                case "2":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 3-ARPayment
                case "3":
                    {
                        iconTxt = "PY";
                        break;
                    }

                // 4-APInvoice
                case "4":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 5-APPayment
                case "5":
                    {
                        iconTxt = "PY";

                        break;
                    }

                // 6-Cheque Deposit
                case "6":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 7-Cash Deposit
                case "7":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 8-Revaluation
                case "8":
                    {
                        iconTxt = "RV";

                        break;
                    }

                // 9-PaymentCheque
                case "9":
                    {
                        iconTxt = "CH";

                        break;
                    }

                // 10-Adjustment
                case "10":
                    {
                        iconTxt = "AJ";

                        break;
                    }
            }
            return iconTxt;
        }

        public InterestTransactionList GetSingle(string id,int tenant)
        {
            InterestTransaction interestTransaction = GetInterestTransaction(id, tenant);
            Journal journal = GetJournalForInterestTransaction(tenant, interestTransaction);
            InterestReport report = GetInterestReportForInterestTransaction(tenant, interestTransaction);
            InterestTransactionList transactionList = MapInterestTransaction(interestTransaction, journal, report);

            return transactionList;
        }

        private static InterestTransactionList MapInterestTransaction(InterestTransaction interestTransaction, Journal journal, InterestReport report)
        {
            return new InterestTransactionList()
            {

                Id = interestTransaction.Id,
                Tenant = interestTransaction.Tenant,
                CreateDateTime = interestTransaction.CreateDateTime,
                UpdateDateTime = interestTransaction.UpdateDateTime,
                SearchFields = interestTransaction.SearchFields,
                GLAccountId = interestTransaction.GLAccountId,
                InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                EntityId = interestTransaction.EntityId,
                OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                LocalAmount = interestTransaction.LocalAmount,
                ForeignAmount = interestTransaction.ForeignAmount,
                CurrencyId = interestTransaction.CurrencyId,
                InterestValueDate = interestTransaction.InterestValueDate,
                InterestReportId = interestTransaction.InterestReportId,
                IsClosed = interestTransaction.IsClosed,
                IsCancelled = interestTransaction.IsCancelled,
                InterestReportNumber = report?.ReportNumber,

                JournalId = journal?.Id,
                JournalNumber = journal?.JournalNumber,
                AccountingDate = journal?.AccountingDate ?? DateTime.MinValue,
                Source = journal?.AccountingEntityReference,
                SourceType = journal?.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                SourceTypeCode = journal?.AccountingEntityCode,
                SourceId = journal?.AccountingEntityId,
                Notes = interestTransaction.Notes,

            };
        }

        private static InterestReport GetInterestReportForInterestTransaction(int tenant, InterestTransaction interestTransaction)
        {
            var reportRepository = new InterestReportRepository(tenant);
            var report = reportRepository.GetSingle(interestTransaction.InterestReportId, tenant);
            return report;
        }

        private static Journal GetJournalForInterestTransaction(int tenant, InterestTransaction interestTransaction)
        {
            var journalQuery = new JournalRepository(tenant);
            var journal = journalQuery.GetJournalByAccountingEntity(interestTransaction.EntityId, interestTransaction.InterestEntityType.AccountingEntityCode, tenant);
            return journal;
        }

        private InterestTransaction GetInterestTransaction(string id, int tenant)
        {
            context = AccountingContext.GetContext(tenant);
            var interestTransaction = (from a in context.InterestTransactions.Include("InterestEntityType")
                                       where a.Id == id && a.Tenant == tenant
                                       select a).FirstOrDefault();
            return interestTransaction;
        }

        private static List<Currency> GetTenantCurrencies(int tenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            var currencies = currencyRepository.GetCurrencies(tenant).ToList();
            return currencies;
        }

        private IQueryable<InterestTransaction> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<InterestTransaction> iQueryable, int tenant)
        {
            var customInterestReportIdFilter = queryOperations.QueryFilterItems.Where(x => x.FieldName == "InterestReportId" && x.IsCustom).FirstOrDefault();
            if (customInterestReportIdFilter != null)
            {
                iQueryable = iQueryable.Where(x => x.InterestReportId == null || x.InterestReportId == DummyInterestReportIdFilterValue);
                return iQueryable;
            }
            else
            {
                return iQueryable;
            }
        }
        private IQueryable<InterestTransaction> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<InterestTransaction> iQueryable, int tenant)
        {
            return iQueryable;
        }



        public IQueryable<InterestTransactionList> GetFutureInterestTransactionsByInterestReportMonth(DateTime reportMonthLastDay, string gLAccountId, int tenant)
        {
            DateTime reportMonthLastDayDate = reportMonthLastDay.Date;
            DateTime nextMonth1st = reportMonthLastDayDate.Date.AddDays(1);

            IQueryable<InterestTransactionList> query = from interestTransaction in context.InterestTransactions.Where(a => a.GLAccountId == gLAccountId
                    && a.InterestReportId == null && a.Tenant == tenant
                    && a.IsCancelled == false && a.IsClosed == false
                    && a.CreateDateTime < nextMonth1st
                    && a.InterestValueDate > reportMonthLastDayDate).Include("Currency")

                    join journal in context.Journals.Include("AccountingEntity")
                    on new { AccountingEntityId = interestTransaction.EntityId, AccountingEntityCode = interestTransaction.AccountingEntityCode, Tenant = interestTransaction.Tenant } equals
                        new
                        {
                            AccountingEntityId = journal.AccountingEntityId,
                            journal.AccountingEntityCode,
                            journal.Tenant
                        }
                    join journallines in context.JournalLines on new { journalId = journal.Id, tenant = journal.Tenant } equals
                            new
                            {
                                journalId = journallines.JournalId,
                                tenant = journallines.Tenant
                            }
                    join ten in context.Tenants on interestTransaction.Tenant equals ten.Id
                    into joinData
                    from x in joinData.DefaultIfEmpty()
                    where journal.AccountingEntityCode == Enums.AccountingEntityValues.ARPayment && journallines.ActionCode == creditActionCodeType
                    && interestTransaction.LocalAmount == journallines.LocalAmount * -1

                    select new InterestTransactionList()
                    {

                        Id = interestTransaction.Id,
                        Tenant = interestTransaction.Tenant,
                        CreateDateTime = interestTransaction.CreateDateTime,
                        UpdateDateTime = interestTransaction.UpdateDateTime,
                        SearchFields = interestTransaction.SearchFields,
                        GLAccountId = interestTransaction.GLAccountId,
                        InterestEntityTypeCode = interestTransaction.InterestEntityTypeCode,
                        EntityId = interestTransaction.EntityId,
                        OriginalEntityLineNumber = interestTransaction.OriginalEntityLineNumber,
                        LocalAmount = interestTransaction.LocalAmount,
                        ForeignAmount = interestTransaction.ForeignAmount,
                        CurrencyId = interestTransaction.CurrencyId,
                        InterestValueDate = interestTransaction.InterestValueDate,
                        //InterestReportId = interestTransaction.InterestReportId,
                        IsClosed = interestTransaction.IsClosed,
                        IsCancelled = interestTransaction.IsCancelled,
                        CurrencyCode = interestTransaction.Currency.Code,
                        //InterestReportNumber = report == null ? null : report.ReportNumber,

                        JournalId = journal.Id,
                        JournalNumber = journal.JournalNumber,
                        AccountingDate = journal.AccountingDate,

                        Source = journal.AccountingEntityReference,
                        SourceType = journal.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                        SourceTypeCode = journal.AccountingEntityCode,
                        SourceId = journal.AccountingEntityId,
                        AccountingEntityCode = interestTransaction.AccountingEntityCode,
                        Notes = interestTransaction.Notes,
                    };


            return query;



        }


    }


}
