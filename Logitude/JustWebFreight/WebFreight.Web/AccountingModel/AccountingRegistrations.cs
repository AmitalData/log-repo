//using Logitude.Accounting.BL.CoreBL.Reconcile;
using Logitude.Accounting.BL.CoreBL.Reconcile;
using Logitude.Accounting.BL.EntityQueryServiceExt;
using Logitude.Accounting.BL.EntityUpdateServiceExt;
using Logitude.Accounting.BL.Messaging;
using Logitude.Accounting.Def.BLExt;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.AccountingModel
{
    public class AccountingRegistrations
    {
        public static void Register()
        {
            // Update Service 
            ContainerAccessor.Container.RegisterType<IJournalUpdateServiceExt, JournalUpdateServiceExt>("JournalUpdateServiceExt", new InjectionFactory(c => new JournalUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IJournalVoidUpdateServiceExt, JournalVoidUpdateServiceExt>("JournalVoidUpdateServiceExt", new InjectionFactory(c => new JournalVoidUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IAutoReconcileServiceExt, AutoReconcileService>("AutoReconcileServiceExt", new InjectionFactory(c => new AutoReconcileService()));
            
            ContainerAccessor.Container.RegisterType<ICashBookUpdateServiceExt, CashBookUpdateServiceExt>("CashBookUpdateServiceExt", new InjectionFactory(c => new CashBookUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IARPaymentChequeUpdateServiceExt, ARPaymentChequeUpdateServiceExt>("ARPaymentChequeUpdateServiceExt", new InjectionFactory(c => new ARPaymentChequeUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<ICashBookLineUpdateServiceExt, CashBookLineUpdateServiceExt>("CashBookLineUpdateServiceExt", new InjectionFactory(c => new CashBookLineUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IBankCodeUpdateServiceExt, BankCodeUpdateServiceExt>("BankCodeUpdateServiceExt", new InjectionFactory(c => new BankCodeUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountUpdateServiceExt, GLAccountUpdateServiceExt>("GLAccountUpdateServiceExt", new InjectionFactory(c => new GLAccountUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IPaymentChequeUpdateServiceExt, PaymentChequeUpdateServiceExt>("PaymentChequeUpdateServiceExt", new InjectionFactory(c => new PaymentChequeUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IReconciliationServiceExt, ReconciliationServiceExt>("ReconciliationServiceExt", new InjectionFactory(c => new ReconciliationServiceExt()));
            ContainerAccessor.Container.RegisterType<IInterestTransactionUpdateServiceExt, InterestTransactionUpdateServiceExt>("InterestTransactionUpdateServiceExt", new InjectionFactory(c => new InterestTransactionUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IInterestReportUpdateServiceExt, InterestReportUpdateServiceExt>("InterestReportUpdateServiceExt", new InjectionFactory(c => new InterestReportUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IInterestReportsConnectedInvoiceUpdateServiceExt, InterestReportsConnectedInvoiceUpdateServiceExt>("InterestReportsConnectedInvoiceUpdateServiceExt", new InjectionFactory(c => new InterestReportsConnectedInvoiceUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IAccountingEntityJournalUpdateServiceExt, AccountingEntityJournalUpdateServiceExt>("AccountingEntityJournalUpdateServiceExt", new InjectionFactory(c => new AccountingEntityJournalUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IAPPaymentInvoicesTransactionFetcherExt, APPaymentInvoicesTransactionFetcherExt>("APPaymentInvoicesTransactionFetcherExt", new InjectionFactory(c => new APPaymentInvoicesTransactionFetcherExt()));
            
            // Query Service
            ContainerAccessor.Container.RegisterType<ICashBookQueryServiceExt, CashBookQueryServiceExt>("CashBookQueryServiceExt", new InjectionFactory(c => new CashBookQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IBankCodeQueryServiceExt, BankCodeQueryServiceExt>("BankCodeQueryServiceExt", new InjectionFactory(c => new BankCodeQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountQueryServiceExt, GLAccountQueryServiceExt>("GLAccountQueryServiceExt", new InjectionFactory(c => new GLAccountQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IFullAccountingSettingQueryServiceExt, FullAccountingSettingQueryServiceExt>("FullAccountingSettingQueryServiceExt", new InjectionFactory(c => new FullAccountingSettingQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IBankAccountQueryServiceExt, BankAccountQueryServiceExt>("BankAccountQueryServiceExt", new InjectionFactory(c => new BankAccountQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IJournalQueryServiceExt, JournalQueryServiceExt>("JournalQueryServiceExt", new InjectionFactory(c => new JournalQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IARPaymentChequeQueryServiceExt, ARPaymentChequeQueryServiceExt>("ARPaymentChequeQueryServiceExt", new InjectionFactory(c => new ARPaymentChequeQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountWithholdingTaxQueryServiceExt, GLAccountWithholdingTaxQueryServiceExt>("GLAccountWithholdingTaxQueryServiceExt", new InjectionFactory(c => new GLAccountWithholdingTaxQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IPaymentChequeQueryServiceExt, PaymentChequeQueryServiceExt>("PaymentChequeQueryServiceExt", new InjectionFactory(c => new PaymentChequeQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IInterestTransactionQueryServiceExt, InterestTransactionQueryServiceExt>("InterestTransactionQueryServiceExt", new InjectionFactory(c => new InterestTransactionQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountCardsDataQueryServiceExt, GLAccountCardsDataQueryServiceExt>("GLAccountCardsDataQueryServiceExt", new InjectionFactory(c => new GLAccountCardsDataQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountCardsDataUpdateServiceExt, GLAccountCardsDataUpdateServiceExt>("GLAccountCardsDataUpdateServiceExt", new InjectionFactory(c => new GLAccountCardsDataUpdateServiceExt()));
            ContainerAccessor.Container.RegisterType<IGLAccountCurrencyQueryServiceExt, GLAccountCurrencyQueryServiceExt>("GLAccountCurrencyQueryServiceExt", new InjectionFactory(c => new GLAccountCurrencyQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<ILedgerTransactionQueryService, LedgerTransactionQueryServiceExt>("LedgerTransactionQueryServiceExt", new InjectionFactory(c => new LedgerTransactionQueryServiceExt()));
            ContainerAccessor.Container.RegisterType<IHSMSignFileService, HSMSignFileService>("HSMSignFileService", new InjectionFactory(c => new HSMSignFileService()));

        }
    }
}