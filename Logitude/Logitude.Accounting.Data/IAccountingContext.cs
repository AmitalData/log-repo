using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data; 
using Logitude.Accounting.Data.EntityMapping;

namespace Logitude.Accounting.Data
{

    public partial interface IAccountingContext : IContext
    {
   
       	 IDbSet<AccountingCompanyType> AccountingCompanyTypes { get; }
		 IDbSet<AccountingEntity> AccountingEntities { get; }
		 IDbSet<AccountingIntegrityCheck> AccountingIntegrityChecks { get; }
		 IDbSet<AccountingNote> AccountingNotes { get; }
		 IDbSet<AccountingPeriod> AccountingPeriods { get; }
		 IDbSet<ARPaymentCheque> ARPaymentCheques { get; }
		 IDbSet<ARPaymentChequeStatus> ARPaymentChequeStatuses { get; }
		 IDbSet<AutomaticExternalRconcilMthod> AutomaticExternalRconcilMthods { get; }
		 IDbSet<AutomaticReconcile> AutomaticReconciles { get; }
		 IDbSet<AutomaticReconcileMethod> AutomaticReconcileMethods { get; }
		 IDbSet<BankAccount> BankAccounts { get; }
		 IDbSet<BankCode> BankCodes { get; }
		 IDbSet<BankDeposit> BankDeposits { get; }
		 IDbSet<BankDepositLine> BankDepositLines { get; }
		 IDbSet<BankPageEntryType> BankPageEntryTypes { get; }
		 IDbSet<CalculatedChartsLineType> CalculatedChartsLineTypes { get; }
		 IDbSet<CalculatedChartsOfAccount> CalculatedChartsOfAccounts { get; }
		 IDbSet<CalculatedChartsOfAccountsLine> CalculatedChartsOfAccountLines { get; }
		 IDbSet<CashBook> CashBooks { get; }
		 IDbSet<CashBookLine> CashBookLines { get; }
		 IDbSet<CashBookType> CashBookTypes { get; }
		 IDbSet<Category1> Category1 { get; }
		 IDbSet<Category2> Category2 { get; }
		 IDbSet<Category3> Category3 { get; }
		 IDbSet<Category4> Category4 { get; }
		 IDbSet<Category5> Category5 { get; }
		 IDbSet<ChartOfAccount> ChartOfAccounts { get; }
		 IDbSet<ChartOfAccountsType> ChartOfAccountsTypes { get; }
		 IDbSet<ExternalPageAdditionalData> ExternalPageAdditionalDatas { get; }
		 IDbSet<ExternalReconciliation> ExternalReconciliations { get; }
		 IDbSet<ExternalReconciliationLine> ExternalReconciliationLines { get; }
		 IDbSet<FullAccountingSetting> FullAccountingSettings { get; }
		 IDbSet<GLAccount> GLAccounts { get; }
		 IDbSet<GLAccountAgingData> GLAccountAgingDatas { get; }
		 IDbSet<GLAccountCounter> GLAccountCounters { get; }
		 IDbSet<GLAccountCurrency> GLAccountCurrencies { get; }
		 IDbSet<GLAccountInterestPeriod> GLAccountInterestPeriods { get; }
		 IDbSet<GLAccountMoreData> GLAccountMoreDatas { get; }
		 IDbSet<GLAccountTotalByMonth> GLAccountTotalByMonths { get; }
		 IDbSet<GLAccountTotalDateType> GLAccountTotalDateTypes { get; }
		 IDbSet<GLAccountType> GLAccountTypes { get; }
		 IDbSet<GLAccountWithholdingTax> GLAccountWithholdingTax { get; }
		 IDbSet<IntegrityCheckStatus> IntegrityCheckStatuses { get; }
		 IDbSet<InterestBasesPeriod> InterestBasesPeriods { get; }
		 IDbSet<InterestBasesType> InterestBasesTypes { get; }
		 IDbSet<InterestEntityType> InterestEntityTypes { get; }
		 IDbSet<InterestLastBatchService> InterestLastBatchServices { get; }
		 IDbSet<InterestReport> InterestReports { get; }
		 IDbSet<InterestReportLine> InterestReportLines { get; }
		 IDbSet<InterestReportLinesByDate> InterestReportLinesByDates { get; }
		 IDbSet<InterestReportsConnectInvoice> InterestReportsConnectInvoices { get; }
		 IDbSet<InterestReportStatuse> InterestReportStatuses { get; }
		 IDbSet<InterestTransaction> InterestTransactions { get; }
		 IDbSet<Journal> Journals { get; }
		 IDbSet<JournalActionType> JournalActionTypes { get; }
		 IDbSet<JournalAdditionalData> JournalAdditionalDatas { get; }
		 IDbSet<JournalExternalReconcile> JournalExternalReconciles { get; }
		 IDbSet<JournalLine> JournalLines { get; }
		 IDbSet<JournalMoreData> JournalMoreDatas { get; }
		 IDbSet<JournalReconcile> JournalReconciles { get; }
		 IDbSet<JournalStatusType> JournalStatusTypes { get; }
		 IDbSet<JournalType> JournalTypes { get; }
		 IDbSet<LedgerTransaction> LedgerTransactions { get; }
		 IDbSet<OpenFormatReport> OpenFormatReports { get; }
		 IDbSet<OpenFormatReportStatus> OpenFormatReportStatuses { get; }
		 IDbSet<PaymentCheque> PaymentCheques { get; }
		 IDbSet<PaymentChequeLine> PaymentChequeLines { get; }
		 IDbSet<PaymentChequeStatus> PaymentChequeStatuses { get; }
		 IDbSet<PeriodType> PeriodTypes { get; }
		 IDbSet<ReconcileExternalPage> ReconcileExternalPages { get; }
		 IDbSet<ReconcileExternalPageLine> ReconcileExternalPageLines { get; }
		 IDbSet<ReconcileExternalPageStatus> ReconcileExternalPageStatuses { get; }
		 IDbSet<ReconcileMethod> ReconcileMethods { get; }
		 IDbSet<Reconciliation> Reconciliations { get; }
		 IDbSet<ReconciliationLine> ReconciliationLines { get; }
		 IDbSet<Revaluation> Revaluations { get; }
		 IDbSet<RevaluationStatus> RevaluationStatuses { get; }
		 IDbSet<RevenueExpenseType> RevenueExpenseTypes { get; }
		 IDbSet<TaxDeductionReport> TaxDeductionReports { get; }
		 IDbSet<TaxDeductionReportStatus> TaxDeductionReportStatuses { get; }
		 IDbSet<TaxReport> TaxReports { get; }
		 IDbSet<TaxReportLine> TaxReportLines { get; }
		 IDbSet<TaxReportLineStatus> TaxReportLineStatuses { get; }
		 IDbSet<TaxReportLineTransmitStatus> TaxReportLineTransmitStatuses { get; }
		 IDbSet<TaxReportLineType> TaxReportLineTypes { get; }
		 IDbSet<TaxReportStatus> TaxReportStatuses { get; }
		 IDbSet<TaxWithholdingAssessOffice> TaxWithholdingAssessOffices { get; }
		 IDbSet<TestEntity> TestEntities { get; }
		 IDbSet<UserDefinedReport> UserDefinedReports { get; }
		 IDbSet<VatReportStatus> VatReportStatuses { get; }
		 IDbSet<WithholdingTaxDeductionType> WithholdingTaxDeductionTypes { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}