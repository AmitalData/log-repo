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
	