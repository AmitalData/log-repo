using System.ComponentModel.DataAnnotations;
//test
namespace WebFreight.Web.DataContracts
{
    public class AccountReceivablesSummary
    {
        [Key]
        public int Id { get; set; }
        public int ARInvoicesDraftsCount { get; set; }
        public int ARInvoicesUnpaidCount { get; set; }
        public int ARInvoicesOpenConstituentCount { get; set; }
        public int ARPaymentsDraftsCount { get; set; }
        public int ARPaymentsOpenedCount { get; set; }
        public int ARGeneralInvoiceDraftCount { get; set; }
        public int ARPaymentsSATFailedCount { get; set; }
        public int ARInvoicesSATFailedCount { get; set; }
        public int ARInvoicesFailedCount { get; set; }
        public int ARPaymentFailedCount { get; set; }
        public int ARInvoicesSATVoidedNotTransferredCount { get; set; }
        public int ARInvoiceSATWaitingCancellationCount { get; set; }
        public int ARPaymentSATWaitingCancellationCount { get; set; }

    }

    public class AccountPayablesSummary
    {
        [Key]
        public int Id { get; set; }
        public int APInvoicesDraftsCount { get; set; }
        public int APInvoicesUnpaidCount { get; set; }
        public int APInvoicesFailedCount { get; set; }
        public int APPaymentFailedCount { get; set; }
        public int APPaymentsDraftsCount { get; set; }
        public int APPaymentsOpenedCount { get; set; }
    }

    public class AccountTransferSummary
    {
        [Key]
        public int Id { get; set; }
        public int ARInvoicesNotReadyCount { get; set; }
        public int ARInvoicesDontTransferCount { get; set; }
        public int ARInvoicesErrorInTransferCount { get; set; }

        public int APInvoicesNotReadyCount { get; set; }
        public int APInvoicesDontTransferCount { get; set; }
        public int APInvoicesErrorInTransferCount { get; set; }

        public int ARPaymentsNotReadyCount { get; set; }
        public int ARPaymentsDontTransferCount { get; set; }
        public int ARPaymentsErrorInTransferCount { get; set; }

        public int APPaymentsNotReadyCount { get; set; }
        public int APPaymentsDontTransferCount { get; set; }
        public int APPaymentsErrorInTransferCount { get; set; }
    }

    public class GLAccountSummary
    {
        [Key]
        public int Id { get; set; }
        public int ActiveGLAccountCount { get; set; }
        public int InactiveGLAccountCount { get; set; }
        public int AllGLAccountCount { get; set; }
        public int OpenFilesCount { get; set; }
        public int OpenMastersCount { get; set; }
        public int ClosedFilesGLAccountCount { get; set; }
        public int AllFilesCount { get; set; }
        public int AllJobsCount { get; set; }

        // Customers
        public int ActiveCustomersCount { get; set; }
        public int InactiveCustomersCount { get; set; }
        public int CollectorsCount { get; set; }
        public int DebitorsCount { get; set; }
        public int AllCustomersCount { get; set; }

        // Vendors
        public int ActiveVendorsCount { get; set; }
        public int InactiveVendorsCount { get; set; }
        //public int CollectorsCount { get; set; }
        //public int DebitorsCount { get; set; }
        public int AllVendorsCount { get; set; }
    }

    public class BankAccountSummary
    {
        [Key]
        public int Id { get; set; }
        public int AllBankAccountsCount { get; set; }
    }

    public class JournalSummary
    {
        [Key]
        public int Id { get; set; }
        public int AllJournalsCount { get; set; }
        public int ApprovedJournalsCount { get; set; }
        public int WaitingJournalsCount { get; set; }
        public int VoidedJournalsCount { get; set; }
        public int DraftJournalsCount { get; set; }
    }

    public class PaymentChequeSummary
    {
        [Key]
        public int Id { get; set; }
        public int AllPaymentChequesCount { get; set; }
    }

    public class BankDepositSummary
    {
        [Key]
        public int Id { get; set; }
        public int TodaysDepositCount { get; set; }
    }

    public class CashBookSummary
    {
        [Key]
        public int Id { get; set; }
        public int CashCashbookCount { get; set; }
        public int ChequeCashbookCount { get; set; }
        public int AllCashbookCount { get; set; }
      
    }

}