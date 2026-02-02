using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel
{
    public interface IInvoiceContext : IContext
    {
        IDbSet<ARInvoice> ARInvoices { get; }
        IDbSet<ARInvoiceAnalytic> ARInvoiceAnalytics { get; }
        IDbSet<APInvoiceAnalytic> APInvoiceAnalytics { get; }
        IDbSet<ARInvoiceLine> ARInvoiceLines { get; }
        IDbSet<ARInvoiceType> ARInvoiceTypes { get; }
        IDbSet<ARInvoiceStatus> ARInvoiceStatuses { get; }
        IDbSet<ARInvoiceTotalVAT> ARInvoiceTotalVATs { get; }
        IDbSet<ARInvoiceEntity> ARInvoiceEntities { get; }
        IDbSet<Account> Accounts { get; }
        IDbSet<AccountType> AccountTypes { get; }
        IDbSet<ARPayment> ARPayments { get; }       
        IDbSet<AccountingPaymentMethod> AccountingPaymentMethods { get; }
        IDbSet<ARPaymentStatus> ARPaymentStatus { get; }
        IDbSet<ARInvoicePayment> ARInvoicePayments { get; }      
        IDbSet<APInvoice> APInvoices { get; }
        IDbSet<APInvoiceLine> APInvoiceLines { get; }
        IDbSet<APInvoiceStatus> APInvoiceStatus { get; }
        IDbSet<APInvoiceTotalVAT> APInvoiceTotalVATs { get; }
        IDbSet<APInvoiceType> APInvoiceTypes { get; }       
        IDbSet<APInvoiceEntity> APInvoiceEntities { get; }
        IDbSet<APPayment> APPayments { get; }
        IDbSet<APPaymentStatus> APPaymentStatus { get; }
        IDbSet<APInvoicePayment> APInvoicePayments { get; }
        IDbSet<CreditCardType> CreditCardTypes { get; }
        IDbSet<AccountingTransferHeader> AccountingTransferHeaders { get; }
        IDbSet<AccountingTransferLine> AccountingTransferLines { get; }
        IDbSet<AccountingTransferType> AccountingTransferTypes { get; }
        IDbSet<ARInvoiceTransferStatus> ARInvoiceTransferStatuses { get; }
        IDbSet<ExternalSystemsTablesCode> ExternalSystemsTablesCodes { get; }
        IDbSet<ExternalSystemsMissingTranslation> ExternalSystemsMissingTranslations { get; }
        IDbSet<ExternalSystemsSyncStatus> ExternalSystemsSyncStatuses { get; set; }
        IDbSet<ExpenseAllocationSetting> ExpenseAllocationSettings { get; set; }
        IDbSet<ExpenseAllocationFlow> ExpenseAllocationFlows { get; set; }

        IDbSet<AccountingSystemsSetting> AccountingSystemsSettings { get; set; }
        IDbSet<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuses { get; set; }
        IDbSet<QuickbooksSyncRequestTicket> QuickbooksSyncRequestTickets { get; set; }
        IDbSet<APInvoiceTransferStatus> APInvoiceTransferStatuses { get; }
        IDbSet<ARInvoiceLineAction> ARInvoiceLineActions { get; }
        IDbSet<ARPaymentTransferStatus> ARPaymentTransferStatuses { get; }
        IDbSet<SATPaymentMethod> SATPaymentMethods { get; }
        IDbSet<APPaymentTransferStatus> APPaymentTransferStatuses { get; }
        IDbSet<SATTransferStatus> SATTransferStatus { get; }
        IDbSet<QBOGlobalTaxCalculation> QBOGlobalTaxCalculations { get; }
        IDbSet<SATInvoiceStatus> SATInvoiceStatus { get; }
        IDbSet<SATInterface> SATInterfaces { get; }
        IDbSet<SATInterfaceSetting> SATInterfaceSettings { get; }
        IDbSet<BankAccountLite> BankAccountLites { get; }
        IDbSet<ARInvoiceStocksStatus> ARInvoiceStocksStatus { get; }
        IDbSet<ARInvoiceStock> ARInvoiceStocks { get; }
        IDbSet<ARInvoiceStockLine> ARInvoiceStockLines { get; }
        IDbSet<ARPaymentChequeReplica> ARPaymentChequeReplicas { get; }
        IDbSet<ARPaymentChequeStatusReplica> ARPaymentChequeStatusReplicas { get; }
        IDbSet<ARInvoiceChargesConstraint> ARInvoiceChargesConstraints { get; }
        IDbSet<ARPaymentBankTranfer> ARPaymentBankTranfers { get; }
        IDbSet<DigitalInvoicesCounterDataView> DigitalInvoicesCounterDataView { get; }
        IDbSet<ControlForInvoiceLinesDataView> ControlForInvoiceLinesDataView { get; }
        IDbSet<BankAccountView> BankAccountView { get; }
        IDbSet<ARInvoicesSignedStatus> ARInvoicesSignedStatuses { get; }
        IDbSet<ConfirmationNumberStatus> ConfirmationNumberStatuses { get; }

        IDbSet<ConfirmationNumberDefault> ConfirmationNumberDefaults { get; }
        IDbSet<MasavInterface> MasavInterfaces { get; }
        IDbSet<MasavInterfaceStatus> MasavInterfaceStatuses { get; }

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
