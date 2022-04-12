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

namespace Logitude.Accounting.Data.EntityListQueryServices
{

	public partial class InterestTransactionListQueryService
	{
		private IQueryable<InterestTransactionList> GetIqueryableList(IQueryable<InterestTransaction> interestTransactionQuery, int tenant)
		{
			var commonContext = CommonDataContext.GetContext(tenant);

			IQueryable<InterestTransactionList> query
				= (from interestTransaction in interestTransactionQuery.Include("InterestEntityType")

				   join journal in context.Journals
				   on new { AccountingEntityId = interestTransaction.EntityId, AccountingEntityCode = interestTransaction.InterestEntityType.AccountingEntityCode } equals
					  new { journal.AccountingEntityId, journal.AccountingEntityCode }

				   //join currency in commonContext.Currencies on journal.CurrencyId equals currency.Id

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
					   InterestReportNumber = report == null ? null : report.ReportNumber,

					   JournalId = journal.Id,
					   JournalNumber = journal.JournalNumber,

					   //CurrencyCode = currency.Code,

					   Source = journal.AccountingEntityReference,
					   SourceTypeCode = journal.AccountingEntityCode,
					   SourceId = journal.AccountingEntityId



				   });
			return query;
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
	