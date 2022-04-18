	using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

	public partial class InterestTransactionListQueryService
	{
		private IQueryable<InterestTransactionList> GetIqueryableList(IQueryable<InterestTransaction> interestTransactionQuery, int tenant)
        {
            IQueryable<InterestTransactionList> query
                = (from interestTransaction in interestTransactionQuery.Include("InterestEntityType")

                   join journal in context.Journals.Include("AccountingEntity")
                   on new { AccountingEntityId = interestTransaction.EntityId, AccountingEntityCode = interestTransaction.InterestEntityType.AccountingEntityCode } equals
                      new { journal.AccountingEntityId, journal.AccountingEntityCode }
                      
                   join report in context.InterestReports on interestTransaction.InterestReportId equals report.Id
                   into reportJoinData
                   from report in reportJoinData.DefaultIfEmpty()

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
                       InterestReportNumber = report == null ? null : report.ReportNumber,

                       JournalId = journal.Id,
                       JournalNumber = journal.JournalNumber,
                       AccountingDate = journal.AccountingDate,

                       Source = journal.AccountingEntityReference,
                       SourceType = journal.AccountingEntity == null ? null : journal.AccountingEntity.EnglishName,
                       SourceTypeCode = journal.AccountingEntityCode,
                       SourceId = journal.AccountingEntityId



                   });

            return query;
        }
        public List<InterestTransactionList> MapListQuery(List<InterestTransactionList> interestTransactions, int tenant)
        {
            List<Currency> currencies = GetTenantCurrencies(tenant);

            List<InterestTransactionList> list
                = (from interestTransaction in interestTransactions
                   join currency in currencies on interestTransaction.CurrencyId equals currency.Id

                   select new InterestTransactionList()
                   {
                       CurrencyCode = currency.Code,

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
                       Source = interestTransaction.Source,
                       SourceTypeCode = interestTransaction.SourceTypeCode,
                       SourceType = interestTransaction.SourceType,
                       SourceId = interestTransaction.SourceId,
                       AccountingDate = interestTransaction.AccountingDate,

                   }).ToList();

            return list;
        }

        public InterestTransactionList GetSingle(string id,int tenant)
        {
            var repository = new InterestTransactionRepository(tenant);
            var transactions = repository.GetAll(tenant);

            var transactionQuery = GetIqueryableList(transactions, tenant).Where(d => d.Id == id).ToList();
            var transaction = MapListQuery(transactionQuery, tenant).FirstOrDefault();
            return transaction;
        }



        private static List<Currency> GetTenantCurrencies(int tenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            var currencies = currencyRepository.GetCurrencies(tenant).ToList();
            return currencies;
        }

        private IQueryable<InterestTransaction> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<InterestTransaction> iQueryable, int tenant)
		{

			return iQueryable;
		}
		private IQueryable<InterestTransaction> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<InterestTransaction> iQueryable, int tenant)
		{
			return iQueryable;
		}

	}


}
	