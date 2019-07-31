using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
//using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Data.Entity.Core;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.Helpers;
//using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class AccountingDomainService : LogitudeDomainService
    {
        private IAccountingContext accountingContext;
        private LedgerTransactionRepository ledgerTransactionRepository;
        private ReconciliationRepository reconciliationRepository;
        private ReconciliationLineRepository reconciliationLineRepository;

        private BankCodeQueryService bankCodeQuery;
        private JournalActionTypeQueryService journalActionTypeQuery;
        private JournalStatusTypeQueryService journalStatusTypeQuery;
        private JournalTypeQueryService journalTypeQuery;
        private AccountingEntityQueryService accountingEntityQuery;
        private ChartOfAccountQueryService chartOfAccountQuery;
        private ChartOfAccountsTypeQueryService chartOfAccountsTypeQuery;
        private GLAccountTypeQueryService gLAccountTypeQuery;
        private GLAccountQueryService gLAccountQuery;
        private RevenueExpenseTypeQueryService revenueExpenseTypeQuery;
        private JournalQueryService journalQuery;
        private GLAccountTotalByMonthQueryService gLAccountTotalByMonthQuery;
        private LedgerTransactionQueryService ledgerTransactionQuery;
        private ReconcileMethodQueryService reconcileMethodQuery;
        private PeriodTypeQueryService periodTypeQuery;
        private AccountingPeriodQueryService accountingPeriodQuery;
        private AutomaticReconcileQueryService automaticReconcileQuery;
        private AutomaticReconcileMethodQueryService automaticReconcileMethodQuery;
        private ReconciliationQueryService reconciliationQuery;
        private ReconciliationLineQueryService reconciliationLineQuery;
        private BankPageEntryTypeQueryService BankPageEntryTypeQuery;
        private GLAccountTotalDateTypeQueryService GLAccountTotalDateTypeQuery;
        private ReconcileExternalPageStatusQueryService ReconcileExternalPageStatusQuery;
        //   private CashBookTypeQueryService cashBookTypeQuery;
        //   private CashBookQueryService cashBookQuery;

        protected override bool PersistChangeSet()
        {
            try
            {
                //       objectContext.SaveChanges();
                accountingContext.SaveChanges();
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }


        public BankAccountSummary GetBankAccountSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            BankAccountSummary result = new BankAccountSummary();

            if (SecurityUtility.CheckTableContactFeature("BankAccount", "READ", tenant))
            {

                BankAccountRepository glAccountRepository = new BankAccountRepository(tenant);
                IQueryable<BankAccount> iQueryable_Data = glAccountRepository.GetAll(tenant);

                // Main BankAccounts
                result.AllBankAccountsCount = iQueryable_Data.Count();
 

            }

            return result;
        }
        public PaymentChequeSummary GetPaymentChequeSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            PaymentChequeSummary result = new PaymentChequeSummary();

            //if (SecurityUtility.CheckTableContactFeature("PaymentCheque", "READ", tenant))
            //{

                PaymentChequeRepository paymentChequeRepository = new PaymentChequeRepository(tenant);
                IQueryable<PaymentCheque> iQueryable_Data = paymentChequeRepository.GetAll(tenant);

                // Main BankAccounts
                result.AllPaymentChequesCount = iQueryable_Data.Count();


         //   }

            return result;
        }



        public BankDepositSummary GetBankDepositSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            BankDepositSummary result = new BankDepositSummary();

            if (SecurityUtility.CheckTableContactFeature("BankDeposit", "READ", tenant))
            {

                BankDepositRepository bankDepositRepository = new BankDepositRepository(tenant);
                IQueryable<BankDeposit> iQueryable_Data = bankDepositRepository.GetAll(tenant);

                
                result.TodaysDepositCount = iQueryable_Data.Where(d=> d.DepositDate.Month == DateTime.Now.Month && d.DepositDate.Day == DateTime.Now.Day && d.DepositDate.Year == DateTime.Now.Year).Count();


            }

            return result;
        }


        public CashBookSummary GetCashbookSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            CashBookSummary result = new CashBookSummary();

            if (SecurityUtility.CheckTableContactFeature("Cashbook", "READ", tenant))
            {

                CashBookRepository cashBookRepository = new CashBookRepository(tenant);
                IQueryable<CashBook> iQueryable_Data = cashBookRepository.GetAll(tenant);


                result.AllCashbookCount = iQueryable_Data.Count();
                result.CashCashbookCount = iQueryable_Data.Where(d=> d.CashBookTypeCode == "1").Count();
                result.ChequeCashbookCount = iQueryable_Data.Where(d => d.CashBookTypeCode == "2").Count();
              
            }

            return result;
        }

      
    }
  
}


